using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using CopilotBuddy.Buddy.Overlay.Notifications;
using Color = System.Windows.Media.Color;
using FontFamily = System.Windows.Media.FontFamily;

namespace CopilotBuddy.Buddy.Overlay.Internal
{
    internal class TextToastComponent : ToastUIComponent
    {
        public Func<string> TextProducer { get; private set; }
        public Color TextColor { get; private set; }
        public Color ShadowColor { get; private set; }
        public FontFamily FontFamily { get; private set; }
        public double FontSize { get; private set; }
        public FontWeight FontWeight { get; private set; }

        private TextBlock _textBlock;

        private TextBlock TextBlock
        {
            get
            {
                TextBlock result;
                if ((result = _textBlock) == null)
                {
                    result = (_textBlock = CreateTextBlock());
                }
                return result;
            }
        }

        public override FrameworkElement GuiElement => TextBlock;

        public TextToastComponent(Func<string> textProducer, TimeSpan duration, Color color,
            Color shadowColor, FontFamily fontFamily, FontWeight fontWeight, double fontSize)
        {
            TextProducer = textProducer;
            DisplayDuration = duration;
            TextColor = color;
            ShadowColor = shadowColor;
            FontFamily = fontFamily;
            FontWeight = fontWeight;
            FontSize = fontSize;
        }

        private TextBlock CreateTextBlock()
        {
            SolidColorBrush brush = new SolidColorBrush(TextColor);
            brush.Freeze();

            TextBlock textBlock = new TextBlock();
            textBlock.Text = TextProducer();
            textBlock.FontFamily = FontFamily;
            textBlock.FontSize = FontSize;
            textBlock.FontWeight = FontWeight;
            textBlock.Foreground = brush;
            textBlock.RenderTransformOrigin = new System.Windows.Point(0.5, 0.5);
            textBlock.IsHitTestVisible = false;
            textBlock.TextAlignment = TextAlignment.Center;
            textBlock.Effect = new DropShadowEffect
            {
                Color = ShadowColor,
                BlurRadius = 15.0,
                ShadowDepth = 0.0
            };

            ScaleTransform scaleTransform = new ScaleTransform(1.0, 1.0);
            textBlock.RenderTransform = scaleTransform;

            DoubleAnimation animation = new DoubleAnimation(2.5, 1.0,
                new Duration(TimeSpan.FromMilliseconds(400.0)))
            {
                EasingFunction = new CircleEase()
            };
            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, animation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, animation);

            return textBlock;
        }

        protected internal override void Update()
        {
            TextBlock.Text = TextProducer();
        }
    }
}
