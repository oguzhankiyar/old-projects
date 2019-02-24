using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using BlockTetris.Entities;
using BlockTetris.Dialogs;
using BlockTetris.Panels;
using BlockTetris.Data;

namespace BlockTetris
{
    public sealed partial class GamePage : Page
    {
        public static Frame CurrentFrame { get; set; }

        public GamePage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            CurrentFrame = this.Frame;
            GamePanel.Create(LayoutRoot);
            ControlPanel.Create();
            Game.Init();
            Game.Current.Create();
            Game.Current.Start();
        }
    }
}
