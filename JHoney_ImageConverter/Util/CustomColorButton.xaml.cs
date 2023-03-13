using JHoney_ImageConverter.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JHoney_ImageConverter.Util
{
    /// <summary>
    /// CustomColorButton.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CustomColorButton : System.Windows.Controls.UserControl
    {
        public CustomColorButton()
        {
            InitializeComponent();
        }
        public static readonly DependencyProperty FillRectProperty =
          DependencyProperty.Register(nameof(FillRect), typeof(Brush), typeof(CustomColorButton),
              new FrameworkPropertyMetadata());

        [TypeConverter(typeof(BrushConverter))]
        public Brush FillRect
        {
            get { return (Brush)GetValue(FillRectProperty); }
            set { Console.WriteLine($"FillRect : {value}"); SetValue(FillRectProperty, (Brush)value); }
        }
    }
}
public class BrushConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    {
        return sourceType == typeof(string);
    }

    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
        string stringValue = value as string;
        if (stringValue != null)
        {
            try
            {
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString(stringValue));
            }
            catch (FormatException)
            {
                // handle invalid color string
            }
        }

        return base.ConvertFrom(context, culture, value);
    }
}