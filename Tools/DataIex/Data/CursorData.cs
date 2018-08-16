using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIex
{
	public class CursorData
	{
		public int GraphicsIndex;

		public static CursorData Read(BinaryReader reader)
		{
			uint dn1 = reader.ReadUInt32();
			uint dn2 = reader.ReadUInt32();

			uint graphicsIndex = reader.ReadUInt32();

			float f0 = reader.ReadSingle();
			float f1 = reader.ReadSingle();
			float f2 = reader.ReadSingle();
			float f3 = reader.ReadSingle();

			uint dn3 = reader.ReadUInt32();
			for (int rdix = 0; rdix < dn3; rdix++)
			{
				uint dn4 = reader.ReadUInt32();
			}

			uint dn5 = reader.ReadUInt32(); //Converted to float

			byte db1 = reader.ReadByte();
			byte db2 = reader.ReadByte();

			uint dn6 = reader.ReadUInt32();
			uint dn7 = reader.ReadUInt32();
			uint dn8 = reader.ReadUInt32();
			uint dn9 = reader.ReadUInt32();

			uint dn10 = reader.ReadUInt32();

			return new CursorData()
			{
				GraphicsIndex = (int)graphicsIndex
			};
		}
	}
}
