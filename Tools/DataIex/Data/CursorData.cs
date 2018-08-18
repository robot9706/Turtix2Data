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
		public uint UnknownUInt1;
		public uint UnknownUInt2;

		public int GraphicsIndex;

		public float ColorRed;
		public float ColorGreen;
		public float ColorBlue;
		public float ColorAlpha;

		public uint[] GraphicsTileIndexArray;

		public uint AnimationSpeed;

		public byte IsLooping;
		public byte UnknownByte2;

		public uint OffsetX;
		public uint OffsetY;

		public uint Width;
		public uint Height;

		public uint UnknownUInt8;

		public static CursorData Read(BinaryReader reader)
		{
			CursorData cursor = new CursorData();

			cursor.UnknownUInt1 = reader.ReadUInt32();
			cursor.UnknownUInt2 = reader.ReadUInt32();

			cursor.GraphicsIndex = (int)reader.ReadUInt32();

			cursor.ColorRed = reader.ReadSingle();
			cursor.ColorGreen = reader.ReadSingle();
			cursor.ColorBlue = reader.ReadSingle();
			cursor.ColorAlpha = reader.ReadSingle();

			uint unknownLength = reader.ReadUInt32();
			cursor.GraphicsTileIndexArray = new uint[unknownLength];
			for (int x = 0; x < unknownLength; x++)
			{
				cursor.GraphicsTileIndexArray[x] = reader.ReadUInt32();
			}

			cursor.AnimationSpeed = reader.ReadUInt32(); //Converted to float

			cursor.IsLooping = reader.ReadByte();
			cursor.UnknownByte2 = reader.ReadByte();

			cursor.OffsetX = reader.ReadUInt32();
			cursor.OffsetY = reader.ReadUInt32();
			cursor.Width = reader.ReadUInt32();
			cursor.Height = reader.ReadUInt32();

			cursor.UnknownUInt8 = reader.ReadUInt32();

			return cursor;
		}

		public static void Write(CursorData cursor, BinaryWriter writer)
		{
			writer.Write(cursor.UnknownUInt1);
			writer.Write(cursor.UnknownUInt2);

			writer.Write(cursor.GraphicsIndex);

			writer.Write(cursor.ColorRed);
			writer.Write(cursor.ColorGreen);
			writer.Write(cursor.ColorBlue);
			writer.Write(cursor.ColorAlpha);

			writer.Write((uint)cursor.GraphicsTileIndexArray.Length);
			for (int x = 0; x < cursor.GraphicsTileIndexArray.Length; x++)
			{
				writer.Write(cursor.GraphicsTileIndexArray[x]);
			}

			writer.Write(cursor.AnimationSpeed);

			writer.Write(cursor.IsLooping);
			writer.Write(cursor.UnknownByte2);

			writer.Write(cursor.OffsetX);
			writer.Write(cursor.OffsetY);
			writer.Write(cursor.Width);
			writer.Write(cursor.Height);

			writer.Write(cursor.UnknownUInt8);
		}
	}
}
