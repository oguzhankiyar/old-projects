using OK.PhoneBook.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace OK.PhoneBook.Views
{
    public partial class RecordsPanel : UserControl
    {
        public RecordsPanel()
        {
            InitializeComponent();

            pnlDetails.Visibility = Visibility.Collapsed;
            
            this.Loaded += RecordsPanel_Loaded;
            this.SizeChanged += RecordsPanel_SizeChanged;
        }

        private void RecordsPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            pnlDetails.MinWidth = Math.Max(250, this.ActualWidth * 0.25);
        }

        private void RecordsPanel_Loaded(object sender, RoutedEventArgs e)
        {
            txtFirstName.TextChanged += txtFirstName_TextChanged;
            txtLastName.TextChanged += txtLastName_TextChanged;
            txtInstitution.TextChanged += txtInstitution_TextChanged;
            
            lsRecords.SelectedItem = null;
            lsRecords.SelectionChanged += lsRecords_SelectionChanged;

            var items = Database.SavedData.Records.OrderBy(x => x.FirstName).ThenBy(x => x.LastName);

            lsRecords.ItemsSource = null;
            lsRecords.ItemsSource = items;

            txtWarning.Visibility = items.Any() ? Visibility.Collapsed : Visibility.Visible;
        }

        private void lsRecords_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var record = lsRecords.SelectedItem as People;
            if (record != null)
            {
                pnlDetails.Visibility = Visibility.Visible;
                pnlDetails.DataContext = null;
                pnlDetails.DataContext = record;

                if (Database.SavedData.Favorites.Any(x => x == record.Id))
                {
                    btnAddToFavorites.Visibility = Visibility.Collapsed;
                    btnRemoveFromFavorites.Visibility = Visibility.Visible;
                }
                else
                {
                    btnAddToFavorites.Visibility = Visibility.Visible;
                    btnRemoveFromFavorites.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                pnlDetails.Visibility = Visibility.Collapsed;
            }
        }

        private void txtFirstName_TextChanged(object sender, TextChangedEventArgs e)
        {
            var items = Database.SavedData.Records.Where(x =>
                                                    (string.IsNullOrEmpty(txtFirstName.Text) || x.FirstName.ToLower().Contains(txtFirstName.Text.ToLower())) &&
                                                    (string.IsNullOrEmpty(txtLastName.Text) || x.LastName.ToLower().Contains(txtLastName.Text.ToLower())) &&
                                                    (string.IsNullOrEmpty(txtInstitution.Text) || x.Institution.ToLower().Contains(txtInstitution.Text.ToLower())))
                                                    .OrderBy(x => x.FirstName).ThenBy(x => x.LastName);

            lsRecords.ItemsSource = null;
            lsRecords.ItemsSource = items;

            lsRecords.SelectedItem = null;

            if (items.Any())
            {
                txtWarning.Visibility = Visibility.Collapsed;
            }
            else
            {
                txtWarning.Text = "Aramanıza uygun kayıt bulunamadı!";
                txtWarning.Visibility = Visibility.Visible;
            }
        }

        private void txtLastName_TextChanged(object sender, TextChangedEventArgs e)
        {
            var items = Database.SavedData.Records.Where(x =>
                                                    (string.IsNullOrEmpty(txtFirstName.Text) || x.FirstName.ToLower().Contains(txtFirstName.Text.ToLower())) &&
                                                    (string.IsNullOrEmpty(txtLastName.Text) || x.LastName.ToLower().Contains(txtLastName.Text.ToLower())) &&
                                                    (string.IsNullOrEmpty(txtInstitution.Text) || x.Institution.ToLower().Contains(txtInstitution.Text.ToLower())))
                                                    .OrderBy(x => x.FirstName).ThenBy(x => x.LastName);

            lsRecords.ItemsSource = null;
            lsRecords.ItemsSource = items;

            lsRecords.SelectedItem = null;

            if (items.Any())
            {
                txtWarning.Visibility = Visibility.Collapsed;
            }
            else
            {
                txtWarning.Text = "Aramanıza uygun kayıt bulunamadı!";
                txtWarning.Visibility = Visibility.Visible;
            }
        }

        private void txtInstitution_TextChanged(object sender, TextChangedEventArgs e)
        {
            var items = Database.SavedData.Records.Where(x =>
                                                    (string.IsNullOrEmpty(txtFirstName.Text) || x.FirstName.ToLower().Contains(txtFirstName.Text.ToLower())) &&
                                                    (string.IsNullOrEmpty(txtLastName.Text) || x.LastName.ToLower().Contains(txtLastName.Text.ToLower())) &&
                                                    (string.IsNullOrEmpty(txtInstitution.Text) || x.Institution.ToLower().Contains(txtInstitution.Text.ToLower())))
                                                    .OrderBy(x => x.FirstName).ThenBy(x => x.LastName);

            lsRecords.ItemsSource = null;
            lsRecords.ItemsSource = items;

            lsRecords.SelectedItem = null;

            if (items.Any())
            {
                txtWarning.Visibility = Visibility.Collapsed;
            }
            else
            {
                txtWarning.Text = "Aramanıza uygun kayıt bulunamadı!";
                txtWarning.Visibility = Visibility.Visible;
            }
        }
        
        private void btnCreateRecord_Clicked(object sender, EventArgs e)
        {
            MainWindow.SetContent(new CreateRecordPanel());
        }

        private void btnAddToFavorites_Clicked(object sender, EventArgs args)
        {
            if (MainWindow.IsAdmin)
            {
                AddToFavorites();
                return;
            }
            var verifyPanel = new VerifyPanel((s, e) =>
            {
                AddToFavorites();
            }, (s, e) =>
            {
                MainWindow.SetContent(this);
            });
            MainWindow.SetContent(verifyPanel);
        }

        private void AddToFavorites()
        {
            var people = lsRecords.SelectedItem as People;
            Database.SavedData.Favorites.Add(people.Id);
            Database.Save();

            btnAddToFavorites.Visibility = Visibility.Collapsed;
            btnRemoveFromFavorites.Visibility = Visibility.Visible;

            MainWindow.SetContent(this);
        }

        private void btnRemoveFromFavorites_Clicked(object sender, EventArgs args)
        {
            if (MainWindow.IsAdmin)
            {
                RemoveFromFavorites();
                return;
            }
            var verifyPanel = new VerifyPanel((s, e) =>
            {
                RemoveFromFavorites();
            }, (s, e) =>
            {
                MainWindow.SetContent(this);
            });
            MainWindow.SetContent(verifyPanel);
        }

        private void RemoveFromFavorites()
        {
            var people = lsRecords.SelectedItem as People;
            Database.SavedData.Favorites.RemoveAll(x => x == people.Id);
            Database.Save();

            btnAddToFavorites.Visibility = Visibility.Visible;
            btnRemoveFromFavorites.Visibility = Visibility.Collapsed;

            MainWindow.SetContent(this);
        }

        private void btnEdit_Clicked(object sender, EventArgs e)
        {
            var people = lsRecords.SelectedItem as People;
            MainWindow.SetContent(new CreateRecordPanel(people));
        }

        private void btnRemove_Clicked(object sender, EventArgs args)
        {
            if (MainWindow.IsAdmin)
            {
                Remove();
                return;
            }
            var verifyPanel = new VerifyPanel((s, e) =>
            {
                Remove();
            }, (s, e) =>
            {
                MainWindow.SetContent(this);
            });
            MainWindow.SetContent(verifyPanel);
        }

        private void Remove()
        {
            var people = lsRecords.SelectedItem as People;
            Database.SavedData.Records.RemoveAll(x => x.Id == people.Id);
            Database.SavedData.Favorites.RemoveAll(x => x == people.Id);
            Database.Save();

            var items = Database.SavedData.Records.OrderBy(x => x.FirstName).ThenBy(x => x.LastName);

            lsRecords.ItemsSource = null;
            lsRecords.ItemsSource = items;

            if (items.Any())
            {
                txtWarning.Visibility = Visibility.Collapsed;
            }
            else
            {
                txtWarning.Visibility = Visibility.Visible;
                txtWarning.Text = "Sisteme kayıtlı kayıt bulunamadı!";
            }

            MainWindow.SetContent(this);
        }
    }
}
