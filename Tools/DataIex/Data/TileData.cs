using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIex
{
	public class TileData
	{
		public long FileOffset;

		public string Type;

		public int GraphicsIndex;

		public uint[] AnimationFrames;

		public static TileData Read(BinaryReader reader)
		{
			long ofs = reader.BaseStream.Position - 1;

			uint flag = reader.ReadUInt32(); //?

			if (flag == 0) //Static?
			{
				string type = reader.ReadTString();

				uint graphics = reader.ReadUInt32();

				uint dn = reader.ReadUInt32();

				byte db1 = reader.ReadByte();
				byte db2 = reader.ReadByte();

				return new TileData()
				{
					Type = type,
					GraphicsIndex = (int)graphics,

					FileOffset = ofs
				};
			}
			else if (flag == 1) //Animated?
			{
				string type = reader.ReadTString();

				uint graphics = reader.ReadUInt32();

				uint frameCount = reader.ReadUInt32();

				uint[] frames = new uint[frameCount];

				for (uint x = 0; x < frameCount; x++)
				{
					frames[x] = reader.ReadUInt32();
				}

				uint dn = reader.ReadUInt32(); //Converted to float divided by / 100

				byte db1 = reader.ReadByte();
				byte db2 = reader.ReadByte();

				//?????????????

				byte db3 = reader.ReadByte();
				byte db4 = reader.ReadByte();

				uint dn2 = reader.ReadUInt32();

				return new TileData()
				{
					Type = type,
					GraphicsIndex = (int)graphics,
					AnimationFrames = frames,

					FileOffset = ofs,
				};
			}
			else if (flag == 2)
			{
				string type = reader.ReadTString();

				uint graphics = reader.ReadUInt32();

				uint i1 = reader.ReadUInt32();

				byte b1 = reader.ReadByte();
				byte b2 = reader.ReadByte();

				uint i2 = reader.ReadUInt32();
				uint i3 = reader.ReadUInt32();
				uint i4 = reader.ReadUInt32();
				uint i5 = reader.ReadUInt32();

				return new TileData()
				{
					Type = type,
					GraphicsIndex = (int)graphics
				};
			}
			else
			{
				throw new Exception("Unknown tile type");
			}
		}
	}
}
