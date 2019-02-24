using OK.PhoneBook.Models;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace OK.PhoneBook.Views
{
    public partial class VerifyPanel : UserControl
    {
        public event EventHandler VerificationSucceeded;
        public event EventHandler VerificationFailed;

        public VerifyPanel(EventHandler succeeded, EventHandler failed)
        {
            InitializeComponent();

            this.KeyUp += VerifyPanel_KeyUp;
            this.VerificationSucceeded = succeeded;
            this.VerificationFailed = failed;
        }

        private void VerifyPanel_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                if (txtPassword.IsFocused)
                    Login();
        }

        private void btnVerify_Clicked(object sender, EventArgs e)
        {
            Login();
        }

        private void btnCancel_Clicked(object sender, EventArgs e)
        {
            if (VerificationFailed != null)
                VerificationFailed(this, new EventArgs());
        }

        private void Login()
        {
            if (txtPassword.Password == Database.SavedData.Settings.AdminPassword)
            {
                if (VerificationSucceeded != null)
                    VerificationSucceeded(this, new EventArgs());
            }
            else
            {
                if (VerificationFailed != null)
                    VerificationFailed(this, new EventArgs());
            }
        }
    }
}
