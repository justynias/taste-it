using System.Collections;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace taste_it.Additionals.LoaderService
{
    //
    // This class is an adorner that allows a FrameworkElement derived class to adorn another FrameworkElement.
    //
    public class FrameworkElementAdorner : Adorner
    {
        //
        // The framework element that is the adorner. 
        //
        private readonly FrameworkElement _child;

        //
        // Placement of the child.
        //
        private readonly AdornerPlacement _horizontalAdornerPlacement = AdornerPlacement.Inside;
        private readonly AdornerPlacement _verticalAdornerPlacement = AdornerPlacement.Inside;

        //
        // Offset of the child.
        //
        private readonly double _offsetX;
        private readonly double _offsetY;

        //
        // Position of the child (when not set to NaN).
        //
        public FrameworkElementAdorner(FrameworkElement adornerChildElement, FrameworkElement adornedElement)
                    : base(adornedElement)
        {
            _child = adornerChildElement;
            AddLogicalChild(adornerChildElement);
            AddVisualChild(adornerChildElement);
        }

        public FrameworkElementAdorner(FrameworkElement adornerChildElement, FrameworkElement adornedElement,
                                                                     AdornerPlacement horizontalAdornerPlacement, AdornerPlacement verticalAdornerPlacement,
                                                                     double offsetX, double offsetY)
                : base(adornedElement)
        {
            _child = adornerChildElement;
            _horizontalAdornerPlacement = horizontalAdornerPlacement;
            _verticalAdornerPlacement = verticalAdornerPlacement;
            _offsetX = offsetX;
            _offsetY = offsetY;

            adornedElement.SizeChanged += adornedElement_SizeChanged;

            AddLogicalChild(adornerChildElement);
            AddVisualChild(adornerChildElement);
        }

        /// <summary>
        /// Event raised when the adorned control's size has changed.
        /// </summary>
        private void adornedElement_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            InvalidateMeasure();
        }

        //
        // Position of the child (when not set to NaN).
        //
        public double PositionX { get; set; } = double.NaN;

        public double PositionY { get; set; } = double.NaN;

        protected override Size MeasureOverride(Size constraint)
        {
            _child.Measure(constraint);
            return _child.DesiredSize;
        }

        /// <summary>
        /// Determine the X coordinate of the child.
        /// </summary>
        private double DetermineX()
        {
            switch (_child.HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    return _horizontalAdornerPlacement == AdornerPlacement.Outside ? -_child.DesiredSize.Width + _offsetX : _offsetX;
                case HorizontalAlignment.Right:
                    return _horizontalAdornerPlacement == AdornerPlacement.Outside
                        ? AdornedElement.ActualWidth + _offsetX
                        : AdornedElement.ActualWidth - _child.DesiredSize.Width + _offsetX;
                case HorizontalAlignment.Center:
                    return (AdornedElement.ActualWidth / 2) - (_child.DesiredSize.Width / 2) + _offsetX;
                default:
                    return 0.0;
            }
        }

        /// <summary>
        /// Determine the Y coordinate of the child.
        /// </summary>
        private double DetermineY()
        {
            switch (_child.VerticalAlignment)
            {
                case VerticalAlignment.Top:
                    return _verticalAdornerPlacement == AdornerPlacement.Outside ? -_child.DesiredSize.Height + _offsetY : _offsetY;
                case VerticalAlignment.Bottom:
                    return _verticalAdornerPlacement == AdornerPlacement.Outside
                        ? AdornedElement.ActualHeight + _offsetY
                        : _child.DesiredSize.Height - AdornedElement.ActualHeight + _offsetY;
                case VerticalAlignment.Center:
                    return (AdornedElement.ActualHeight / 2) - (_child.DesiredSize.Height / 2) + _offsetY;
                default:
                    return 0.0;
            }
        }

        /// <summary>
        /// Determine the width of the child.
        /// </summary>
        private double DetermineWidth()
        {
            if (!double.IsNaN(PositionX))
                return _child.DesiredSize.Width;

            switch (_child.HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    return _child.DesiredSize.Width;
                case HorizontalAlignment.Right:
                    return _child.DesiredSize.Width;
                case HorizontalAlignment.Center:
                    return _child.DesiredSize.Width;
                case HorizontalAlignment.Stretch:
                    return AdornedElement.ActualWidth;
                default:
                    return 0.0;
            }
        }

        /// <summary>
        /// Determine the height of the child.
        /// </summary>
        private double DetermineHeight()
        {
            if (!double.IsNaN(PositionY))
                return _child.DesiredSize.Height;

            switch (_child.VerticalAlignment)
            {
                case VerticalAlignment.Top:
                    return _child.DesiredSize.Height;
                case VerticalAlignment.Bottom:
                    return _child.DesiredSize.Height;
                case VerticalAlignment.Center:
                    return _child.DesiredSize.Height;
                case VerticalAlignment.Stretch:
                    return AdornedElement.ActualHeight;
                default:
                    return 0.0;
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double x = double.IsNaN(PositionX) ? DetermineX() : PositionX;
            double y = double.IsNaN(PositionX) ? DetermineY() : PositionY;
            double adornerWidth = DetermineWidth();
            double adornerHeight = DetermineHeight();
            _child.Arrange(new Rect(x, y, adornerWidth, adornerHeight));
            return finalSize;
        }

        protected override int VisualChildrenCount => 1;

        protected override Visual GetVisualChild(int index)
        {
            return _child;
        }

        protected override IEnumerator LogicalChildren => (new object[] { _child }).GetEnumerator();

        /// <summary>
        /// Disconnect the child element from the visual tree so that it may be reused later.
        /// </summary>
        public void DisconnectChild()
        {
            RemoveLogicalChild(_child);
            RemoveVisualChild(_child);
        }

        /// <summary>
        /// Override AdornedElement from base class for less type-checking.
        /// </summary>
        public new FrameworkElement AdornedElement => (FrameworkElement)base.AdornedElement;
    }
}
