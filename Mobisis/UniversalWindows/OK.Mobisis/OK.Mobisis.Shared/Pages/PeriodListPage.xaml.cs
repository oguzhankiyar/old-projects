using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using OK.Mobisis.Api.Helper;
using OK.Mobisis.Data;
using OK.Mobisis.Entities.ModelEntities;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace OK.Mobisis.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PeriodListPage : Page
    {
        public PeriodListPage()
        {
            this.InitializeComponent();
            new SwipeMenu().AddTo(this);

            Prepare();
        }

        private void Prepare()
        {
            if (Database.TempData.Student == null)
            {
                this.LayoutRoot.Children.Add(new LoginRequiredControl());
            }
            else
            {
                var periods = Database.TempData.Student.Programs[0].Periods;
                if (!periods.Any())
                {
                    this.LayoutRoot.Children.Add(new MessageControl("Henüz bir döneme başlamadınız.."));
                    return;
                }

                this.PeriodList.ItemsSource = periods;
                this.PeriodList.SelectionChanged += PeriodList_SelectionChanged;
            }
        }

        private async void PeriodList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var period = PeriodList.SelectedItem as Period;
            if (period != null)
            {
                Database.TempData.Period = period;
                await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    this.Frame.Navigate(typeof(LessonListPage));
                });
            }
        }
    }
}
