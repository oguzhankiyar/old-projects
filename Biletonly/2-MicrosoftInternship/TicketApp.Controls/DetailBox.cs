using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace TicketApp.Controls
{
    public class DetailBox : Control
    {
        #region Members
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(DetailBox), null);
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(string), typeof(DetailBox), null);
        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(UIElement), typeof(DetailBox), null);
        public UIElement Content
        {
            get { return (UIElement)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public static readonly DependencyProperty ModeProperty =
            DependencyProperty.Register("Mode", typeof(HorizontalAlignment), typeof(DetailBox), null);
        public HorizontalAlignment Mode
        {
            get { return (HorizontalAlignment)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }

        public event EventHandler Clicked;

        private Grid _layoutRoot;
        #endregion

        #region Constructor
        public DetailBox()
        {
            this.Mode = HorizontalAlignment.Right;
            DefaultStyleKey = typeof(DetailBox);
        }
        #endregion

        #region Methods
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            
            _layoutRoot = GetTemplateChild("LayoutRoot") as Grid;
            if (Clicked != null)
                _layoutRoot.Tap += LayoutRoot_Tap;

            if (string.IsNullOrEmpty(Value) && Content != null)
            {
                var panel = GetTemplateChild("ContentPanel") as StackPanel;
                panel.Children.Add(Content);
            }
        }

        private void LayoutRoot_Tap(object sender, GestureEventArgs e)
        {
            VisualStateManager.GoToState(this, "Pressed", true);
            var visualGroup = VisualStateManager.GetVisualStateGroups(_layoutRoot)[0] as VisualStateGroup;
            visualGroup.CurrentState.Storyboard.Completed -= Storyboard_Completed;
            visualGroup.CurrentState.Storyboard.Completed += Storyboard_Completed;
        }

        private void Storyboard_Completed(object sender, EventArgs e)
        {
            VisualStateManager.GoToState(this, "Normal", true);
            if (Clicked != null)
                Clicked(sender, e);
        }
        #endregion
    }
}
