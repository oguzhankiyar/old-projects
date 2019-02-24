using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Resources;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TicketApp.Controls;
using TicketApp.Controls.Enums;
using TicketApp.Models;
using TicketApp.Resources;
using TicketApp.UserControls;
using Windows.Phone.UI.Input;
using System.Linq;

namespace TicketApp
{
    public partial class App : Application
    {
        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public static PhoneApplicationFrame RootFrame { get; private set; }

        public static bool IsInternetAvailable
        {
            get
            {
                return !(Microsoft.Phone.Net.NetworkInformation.NetworkInterface.NetworkInterfaceType == Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.None);
            }
        }
        
        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions.
            UnhandledException += Application_UnhandledException;

            // Standard XAML initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();

            // Language display initialization
            InitializeLanguage();

            // Set the default uri mapper
            RootFrame.UriMapper = new AssociationUriMapper();

            // Select the dart theme
            ThemeManager.ToDarkTheme();

            // Auth biletall web service
            Biletall.Helper.Global.Authorize(Constraints.ApiUsername, Constraints.ApiPassword);

            // Initialize invoke action for multi-threading
            Biletall.Helper.Global.Invoke = (action) =>
            {
                RootFrame.Dispatcher.BeginInvoke(action);
            };

            Biletall.Helper.Global.OnRequestCalled = (name, parameters) =>
            {
                Logger.RequestCalled(name, parameters);
            };

            Biletall.Helper.Global.OnRequestReturned = (response) =>
            {
                Logger.RequestReturned(response);
            };

            Biletall.Helper.Global.OnResultParsed = (response) =>
            {
                Logger.ResultParsed(response);
            };

            Biletall.Helper.Global.OnRequestCompleted = () =>
            {
                Logger.RequestCompleted();
            };

            Biletall.Helper.Global.OnRequestCanceled = () =>
            {
                Logger.RequestCanceled();
            };

            Biletall.Helper.Global.OnRequestFailed = (exp) =>
            {
                Logger.RequestFailed(exp);
            };

            Biletall.Helper.Global.OnRequestParsed = (xml) =>
            {
                Logger.RequestParsed(xml);
            };

            RootFrame.BackKeyPress += (s, e) =>
            {
                Logger.WriteLine("back pressed");
                e.Cancel = true;
                App.BackPressed();
            };

            // Header back pressed event
            Header.BackPressed += (s, e) =>
            {
                Logger.WriteLine("header back clicked");
                App.BackPressed();
            };
            
