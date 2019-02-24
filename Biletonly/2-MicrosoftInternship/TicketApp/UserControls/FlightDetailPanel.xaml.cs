
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using Biletall.Helper.Entities;
using Biletall.Helper.Requests;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TicketApp.Models;
using TicketApp.Pages.Airplane;

namespace TicketApp.UserControls
{
    public partial class FlightDetailPanel : UserControl
    {
        private Segment _segment;
        List<RadioButton> _buttonList = new List<RadioButton>();
        RadioButton _selectedButton;

        public FlightDetailPanel()
        {
            InitializeComponent();
            this.Loaded += (c, e) =>
            {
                _segment = this.DataContext as Segment;
                SetGroupName(ClassList);

                if (!_segment.Classes.Any())
                    ClassList.Visibility = Visibility.Collapsed;

                if (_segment.SelectedClass != null)
                {
                    var selectedClass = _segment.SelectedClass;
                    ClassName.Value = selectedClass.Name;
                    if (JourneyDetailsPage.OnClassesChecked != null)
                        JourneyDetailsPage.OnClassesChecked();
                }
                else if (_segment != null && _segment.SelectedClass != null)
                {
                    int index = _segment.Classes.IndexOf(_segment.Classes.SingleOrDefault(x => x.Name == _segment.SelectedClass.Name));
                    GetListBoxItems(ClassList)[index].IsChecked = true;
                }
            };
        }
        
        private void rbClass_Checked(object sender, RoutedEventArgs e)
        {
            var source = e.OriginalSource as RadioButton;
            _segment.SelectedClass = source.Tag as JourneyClass;

            if (_selectedButton != source && JourneyRequests.PriceDetailsRequest.IsCompleted)
            {
                if (Database.TempData.Ticket.Journeys.Count(x => x.Segments.Any(y => y.SelectedClass == null)) == 0 && JourneyDetailsPage.OnClassesChecked != null)
                {
                    JourneyDetailsPage.OnClassesChecked();
                }

                ClassName.Value = _segment.SelectedClass.Name;
                _selectedButton = source;
            }
            else if (_selectedButton != null)
                _selectedButton.IsChecked = true;
        }

        private List<RadioButton> GetListBoxItems(DependencyObject targetElement)
        {
            var count = VisualTreeHelper.GetChildrenCount(targetElement);

            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(targetElement, i);
                if (child is RadioButton)
                    _buttonList.Add((RadioButton)child);
                else
                    GetListBoxItems(child);
            }
            return _buttonList;
        }

        private void SetGroupName(DependencyObject targetElement)
        {
            var count = VisualTreeHelper.GetChildrenCount(targetElement);

            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(targetElement, i);
                if (child is RadioButton)
                    ((RadioButton)child).GroupName = _segment.Id + "_Class";
                else
                    SetGroupName(child);
            }
        }
    }
}
