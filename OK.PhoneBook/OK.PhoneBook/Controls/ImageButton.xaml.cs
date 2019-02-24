using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OK.PhoneBook.Controls
{
    /// <summary>
    /// Interaction logic for ImageButton.xaml
    /// </summary>
    public partial class ImageButton : UserControl
    {
        public static DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(ImageSource), typeof(ImageButton), new PropertyMetadata(null, OnSourcePropertyChanged));
        private static void OnSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ImageButton).btnLayout.Background = new ImageBrush((ImageSource)e.NewValue);
        }
        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static DependencyProperty IsActiveProperty =
            DependencyProperty.Register("IsActive", typeof(bool), typeof(ImageButton), new PropertyMetadata(false, OnIsActivePropertyChanged));
        private static void OnIsActivePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == true)
            {
                (d as ImageButton).btnLayout.Opacity = .9;
                (d as ImageButton).btnLayout.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x36, 0x36, 0x36));
            }
            else
            {
                (d as ImageButton).btnLayout.Opacity = .75;
                (d as ImageButton).btnLayout.Foreground = new SolidColorBrush(Colors.Transparent);
            }
        }
        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        public event EventHandler Clicked;

        public ImageButton()
        {
            InitializeComponent();
            btnLayout.Click += btnLayout_Click;
        }

        private void btnLayout_Click(object sender, RoutedEventArgs e)
        {
            if (Clicked != null)
                Clicked(this, new EventArgs());
        }
    }
}
