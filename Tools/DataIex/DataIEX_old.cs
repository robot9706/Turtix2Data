//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace DataIex
//{
//    public class DataIEX
//    {
//		public string GameFolderName;
//		public string GameSaveFileName;

//		public string[] Strings;

//		public Lookup[] GraphicsLookup;

//		public string[] SoundLookup;

//		public SoundData[] SoundDataArray;

//		public FontData[] FontDataArray;

//		public CursorData[] CursorDataArray;

//		public TileData[] TileDataArray;

//		public ObjectData[] ObjectDataArray;

//		public CollisionData[] CollisionDataArray;

//		public ParticleData[] ParticleDataArray;

//		public LevelData[] LevelDataArray;

//		public GUIData[] GUIDataArray;

//		public void Read(BinaryReader reader)
//		{
//			//Header (Done)
//			{
//				uint headerLength = reader.ReadUInt32();

//				GameFolderName = reader.ReadTString();
//				GameSaveFileName = reader.ReadTString();
//			}

//			//Strings (Done)
//			{
//				uint stringNum = reader.ReadUInt32();

//				Strings = new string[(int)stringNum];

//				for (uint x = 0; x < stringNum; x++)
//				{
//					Strings[x] = reader.ReadTString();
//				}
//			}

//			//Graphics lookup table (Done)
//			{
//				uint numEntries = reader.ReadUInt32();

//				GraphicsLookup = new Lookup[numEntries];

//				for (uint x = 0; x < numEntries; x++)
//				{
//					byte header = reader.ReadByte();
//					if (header != 0x01)
//					{
//						throw new Exception("Unexpected header!");
//					}

//					uint dataLength = reader.ReadUInt32();
//					long dataEnd = reader.BaseStream.Position + dataLength;

//					string file = reader.ReadTString();
//					string iex = reader.ReadTString();

//					GraphicsLookup[x] = new Lookup(file, iex);

//					if (reader.BaseStream.Position != dataEnd)
//					{
//						throw new Exception();
//					}
//				}
//			}

//			//Sound lookup - Waves data (Done)
//			{
//				uint numEntries = reader.ReadUInt32();

//				SoundLookup = new string[numEntries];

//				for (uint x = 0; x < numEntries; x++)
//				{
//					uint dataLength = reader.ReadUInt32();
//					long dataEnd = reader.BaseStream.Position + dataLength;

//					SoundLookup[x] = reader.ReadTString();

//					if (reader.BaseStream.Position != dataEnd)
//					{
//						throw new Exception();
//					}
//				}
//			}

//			//Sound data table (Data skipped, structure unknown)
//			{
//				uint numEntries = reader.ReadUInt32();

//				SoundDataArray = new SoundData[numEntries];

//				for (uint x = 0; x < numEntries; x++)
//				{
//					byte header = reader.ReadByte();
//					if (header == 0x00)
//					{
//						SoundDataArray[x] = null; //??

//						continue;
//					}
//					else if (header != 0x01)
//					{
//						throw new Exception("Unexpected header!");
//					}

//					uint dataLength = reader.ReadUInt32();
//					long dataEnd = reader.BaseStream.Position + dataLength;

//					SoundDataArray[x] = new SoundData()
//					{
//						Raw = reader.ReadBytes((int)dataLength)
//					};

//					if (reader.BaseStream.Position != dataEnd)
//					{
//						throw new Exception();
//					}
//				}
//			}

//			//Font data (Done)
//			{
//				uint numEntries = reader.ReadUInt32();

//				FontDataArray = new FontData[numEntries];

//				for (uint x = 0; x < numEntries; x++)
//				{
//					byte header = reader.ReadByte();
//					if (header == 0x00)
//					{
//						FontDataArray[x] = null; //??

//						continue;
//					}
//					else if (header != 0x01)
//					{
//						throw new Exception("Unexpected header!");
//					}

//					uint dataLength = reader.ReadUInt32();
//					long dataEnd = reader.BaseStream.Position + dataLength;

//					FontDataArray[x] = FontData.Read(reader);

//					if (reader.BaseStream.Position != dataEnd)
//					{
//						throw new Exception();
//					}
//				}
//			}

//			//Cursor data (Reads, has unknowns)
//			{
//				uint numEntries = reader.ReadUInt32();

//				CursorDataArray = new CursorData[numEntries];

//				for (uint x = 0; x < numEntries; x++)
//				{
//					byte header = reader.ReadByte();
//					if (header == 0x00)
//					{
//						CursorDataArray[x] = null; //??

//						continue;
//					}
//					else if (header != 0x01)
//					{
//						throw new Exception("Unexpected header!");
//					}

//					uint dataLength = reader.ReadUInt32();
//					long dataEnd = reader.BaseStream.Position + dataLength;

//					CursorDataArray[x] = CursorData.Read(reader);

//					if (reader.BaseStream.Position != dataEnd)
//					{
//						throw new Exception();
//					}
//				}
//			}

//			//Tile data (Reads, lots of unknowns)
//			{
//				uint numEntries = reader.ReadUInt32();

