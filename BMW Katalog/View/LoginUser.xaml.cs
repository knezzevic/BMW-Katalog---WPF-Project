﻿using System;
using System.Collections.Generic;
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
using BMW_Katalog.Helpers;
using BMW_Katalog.Model;
using MaterialDesignThemes.Wpf;

namespace BMW_Katalog.View
{
    /// <summary>
    /// Interaction logic for LoginUser.xaml
    /// </summary>
    public partial class LoginUser : Window
    {
        public LoginUser()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }

        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            Login();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Login();
            }
        }

        private void Login()
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Unesite korisničko ime i lozinku!","Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var users = UserRepository.LoadUsers();
            var matchedUser = users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (matchedUser != null)
            {
                if (matchedUser.Role == UserRole.Admin)
                {
                    User user = new User
                    {
                        Username = username,
                        Password = password
                    };

                    MainWindow mw = new MainWindow(user);
                    mw.Show();
                    this.Close();

                }
                else if (matchedUser.Role == UserRole.Visitor)
                {
                    User user = new User
                    {
                        Username = username,
                        Password = password
                    };

                    MainWindow mw = new MainWindow(user);
                    mw.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Greska! Nije dobar unos lozinke ili korisnickog imena!", "Greska", MessageBoxButton.OK);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Uneta pogresna lozinka ili korisnicko ime!", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
    
}
