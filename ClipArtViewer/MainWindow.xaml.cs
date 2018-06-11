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
using System.Windows.Shapes;
using System.Diagnostics;
using System.Windows.Threading;
using System.ComponentModel;
using ClipArt;

namespace ClipArtViewer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		DependencyProperty PathProperty = DependencyProperty.Register("Path", typeof(string), typeof(MainWindow));
		public string Path
		{
			get { return GetValue(PathProperty) as string; }
			set { SetValue(PathProperty, value); }
		}

		static DependencyProperty FilenameProperty = DependencyProperty.RegisterAttached("Filename", typeof(string), typeof(MainWindow));
		public static void SetFilename(DependencyObject obj, string filename)
		{
			obj.SetValue(FilenameProperty, filename);
		}
		public static string GetFilename(DependencyObject obj)
		{
			return obj.GetValue(FilenameProperty) as string;
		}
		
		static DependencyProperty SVGImageProperty = DependencyProperty.RegisterAttached("SVGImage", typeof(DrawingGroup), typeof(MainWindow));
		public static void SetSVGImage(DependencyObject obj, DrawingGroup svgimage)
		{
			obj.SetValue(SVGImageProperty, svgimage);
		}
		public static DrawingGroup GetSVGImage(DependencyObject obj)
		{
			return obj.GetValue(SVGImageProperty) as DrawingGroup;
		}
		List<SVGItem> m_items = new List<SVGItem>();
		public List<SVGItem> Items
		{
			get { return m_items; }
		}
		public MainWindow()
		{
			InitializeComponent();

			// default path
			string s2 = Process.GetCurrentProcess().MainModule.FileName;
			string path = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(s2), @"Samples");
			Path = System.IO.Path.GetFullPath(path);

			ListFiles();
			DataContext = this;
		}
		void ListFiles()
		{
			string path = Path;
			string[] files = System.IO.Directory.GetFiles(path, "*.svg");
			foreach( string file in files)
				m_items.Add(new SVGItem(file));

			//m_filelist.GetBindingExpression(ListBox.ItemsSourceProperty).UpdateTarget();
			m_filelist.Items.Refresh();
		}
		private void OnReloadItem(object sender, RoutedEventArgs e)
		{
			SVGItem item = m_filelist.SelectedItem as SVGItem;
			if (item != null)
				item.Reload();

		}
		private void OnReloadAll(object sender, RoutedEventArgs e)
		{
			m_items.Clear();
			ListFiles();
		}
	}

		public class SVGItem
		{
			DrawingGroup m_image = null;
			
			public string FullPath			{get; private set;}
			public string Filename			{get; private set;}
			public SVGRender SVGRender		{get; private set;}
			public DrawingGroup	SVGImage
			{
				get 
				{
					EnsureLoaded();
					return m_image;
				}
			}
			public SVGItem(string fullpath)
			{
				FullPath = fullpath;
				Filename = System.IO.Path.GetFileNameWithoutExtension(fullpath);
			}
			public void Reload()
			{
				m_image = SVGRender.LoadDrawing(FullPath);
			}
			void EnsureLoaded()
			{
				if (m_image != null)
					return;
				//Console.WriteLine("{0} - loading {1}", DateTime.Now.ToLongDateString(), Filename);
				SVGRender = new SVGRender();
				m_image = SVGRender.LoadDrawing(FullPath);
			}
		}

}
