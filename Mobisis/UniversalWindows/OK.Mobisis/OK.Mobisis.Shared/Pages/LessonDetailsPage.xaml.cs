using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using OK.Mobisis.Api.Helper;
using OK.Mobisis.Api.Helper.Enums;
using OK.Mobisis.Api.Helper.Parsings;
using OK.Mobisis.Api.Helper.Requests;
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
    public sealed partial class LessonDetailsPage : Page
    {
        public LessonDetailsPage()
        {
            this.InitializeComponent();
            new SwipeMenu().AddTo(this);

            Prepare();
        }

        private async void Prepare()
        {
            if (Database.TempData.Student == null)
            {
                this.LayoutRoot.Children.Add(new LoginRequiredControl());
            }
            else
            {
                var lesson = Database.TempData.Lesson;
                if (lesson == null)
                {
                    await new MessageDialog("Bir sorun oluştu!").ShowAsync();

                    await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        this.Frame.Navigate(typeof(LessonListPage));
                    });
                }
                this.LayoutRoot.DataContext = Database.TempData.Lesson;
            }
        }
    }
}
