using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace ClipArt
{
	public class Fill
	{
		public enum eFillRule
		{
			nonzero,
			evenodd
		}
		public eFillRule FillRule { get; set;}
		public PaintServer Color {get; set;}
		public double Opacity {get; set;}
		public Fill()
		{
			FillRule = eFillRule.nonzero;
			Color = SVG.PaintServers.Parse("#5a5b5d");
			Opacity = 100;
		}

        Brush fillBrush;

        public Brush FillBrush
		{
            get
            {
                if (fillBrush != null)
                    return fillBrush;

                if (Color != null)
                    return Color.GetBrush(Opacity);
                return null;
            }

            set
            {
                fillBrush = value;
            }
		}
	}
}
