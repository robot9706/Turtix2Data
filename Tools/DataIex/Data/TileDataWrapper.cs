using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIex.Data
{
	public class TileDataWrapper
	{
		public static TileData Read(BinaryReader reader)
		{
			uint magicHeader = reader.ReadUInt32();
			if (magicHeader != 0x01)
			{
				throw new Exception("Unexpected tile header!");
			}

			return TileData.Read(reader);
		}

		public static void Write(TileData data, BinaryWriter writer)
		{
			writer.Write(0x01);
			
			//TileData.Write(reader);
		}
	}
}
