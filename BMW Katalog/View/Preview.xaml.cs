using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BMW_Katalog.Model;
using BMWKatalog.Helpers;

namespace BMW_Katalog.View
{
    /// <summary>
    /// Interaction logic for Preview.xaml
    /// </summary>
    public partial class Preview : Window
    {
 
        public Preview(Cars selectedCar)
        {
            InitializeComponent();
            this.DataContext = selectedCar;

            if (!string.IsNullOrEmpty(selectedCar.UrlRtf))
            {
                try
                {

                    FlowDocument flowDoc = new FlowDocument();
                    TextRange textRange = new TextRange(flowDoc.ContentStart, flowDoc.ContentEnd);
                    using (var stream = new System.IO.FileStream(selectedCar.UrlRtf, System.IO.FileMode.Open))
                    {
                        textRange.Load(stream, DataFormats.Rtf);
                    }
                    EditorRichTextBox.Document = flowDoc;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading RTF document: " + ex.Message);
                }
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
