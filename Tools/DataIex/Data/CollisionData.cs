using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIex
{
	public class CollisionData
	{
		public uint ColliderGraphics;

		public static CollisionData Read(BinaryReader reader)
		{
			CollisionData data = new CollisionData();

			data.ColliderGraphics = reader.ReadUInt32();

			uint i1 = reader.ReadUInt32();
			uint i2 = reader.ReadUInt32();
			uint i3 = reader.ReadUInt32();

			return data;
		}
	}
}
