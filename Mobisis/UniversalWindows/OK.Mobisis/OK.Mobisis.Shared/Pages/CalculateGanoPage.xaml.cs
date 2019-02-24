using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using OK.Mobisis.Data;
using OK.Mobisis.Entities.ModelEntities;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class CalculateGanoPage : Page
    {
        private GanoHelper ganoHelper = new GanoHelper();

        public CalculateGanoPage()
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
                tbGANO.Text = Database.TempData.Student.Programs[0].GANO.ToString("0.00");
                var program = Database.TempData.Student.Programs[0];
                ganoHelper.Program = program;
                var period = program.Periods.FirstOrDefault();

                if (period == null)
                {
                    this.LayoutRoot.Children.Add(new MessageControl("Henüz ders almıyorsunuz.."));
                    return;
                }

                foreach (var lesson in period.Lessons.Clone())
	            {
                    this.LessonsPanel.Children.Add(ganoHelper.GetLessonPanel(lesson));
	            }
            }
        }

        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            double newGano = ganoHelper.Calculate();
            tbGANO.Text = newGano.ToString("0.00");
        }
    }
}
