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
		public void SetImage(Drawing drawing)
		{
			m_canvas1.SetImage(drawing);
		}
		public FullSizeForm()
		{
			InitializeComponent();
			Loaded += new RoutedEventHandler(FullSizeForm_Loaded);
		}

		void FullSizeForm_Loaded(object sender, RoutedEventArgs e)
		{
			m_sizeTypeCombo.SelectedValue = m_canvas1.SizeType.ToString();
		}
	}
}
