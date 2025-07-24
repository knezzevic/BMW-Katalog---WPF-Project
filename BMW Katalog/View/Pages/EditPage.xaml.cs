using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using BMW_Katalog.Helpers;
using BMW_Katalog.Model;
using BMWKatalog.Helpers;
using Microsoft.Win32;

namespace BMW_Katalog.View.Pages
{

    public partial class EditPage : Page, INotifyPropertyChanged
    {
        private Cars _carToEdit;
        private string _imagePath;
        DataGridPage _dataGridPage;
        private string _originalCarName;
        private ObservableCollection<Cars> _mainCarsList;
        private Action _saveCarsCallback;
        private DataIO serializer = new DataIO();
        public string ImagePath
        {
            get => _imagePath;
            set
            {
                _imagePath = value;
                OnPropertyChanged(nameof(ImagePath));
            }
        }
        public EditPage(Cars car, ObservableCollection<Cars> mainCarsList, Action saveCarsCallback)
        {
            InitializeComponent();
            LoadSystemColors();
            FontFamilyComboBox.SelectedIndex = 51;
            FontSizeComboBox.SelectedIndex = 2;
            FontColorComboBox.SelectedIndex = 7;
            _saveCarsCallback = saveCarsCallback;
            _carToEdit = car;
            _originalCarName = _carToEdit.Name;
            _mainCarsList = mainCarsList;
            this.DataContext = _carToEdit;
            txtName.Text = _carToEdit.Name;
            txtYear.Text = _carToEdit.RealseDate.ToString();
            txtImage.Text = _carToEdit.UrlImg;

            if (!string.IsNullOrEmpty(_carToEdit.UrlImg))
            {
                try
                {
                    string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    string fullImagePath = System.IO.Path.Combine(baseDirectory, _carToEdit.UrlImg);

                    if (File.Exists(fullImagePath))
                    {
                        BitmapImage image = new BitmapImage();
                        image.BeginInit();
                        image.UriSource = new Uri(fullImagePath, UriKind.Absolute);
                        image.CacheOption = BitmapCacheOption.OnLoad; 
                        image.EndInit();
                        imgPreview.Source = image;
                    }
                    else
                    {
                        MessageBox.Show($"Slika nije pronađena na očekivanoj putanji: '{fullImagePath}'.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                        imgPreview.Source = null; 
                    }
                }
                catch (UriFormatException uriEx)
                {
                    MessageBox.Show($"Greška formata URI-ja za sliku: {uriEx.Message}\nPutanja: {_carToEdit.UrlImg}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    imgPreview.Source = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Greška pri učitavanju slike: {ex.Message}\nPutanja: {_carToEdit.UrlImg}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    imgPreview.Source = null;
                }
            }
            else
            {
                imgPreview.Source = null; 
            }

            if (!string.IsNullOrEmpty(_carToEdit.UrlRtf))
            {
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string fullRtfLoadPath = System.IO.Path.Combine(baseDirectory, _carToEdit.UrlRtf);

                if (File.Exists(fullRtfLoadPath))
                {
                    try
                    {
                        using (FileStream fs = new FileStream(fullRtfLoadPath, FileMode.Open, FileAccess.Read))
                        {
                            var range = new TextRange(EditorRichTextBox.Document.ContentStart, EditorRichTextBox.Document.ContentEnd);
                            range.Load(fs, DataFormats.Rtf);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Greška pri učitavanju RTF fajla '{fullRtfLoadPath}': {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show($"RTF fajl '{fullRtfLoadPath}' nije pronađen. Putanja je bila: '{_carToEdit.UrlRtf}'.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
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

        private void txtImage_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";

            if (ofd.ShowDialog() == true)
            {
                txtImage.Text = ofd.FileName;
                _carToEdit.UrlImg = ofd.FileName;

                if (File.Exists(ofd.FileName))
                {
                    imgPreview.Source = new BitmapImage(new Uri(ofd.FileName));
                }
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
            FontSizeComboBox.SelectedItem = fontSize;

            object fontColor = EditorRichTextBox.Selection.GetPropertyValue(TextElement.ForegroundProperty);
            FontColorComboBox.SelectedItem = fontColor;

        }

        private void EditorRichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = new TextRange(EditorRichTextBox.Document.ContentStart, EditorRichTextBox.Document.ContentEnd).Text;
            int wordCount = Regex.Matches(text, @"\b\p{L}+\b").Count;
            WordCountTextBlock.Text = $"Words: {wordCount}";
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


        private string SanitizeFileName(string fileName)
        {
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(c, '_'); 
            }
            return fileName;
        }

        public bool data()
        {
            bool temp = false;

            if (txtName.Text == string.Empty)
            {
                temp = true;
                MessageBox.Show("Prazno polje za ime!", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            if (txtImage.Text == string.Empty)
            {
                temp = true;
                MessageBox.Show("Prazno polje za unos slike!", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            if (txtYear.Text == string.Empty)
            {
                temp = true;
                MessageBox.Show("Prazno polje za datum!", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            if (!int.TryParse(txtYear.Text, out _))
            {
                MessageBox.Show("Uneli ste nesto sto nije broj!", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            TextRange range = new TextRange(EditorRichTextBox.Document.ContentStart, EditorRichTextBox.Document.ContentEnd);

            if (string.IsNullOrEmpty(range.Text.ToString().Trim()))
            {
                MessageBox.Show("Niste nista uneli za opis!", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                temp = true;
            }

            if (!temp)
            {

                string rtfFolderPath = @"../../../RTFs/"; 

                if (!Directory.Exists(rtfFolderPath))
                {
                    Directory.CreateDirectory(rtfFolderPath);
                }

                string newRtfFileName = SanitizeFileName(txtName.Text) + ".rtf";
                string fullNewRtfPath = System.IO.Path.Combine(rtfFolderPath, newRtfFileName); 

                string relativeRtfPathToSave = System.IO.Path.Combine(rtfFolderPath, newRtfFileName); 

                if (txtName.Text != _originalCarName)
                {
                    string oldRtfFileName = SanitizeFileName(_originalCarName) + ".rtf";
                    string fullOldRtfPath = System.IO.Path.Combine(rtfFolderPath, oldRtfFileName); 
                    if (File.Exists(fullOldRtfPath))
                    {
                        try { File.Delete(fullOldRtfPath); }
                        catch (Exception ex) { /* ... */ return false; }
                    }
                }

                try
                {
                    using (FileStream fs = new FileStream(fullNewRtfPath, FileMode.Create))
                    {
                        range = new TextRange(EditorRichTextBox.Document.ContentStart, EditorRichTextBox.Document.ContentEnd);
                        range.Save(fs, DataFormats.Rtf);
                    }
                }
                catch (Exception ex) { /* ... */ return false; }


                _carToEdit.Name = txtName.Text;
                _carToEdit.UrlImg = txtImage.Text; 
                _carToEdit.RealseDate = int.Parse(txtYear.Text);
                _carToEdit.UrlRtf = relativeRtfPathToSave; 

                return true;
            }
                

            return false;
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (data())
            {
                serializer.SerializeObject(_mainCarsList, @"../../../Cars.xml");
                if (NavigationService.CanGoBack)
                    NavigationService.GoBack();
                else
                    NavigationService.Navigate(new DataGridPage(_mainCarsList)); 
            }
        }

        
    }
}
