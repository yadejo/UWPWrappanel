using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TWIXL_REVIEWER_V2.Views.Helpers {
    public class WrapPanel : Panel {


        public Orientation Direction {
            get { return (Orientation)GetValue(DirectionProperty); }
            set {
                SetValue(DirectionProperty, value);
                InvalidateArrange();
                InvalidateMeasure();
                MeasureOverride(this.RenderSize);
                ArrangeOverride(this.RenderSize);
            }
        }

        public double HorizontalSpacing {
            get { return (double)GetValue(HorizontalSpacingProperty); }
            set {
                SetValue(HorizontalSpacingProperty, value);
                InvalidateArrange();
                InvalidateMeasure();
                MeasureOverride(this.RenderSize);
                ArrangeOverride(this.RenderSize);

            }
        }

        public double VerticalSpacing {
            get { return (double)GetValue(VerticalSpacingProperty); }
            set {
                SetValue(VerticalSpacingProperty, value);
                InvalidateArrange();
                InvalidateMeasure();
                MeasureOverride(this.RenderSize);
                ArrangeOverride(this.RenderSize);
            }
        }

        

        public WrapPanel() : base() {

        }


        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register("Direction",
            typeof(Orientation), typeof(WrapPanel), null);

        public static readonly DependencyProperty HorizontalSpacingProperty =
            DependencyProperty.Register("HorizontalSpacing",
            typeof(double), typeof(WrapPanel), null);

        public static readonly DependencyProperty VerticalSpacingProperty =
            DependencyProperty.Register("VerticalSpacing",
            typeof(double), typeof(WrapPanel), null);

       

        protected override Size MeasureOverride(Size availableSize) {
            foreach (UIElement child in Children) {
                child.Measure(new Size(availableSize.Width, availableSize.Height));
            }


            return base.MeasureOverride(availableSize);
        }

        protected override Size ArrangeOverride(Size finalSize) {
            var x = this.RenderTransform;
            Point point = new Point(0, 0);
            int i = 0;
            double height = 0;

            if (Direction == Orientation.Horizontal) {
                double largestHeight = 0.0;

                foreach (UIElement child in Children) {
                    child.Arrange(new Rect(point, new Point(point.X +
                        child.DesiredSize.Width, point.Y + child.DesiredSize.Height)));

                    if (child.DesiredSize.Height > largestHeight)
                        largestHeight = child.DesiredSize.Height;

                    point.X = point.X + child.DesiredSize.Width;

                    if ((i + 1) < Children.Count) {
                        if ((point.X + Children[i + 1].DesiredSize.Width) > finalSize.Width) {
                            point.X = 0;
                            
                            point.Y = point.Y + largestHeight + VerticalSpacing;
                            height = point.Y + largestHeight;
                            largestHeight = 0.0;
                        }
                        else {
                            point.X += HorizontalSpacing;
                            Debug.WriteLine(point.X);
                        }

                    }
                    else {
                        height = point.Y + largestHeight;
                    }
                    i++;
                }

            }
            else {
                double largestWidth = 0.0;

                foreach (UIElement child in Children) {
                    child.Arrange(new Rect(point, new Point(point.X +
                        child.DesiredSize.Width, point.Y + child.DesiredSize.Height)));

                    if (child.DesiredSize.Width > largestWidth)
                        largestWidth = child.DesiredSize.Width;

                    point.Y = point.Y + child.DesiredSize.Height;

                    if ((i + 1) < Children.Count) {
                        if ((point.Y + Children[i + 1].DesiredSize.Height) > finalSize.Height) {
                            point.Y = 0;
                            point.X = point.X + largestWidth + HorizontalSpacing;
                            height = point.X + largestWidth;
                            largestWidth = 0.0;
                        }
                        else {
                            point.Y += VerticalSpacing;
                        }
                    }
                    else {
                        height = point.X + largestWidth;
                    }

                    i++;
                }
            }
            if (height > 0) {
                if (Direction == Orientation.Horizontal) {
                    this.Height = height;
                    this.Width = Window.Current.Bounds.Width;
                }
                else {
                    this.Width = height;
                    this.Height = Window.Current.Bounds.Height;
                }
            }
            return finalSize;
        }

    }
}
