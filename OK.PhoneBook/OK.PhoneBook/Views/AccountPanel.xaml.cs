using OK.PhoneBook.Models;
using System;
using System.Windows.Controls;

namespace OK.PhoneBook.Views
{
    public partial class AccountPanel : UserControl
    {
        public AccountPanel()
        {
            InitializeComponent();
        }

        private void btnSend_Clicked(object sender, EventArgs e)
        {
            if (txtCurrentPassword.Password == Database.SavedData.Settings.Password)
            {
                if (txtNewPassword.Password != txtVerifyNewPassword.Password)
                    txtPasswordWarning.Text = "Yeni Şifre ile Yeni Şifre (Tekrar) uyuşmuyor!";
                else
                {
                    Database.SavedData.Settings.Username = txtUsername.Text;
                    Database.SavedData.Settings.Password = txtNewPassword.Password;
                    Database.Save();
                    
                    txtUsername.Text = "";
                    txtCurrentPassword.Password = "";
                    txtNewPassword.Password = "";
                    txtVerifyNewPassword.Password = "";
                    txtPasswordWarning.Text = "Şifreniz başarıyla değiştirildi.";
                }
            }
            else
                txtPasswordWarning.Text = "Geçerli Şifre alanını yanlış girdiniz!";
        }

        private void btnCancel_Clicked(object sender, EventArgs e)
        {
            txtPasswordWarning.Text = "";
            txtUsername.Text = "";
            txtCurrentPassword.Password = "";
            txtNewPassword.Password = "";
            txtVerifyNewPassword.Password = "";
        }

        private void btnAdminPasswordSend_Clicked(object sender, EventArgs e)
        {
            if (txtCurrentAdminPassword.Password == Database.SavedData.Settings.AdminPassword)
            {
                if (txtNewAdminPassword.Password != txtVerifyNewAdminPassword.Password)
                    txtAdminPasswordWarning.Text = "Yeni Şifre ile Yeni Şifre (Tekrar) uyuşmuyor!";
                else
                {
                    Database.SavedData.Settings.AdminPassword = txtNewAdminPassword.Password;
                    Database.Save();

                    txtCurrentAdminPassword.Password = "";
                    txtNewAdminPassword.Password = "";
                    txtVerifyNewAdminPassword.Password = "";
                    txtAdminPasswordWarning.Text = "Şifreniz başarıyla değiştirildi.";
                }
            }
            else
                txtAdminPasswordWarning.Text = "Geçerli Şifre alanını yanlış girdiniz!";
        }

        private void btnAdminPasswordCancel_Clicked(object sender, EventArgs e)
        {
            txtAdminPasswordWarning.Text = "";
            txtCurrentAdminPassword.Password = "";
            txtNewAdminPassword.Password = "";
            txtVerifyNewAdminPassword.Password = "";
        }
    }
}
