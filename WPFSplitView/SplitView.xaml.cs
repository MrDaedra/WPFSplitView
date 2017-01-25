using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFSplitView
{
    [TemplateVisualState(GroupName = "DisplayModeStates", Name = "Closed")]
    [TemplateVisualState(GroupName = "DisplayModeStates", Name = "ClosedCompactLeft")]
    [TemplateVisualState(GroupName = "DisplayModeStates", Name = "ClosedCompactRight")]
    [TemplateVisualState(GroupName = "DisplayModeStates", Name = "OpenOverlayLeft")]
    [TemplateVisualState(GroupName = "DisplayModeStates", Name = "OpenOverlayRight")]
    [TemplateVisualState(GroupName = "DisplayModeStates", Name = "OpenInlineLeft")]
    [TemplateVisualState(GroupName = "DisplayModeStates", Name = "OpenInlineRight")]
    [TemplateVisualState(GroupName = "DisplayModeStates", Name = "OpenCompactOverlayLeft")]
    [TemplateVisualState(GroupName = "DisplayModeStates", Name = "OpenCompactOverlayRight")]
    [TemplatePart(Name = "LightDismissLayer", Type = typeof(UIElement))]
    [DefaultProperty("Content")]
    [ContentProperty("Content")]
    public partial class SplitView : Control
    {        
        public SplitView()
        {
            InitializeComponent();              
        }

        public override void OnApplyTemplate()
        {
            TemplateSettings = new SplitViewTemplateSettings(this);

            base.OnApplyTemplate();

            UIElement lightDismissLayer;

            lightDismissLayer = GetTemplateChild("LightDismissLayer") as UIElement;
            if (lightDismissLayer != null)
            {
                lightDismissLayer.PreviewTouchDown += OnLightDismiss;
                lightDismissLayer.PreviewStylusDown += OnLightDismiss;
                lightDismissLayer.PreviewMouseDown += OnLightDismiss;
            }

            Control proxy = GetTemplateChild("proxy") as Control;
        }

        protected virtual void OnLightDismiss()
        {
            if (LightDismissOverlayMode == LightDismissOverlayMode.On || LightDismissOverlayMode == LightDismissOverlayMode.Auto)
            {
                if (IsPaneOpen && !IsInline)
                    IsPaneOpen = false;
            }            
        }

        private void OnLightDismiss(object sender, MouseButtonEventArgs e)
        {
            OnLightDismiss();
        }

        private void OnLightDismiss(object sender, StylusDownEventArgs e)
        {
            OnLightDismiss();
        }

        private void OnLightDismiss(object sender, TouchEventArgs e)
        {
            OnLightDismiss();
        }

        public UIElement Pane
        {
            get { return (UIElement)GetValue(PaneProperty); }
            set { SetValue(PaneProperty, value); }
        }
        
        public static readonly DependencyProperty PaneProperty = 
            DependencyProperty.Register("Pane", typeof(UIElement), typeof(SplitView), new PropertyMetadata(null));



        public UIElement Content
        {
            get { return (UIElement)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }
        
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(UIElement), typeof(SplitView), new PropertyMetadata(null));



        public double CompactPaneLength
        {
            get { return (double)GetValue(CompactPaneLengthProperty); }
            set { SetValue(CompactPaneLengthProperty, value); }
        }
        
        public static readonly DependencyProperty CompactPaneLengthProperty =
            DependencyProperty.Register("CompactPaneLength", typeof(double), typeof(SplitView), new PropertyMetadata((double)48, OnLengthChanged));

        private static void OnLengthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SplitView sv = d as SplitView;
            sv.OnLengthChanged();
        }

        private void OnLengthChanged()
        {
            TemplateSettings = new SplitViewTemplateSettings(this);
        }

        public SplitViewDisplayMode DisplayMode
        {
            get { return (SplitViewDisplayMode)GetValue(DisplayModeProperty); }
            set { SetValue(DisplayModeProperty, value); }
        }
        
        public static readonly DependencyProperty DisplayModeProperty =
            DependencyProperty.Register("DisplayMode", typeof(SplitViewDisplayMode), typeof(SplitView), new PropertyMetadata(SplitViewDisplayMode.Overlay,OnVisualStateChanged));
                
        public bool IsPaneOpen
        {
            get { return (bool)GetValue(IsPaneOpenProperty); }
            set { SetValue(IsPaneOpenProperty, value); }
        }
        
        public static readonly DependencyProperty IsPaneOpenProperty =
            DependencyProperty.Register("IsPaneOpen", typeof(bool), typeof(SplitView), new PropertyMetadata(false, OnIsPaneOpenChanged));

        public LightDismissOverlayMode LightDismissOverlayMode
        {
            get { return (LightDismissOverlayMode)GetValue(LightDismissOverlayModeProperty); }
            set { SetValue(LightDismissOverlayModeProperty, value); }
        }
        
        public static readonly DependencyProperty LightDismissOverlayModeProperty =
            DependencyProperty.Register("LightDismissOverlayMode", typeof(LightDismissOverlayMode), typeof(SplitView), new PropertyMetadata(LightDismissOverlayMode.Auto));
                
        public double OpenPaneLength
        {
            get { return (double)GetValue(OpenPaneLengthProperty); }
            set { SetValue(OpenPaneLengthProperty, value); }
        }
        
        public static readonly DependencyProperty OpenPaneLengthProperty =
            DependencyProperty.Register("OpenPaneLength", typeof(double), typeof(SplitView), new PropertyMetadata((double)320, OnLengthChanged));
        
        public Brush PaneBackground
        {
            get { return (Brush)GetValue(PaneBackgroundProperty); }
            set { SetValue(PaneBackgroundProperty, value); }
        }
        
        public static readonly DependencyProperty PaneBackgroundProperty =
            DependencyProperty.Register("PaneBackground", typeof(Brush), typeof(SplitView), new PropertyMetadata(Brushes.DimGray));
        
        public SplitViewPanePlacement PanePlacement
        {
            get { return (SplitViewPanePlacement)GetValue(PanePlacementProperty); }
            set { SetValue(PanePlacementProperty, value); }
        }
        
        public static readonly DependencyProperty PanePlacementProperty =
            DependencyProperty.Register("PanePlacement", typeof(SplitViewPanePlacement), typeof(SplitView), new PropertyMetadata(SplitViewPanePlacement.Left,OnVisualStateChanged));

        private static void OnIsPaneOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SplitView sv = d as SplitView;
            sv.OnIsPaneOpenChanged((bool)e.NewValue);
        }

        protected virtual void OnIsPaneOpenChanged(bool newValue)
        {
            if (newValue)
            {
                OnVisualStateChanged();
            }
            else
            {
                if (PaneClosing != null)
                {
                    SplitViewPaneClosingEventArgs args = new SplitViewPaneClosingEventArgs();
                    foreach (TypedEventHandler<SplitView,SplitViewPaneClosingEventArgs> handler in PaneClosing.GetInvocationList())
                    {
                        handler(this, args);
                        if (args.Cancel)
                        {
                            IsPaneOpen = true;
                            return;
                        }
                    }
                }
                OnVisualStateChanged();
                PaneClosed?.Invoke(this, null);
            }
        }

        private static void OnVisualStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SplitView sv = d as SplitView;
            sv.OnVisualStateChanged();
        }

        protected virtual void OnVisualStateChanged(bool useTransitions = true)
        {
            VisualStateManager.GoToState(this, GetVisualState(), useTransitions);
        }

        bool IsCompact
        {
            get
            {
                if (DisplayMode == SplitViewDisplayMode.CompactInline || DisplayMode == SplitViewDisplayMode.CompactOverlay)
                    return true;
                return false;
            }
        }

        bool IsInline
        {
            get
            {
                if (DisplayMode == SplitViewDisplayMode.CompactInline || DisplayMode == SplitViewDisplayMode.Inline)
                    return true;
                return false;
            }
        }

        protected virtual string GetVisualState()
        {
            string state = string.Empty;
            if (IsPaneOpen)
            {
                state = "Open";
                state += IsInline ? "Inline" : DisplayMode.ToString();
            }
            else
            {
                state = "Closed";
                if (IsCompact)
                    state += "Compact";
                else
                    return state;
            }
            state += PanePlacement.ToString();
            return state;
        }

        public SplitViewTemplateSettings TemplateSettings
        {
            get { return (SplitViewTemplateSettings)GetValue(TemplateSettingsProperty); }
            set { SetValue(TemplateSettingsProperty, value); }
        }
        
        public static readonly DependencyProperty TemplateSettingsProperty =
            DependencyProperty.Register("TemplateSettings", typeof(SplitViewTemplateSettings), typeof(SplitView), new PropertyMetadata());

        public event TypedEventHandler<SplitView, object> PaneClosed;
        public event TypedEventHandler<SplitView, SplitViewPaneClosingEventArgs> PaneClosing;
    }
}
