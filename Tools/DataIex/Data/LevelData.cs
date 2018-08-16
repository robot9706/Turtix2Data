using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIex
{
	public class LevelData
	{
		public string Name;

		public Layer[] Layers;

		public class Layer
		{
			public string Name;

			public uint TilesX;
			public uint TilesY;
		}

		public class LayerType0 : Layer
		{
			public int[] TileData;

			public static LayerType0 Read(BinaryReader reader)
			{
				LayerType0 layer = new LayerType0();

				layer.Name = reader.ReadTString();

				uint i1 = reader.ReadUInt32();
				uint i2 = reader.ReadUInt32();

				layer.TilesX = reader.ReadUInt32(); //as float
				layer.TilesY = reader.ReadUInt32(); //as float

				layer.TileData = new int[layer.TilesX * layer.TilesY];

				for (int x = 0; x < layer.TileData.Length; x++)
				{
					byte data = reader.ReadByte();
					if (data == 0)
					{
						layer.TileData[x] = -1;
					}
					else
					{
						layer.TileData[x] = reader.ReadInt32();
					}
				}

				byte b1 = reader.ReadByte();
				byte b2 = reader.ReadByte();

				float f1 = reader.ReadSingle();
				float f2 = reader.ReadSingle();
				float f3 = reader.ReadSingle();
				float f4 = reader.ReadSingle();

				return layer;
			}
		}

		public class LayerType1 : Layer
		{
			public int[] TileData;

			public static LayerType1 Read(BinaryReader reader)
			{
				LayerType1 layer = new LayerType1();

				layer.Name = reader.ReadTString();

				uint i1 = reader.ReadUInt32();
				uint i2 = reader.ReadUInt32();

				layer.TilesX = reader.ReadUInt32();
				layer.TilesY = reader.ReadUInt32();

				layer.TileData = new int[layer.TilesX * layer.TilesY];

				for (int x = 0; x < layer.TileData.Length; x++)
				{
					byte data = reader.ReadByte();
					if (data == 0)
					{
						layer.TileData[x] = -1;
					}
					else
					{
						layer.TileData[x] = reader.ReadInt32();
					}
				}

				return layer;
			}
		}

		public class LayerType2 : Layer
		{
			public int[] TileData;

			public static LayerType2 Read(BinaryReader reader)
			{
				LayerType2 layer = new LayerType2();

				layer.Name = reader.ReadTString();

				uint i1 = reader.ReadUInt32(); //as float
				uint i2 = reader.ReadUInt32(); //as float

				layer.TilesX = reader.ReadUInt32();
				layer.TilesY = reader.ReadUInt32();

				layer.TileData = new int[layer.TilesX * layer.TilesY];

				for (int x = 0; x < layer.TileData.Length; x++)
				{
					byte data = reader.ReadByte();
					if (data == 0)
					{
						layer.TileData[x] = -1;
					}
					else
					{
						layer.TileData[x] = reader.ReadInt32();

						uint i3 = reader.ReadUInt32();
					}
				}

				return layer;
			}
		}

		public class LayerType3 : Layer
		{
			public static LayerType3 Read(BinaryReader reader)
			{
				LayerType3 layer = new LayerType3();

				layer.Name = reader.ReadTString();

				uint i1 = reader.ReadUInt32();
				uint i2 = reader.ReadUInt32();

				uint numData = reader.ReadUInt32();
				for (uint x = 0; x < numData; x++)
				{
					uint objIndex = reader.ReadUInt32();

					int i3 = reader.ReadInt32();
					int i4 = reader.ReadInt32();
					int i5 = reader.ReadInt32();

					byte b1 = reader.ReadByte();
					if (b1 > 0)
					{
						uint i6 = reader.ReadUInt32();
						uint i7 = reader.ReadUInt32();
					}

					b1 = reader.ReadByte();
					if (b1 > 0)
					{
						uint i8 = reader.ReadUInt32();
						uint i9 = reader.ReadUInt32();
						uint i10 = reader.ReadUInt32();
						uint i11 = reader.ReadUInt32();
					}

					uint numActions = reader.ReadUInt32();
					for (uint y = 0; y < numActions; y++)
					{
						GameFunction func = GameFunction.ReadFunction(reader);
					}
				}

				return layer;
			}
		}

		public static LevelData Read(BinaryReader reader)
		{
			LevelData level = new LevelData();

			level.Name = reader.ReadTString();

			uint dataLength = reader.ReadUInt32();
			long dataEnd = reader.BaseStream.Position + dataLength;

			//Read level
			{
				uint widthPixel = reader.ReadUInt32();
				uint heightPixel = reader.ReadUInt32();

				uint i1 = reader.ReadUInt32();

				uint layerCount = reader.ReadUInt32();
				level.Layers = new Layer[layerCount];

				for (uint x = 0; x < layerCount; x++)
				{
					uint type = reader.ReadUInt32();

					Layer layer = null;
					switch (type)
					{
						case 0:
							layer = LayerType0.Read(reader);
							break;
						case 1:
							layer = LayerType1.Read(reader);
							break;
						case 2:
							layer = LayerType2.Read(reader);
							break;
						case 3:
							layer = LayerType3.Read(reader);
							break;

						default:
							throw new Exception();
					}

					level.Layers[x] = layer;
				}
			}

			uint levelActions = reader.ReadUInt32();
			for (uint x = 0; x < levelActions; x++)
			{
				GameFunction func = GameFunction.ReadFunction(reader);
			}

			if (reader.BaseStream.Position != dataEnd)
			{
				throw new Exception();
			}

			return level;
		}
	}
}
