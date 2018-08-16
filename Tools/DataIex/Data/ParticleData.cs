using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIex
{
	public class ParticleData
	{
		public static ParticleData Read(BinaryReader reader)
		{
			byte b1 = reader.ReadByte();
			float f1 = reader.ReadSingle();

			uint i1 = reader.ReadUInt32();
			for (int x = 0; x < i1; x++)
			{
				uint graphicsIndex = reader.ReadUInt32();

				uint i2 = reader.ReadUInt32();
				uint i3 = reader.ReadUInt32(); //as float
				uint i4 = reader.ReadUInt32(); //as float
				uint i5 = reader.ReadUInt32();
				uint i6 = reader.ReadUInt32();

				float f2 = reader.ReadSingle();

				byte b2 = reader.ReadByte();
				byte b3 = reader.ReadByte();
				byte b4 = reader.ReadByte();
				byte b5 = reader.ReadByte();

				for (int y = 0; y < 35; y++)
				{
					Vec2[] ar = ReadArray(reader);
				}
			}

			return null;
		}

		private static Vec2[] ReadArray(BinaryReader reader)
		{
			int len = reader.ReadInt32();

			Vec2[] ar = new Vec2[len];

			for (int x = 0; x < len; x++)
			{
				ar[x] = new Vec2(reader.ReadSingle(), reader.ReadSingle());
			}

			return ar;
		}
	}
}
