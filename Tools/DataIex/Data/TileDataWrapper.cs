using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIex
{
	public class TileDataWrapper
	{
		public TileData Tile;

		public static TileDataWrapper Read(BinaryReader reader)
		{
			uint magicHeader = reader.ReadUInt32();
			if (magicHeader != 0x01)
			{
				throw new Exception("Unexpected tile header!");
			}

			TileDataWrapper wrapper = new TileDataWrapper();
			wrapper.Tile = TileData.Read(reader);

			return wrapper;
		}

		public static void Write(TileDataWrapper data, BinaryWriter writer)
		{
			writer.Write(0x01);

			TileData.Write(data.Tile, writer);
		}
	}
}
