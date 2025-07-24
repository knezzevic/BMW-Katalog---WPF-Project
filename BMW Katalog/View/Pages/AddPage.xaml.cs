using System;
using System.Collections.ObjectModel; // Potrebno za ObservableCollection
using System.ComponentModel; // Potrebno za INotifyPropertyChanged
using System.IO; // Potrebno za File, Path, Directory
using System.Linq; 
using System.Text.RegularExpressions; 
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents; 
using System.Windows.Input; 
using System.Windows.Media; 
using System.Windows.Media.Imaging; 
using BMW_Katalog.Helpers; 
using BMW_Katalog.Model; 
using BMWKatalog.Helpers;
using Microsoft.Win32; 

namespace BMW_Katalog.View.Pages
{
    /// <summary>
    /// Interaction logic for AddPage.xaml
    /// </summary>
    public partial class AddPage : Page, INotifyPropertyChanged
    {
        private string _imagePath;
        public string ImagePath
        {
            get => _imagePath;
            set
            {
                _imagePath = value;
                OnPropertyChanged(nameof(ImagePath));
            }
        }
        private Action _saveCarsCallback;
        private DataIO serializer = new DataIO();
        private ObservableCollection<Cars> _mainCarsList;

        public AddPage(ObservableCollection<Cars> mainCarsList, Action saveCarsCallback)
        {
            InitializeComponent();
            _mainCarsList = mainCarsList; 
            DataContext = this; 
            LoadSystemColors();
            _saveCarsCallback = saveCarsCallback;
            FontFamilyComboBox.SelectedIndex = 51; 
            FontSizeComboBox.SelectedIndex = 2;
            FontColorComboBox.SelectedIndex = 7;
        }

        private void txtImage_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";
            if (ofd.ShowDialog() == true)
            {
                ImagePath = ofd.FileName;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private void LoadSystemColors()
        {
            FontColorComboBox.ItemsSource = typeof(Colors)
                .GetProperties()
                .Select(p => new SolidColorBrush((Color)p.GetValue(null)));
        }

        private void FontFamilyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FontFamilyComboBox.SelectedItem != null && !EditorRichTextBox.Selection.IsEmpty)
            {
                EditorRichTextBox.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, FontFamilyComboBox.SelectedItem);
            }
        }

        private void FontSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FontSizeComboBox.SelectedItem != null && !EditorRichTextBox.Selection.IsEmpty)
            {
                ComboBoxItem selectedItem = FontSizeComboBox.SelectedItem as ComboBoxItem;
                if (selectedItem != null && double.TryParse(selectedItem.Content.ToString(), out double size))
                    EditorRichTextBox.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, size);
            }
        }

        private void EditorRichTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            object fontBold = EditorRichTextBox.Selection.GetPropertyValue(Inline.FontWeightProperty);
            BoldToggleButton.IsChecked = (fontBold != DependencyProperty.UnsetValue) && (fontBold.Equals(FontWeights.Bold));

            object fontItalic = EditorRichTextBox.Selection.GetPropertyValue(Inline.FontStyleProperty);
            ItalicToggleButton.IsChecked = (fontItalic != DependencyProperty.UnsetValue) && (fontItalic.Equals(FontStyles.Italic));

            object fontUnderline = EditorRichTextBox.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            UnderlineToggleButton.IsChecked = (fontUnderline != DependencyProperty.UnsetValue) && (fontUnderline.Equals(TextDecorations.Underline));

            object fontFamily = EditorRichTextBox.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            FontFamilyComboBox.SelectedItem = fontFamily;

            object fontSize = EditorRichTextBox.Selection.GetPropertyValue(TextElement.FontSizeProperty);

            if (fontSize != DependencyProperty.UnsetValue)
            {
                foreach (ComboBoxItem item in FontSizeComboBox.Items)
                {
                    if (item.Content.ToString() == fontSize.ToString())
                    {
                        FontSizeComboBox.SelectedItem = item;
                        break;
                    }
                }
            }


            object fontColor = EditorRichTextBox.Selection.GetPropertyValue(TextElement.ForegroundProperty);
            FontColorComboBox.SelectedItem = fontColor;
        }

        private void EditorRichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = new TextRange(EditorRichTextBox.Document.ContentStart, EditorRichTextBox.Document.ContentEnd).Text;
            int wordCount = Regex.Matches(text, @"\b\p{L}+\b").Count;
            WordCountTextBlock.Text = $"Words: {wordCount}";
        }

        private void FontColorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FontColorComboBox.SelectedItem != null)
            {
                Brush brush = (Brush)FontColorComboBox.SelectedItem;
                if (!EditorRichTextBox.Selection.IsEmpty)
                {
                    EditorRichTextBox.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, brush);
                }
            }
        }

        private bool ProcessAndAddCarData()
        {
            // Validacija polja
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Molimo unesite naziv automobila.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtYear.Text) || !int.TryParse(txtYear.Text, out int year))
            {
                MessageBox.Show("Molimo unesite ispravnu godinu izdanja.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(ImagePath)) 
            {
                MessageBox.Show("Molimo odaberite sliku automobila.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            TextRange range = new TextRange(EditorRichTextBox.Document.ContentStart, EditorRichTextBox.Document.ContentEnd);
            if (string.IsNullOrEmpty(range.Text.ToString().Trim()))
            {
                MessageBox.Show("Niste nista uneli za opis!", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            string rtfFolderPath = @"../../../RTFs/";

            string sanitizedCarName = XMLHelper.SanitizeFileName(txtName.Text); 
            string newRtfFileName = $"{sanitizedCarName}.rtf";
            string fullNewRtfPath = Path.Combine(rtfFolderPath, newRtfFileName);
            string relativeRtfPathToSave = Path.Combine(rtfFolderPath, newRtfFileName); 
 
            if (_mainCarsList.Any(c => c.Name.Equals(txtName.Text, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Automobil sa istim imenom već postoji! Molimo izaberite drugo ime.", "Greška pri dodavanju", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            try
            {
                using (FileStream fs = new FileStream(fullNewRtfPath, FileMode.Create))
                {
                    range.Save(fs, DataFormats.Rtf);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška prilikom čuvanja RTF fajla: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            

            Cars newCar = new Cars
            {
                Name = txtName.Text,
                RealseDate = year, 
                UrlImg = txtImage.Text, 
                UrlRtf = relativeRtfPathToSave, 
                Date = DateTime.Now.ToString("dd.MM.yyyy") 
            };

            _mainCarsList.Add(newCar);

            MessageBox.Show("Automobil je uspešno dodat!", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);
            return true; 
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (ProcessAndAddCarData()) 
            {
                serializer.SerializeObject(_mainCarsList, @"../../../Cars.xml");
                txtName.Text = string.Empty;
                txtYear.Text = string.Empty;
                ImagePath = string.Empty; 
                EditorRichTextBox.Document.Blocks.Clear();
                WordCountTextBlock.Text = string.Empty;

                if (NavigationService.CanGoBack)
                    NavigationService.GoBack();
                else
                    NavigationService.Navigate(new DataGridPage(_mainCarsList)); 
            }
        }
    }
}