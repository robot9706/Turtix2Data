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
		public uint Type;

		public TileDataInternal Data;

		public abstract class TileDataInternal
		{
			public abstract void Read(BinaryReader reader);
			public abstract void Write(BinaryWriter writer);
		}

		public class DataType0 : TileDataInternal
		{
			public string Name;

			public uint GraphicsIndex;

			public uint GraphicsTileIndex;

			public byte FlipX;
			public byte FlipY;

			public override void Read(BinaryReader reader)
			{
				Name = reader.ReadTString();

				GraphicsIndex = reader.ReadUInt32();

				GraphicsTileIndex = reader.ReadUInt32();

				FlipX = reader.ReadByte();
				FlipY = reader.ReadByte();
			}

			public override void Write(BinaryWriter writer)
			{
				writer.WriteTString(Name);

				writer.Write(GraphicsIndex);

				writer.Write(GraphicsTileIndex);

				writer.Write(FlipX);
				writer.Write(FlipY);
			}
		}

		public class DataType1 : TileDataInternal
		{
			public string Name;

			public uint GraphicsIndex;

			public uint[] AnimationFrames;

			public uint AnimationSpeed;

			public byte IsLoopingAnimation;
			public byte UnknownByte2;

			public byte FlipX;
			public byte FlipY;

			public uint UnknownUInt2;

			public override void Read(BinaryReader reader)
			{
				Name = reader.ReadTString();

				GraphicsIndex = reader.ReadUInt32();

				uint frameCount = reader.ReadUInt32();
				AnimationFrames = new uint[frameCount];
				for (uint x = 0; x < frameCount; x++)
				{
					AnimationFrames[x] = reader.ReadUInt32();
				}

				AnimationSpeed = reader.ReadUInt32(); //Converted to float divided by / 100

				IsLoopingAnimation = reader.ReadByte();
				UnknownByte2 = reader.ReadByte();

				FlipX = reader.ReadByte();
				FlipY = reader.ReadByte();

				UnknownUInt2 = reader.ReadUInt32();
			}

			public override void Write(BinaryWriter writer)
			{
				writer.WriteTString(Name);

				writer.Write(GraphicsIndex);

				writer.Write((uint)AnimationFrames.Length);
				for (int x = 0; x < AnimationFrames.Length; x++)
				{
					writer.Write((uint)AnimationFrames[x]);
				}

				writer.Write(AnimationSpeed);

				writer.Write(IsLoopingAnimation);
				writer.Write(UnknownByte2);

				writer.Write(FlipX);
				writer.Write(FlipY);

				writer.Write(UnknownUInt2);
			}
		}

		/// <summary>
		/// Game does not seem to contain this type.
		/// </summary>
		public class DataType2 : TileDataInternal
		{
			public string Name;

			public uint GraphicsIndex;

			public uint UnknownUInt1;

			public byte UnknownByte1;
			public byte UnknownByte2;

			public uint UnknownUInt2;
			public uint UnknownUInt3;
			public uint UnknownUInt4;
			public uint UnknownUInt5;

			public override void Read(BinaryReader reader)
			{
				Name = reader.ReadTString();

				GraphicsIndex = reader.ReadUInt32();

				UnknownUInt1 = reader.ReadUInt32();

				UnknownByte1 = reader.ReadByte();
				UnknownByte2 = reader.ReadByte();

				UnknownUInt2 = reader.ReadUInt32();
				UnknownUInt3 = reader.ReadUInt32();
				UnknownUInt4 = reader.ReadUInt32();
				UnknownUInt5 = reader.ReadUInt32();
			}

			public override void Write(BinaryWriter writer)
			{
				writer.WriteTString(Name);

				writer.Write(GraphicsIndex);

				writer.Write(UnknownUInt1);

				writer.Write(UnknownByte1);
				writer.Write(UnknownByte2);

				writer.Write(UnknownUInt2);
				writer.Write(UnknownUInt3);
				writer.Write(UnknownUInt4);
				writer.Write(UnknownUInt5);
			}
		}

		public static TileData Read(BinaryReader reader)
		{
			TileData tile = new TileData();

			tile.Type = reader.ReadUInt32(); //?

			switch (tile.Type)
			{
				case 0:
					tile.Data = new DataType0();
					break;
				case 1:
					tile.Data = new DataType1();
					break;
				case 2:
					tile.Data = new DataType2();
					break;

				default:
					throw new Exception("Unknown tile type: " + tile.Type.ToString());
			}

			tile.Data.Read(reader);

			return tile;
		}

		public static void Write(TileData data, BinaryWriter writer)
		{
			writer.Write(data.Type);

			data.Data.Write(writer);
		}
	}
}
