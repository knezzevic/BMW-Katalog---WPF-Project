using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
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
using BMW_Katalog.View;
using BMW_Katalog.View.Pages;
using BMWKatalog.Helpers;

namespace BMW_Katalog
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    

    public partial class MainWindow : Window
    {
        private User _loggedUser;
        public ObservableCollection<Cars> ListOfCars { get; set; }
        Cars car {  get; set; }

        public MainWindow(User loggedUser)
        {
            InitializeComponent();
            _loggedUser = loggedUser;
            SetUserProfile(_loggedUser);
            LoadCars();
            Frame.Navigated += (s, e) => { btnColorAndFunction(); };
            this.Closing += MainWindow_Closing;
            Frame.Content = new DataGridPage(ListOfCars);
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            SaveCars();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void LoadCars()
        {
            string xmlPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Cars.xml");
            try
            {
                if (File.Exists(xmlPath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Cars>));
                    using (StreamReader reader = new StreamReader(xmlPath))
                    {
                        ListOfCars = new ObservableCollection<Cars>((List<Cars>)serializer.Deserialize(reader));
                    }
                }
                else
                {
                    ListOfCars = new ObservableCollection<Cars>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri učitavanju Cars.xml: {ex.Message}\nPutanja: {xmlPath}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                ListOfCars = new ObservableCollection<Cars>(); 
            }
        }

        private void SaveCars()
        {
            string xmlPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Cars.xml");
            XmlSerializer serializer = new XmlSerializer(typeof(List<Cars>));
            List<Cars> carsToSave = ListOfCars.ToList(); 

            try
            {
                using (StreamWriter writer = new StreamWriter(xmlPath))
                {
                    serializer.Serialize(writer, carsToSave);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri čuvanju Cars.xml: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void SetUserProfile(User user)
        {

            if (user.Username == "admin" && user.Password == "admin123")
            {
                FullNameUser.Text = "Nikola Knezevic";
                UsernameUser.Text = "@" + user.Username;
                ImageBrush imageBrush = new ImageBrush();
                imageBrush.ImageSource = new BitmapImage(new Uri("Assets/logo.png", UriKind.Relative));
                Pfp.Fill = imageBrush;
            }
            else if (user.Username == "fikilauda" && user.Password == "starwars123")
            {
                FullNameUser.Text = "Filip Djordjevic";
                UsernameUser.Text = "@" + user.Username;
                ImageBrush imageBrush = new ImageBrush();
                imageBrush.ImageSource = new BitmapImage(new Uri("Assets/filip.jpeg", UriKind.Relative));
                Pfp.Fill = imageBrush;
                btnAdd.Visibility = Visibility.Collapsed;
                btnEdit.Visibility = Visibility.Collapsed;
                btnRemove.Visibility = Visibility.Collapsed;
            }
            
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Frame.Content = new AddPage(ListOfCars, SaveCars);

        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.Content is DataGridPage currentPage)
            {
                currentPage.DeleteSelectedCars(ListOfCars);
                SaveCars();
            }
            else
            {
                MessageBox.Show("Nije moguće brisati sa ove stranice.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.Content is DataGridPage currentPage)
            {
                var selectedCar2 = ListOfCars.Where(c => c.CheckBox).ToList();
                if(selectedCar2.Count > 0)
                {
                    var selectedCar = currentPage.BMWDataGrid.SelectedItem as Cars; 
                    if (selectedCar != null)
                    {
                        Frame.Content = new EditPage(selectedCar, ListOfCars, SaveCars); 
                    }
                    
                }
                else
                {
                    MessageBox.Show("Niste izabrali nijedan automobil.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

            }
            else
            {
                MessageBox.Show("Molimo vas, pređite na DataGrid stranicu da biste izmenili automobil.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            SaveCars();
            LoginUser lgu = new LoginUser();
            lgu.Show();
            this.Close();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {

            if (Frame.CanGoBack)
                Frame.GoBack();
            else
                Frame.Content = new DataGridPage(ListOfCars);
        }

        private void btnColorAndFunction()
        {
            if (Frame.Content is AddPage addPage || Frame.Content is EditPage Page)
            {
                btnBack.Foreground = new SolidColorBrush(Colors.White);
                btnBack.IsEnabled = true;
            }
            else
            {
                btnBack.Foreground = new SolidColorBrush(Colors.Gray);
                btnBack.IsEnabled = false;
            }
        }
    }
}