using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using CopilotBuddy.Buddy.Overlay.Commands;
using CopilotBuddy.Buddy.Overlay.Internal;
using Point = System.Windows.Point;
using HorizontalAlignment = System.Windows.HorizontalAlignment;

namespace CopilotBuddy.Buddy.Overlay.Controls
{
    public class OverlayControl : ContentControl
    {
        public static readonly DependencyProperty AllowMovingProperty =
            DependencyProperty.Register("AllowMoving", typeof(bool), typeof(OverlayControl));

        public static readonly DependencyProperty AllowResizingProperty =
            DependencyProperty.Register("AllowResizing", typeof(bool), typeof(OverlayControl));

        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register("X", typeof(double), typeof(OverlayControl),
                new PropertyMetadata(new PropertyChangedCallback(OnXChanged)));

        public static readonly DependencyProperty YProperty =
            DependencyProperty.Register("Y", typeof(double), typeof(OverlayControl),
                new PropertyMetadata(new PropertyChangedCallback(OnYChanged)));

        private Canvas _parentCanvas;
        private OverlayWindowBase _overlayWindow;
        private ICommand _dragMoveCommand;
        private ICommand _dragResizeCommand;
        private bool _isMoving;
        private Point _moveOrigin;
        private Point _dragStart;
        private int _horizontalDirection;
        private bool _isResizing;
        private Point _lastMousePosition;
        private int _verticalDirection;
        private double _resizeStartWidth;
        private double _resizeStartHeight;

        public bool AllowMoving
        {
            get { return (bool)GetValue(AllowMovingProperty); }
            set { SetValue(AllowMovingProperty, value); }
        }

        public bool AllowResizing
        {
            get { return (bool)GetValue(AllowResizingProperty); }
            set { SetValue(AllowResizingProperty, value); }
        }

        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }

        public double Y
        {
            get { return (double)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }

        private static void OnXChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Canvas.SetLeft((OverlayControl)d, (double)e.NewValue);
        }

