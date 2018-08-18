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

		public uint Probably_SolidCollider;
		public uint Probably_HangCollider;
		public uint Probably_ClimbCollider;

		public static CollisionData Read(BinaryReader reader)
		{
			CollisionData data = new CollisionData();

			data.ColliderGraphics = reader.ReadUInt32();

			data.Probably_SolidCollider = reader.ReadUInt32();
			data.Probably_HangCollider = reader.ReadUInt32();
			data.Probably_ClimbCollider = reader.ReadUInt32();

			return data;
		}

		public static void Write(CollisionData data, BinaryWriter writer)
		{
			writer.Write(data.ColliderGraphics);

			writer.Write(data.Probably_SolidCollider);
			writer.Write(data.Probably_HangCollider);
			writer.Write(data.Probably_ClimbCollider);
		}
	}
}
