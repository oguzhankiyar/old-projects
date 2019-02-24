using OK.PhoneBook.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OK.PhoneBook.Views
{
    public partial class LoginPanel : UserControl
    {
        public LoginPanel()
        {
            InitializeComponent();

            if (MainWindow.IsLogin)
                MainWindow.SetContent(new HomePanel());

            this.KeyUp += LoginPanel_KeyUp;
            txtUsername.TextChanged += (s, e) => { txtWarning.Text = ""; };
            txtPassword.PasswordChanged += (s, e) => { txtWarning.Text = ""; };
            txtAdminPassword.PasswordChanged += (s, e) => { txtWarning.Text = ""; };
        }

        private void LoginPanel_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                if (txtUsername.IsFocused || txtPassword.IsFocused || txtAdminPassword.IsFocused)
                    Login();
        }

        private void btnSend_Clicked(object sender, EventArgs e)
        {
            Login();
        }

        private void btnCancel_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainWindow.Close();
        }

        private void Login()
        {
            if (txtUsername.Text == Database.SavedData.Settings.Username && txtPassword.Password == Database.SavedData.Settings.Password)
            {
                if (!string.IsNullOrEmpty(txtAdminPassword.Password) &&
                    Database.SavedData.Settings.AdminPassword == txtAdminPassword.Password)
                    MainWindow.IsAdmin = true;
                MainWindow.IsLogin = true;
                MainWindow.SetContent(new HomePanel());
            }
            else
            {
                txtWarning.Text = "Kullanıcı adı ve şifre uyuşmuyor!";
            }
        }
    }
}