//				TileDataArray = new TileData[numEntries];

//				for (uint x = 0; x < numEntries; x++)
//				{
//					byte header = reader.ReadByte();
//					if (header == 0x00)
//					{
//						TileDataArray[x] = null; //??

//						continue;
//					}
//					else if (header != 0x01)
//					{
//						throw new Exception("Unexpected header!");
//					}

//					uint dataLength = reader.ReadUInt32();
//					long dataEnd = reader.BaseStream.Position + dataLength;

//					uint numData = reader.ReadUInt32(); //???
//					if (numData != 1)
//					{
//						throw new Exception();
//					}

//					TileDataArray[x] = TileData.Read(reader);

//					if (reader.BaseStream.Position != dataEnd)
//					{
//						throw new Exception();
//					}
//				}
//			}

//			//Object data (Reads, lots of unknowns)
//			{
//				uint numEntries = reader.ReadUInt32();

//				ObjectDataArray = new ObjectData[numEntries];

//				for (uint x = 0; x < numEntries; x++)
//				{
//					byte header = reader.ReadByte();
//					if (header == 0x00)
//					{
//						ObjectDataArray[x] = null; //??

//						continue;
//					}
//					else if (header != 0x01)
//					{
//						throw new Exception("Unexpected header!");
//					}

//					uint dataLength = reader.ReadUInt32();
//					long dataEnd = reader.BaseStream.Position + dataLength;

//					ObjectDataArray[x] = ObjectData.Read(reader);

//					if (reader.BaseStream.Position != dataEnd)
//					{
//						throw new Exception();
//					}
//				}
//			}

//			//Collision data (Reads, has unknowns)
//			{
//				uint numEntries = reader.ReadUInt32();

//				CollisionDataArray = new CollisionData[numEntries];

//				for (uint x = 0; x < numEntries; x++)
//				{
//					byte header = reader.ReadByte();
//					if (header == 0x00)
//					{
//						CollisionDataArray[x] = null; //??

//						continue;
//					}
//					else if (header != 0x01)
//					{
//						throw new Exception("Unexpected header!");
//					}

//					uint dataLength = reader.ReadUInt32();
//					long dataEnd = reader.BaseStream.Position + dataLength;

//					CollisionDataArray[x] = CollisionData.Read(reader);

//					if (reader.BaseStream.Position != dataEnd)
//					{
//						throw new Exception();
//					}
//				}
//			}

//			//Particles (Reads, lots of unknowns)
//			{
//				uint numEntries = reader.ReadUInt32();

//				ParticleDataArray = new ParticleData[numEntries];

//				for (uint x = 0; x < numEntries; x++)
//				{
//					byte header = reader.ReadByte();
//					if (header == 0x00)
//					{
//						ParticleDataArray[x] = null; //??

//						continue;
//					}
//					else if (header != 0x01)
//					{
//						throw new Exception("Unexpected header!");
//					}

//					uint dataLength = reader.ReadUInt32();
//					long dataEnd = reader.BaseStream.Position + dataLength;

//					ParticleDataArray[x] = ParticleData.Read(reader);

//					if (reader.BaseStream.Position != dataEnd)
//					{
//						throw new Exception();
//					}
//				}
//			}

//			//Read 3 lookups (#1 unknown, others read)
//			{
//				//#1
//				uint dataLength = reader.ReadUInt32();
//				long dataEnd = reader.BaseStream.Position + dataLength;

//				//Unknown

//				reader.BaseStream.Position = dataEnd;

//				//#2
//				dataLength = reader.ReadUInt32();
//				dataEnd = reader.BaseStream.Position + dataLength;

//				uint numActions = reader.ReadUInt32();
//				for (uint x = 0; x < numActions; x++)
//				{
//					GameFunction func = GameFunction.ReadFunction(reader);
//				}

//				if (reader.BaseStream.Position != dataEnd)
//				{
//					throw new Exception();
//				}

//				//#3
//				dataLength = reader.ReadUInt32();
//				dataEnd = reader.BaseStream.Position + dataLength;

//				numActions = reader.ReadUInt32();
//				for (uint x = 0; x < numActions; x++)
//				{
//					GameFunction func = GameFunction.ReadFunction(reader);
//				}

//				if (reader.BaseStream.Position != dataEnd)
//				{
//					throw new Exception();
//				}
//			}

//			//Level data (Reads, has unknowns)
//			{
//				uint numEntries = reader.ReadUInt32();

//				LevelDataArray = new LevelData[numEntries];

//				for (int x = 0; x < numEntries; x++)
//				{
//					LevelDataArray[x] = LevelData.Read(reader);
//				}
//			}

//			//GUI data (Reads, has lots of unknowns)
//			{
//				uint numEntries = reader.ReadUInt32();

//				GUIDataArray = new GUIData[numEntries];

//				for (int x = 0; x < numEntries; x++)
//				{
//					GUIDataArray[x] = GUIData.Read(reader);
//				}
//			}

//			if (reader.BaseStream.Position != reader.BaseStream.Length)
//			{
//				throw new Exception();
//			}
//		}
//	}
//}