            // Show graphics profiling information while debugging.
            if (Debugger.IsAttached)
            {
                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode,
                // which shows areas of a page that are handed off to GPU with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Prevent the screen from turning off while under the debugger by disabling
                // the application's idle detection.
                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }

        }
        
        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            Logger.WriteLine("app launched");
            Database.Init();
            // Launched function calls
            if (Database.SavedData.IsInstallationCompleted)
                Functions.UpdateStations();
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            if (!Database.SavedData.IsSentLogs && App.IsInternetAvailable)
                Logger.SendTodayLog();
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            Logger.WriteLine("app activated");
            Database.Init();
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
            Logger.WriteLine("app deactivated");
            Database.Update();
            Logger.SaveStoredLog();
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
            Logger.WriteLine("app closed");
            Database.Update();
            Logger.SaveStoredLog();
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            Logger.NavigationFailed(e.Exception);
            Database.Update();
            if (Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                Debugger.Break();
            }
            e.Handled = true;
            RootFrame.Navigate(new Uri("/Pages/MainPage.xaml", UriKind.RelativeOrAbsolute));
            App.ShowProgress("bir sorun oluştu, tekrar deneyiniz", ProgressType.Error, ProgressTime.Normal);
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            Logger.UnhandledException(e.ExceptionObject);
            Database.Update();
            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            }
            e.Handled = true;
            RootFrame.Navigate(new Uri("/Pages/MainPage.xaml", UriKind.RelativeOrAbsolute));
            App.ShowProgress("bir sorun oluştu, tekrar deneyiniz", ProgressType.Error, ProgressTime.Normal);
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += (s, e) =>
            {
                Logger.Navigated(e.Uri);
            };
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Handle reset requests for clearing the backstack
            RootFrame.Navigated += CheckForResetNavigation;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        private void CheckForResetNavigation(object sender, NavigationEventArgs e)
        {
            // If the app has received a 'reset' navigation, then we need to check
            // on the next navigation to see if the page stack should be reset
            if (e.NavigationMode == NavigationMode.Reset)
                RootFrame.Navigated += ClearBackStackAfterReset;
        }

        private void ClearBackStackAfterReset(object sender, NavigationEventArgs e)
        {
            // Unregister the event so it doesn't get called again
            RootFrame.Navigated -= ClearBackStackAfterReset;

            // Only clear the stack for 'new' (forward) and 'refresh' navigations
            if (e.NavigationMode != NavigationMode.New && e.NavigationMode != NavigationMode.Refresh)
                return;

            // For UI consistency, clear the entire page stack
            while (RootFrame.RemoveBackEntry() != null)
            {
                ; // do nothing
            }
        }

        #endregion

        // Initialize the app's font and flow direction as defined in its localized resource strings.
        //
        // To ensure that the font of your application is aligned with its supported languages and that the
        // FlowDirection for each of those languages follows its traditional direction, ResourceLanguage
        // and ResourceFlowDirection should be initialized in each resx file to match these values with that
        // file's culture. For example:
        //
        // AppResources.es-ES.resx
        //    ResourceLanguage's value should be "es-ES"
        //    ResourceFlowDirection's value should be "LeftToRight"
        //
        // AppResources.ar-SA.resx
        //     ResourceLanguage's value should be "ar-SA"
        //     ResourceFlowDirection's value should be "RightToLeft"
        //
        // For more info on localizing Windows Phone apps see http://go.microsoft.com/fwlink/?LinkId=262072.
        //
        private void InitializeLanguage()
        {
            try
            {
                // Set the font to match the display language defined by the
                // ResourceLanguage resource string for each supported language.
                //
                // Fall back to the font of the neutral language if the Display
                // language of the phone is not supported.
                //
                // If a compiler error is hit then ResourceLanguage is missing from
                // the resource file.
                RootFrame.Language = XmlLanguage.GetLanguage(AppResources.ResourceLanguage);

                // Set the FlowDirection of all elements under the root frame based
                // on the ResourceFlowDirection resource string for each
                // supported language.
                //
                // If a compiler error is hit then ResourceFlowDirection is missing from
                // the resource file.
                FlowDirection flow = (FlowDirection)Enum.Parse(typeof(FlowDirection), AppResources.ResourceFlowDirection);
                RootFrame.FlowDirection = flow;
            }
            catch
            {
                // If an exception is caught here it is most likely due to either
                // ResourceLangauge not being correctly set to a supported language
                // code or ResourceFlowDirection is set to a value other than LeftToRight
                // or RightToLeft.

                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }

                throw;
            }
        }

        #region Special Methods
        private static Stack<Action> _backPressedEventStack = new Stack<Action>();

        internal static void BackPressed()
        {
            if (_backPressedEventStack.Any())
            {
                var action = _backPressedEventStack.Peek();
                action();
            }
            else
            {
                var frame = Application.Current.RootVisual as PhoneApplicationFrame;
                if (frame.BackStack.Any())
                    frame.GoBack();
                else
                    Application.Current.Terminate();
            }
        }

        internal static void AddBackPressedEvent(Action action)
        {
            Logger.MethodCalled("App.AddBackPressedEvent(Action)");
            RemoveBackPressedEvent(action);
            _backPressedEventStack.Push(action);
        }

        internal static void RemoveBackPressedEvent(Action action)
        {
            Logger.MethodCalled("App.RemoveBackPressedEvent(Action)");
            if (_backPressedEventStack.Contains(action))
            {
                var result = _backPressedEventStack.Pop();
            }
        }

        internal static void ClearBackHistory()
        {
            Logger.MethodCalled("App.ClearBackHistory()");
            var backStackCount = RootFrame.BackStack.Count();
            for (int i = 0; i < backStackCount; i++)
            {
                RootFrame.RemoveBackEntry();
            }
        }

        public static void SetTitle(string title)
        {
            RootFrame.Dispatcher.BeginInvoke(() =>
            {
                Logger.HeaderChanged(title);
                Header.SetTitle(title);
            });
        }

        public static void ShowProgress(string text = null, ProgressType type = ProgressType.Loading, ProgressTime time = ProgressTime.Infinite, List<string> buttons = null)
        {
            RootFrame.Dispatcher.BeginInvoke(() =>
            {
                Logger.HeaderChanged(text);
                Header.ShowProgress(text, type, time, buttons);
            });
        }

        public static void HideProgress()
        {
            RootFrame.Dispatcher.BeginInvoke(() =>
            {
                Logger.MethodCalled("App.HideProgress()");
                Header.HideProgress();
            });
        }

        public static void DisablePage()
        {
            Logger.MethodCalled("App.DisablePage()");
            var page = RootFrame.Content as PhoneApplicationPage;
            var panel = page.Content;
            if (panel is Grid)
            {
                var disabledGrid = new Grid() { Name = "DisabledGrid", Background = new SolidColorBrush(Colors.Transparent) };
                Grid.SetRow(disabledGrid, 1);
                ((Grid)panel).Children.Add(disabledGrid);
                BackgroundAnimation(disabledGrid, Color.FromArgb(0x20, 0xFF, 0xFF, 0xFF));
            }
        }

        public static void EnablePage()
        {
            Logger.MethodCalled("App.EnablePage()");
            var page = RootFrame.Content as PhoneApplicationPage;
            var panel = page.Content;
            if (panel is Grid)
            {
                var disabledGrid = ((Grid)panel).FindName("DisabledGrid") as Grid;
                BackgroundAnimation(disabledGrid, Colors.Transparent).Completed += (s, e) =>
                {
                    ((Grid)panel).Children.Remove(disabledGrid);
                };
            }
        }

        private static Storyboard BackgroundAnimation(UIElement grid, Color color)
        {
            Logger.MethodCalled("App.BackgroundAnimation(UIElement, Color)");
            var d = new ColorAnimation() { To = color, Duration = TimeSpan.FromMilliseconds(250) };
            var s = new Storyboard();
            Storyboard.SetTarget(d, grid);
            Storyboard.SetTargetProperty(d, new PropertyPath("(Grid.Background).(SolidColorBrush.Color)"));
            s.Children.Add(d);
            s.Begin();
            return s;
        }
        #endregion
    }
}