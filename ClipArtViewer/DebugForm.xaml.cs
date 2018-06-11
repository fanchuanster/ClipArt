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
using ClipArt;

namespace ClipArtViewer
{
	/// <summary>
	/// Interaction logic for FullSizeForm.xaml
	/// </summary>
	public partial class DebugForm : UserControl
	{
		public static DependencyProperty SVGItemSourcePoperty = DependencyProperty.Register("SVGItemSource",
			typeof(SVGItem),
			typeof(DebugForm),
			new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnSVGItemChanged)));
		
		static void OnSVGItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DebugForm)d).SetSVGItemSource(e.NewValue as SVGItem);
		}
		public SVGItem SVGItemSource
		{
			get { return (SVGItem)GetValue(SVGItemSourcePoperty); }
			set { SetValue(SVGItemSourcePoperty, value); }
		}
		public void SetSVGItemSource(SVGItem svg)
		{
			if (svg == null)
				return;
			m_canvas.SetDrawing(svg.SVGImage);
			m_objTree.Populate(svg.SVGRender.SVG);
		}
		public DebugForm()
		{
			InitializeComponent();
		}

		private void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			Shape shape = m_objTree.SelectedShape;
			if (shape != null)
			{
				m_canvas.ClearHighligh();
				m_canvas.AddHighlight(SVGItemSource.SVGRender.CreateDrawing(shape));
			}
			else
				m_canvas.ClearHighligh();
		}


	}
}
