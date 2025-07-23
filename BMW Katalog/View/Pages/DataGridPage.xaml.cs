using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using BMW_Katalog.Model;
using BMWKatalog.Helpers;
using static MaterialDesignThemes.Wpf.Theme;

namespace BMW_Katalog.View.Pages
{
    /// <summary>
    /// Interaction logic for DataGridPage.xaml
    /// </summary>
    public partial class DataGridPage : Page
    {
        public ObservableCollection<Cars> ListOfCars { get; set; }
        private DataIO serializer = new DataIO();
        
        public DataGridPage()
        {
            InitializeComponent();
            ListOfCars = serializer.DeSerializeObject<ObservableCollection<Cars>>("D:\\BMW Katalog\\BMW-Katalog---WPF-Project\\BMW Katalog\\Cars.xml");
            DataContext = this;
            if(ListOfCars == null)
            {
                ListOfCars = new ObservableCollection<Cars>();
            }

        }

        public List<Cars> GetSelectedCars()
        {
            return ListOfCars.Where(c => c.CheckBox).ToList();
        }

        public void DeleteSelectedCars()
        {
            var selected = GetSelectedCars();

            if (selected.Count == 0)
            {
                MessageBox.Show("Niste selektovali nijedan red.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            foreach (var car in selected.ToList())
            {
                ListOfCars.Remove(car);
                File.Delete(car.UrlRtf);
            }

            serializer.SerializeObject(ListOfCars, "D:\\BMW Katalog\\BMW-Katalog---WPF-Project\\BMW Katalog\\Cars.xml");
        }


    }
}