        private static void OnYChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Canvas.SetTop((OverlayControl)d, (double)e.NewValue);
        }

        static OverlayControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(OverlayControl),
                new FrameworkPropertyMetadata(typeof(OverlayControl)));
        }

        public OverlayControl()
        {
            MouseMove += OverlayControl_MouseMove;
            MouseLeftButtonUp += OverlayControl_MouseLeftButtonUp;
            LostMouseCapture += OverlayControl_LostMouseCapture;
        }

        public ICommand DragMoveCommand
        {
            get
            {
                ICommand result;
                if ((result = _dragMoveCommand) == null)
                {
                    result = (_dragMoveCommand = new RelayCommand(
                        new Action<object>(ExecuteDragMove),
                        new Predicate<object>(CanDragMove)));
                }
                return result;
            }
        }

        public ICommand DragResizeCommand
        {
            get
            {
                ICommand result;
                if ((result = _dragResizeCommand) == null)
                {
                    result = (_dragResizeCommand = new RelayCommand(
                        new Action<object>(ExecuteDragResize),
                        new Predicate<object>(CanDragResize)));
                }
                return result;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            _parentCanvas = FindVisualParent<Canvas>(this);
            if (_parentCanvas == null)
                throw new InvalidOperationException("OverlayControl must have a canvas parent");

            _overlayWindow = FindVisualParent<OverlayWindowBase>(this);
            if (_overlayWindow == null)
                throw new InvalidOperationException("OverlayControl can only be used in an OverlayWindow");

            double inverseScaleX = 1.0 / _overlayWindow.ScaleX;
            double inverseScaleY = 1.0 / _overlayWindow.ScaleY;
            X = Clamp(X, 0.0, _overlayWindow.ActualWidth * inverseScaleX - ActualWidth);
            Y = Clamp(Y, 0.0, _overlayWindow.ActualHeight * inverseScaleY - ActualHeight);
        }

        public void DragMove()
        {
            if (!AllowMoving)
                return;
            if (Mouse.LeftButton == MouseButtonState.Pressed && !_isMoving && !_isResizing)
            {
                _isMoving = true;
                _moveOrigin = new Point(X, Y);
                BeginDrag();
            }
        }

        private void BeginDrag()
        {
            _lastMousePosition = (_dragStart = Mouse.GetPosition(null));
            CaptureMouse();
        }

        public bool CancelMove()
        {
            if (!_isMoving)
                return false;
            if (IsMouseCaptured)
                ReleaseMouseCapture();
            _isMoving = false;
            return true;
        }

        private void OverlayControl_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!_isMoving && !_isResizing)
                return;

            if (e.MouseDevice.LeftButton == MouseButtonState.Pressed)
            {
                Point position = e.GetPosition(null);
                if (position != _lastMousePosition)
                {
                    _lastMousePosition = position;
                    e.Handled = true;
                    double inverseScaleX = 1.0 / _overlayWindow.ScaleX;
                    double inverseScaleY = 1.0 / _overlayWindow.ScaleY;
                    double deltaX = (position.X - _dragStart.X) * inverseScaleX;
                    double deltaY = (position.Y - _dragStart.Y) * inverseScaleY;

                    if (_isMoving)
                    {
                        double maxX = _overlayWindow.ActualWidth * inverseScaleX - ActualWidth;
                        double maxY = _overlayWindow.ActualHeight * inverseScaleY - ActualHeight;
                        ApplyMove(deltaX, deltaY, maxX, maxY);
                    }
                    else if (_isResizing)
                    {
                        ApplyResize(deltaX, deltaY);
                    }
                }
            }
            else
            {
                CancelResize();
                CancelMove();
            }
        }

        private void ApplyMove(double deltaX, double deltaY, double maxX, double maxY)
        {
            double newX = _moveOrigin.X + deltaX;
            double newY = _moveOrigin.Y + deltaY;
            X = Clamp(newX, 0.0, maxX);
            Y = Clamp(newY, 0.0, maxY);
        }

        private void OverlayControl_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (CancelResize() || CancelMove())
                e.Handled = true;
        }

        private void OverlayControl_LostMouseCapture(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (CancelResize() || CancelMove())
                e.Handled = true;
        }

        public void DragResize(System.Windows.HorizontalAlignment horizontalDirection = System.Windows.HorizontalAlignment.Right,
            System.Windows.VerticalAlignment verticalDirection = System.Windows.VerticalAlignment.Bottom)
        {
            if (!AllowResizing)
                return;
            if (Mouse.LeftButton == MouseButtonState.Pressed && !_isMoving && !_isResizing)
            {
                _isResizing = true;
                _horizontalDirection = (horizontalDirection == System.Windows.HorizontalAlignment.Right) ? 1 :
                    (horizontalDirection == System.Windows.HorizontalAlignment.Left) ? -1 : 0;
                _verticalDirection = (verticalDirection == System.Windows.VerticalAlignment.Bottom) ? 1 :
                    (verticalDirection == System.Windows.VerticalAlignment.Top) ? -1 : 0;
                _resizeStartWidth = ActualWidth;
                _resizeStartHeight = ActualHeight;
                BeginDrag();
            }
        }

        private void ApplyResize(double deltaX, double deltaY)
        {
            double newWidth = _resizeStartWidth + deltaX * _horizontalDirection;
            double newHeight = _resizeStartHeight + deltaY * _verticalDirection;
            Width = Clamp(newWidth, MinWidth, MaxWidth);
            Height = Clamp(newHeight, MinHeight, MaxHeight);
        }

        public bool CancelResize()
        {
            if (!_isResizing)
                return false;
            if (IsMouseCaptured)
                ReleaseMouseCapture();
            _isResizing = false;
            return true;
        }

        private static T FindVisualParent<T>(FrameworkElement element) where T : FrameworkElement
        {
            if (element == null)
                return default(T);
            FrameworkElement parent = element.Parent as FrameworkElement;
            if (parent is T)
                return (T)(object)parent;
            FrameworkElement templatedParent = element.TemplatedParent as FrameworkElement;
            if (templatedParent is T)
                return (T)(object)templatedParent;
            return FindVisualParent<T>(parent ?? templatedParent);
        }

        private static double Clamp(double value, double min, double max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        private void ExecuteDragMove(object parameter)
        {
            DragMove();
        }

        private bool CanDragMove(object parameter)
        {
            return AllowMoving;
        }

        private void ExecuteDragResize(object parameter)
        {
            DragResize(System.Windows.HorizontalAlignment.Right, System.Windows.VerticalAlignment.Bottom);
        }

        private bool CanDragResize(object parameter)
        {
            return AllowResizing;
        }
    }
}
