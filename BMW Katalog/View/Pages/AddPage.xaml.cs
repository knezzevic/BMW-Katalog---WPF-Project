using System;
using System.Collections.Generic;
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
using BMW_Katalog.Helpers;
using BMW_Katalog.Model;
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

        DataGridPage _dataGridPage { get; set; }
        public AddPage()
        {
            InitializeComponent();
            DataContext = this;
            LoadSystemColors();
            FontFamilyComboBox.SelectedIndex = 51;
            FontSizeComboBox.SelectedIndex = 2;
            FontColorComboBox.SelectedIndex = 7;
            _dataGridPage = new DataGridPage();
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

        public Cars data()
        {
            bool temp = false;
            Cars car = null;

            if (txtName.Text == string.Empty)
            {
                temp = true;
                MessageBox.Show("Prazno polje");
            }

            if (txtImage.Text == string.Empty)
            {
                temp = true;
                MessageBox.Show("Prazno polje");
            }

            if (txtYear.Text == string.Empty)
            {
                temp = true;
                MessageBox.Show("Prazno polje");
            }

            if (!int.TryParse(txtYear.Text, out _))
            {
                MessageBox.Show("Nije broj");
            }

            TextRange range = new TextRange(EditorRichTextBox.Document.ContentStart, EditorRichTextBox.Document.ContentEnd);

            if (string.IsNullOrEmpty(range.Text.ToString().Trim()))
            {
                MessageBox.Show("Prazno polje");
                temp = true;
            }

            if (!temp)
            {
                string path = $"D:\\BMW Katalog\\BMW-Katalog---WPF-Project\\BMW Katalog\\RTFs\\{txtName.Text}.rtf";

                if (!File.Exists(path))
                {
                    using (FileStream fs = new FileStream(path, FileMode.Create))
                    {
                        range.Save(fs, DataFormats.Rtf);
                    }

                    car = new Cars(txtName.Text, Convert.ToInt32(txtYear.Text), txtImage.Text, path);
                    XMLHelper.DodajAuto(car);
                }
            }
            
            return car;
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            data();
            this.NavigationService.Navigate(new DataGridPage());
        }
    }
}
