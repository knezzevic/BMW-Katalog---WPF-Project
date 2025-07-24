using System; // Dodajte ovo ako već nije
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO; // Obavezno!
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using BMW_Katalog.Helpers; 
using BMW_Katalog.Model;
using BMWKatalog.Helpers;
using static MaterialDesignThemes.Wpf.Theme; 

namespace BMW_Katalog.View.Pages
{
    public partial class DataGridPage : Page
    {
        public ObservableCollection<Cars> ListOfCars { get; set; }
        private DataIO serializer = new DataIO(); 
        Preview _pw { get; set; }

        public DataGridPage(ObservableCollection<Cars> carsList)
        {
            InitializeComponent();
            ListOfCars = carsList; 
            DataContext = this;
        }

        public List<Cars> GetSelectedCars()
        {
            return ListOfCars.Where(c => c.CheckBox).ToList();
        }

        public void DeleteSelectedCars(ObservableCollection<Cars> mainCarsList)
        {
            var selected = mainCarsList.Where(c => c.CheckBox).ToList();

            if (selected.Count == 0)
            {
                MessageBox.Show("Niste selektovali nijedan red.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            foreach (var car in selected)
            {
                string fullRtfPathToDelete = System.IO.Path.Combine(baseDirectory, car.UrlRtf);

                ListOfCars.Remove(car); 
                if (File.Exists(fullRtfPathToDelete))
                {
                    try
                    {
                        File.Delete(fullRtfPathToDelete);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Greška prilikom brisanja RTF fajla '{fullRtfPathToDelete}': {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                serializer.SerializeObject(ListOfCars, @"../../../Cars.xml");
            }
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Hyperlink hyperlink = sender as Hyperlink;
            if (hyperlink == null) return;

            var selectedCar = (hyperlink.DataContext as Cars);

            if (selectedCar != null)
            {
                Preview previewWindow = new Preview(selectedCar);
                previewWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Nije izabran automobil za pregled.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.CheckBox checkBox = (System.Windows.Controls.CheckBox)sender;
            if (checkBox.DataContext is Cars car)
            {
                car.CheckBox = checkBox.IsChecked == true;
               
            }
        }

    }
}