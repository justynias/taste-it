using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;


namespace taste_it.Additionals.LoaderService
{
    public class WaitAdorner : ContentControl
    {
        /// <summary>
        /// Caches the adorner layer.
        /// </summary>
        private AdornerLayer _adornerLayer;

        /// <summary>
        /// The actual adorner create to contain our 'adorner UI content'.
        /// </summary>
        private FrameworkElementAdorner _adorner;

        /// <summary>
        /// Dependency properties.
        /// </summary>
        public static readonly DependencyProperty IsAdornerVisibleProperty =
                DependencyProperty.Register("IsAdornerVisible", typeof(bool), typeof(WaitAdorner),
                        new FrameworkPropertyMetadata(IsAdornerVisible_PropertyChanged));

        public static readonly DependencyProperty AdornerContentProperty =
                DependencyProperty.Register("AdornerContent", typeof(FrameworkElement), typeof(WaitAdorner),
                        new FrameworkPropertyMetadata(AdornerContent_PropertyChanged));

        public static readonly DependencyProperty HorizontalAdornerPlacementProperty =
                DependencyProperty.Register("HorizontalAdornerPlacement", typeof(AdornerPlacement), typeof(WaitAdorner),
                        new FrameworkPropertyMetadata(AdornerPlacement.Inside));

        public static readonly DependencyProperty VerticalAdornerPlacementProperty =
                DependencyProperty.Register("VerticalAdornerPlacement", typeof(AdornerPlacement), typeof(WaitAdorner),
                        new FrameworkPropertyMetadata(AdornerPlacement.Inside));

        public static readonly DependencyProperty AdornerOffsetXProperty =
                DependencyProperty.Register("AdornerOffsetX", typeof(double), typeof(WaitAdorner));
        public static readonly DependencyProperty AdornerOffsetYProperty =
                DependencyProperty.Register("AdornerOffsetY", typeof(double), typeof(WaitAdorner));

        /// <summary>
        /// Commands.
        /// </summary>
        public static readonly RoutedCommand ShowAdornerCommand = new RoutedCommand("ShowAdorner", typeof(WaitAdorner));
        public static readonly RoutedCommand HideAdornerCommand = new RoutedCommand("HideAdorner", typeof(WaitAdorner));

        public WaitAdorner()
        {
            DataContextChanged += (sender, args) => UpdateAdornerDataContext();
        }

        /// <summary>
        /// Update the DataContext of the adorner from the adorned control.
        /// </summary>
        private void UpdateAdornerDataContext()
        {
            if (AdornerContent != null)
                AdornerContent.DataContext = DataContext;
        }

        /// <summary>
        /// Shows or hides the adorner.
        /// Set to 'true' to show the adorner or 'false' to hide the adorner.
        /// </summary>
        public bool IsAdornerVisible
        {
            get { return (bool)GetValue(IsAdornerVisibleProperty); }
            set { SetValue(IsAdornerVisibleProperty, value); }
        }

        /// <summary>
        /// Used in XAML to define the UI content of the adorner.
        /// </summary>
        public FrameworkElement AdornerContent
        {
            get { return (FrameworkElement)GetValue(AdornerContentProperty); }
            set { SetValue(AdornerContentProperty, value); }
        }

        /// <summary>
        /// Specifies the horizontal placement of the adorner relative to the adorned control.
        /// </summary>
        public AdornerPlacement HorizontalAdornerPlacement
        {
            get { return (AdornerPlacement)GetValue(HorizontalAdornerPlacementProperty); }
            set { SetValue(HorizontalAdornerPlacementProperty, value); }
        }

        /// <summary>
        /// Specifies the vertical placement of the adorner relative to the adorned control.
        /// </summary>
        public AdornerPlacement VerticalAdornerPlacement
        {
            get { return (AdornerPlacement)GetValue(VerticalAdornerPlacementProperty); }
            set { SetValue(VerticalAdornerPlacementProperty, value); }
        }

        /// <summary>
        /// X offset of the adorner.
        /// </summary>
        public double AdornerOffsetX
        {
            get { return (double)GetValue(AdornerOffsetXProperty); }
            set { SetValue(AdornerOffsetXProperty, value); }
        }

        /// <summary>
        /// Y offset of the adorner.
        /// </summary>
        public double AdornerOffsetY
        {
            get { return (double)GetValue(AdornerOffsetYProperty); }
            set { SetValue(AdornerOffsetYProperty, value); }
        }

        #region Private Data Members

        /// <summary>
        /// Command bindings.
        /// </summary>
        private static readonly CommandBinding ShowAdornerCommandBinding = new CommandBinding(ShowAdornerCommand, ShowAdornerCommand_Executed);
        private static readonly CommandBinding HideAdornerCommandBinding = new CommandBinding(HideAdornerCommand, HideAdornerCommand_Executed);


        #endregion

        #region Private/Internal Functions

        /// <summary>
        /// Static constructor to register command bindings.
        /// </summary>
        static WaitAdorner()
        {
            CommandManager.RegisterClassCommandBinding(typeof(WaitAdorner), ShowAdornerCommandBinding);
            CommandManager.RegisterClassCommandBinding(typeof(WaitAdorner), HideAdornerCommandBinding);
        }

        /// <summary>
        /// Event raised when the Show command is executed.
        /// </summary>
        private static void ShowAdornerCommand_Executed(object target, ExecutedRoutedEventArgs e)
        {
            ((WaitAdorner)target).IsAdornerVisible = true;
        }

        /// <summary>
        /// Event raised when the Hide command is executed.
        /// </summary>
        private static void HideAdornerCommand_Executed(object target, ExecutedRoutedEventArgs e)
        {
            ((WaitAdorner)target).IsAdornerVisible = false;
        }

        /// <summary>
        /// Event raised when the value of IsAdornerVisible has changed.
        /// </summary>
        private static void IsAdornerVisible_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            WaitAdorner c = (WaitAdorner)o;
            c.ShowOrHideAdornerInternal();
        }

        /// <summary>
        /// Event raised when the value of AdornerContent has changed.
        /// </summary>
        private static void AdornerContent_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            WaitAdorner c = (WaitAdorner)o;
            c.ShowOrHideAdornerInternal();
        }

        /// <summary>
        /// Internal method to show or hide the adorner based on the value of IsAdornerVisible.
        /// </summary>
        private void ShowOrHideAdornerInternal()
        {
            if (IsAdornerVisible)
                ShowAdornerInternal();
            else
                HideAdornerInternal();
        }

        /// <summary>
        /// Internal method to show the adorner.
        /// </summary>
        private void ShowAdornerInternal()
        {
            if (_adorner == null && AdornerContent != null)
            {
                if (_adornerLayer == null)
                    _adornerLayer = AdornerLayer.GetAdornerLayer(this);

                if (_adornerLayer != null)
                {
                    _adorner = new FrameworkElementAdorner(AdornerContent, this, HorizontalAdornerPlacement, VerticalAdornerPlacement,
                                                                                     AdornerOffsetX, AdornerOffsetY);
                    _adornerLayer.Add(_adorner);
                    UpdateAdornerDataContext();
                }
            }
        }

        /// <summary>
        /// Internal method to hide the adorner.
        /// </summary>
        private void HideAdornerInternal()
        {
            if (_adornerLayer != null && _adorner != null)
            {
                _adornerLayer.Remove(_adorner);
                _adorner.DisconnectChild();
                _adorner = null;
                _adornerLayer = null;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ShowOrHideAdornerInternal();
        }
        #endregion
    }

}
