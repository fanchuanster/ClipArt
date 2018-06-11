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
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class Window1 : Window
	{
		public Window1()
		{
			InitializeComponent();

			//SVG svg = new SVG(@"c:\temp\jktest.svg");
			//SVG svg = r.LoadFile(@"c:\temp\tranberry_locked_exclamation_mark_-_padlock.svg");
			//SVG svg = r.LoadFile(@"c:\temp\molumen_red_square_error_warning_icon.svg");
			//SVG svg = r.LoadFile(@"c:\temp\peterm_penguin_with_a_shirt.svg");
			//SVG svg = r.LoadFile(@"c:\temp\zafx_tux-vache.svg");

			//computer-aj_aj_ashton_01
			SVGRender render = new SVGRender();
			//DrawingImage geometryImage = new DrawingImage(render.LoadDrawing(@"c:\temp\jktest.svg"));
			//DrawingImage geometryImage = new DrawingImage(render.LoadDrawing(@"c:\temp\molumen_red_square_error_warning_icon_1.svg"));
			DrawingGroup clip01 = render.LoadDrawing(@"c:\temp\ixia_logo-test.svg");
			//DrawingGroup clip01 = render.LoadDrawing(@"c:\temp\computer-aj_aj_ashton_01.svg");
			//DrawingGroup clip02 = render.LoadDrawing(@"c:\temp\molumen_red_square_error_warning_icon_1.svg");
			//DrawingGroup clip03 = render.LoadDrawing(@"c:\temp\peterm_penguin_with_a_shirt.svg");
			//DrawingImage geometryImage = new DrawingImage(drawing);
			//DrawingImage geometryImage = new DrawingImage(render.LoadDrawing(@"c:\temp\tranberry_locked_exclamation_mark_-_padlock.svg"));
			//DrawingImage geometryImage = new DrawingImage(render.LoadDrawing(@"c:\temp\peterm_penguin_with_a_shirt.svg"));
			//DrawingImage geometryImage = new DrawingImage(render.LoadDrawing(@"c:\temp\zafx_tux-vache.svg"));

			//m_canvas.AddDrawing(clip03);
			//m_canvas.AddDrawing(clip01);
			//m_canvas.AddDrawing(clip02);
		}

		private void OnResetZoom(object sender, RoutedEventArgs e)
		{
			//m_canvas.Zoom = 100;
		}

		private void DoOpen(object sender, ExecutedRoutedEventArgs e)
		{
			Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
			dlg.DefaultExt = ".svg";
			dlg.Filter = "Scaleable Vector Graphic (.svg)|*.svg";
			Nullable<bool> result = dlg.ShowDialog();
			if (result == true)
			{
				string filename = dlg.FileName;
				Viewer v = new Viewer(filename);
				v.Show();
			}
		}
	}
}
