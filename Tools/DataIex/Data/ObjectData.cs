using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIex
{
	public class ObjectData
	{
		public static ObjectData Read(BinaryReader reader)
		{
			uint i1 = reader.ReadUInt32(); //As float
			uint i2 = reader.ReadUInt32(); //As float

			uint i3 = reader.ReadUInt32();
			uint i4 = reader.ReadUInt32();
			uint i5 = reader.ReadUInt32();
			uint i6 = reader.ReadUInt32();

			string s1 = reader.ReadTString();

			int i7 = reader.ReadInt32();
			if (i7 > 0) //Load some kind of array
			{
				uint[] ar1 = new uint[i7];

				for (int x = 0; x < i7; x++)
				{
					ar1[x] = reader.ReadUInt32();
				}
			}

			uint i8 = reader.ReadUInt32();
			if (i8 > 0) //Load some kind of array
			{
				uint[] ar2 = new uint[i8];

				for (int x = 0; x < i8; x++)
				{
					ar2[x] = reader.ReadUInt32();
				}
			}

			//Num tiles?
			uint numTiles = reader.ReadUInt32();

			TileData[] tiles = new TileData[numTiles];
			for (uint tx = 0; tx < numTiles; tx++)
			{
				tiles[tx] = TileData.Read(reader);
			}

			uint i9 = reader.ReadUInt32(); //Function num?
			if (i9 > 0)
			{
				//0x004375A6
				for (int fx = 0; fx < i9; fx++)
				{
					GameFunction func = GameFunction.ReadFunction(reader);
				}
			}

			return null;
		}
	}
}
