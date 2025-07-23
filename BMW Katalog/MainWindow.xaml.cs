using System.Collections.ObjectModel;
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
        DataGridPage _dataGridPage {  get; set; }
        AddPage _addPage { get; set; }
        public MainWindow(User loggedUser)
        {
            InitializeComponent();
            _loggedUser = loggedUser;
            SetUserProfile(_loggedUser);
            _dataGridPage = new DataGridPage();
            _addPage = new AddPage();
            Frame.Navigated += (s, e) => btnColorAndFunction();
            Frame.Content = _dataGridPage;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void SetUserProfile(User user)
        {

            if (user.Username == "admin" && user.Password == "admin123")
            {
                FullNameUser.Text = "Nikola Knezevic";
                UsernameUser.Text = "@" + user.Username;
                ImageBrush imageBrush = new ImageBrush();
                imageBrush.ImageSource = new BitmapImage(new Uri("D:\\BMW Katalog\\BMW-Katalog---WPF-Project\\BMW Katalog\\Assets\\logo.png", UriKind.Relative));
                Pfp.Fill = imageBrush;
            }
            else if (user.Username == "fikilauda" && user.Password == "starwars123")
            {
                FullNameUser.Text = "Filip Djordjevic";
                UsernameUser.Text = "@" + user.Username;
                ImageBrush imageBrush = new ImageBrush();
                imageBrush.ImageSource = new BitmapImage(new Uri("D:\\BMW Katalog\\BMW-Katalog---WPF-Project\\BMW Katalog\\Assets\\filip.jpeg", UriKind.Relative));
                Pfp.Fill = imageBrush;
                btnAdd.Visibility = Visibility.Collapsed;
                btnEdit.Visibility = Visibility.Collapsed;
                btnRemove.Visibility = Visibility.Collapsed;
            }
            
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Frame.Content = _addPage;
            
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            _dataGridPage.DeleteSelectedCars();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            LoginUser lgu = new LoginUser();
            lgu.Show();
            this.Close();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
           
            Frame.Content = _dataGridPage;
        }

        private void btnColorAndFunction()
        {
            if (Frame.Content is AddPage addPage)
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