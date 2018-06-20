using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClipArtViewer
{
    /// <summary>
    /// Interaction logic for FullSizeForm.xaml
    /// </summary>
    public partial class FullSizeForm : UserControl
    {
        public static DependencyProperty ImageSourcePoperty = DependencyProperty.Register("ImageSource",
            typeof(Drawing),
            typeof(FullSizeForm),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnImageSourceChanged)));

        static void OnImageSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((FullSizeForm)d).SetImage(e.NewValue as Drawing);
        }
        public Drawing ImageSource
        {
            get { return (Drawing)GetValue(ImageSourcePoperty); }
            set { SetValue(ImageSourcePoperty, value); }
        }

        public static DependencyProperty ImageSourceFilePoperty = DependencyProperty.Register("ImageSourceFile",
            typeof(string),
            typeof(FullSizeForm),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender,
                new PropertyChangedCallback(OnImageSourceFileChanged)));

        static void OnImageSourceFileChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var newValue = e.NewValue as string;
            if (newValue != null)
            {
                ((FullSizeForm)d).m_canvas1.ImageSourceFile = (e.NewValue as string);
            }
        }
        public string ImageSourceFile
        {
            get { return (string)GetValue(ImageSourceFilePoperty); }
            set { SetValue(ImageSourceFilePoperty, value); }
        }

        public void SetImage(Drawing drawing)
        {
            m_canvas1.SetImage(drawing);
        }
        public FullSizeForm()
        {
            InitializeComponent();
        }
    }
}
