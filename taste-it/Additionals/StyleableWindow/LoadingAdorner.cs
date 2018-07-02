using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace taste_it.Additionals.StyleableWindow
{
    public class LoadingAdorner : Adorner
    {
        private readonly FrameworkElement mAdorningElement;
        private AdornerLayer mLayer;

        public LoadingAdorner(FrameworkElement adornedElement, FrameworkElement adorningElement)
            : base(adornedElement)
        {
            mAdorningElement = adorningElement;

            if (adorningElement != null)
                AddVisualChild(adorningElement);
        }

        protected override int VisualChildrenCount
        {
            get { return mAdorningElement != null ? 1 : 0; }
        }

        protected override System.Windows.Media.Visual GetVisualChild(int index)
        {
            if (index == 0 && mAdorningElement != null)
                return mAdorningElement;

            return base.GetVisualChild(index);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (mAdorningElement != null)
                mAdorningElement.Arrange(new Rect
                (new Point(0, 0), AdornedElement.RenderSize));

            return finalSize;
        }

        public void SetLayer(AdornerLayer layer)
        {
            mLayer = layer;
            mLayer.Add(this);
        }

        public void RemoveLayer()
        {
            if (mLayer != null)
            {
                mLayer.Remove(this);
                RemoveVisualChild(mAdorningElement);
            }
        }
    }

    public class AdornerBehaviour
    {
        public static readonly DependencyProperty ShowAdornerProperty =
        DependencyProperty.RegisterAttached("ShowAdorner", typeof(bool),
        typeof(AdornerBehaviour), new UIPropertyMetadata(false, OnShowAdornerChanged));
        public static readonly DependencyProperty ControlProperty =
        DependencyProperty.RegisterAttached("Control", typeof(FrameworkElement),
        typeof(AdornerBehaviour), new UIPropertyMetadata(null));
        private static readonly DependencyProperty CtrlAdornerProperty =
        DependencyProperty.RegisterAttached("CtrlAdorner", typeof(LoadingAdorner),
        typeof(AdornerBehaviour), new UIPropertyMetadata(null));

        public static bool GetShowAdorner(DependencyObject obj)
        {
            return (bool)obj.GetValue(ShowAdornerProperty);
        }

        public static void SetShowAdorner(DependencyObject obj, bool value)
        {
            obj.SetValue(ShowAdornerProperty, value);
        }


        public static FrameworkElement GetControl(DependencyObject obj)
        {
            return (FrameworkElement)obj.GetValue(ControlProperty);
        }

        public static void SetControl(DependencyObject obj, UIElement value)
        {
            obj.SetValue(ControlProperty, value);
        }

        private static void OnShowAdornerChanged
        (DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement)
            {
                if (e.NewValue != null)
                {
                    FrameworkElement adornedElement = d as FrameworkElement;
                    bool bValue = (bool)e.NewValue;
                    FrameworkElement adorningElement = GetControl(d);

                    LoadingAdorner ctrlAdorner =
                       adornedElement.GetValue(CtrlAdornerProperty) as LoadingAdorner;
                    if (ctrlAdorner != null)
                        ctrlAdorner.RemoveLayer();

                    if (bValue && adorningElement != null)
                    {
                        ctrlAdorner = new LoadingAdorner(adornedElement, adorningElement);
                        var adornerLayer = AdornerLayer.GetAdornerLayer(adornedElement);
                        ctrlAdorner.SetLayer(adornerLayer);
                        d.SetValue(CtrlAdornerProperty, ctrlAdorner);
                    }
                }
            }
        }
    }
}
