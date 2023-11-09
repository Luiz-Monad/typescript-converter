using System.Windows;
using System.Windows.Controls;

namespace SyntaxEditor.Controls
{

    public class SplitPanel : Grid
    {
        public SplitPanel()
        {
            Loaded += SplitPanel_Loaded;
        }

        private void SplitPanel_Loaded(object sender, RoutedEventArgs e)
        {
            //if (Children.Count != 2)
            //    throw new System.InvalidOperationException("SplitterGrid can only contain two children.");
            UpdateSplitLayout();
        }

        private void UpdateSplitLayout()
        {

            ColumnDefinitions.Clear();
            RowDefinitions.Clear();
            if (Children.Count > 2)
                Children.RemoveAt(2); // Remove old splitter

            if (Orientation == Orientation.Vertical)
            {
                var splitter = new GridSplitter {
                    Width = SplitterSize,
                    Height = double.NaN,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    ResizeDirection = GridResizeDirection.Columns
                };
                Children.Add(splitter);

                ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                SetRow(Children[0], 0);
                SetRow(splitter, 0);
                SetRow(Children[1], 0);
                SetColumn(Children[0], 0);
                SetColumn(splitter, 1);
                SetColumn(Children[1], 2);
            }
            else
            {
                var splitter = new GridSplitter {
                    Height = SplitterSize,
                    Width = double.NaN,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    ResizeDirection = GridResizeDirection.Rows
                };
                Children.Add(splitter);

                RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

                SetRow(Children[0], 0);
                SetRow(splitter, 1);
                SetRow(Children[1], 2);
                SetColumn(Children[0], 0);
                SetColumn(splitter, 0);
                SetColumn(Children[1], 0);
            }
        }

        public Orientation Orientation {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(SplitPanel), new PropertyMetadata(Orientation.Horizontal, OnOrientationOrSplitterSizeChanged));

        public double SplitterSize {
            get { return (double)GetValue(SplitterSizeProperty); }
            set { SetValue(SplitterSizeProperty, value); }
        }

        public static readonly DependencyProperty SplitterSizeProperty =
            DependencyProperty.Register("SplitterSize", typeof(double), typeof(SplitPanel), new PropertyMetadata(5.0, OnOrientationOrSplitterSizeChanged));

        private static void OnOrientationOrSplitterSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((SplitPanel)d).UpdateSplitLayout();
        }
    }

}
