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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Major_project
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        BackendConnect Backend = new BackendConnect();
        MainWindow mainWindow;

        public bool dontclose = true;

        public Login(MainWindow window)
        {
            InitializeComponent();
            mainWindow = window;
            MoveTo1(Background1, -858, 256);
        }

        public static void MoveTo1(Image target, double newX, double newY)
        {
            Vector offset = VisualTreeHelper.GetOffset(target);
            var top = offset.Y;
            var left = offset.X;
            TranslateTransform trans = new TranslateTransform();
            target.RenderTransform = trans;
            DoubleAnimation anim1 = new DoubleAnimation(0, newY - top, TimeSpan.FromSeconds(80));
            DoubleAnimation anim2 = new DoubleAnimation(0, newX - left, TimeSpan.FromSeconds(80));
            trans.BeginAnimation(TranslateTransform.YProperty, anim1);
            trans.BeginAnimation(TranslateTransform.XProperty, anim2);
        }

        public bool CheckConnectionToServer()
        {
            if (Backend.Get(BackendConnect.server + "ping") == null)
            {
                login_error.Text = "Can't connect to server :(";
                return false;
            } else
            {
                return true;
            }
        }

        private void Enter_Login(object sender, KeyEventArgs e)
        {
            {
                if (e.Key == Key.Return)
                {
                    Try_Login();
                    e.Handled = true;
                }
            }
        }

        private void Click_Login(object sender, RoutedEventArgs e)
        {
            Try_Login();
        }

        private void Click_Register(object sender, RoutedEventArgs e)
        {
            Try_Register();
        }

        private async void Try_Login()
        {
            if (CheckConnectionToServer())
            {
                BackendConnect.Post_message_class data = new BackendConnect.Post_message_class()
                {
                    Username = text_username.Text,
                    Password = text_password.Password
                };
                string request = BackendConnect.server + "auth/login";
                var content = await Backend.Post(data, request);
                
                if(content != null)
                {
                    if (content[0].Id != null)
                    {
                        var user_id = content[0].Id;
                        Properties.Settings.Default.id = Int32.Parse(user_id);
                        Properties.Settings.Default.Save();
                        mainWindow.LoggedIn();
                        mainWindow.Show();
                        dontclose = false;
                        this.Close();
                    }
                    else
                    {
                        login_error.Text = "Username or Password is wrong";
                    }
                }
                else
                {
                    login_error.Text = "Username or Password is wrong";
                }
            }
        }

        private async void Try_Register()
        {
            if (CheckConnectionToServer())
            {

                var Username = text_username.Text;
                var Password = text_password.Password;

                if (Username == "" || Password == "")
                {
                    login_error.Text = "Please fill out both fields";
                    return;
                }

                if (Password.Length < 6)
                {
                    login_error.Text = "Please Make your password more than 6 characters";
                    return;
                }

                BackendConnect.Post_message_class data = new BackendConnect.Post_message_class()
                {
                    Username = Username,
                    Password = Password
                };
                string request = BackendConnect.server + "auth/Register";
                var content = await Backend.Post(data, request);

                if (content != null)
                {
                    if (content[0].Id != null)
                    {
                        var user_id = content[0].Id;
                        Properties.Settings.Default.id = Int32.Parse(user_id);
                        Properties.Settings.Default.Save();
                        mainWindow.LoggedIn();
                        mainWindow.Show();
                        dontclose = false;
                        this.Close();
                    }
                    else
                    {
                        login_error.Text = "Username or Password is wrong";
                    }
                }
                else
                {
                    login_error.Text = "Username or Password is wrong";
                }
            }
        }

        private void Closed_Login(object sender, EventArgs e)
        {
            if (dontclose)
            {
                Application.Current.Shutdown();
            }
        }

        private void Username_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox txtBox = sender as TextBox;
            if (txtBox.Text == "Enter Username Here...")
                txtBox.Text = string.Empty;
        }
    }
}
