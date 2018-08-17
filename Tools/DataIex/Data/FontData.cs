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
		public byte A;
		public byte R;
		public byte G;
		public byte B;

		public static FontData Read(BinaryReader reader)
		{
			return new FontData()
			{
				GraphicsIndex = (int)reader.ReadUInt32(),
				A = reader.ReadByte(),
				R = reader.ReadByte(),
				G = reader.ReadByte(),
				B = reader.ReadByte(),
			};
		}

		public static void Write(FontData data, BinaryWriter writer)
		{
			writer.Write((uint)data.GraphicsIndex);
			writer.Write((byte)data.A);
			writer.Write((byte)data.R);
			writer.Write((byte)data.G);
			writer.Write((byte)data.B);
		}
	}
}
