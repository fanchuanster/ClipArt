using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace ClipArt
{
	public class Stroke
	{
		public enum eLineCap
		{
			butt,
			round,
			square,
		}
		public enum eLineJoin
		{
			miter,
			round,
			bevel,
		}
		public PaintServer Color {get; set;}
		public double Width {get; set;}
		public double Opacity {get; set;}
		public eLineCap LineCap {get; set;}
		public eLineJoin LineJoin {get; set;}
		public double[] StrokeArray {get; set;} 
		public Stroke()
		{
			Color = new SolidColor(SVG.PaintServers, Colors.Black);
			Width = 1;
			LineCap = eLineCap.butt;
			LineJoin = eLineJoin.miter;
			Opacity = 100;
		}
		public Brush StrokeBrush
		{
            get
            {
                if (Color != null)
                    return Color.GetBrush(Opacity);
                return null;
            }
		}
	}
}
