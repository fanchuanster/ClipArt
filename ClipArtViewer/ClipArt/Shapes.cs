﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Xml;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ClipArt
{
	public class Shape : ClipArtElement
	{
		Fill m_fill;
		Stroke m_stroke;
		TextStyle m_textstyle;
		public virtual Stroke Stroke 
		{ 
			get
			{
				if (m_stroke != null)
					return m_stroke;
                if (Parent != null)
                    return Parent.Stroke;

                return null;
			}
		}
		public virtual Fill Fill 
		{ 
			get 
			{
				if (m_fill != null)
					return m_fill;
                if (Parent != null)
                    return Parent.Fill;
				return null;
			}
		}
		public virtual TextStyle TextStyle
		{
			get 
			{
				if (m_textstyle != null)
					return m_textstyle;
				while (Parent != null)
				{
					if (Parent.m_textstyle != null)
						return Parent.m_textstyle;
					Parent = Parent.Parent;
				}
				return null;
			}
		}
		public virtual Transform Transform { get; private set;}
		public Shape Parent { get; set; }
		public Shape(XmlNode node) : this(node, null) {}
		public Shape(XmlNode node, Shape parent) : base(node)
		{
			Parent = parent;
			if (node != null)
			{
				foreach (XmlAttribute attr in node.Attributes)
					Parse(attr);
			}
		}
		public Shape(List<ShapeUtil.Attribute> attrs, Shape parent) : base(null)
		{
			Parent = parent;
			if (attrs != null)
			{
				foreach (ShapeUtil.Attribute attr in attrs)
					Parse(attr);
			}
		}
		protected virtual void Parse(XmlAttribute attr)
		{
			string name = attr.Name;
			string value = attr.Value;
			Parse(name, value);
		}
		protected virtual void Parse(ShapeUtil.Attribute attr)
		{
			string name = attr.Name;
			string value = attr.Value;
			Parse(name, value);
		}
		protected virtual void Parse(string name, string value)
		{
			if (name == SVGTags.sTransform)
			{
				Transform = ShapeUtil.ParseTransform(value.ToLower());
				return;
			}
			if (name == SVGTags.sStroke)
			{
				GetStroke().Color = SVG.PaintServers.Parse(value);
				return;
			}
			if (name == SVGTags.sStrokeWidth)
			{
				GetStroke().Width = XmlUtil.ParseDouble(value);
				return;
			}
			if (name == SVGTags.sStrokeOpacity)
			{
				GetStroke().Opacity = XmlUtil.ParseDouble(value) * 100;
				return;
			}
			if (name == SVGTags.sStrokeDashArray)
			{
				if (value == "none")
				{
					GetStroke().StrokeArray = null;
					return;
				}
				ShapeUtil.StringSplitter sp = new ShapeUtil.StringSplitter(value);
				List<double> a = new List<double>();
				while(sp.More)
				{
					a.Add(sp.ReadNextValue());
				}
				GetStroke().StrokeArray = a.ToArray();
				return;
			}
			if (name == SVGTags.sStrokeLinecap)
			{
				GetStroke().LineCap = (Stroke.eLineCap)Enum.Parse(typeof(Stroke.eLineCap), value);
				return;
			}
			if (name == SVGTags.sStrokeLinejoin)
			{
				GetStroke().LineJoin= (Stroke.eLineJoin)Enum.Parse(typeof(Stroke.eLineJoin), value);
				return;
			}
			if (name == SVGTags.sFill)
			{
				GetFill().Color = SVG.PaintServers.Parse(value);
				return;
			}
			if (name == SVGTags.sFillOpacity)
			{
				GetFill().Opacity = XmlUtil.ParseDouble(value) * 100;
				return;
			}
			if (name == SVGTags.sFillRule)
			{
				GetFill().FillRule = (Fill.eFillRule)Enum.Parse(typeof(Fill.eFillRule), value);
				return;
			}
			if (name == SVGTags.sStyle)
			{
				foreach (ShapeUtil.Attribute item in XmlUtil.SplitStyle(value))
					Parse(item);
			}
			//********************** text *******************
			if (name == SVGTags.sFontFamily)
			{
				GetTextStyle().FontFamily = value;
				return;
			}
			if (name == SVGTags.sFontSize)
			{
				GetTextStyle().FontSize = XmlUtil.AttrValue(new ShapeUtil.Attribute(name, value));
				return;
			}
			if (name == SVGTags.sFontWeight)
			{
				GetTextStyle().Fontweight = (FontWeight)new FontWeightConverter().ConvertFromString(value);
				return;
			}
			if (name == SVGTags.sFontStyle)
			{
				GetTextStyle().Fontstyle = (FontStyle)new FontStyleConverter().ConvertFromString(value);
				return;
			}
			if (name == SVGTags.sTextDecoration)
			{
				TextDecoration t = new TextDecoration();
				if (value == "none")
					return;
				if (value == "underline")
					t.Location = TextDecorationLocation.Underline;
				if (value == "overline")
					t.Location = TextDecorationLocation.OverLine;
				if (value == "line-through")
					t.Location = TextDecorationLocation.Strikethrough;
				TextDecorationCollection tt = new TextDecorationCollection();
				tt.Add(t);
				GetTextStyle().TextDecoration = tt;
				return;
			}
			if (name == SVGTags.sTextAnchor)
			{
				if (value == "start")
					GetTextStyle().TextAlignment = TextAlignment.Left;
				if (value == "middle")
					GetTextStyle().TextAlignment = TextAlignment.Center;
				if (value == "end")
					GetTextStyle().TextAlignment = TextAlignment.Right;
				return;
			}
			if(name == "word-spacing")
			{
				GetTextStyle().WordSpacing = XmlUtil.AttrValue(new ShapeUtil.Attribute(name, value));
				return;
			}
			if(name == "letter-spacing")
			{
				GetTextStyle().LetterSpacing = XmlUtil.AttrValue(new ShapeUtil.Attribute(name, value));
				return;
			}
			if (name == "baseline-shift")
			{
				//GetTextStyle().BaseLineShift = XmlUtil.AttrValue(new ShapeUtil.Attribute(name, value));
				GetTextStyle().BaseLineShift = value;
				return;
			}
		}
		Stroke GetStroke()
		{
			if (m_stroke == null)
				m_stroke = new Stroke();
			return m_stroke;
		}
		public Fill GetFill()
		{
			if (m_fill == null)
				m_fill = new Fill();
			return m_fill;
		}
		protected TextStyle GetTextStyle()
		{
			if (m_textstyle == null)
				m_textstyle = new TextStyle(this);
			return m_textstyle;
		}
	}
	class RectangleShape : Shape
	{
		public double X { get; set; }
		public double Y { get; set; }
		public double Width { get; set; }
		public double Height { get; set; }
		public double RX { get; set; }
		public double RY { get; set; }
		public RectangleShape(XmlNode node) : base(node)
		{
			X = XmlUtil.AttrValue(node, "x", 0);
			Y = XmlUtil.AttrValue(node, "y", 0);
			Width = XmlUtil.AttrValue(node, "width", 0);
			Height = XmlUtil.AttrValue(node, "height", 0);
			RX = XmlUtil.AttrValue(node, "rx", 0);
			RY = XmlUtil.AttrValue(node, "ry", 0);
		}
	}
	class CircleShape : Shape
	{
		public double CX { get; set; }
		public double CY { get; set; }
		public double R { get; set; }
		public CircleShape(XmlNode node) : base(node)
		{
			CX = XmlUtil.AttrValue(node, "cx", 0);
			CY = XmlUtil.AttrValue(node, "cy", 0);
			R = XmlUtil.AttrValue(node, "r", 0);
		}
	}
	class EllipseShape : Shape
	{
		public double CX { get; set; }
		public double CY { get; set; }
		public double RX { get; set; }
		public double RY { get; set; }
		public EllipseShape(XmlNode node) : base(node)
		{
			CX = XmlUtil.AttrValue(node, "cx", 0);
			CY = XmlUtil.AttrValue(node, "cy", 0);
			RX = XmlUtil.AttrValue(node, "rx", 0);
			RY = XmlUtil.AttrValue(node, "ry", 0);
		}
	}
	class LineShape : Shape
	{
		public Point P1 {get; private set;}
		public Point P2 {get; private set;}
		public LineShape(XmlNode node) : base(node)
		{
			double x1 = XmlUtil.AttrValue(node, "x1", 0);
			double y1 = XmlUtil.AttrValue(node, "y1", 0);
			double x2 = XmlUtil.AttrValue(node, "x2", 0);
			double y2 = XmlUtil.AttrValue(node, "y2", 0);
			P1 = new Point(x1, y1);
			P2 = new Point(x2, y2);
		}
	}
	class PolylineShape : Shape
	{
		public Point[] Points {get; private set;}
		public PolylineShape(XmlNode node) : base(node)
		{
			string points = XmlUtil.AttrValue(node, SVGTags.sPoints, string.Empty);
			ShapeUtil.StringSplitter split = new ShapeUtil.StringSplitter(points);
			List<Point> list = new List<Point>();
			while (split.More)
			{
				list.Add(split.ReadNextPoint());
			}
			Points = list.ToArray();
		}
	}
	class PolygonShape : Shape
	{
		public Point[] Points {get; private set;}
		public PolygonShape(XmlNode node) : base(node)
		{
			string points = XmlUtil.AttrValue(node, SVGTags.sPoints, string.Empty);
			ShapeUtil.StringSplitter split = new ShapeUtil.StringSplitter(points);
			List<Point> list = new List<Point>();
			while (split.More)
			{
				list.Add(split.ReadNextPoint());
			}
			Points = list.ToArray();
		}
	}
	class UseShape : Shape
	{
		public double X { get; set; }
		public double Y { get; set; }
		public string hRef { get; set; }
		public UseShape(XmlNode node) : base(node)
		{
			X = XmlUtil.AttrValue(node, "x", 0);
			Y = XmlUtil.AttrValue(node, "y", 0);
			hRef = XmlUtil.AttrValue(node, "xlink:href", string.Empty);
			if (hRef.StartsWith("#"))
				hRef = hRef.Substring(1);
		}
	}
	class ImageShape : Shape
	{
		public double X { get; set; }
		public double Y { get; set; }
		public double Width { get; set; }
		public double Height { get; set; }
		public ImageSource ImageSource {get; private set;}
		public ImageShape(XmlNode node) : base(node)
		{
			X = XmlUtil.AttrValue(node, "x", 0);
			Y = XmlUtil.AttrValue(node, "y", 0);
			Width = XmlUtil.AttrValue(node, "width", 0);
			Height = XmlUtil.AttrValue(node, "height", 0);
			string hRef = XmlUtil.AttrValue(node, "xlink:href", string.Empty);
			if (hRef.Length > 0)
			{
				// filename given must be relative to the location of the svg file
				//string svgpath = System.IO.Path.GetDirectoryName(svg.Filename);
				//string filename = System.IO.Path.Combine(svgpath, hRef);

				//BitmapImage b = new  BitmapImage();
				//b.BeginInit();
				//b.UriSource = new Uri(filename, UriKind.RelativeOrAbsolute);
				//b.EndInit();
				//ImageSource = b;
			}
		}
	}
	class Group : Shape
	{
		List<Shape> m_elements = new List<Shape>();
		public IList<Shape> Elements
		{
			get { return m_elements.AsReadOnly(); }
		}

		Shape AddChild(Shape shape)
		{
			m_elements.Add(shape);
			shape.Parent = this;
			return shape;
		}
		public Group(XmlNode node, Shape parent) : base(node)
		{
			// parent on group must be set before children are added
			this.Parent = parent; 
			foreach (XmlNode childnode in node.ChildNodes)
			{
				Shape shape = AddToList(m_elements, childnode, this);
				if (shape != null)
					shape.Parent = this;
			}
			//if (Id.Length > 0)
			//	svg.AddShape(Id, this);
		}
		static public Shape AddToList(List<Shape> list, XmlNode childnode, Shape parent)
		{
			if (childnode.NodeType != XmlNodeType.Element)
				return null;
			if (childnode.Name == SVGTags.sShapeRect)
			{
				list.Add(new RectangleShape(childnode));
				return list[list.Count - 1];
			}
			if (childnode.Name == SVGTags.sShapeCircle)
			{
				list.Add(new CircleShape(childnode));
				return list[list.Count - 1];
			}
			if (childnode.Name == SVGTags.sShapeEllipse)
			{
				list.Add(new EllipseShape(childnode));
				return list[list.Count - 1];
			}
			if (childnode.Name == SVGTags.sShapeLine)
			{
				list.Add(new LineShape(childnode));
				return list[list.Count - 1];
			}
			if (childnode.Name == SVGTags.sShapePolyline)
			{
				list.Add(new PolylineShape(childnode));
				return list[list.Count - 1];
			}
			if (childnode.Name == SVGTags.sShapePolygon)
			{
				list.Add(new PolygonShape(childnode));
				return list[list.Count - 1];
			}
			if (childnode.Name == SVGTags.sShapePath)
			{
				list.Add(new PathShape(childnode));
				return list[list.Count - 1];
			}
			if (childnode.Name == SVGTags.sShapeGroup)
			{
				list.Add(new Group(childnode, parent));
				return list[list.Count - 1];
			}
			if (childnode.Name == SVGTags.sLinearGradient)
			{
				SVG.PaintServers.Create(childnode);
				return null;
			}
			if (childnode.Name == SVGTags.sRadialGradient)
			{
				SVG.PaintServers.Create(childnode);
				return null;
			}
			if (childnode.Name == SVGTags.sDefinitions)
			{
				ReadDefs(list, childnode);
				return null;
			}
			if (childnode.Name == SVGTags.sShapeUse)
			{
				list.Add(new UseShape(childnode));
				return list[list.Count - 1];
			}
			if (childnode.Name == SVGTags.sShapeImage)
			{
				list.Add(new ImageShape(childnode));
				return list[list.Count - 1];
			}
			if (childnode.Name == "text")
			{
				list.Add(new TextShape(childnode, parent));
				return list[list.Count - 1];
			}
			return null;
		}
		static void ReadDefs(List<Shape> list, XmlNode node)
		{
			list = new List<Shape>(); // temp list, not needed. 
			//ShapeGroups defined in the 'def' section is added the the 'Shapes' dictionary in SVG for later reference
			foreach (XmlNode childnode in node.ChildNodes)
			{
				if (childnode.Name == SVGTags.sLinearGradient)
				{
					SVG.PaintServers.Create(childnode);
					continue;
				}
				if (childnode.Name == SVGTags.sRadialGradient)
				{
					SVG.PaintServers.Create(childnode);
					continue;
				}
				Group.AddToList(list, childnode, null);
			}
		}
	}
}
