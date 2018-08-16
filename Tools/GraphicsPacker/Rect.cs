using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsPacker
{
	struct Rect
	{
		public int X;
		public int Y;
		public int Width;
		public int Height;

		public Rect(int x, int y, int w, int h)
		{
			X = x;
			Y = y;
			Width = w;
			Height = h;
		}
	}
}
