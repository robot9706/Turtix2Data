using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIex
{
	public class FontData
	{
		public int GraphicsIndex;

		public uint Color;

		public static FontData Read(BinaryReader reader)
		{
			return new FontData()
			{
				GraphicsIndex = (int)reader.ReadUInt32(),
				Color = reader.ReadUInt32()
			};
		}
	}
}
