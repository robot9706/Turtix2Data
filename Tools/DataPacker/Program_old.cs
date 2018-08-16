//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace DataPacker
//{
//	class Program
//	{
//		static void Main(string[] args)
//		{
//			if (args.Length != 3)
//			{
//				Console.WriteLine("Invalid params!");

//				return;
//			}

//			switch (args[0])
//			{
//				case "p":
//				case "pack":
//					if (!Directory.Exists(args[2]))
//					{
//						Console.WriteLine("Input directory not found!");
//					}

//					Pack(args[1], args[2]);
//					break;

//				case "u":
//				case "unpack":
//					if (!File.Exists(args[1]))
//					{
//						Console.WriteLine("Input file not found!");

//						return;
//					}

//					string dir = args[2];
//					Directory.CreateDirectory(dir);

//					Unpack(args[1], dir);
//					break;
//			}
//		}

//		static void Pack(string iex, string dir)
//		{

//		}

//		static void Unpack(string iexDir, string dir)
//		{
//			using (FileStream fs = File.OpenRead(iexDir))
//			{
//				using (BinaryReader br = new BinaryReader(fs))
//				{
//					DataIEX iex = new DataIEX();
//					iex.Read(br);
//				}
//			}
//		}

//		class Lookup
//		{
//			public string File;
//			public string IEX;

//			public Lookup(string file, string iex)
//			{
//				File = file;
//				IEX = iex;
//			}
//		}

//		class SoundData
//		{
//			public byte[] Raw;
//		}

//		class FontData
//		{
//			public int GraphicsIndex;

//			public uint Color;

//			public static FontData Read(BinaryReader reader)
//			{
//				return new FontData()
//				{
//					GraphicsIndex = (int)reader.ReadUInt32(),
//					Color = reader.ReadUInt32()
//				};
//			}
//		}

//		class CursorData
//		{
//			public int GraphicsIndex;

//			public static CursorData Read(BinaryReader reader)
//			{
//				uint dn1 = reader.ReadUInt32();
//				uint dn2 = reader.ReadUInt32();

//				uint graphicsIndex = reader.ReadUInt32();

//				float f0 = reader.ReadSingle();
//				float f1 = reader.ReadSingle();
//				float f2 = reader.ReadSingle();
//				float f3 = reader.ReadSingle();

//				uint dn3 = reader.ReadUInt32();
//				for (int rdix = 0; rdix < dn3; rdix++)
//				{
//					uint dn4 = reader.ReadUInt32();
//				}

//				uint dn5 = reader.ReadUInt32(); //Converted to float

//				byte db1 = reader.ReadByte();
//				byte db2 = reader.ReadByte();

//				uint dn6 = reader.ReadUInt32();
//				uint dn7 = reader.ReadUInt32();
//				uint dn8 = reader.ReadUInt32();
//				uint dn9 = reader.ReadUInt32();

//				uint dn10 = reader.ReadUInt32();

//				return new CursorData()
//				{
//					GraphicsIndex = (int)graphicsIndex
//				};
//			}
//		}

//		class TileData
//		{
//			public long FileOffset;

//			public string Type;

//			public int GraphicsIndex;

//			public uint[] AnimationFrames;

//			public static TileData Read(BinaryReader reader)
//			{
//				long ofs = reader.BaseStream.Position - 1;

//				uint flag = reader.ReadUInt32(); //?

//				if (flag == 0) //Static?
//				{
//					string type = reader.ReadTString();

//					uint graphics = reader.ReadUInt32();

//					uint dn = reader.ReadUInt32();

//					byte db1 = reader.ReadByte();
//					byte db2 = reader.ReadByte();

//					return new TileData()
//					{
//						Type = type,
//						GraphicsIndex = (int)graphics,

//						FileOffset = ofs
//					};
//				}
//				else if (flag == 1) //Animated?
//				{
//					string type = reader.ReadTString();

//					uint graphics = reader.ReadUInt32();

//					uint frameCount = reader.ReadUInt32();

//					uint[] frames = new uint[frameCount];

//					for (uint x = 0; x < frameCount; x++)
//					{
//						frames[x] = reader.ReadUInt32();
//					}

//					uint dn = reader.ReadUInt32(); //Converted to float divided by / 100

//					byte db1 = reader.ReadByte();
//					byte db2 = reader.ReadByte();

//					//?????????????

//					byte db3 = reader.ReadByte();
//					byte db4 = reader.ReadByte();

//					uint dn2 = reader.ReadUInt32();

//					return new TileData()
//					{
//						Type = type,
//						GraphicsIndex = (int)graphics,
//						AnimationFrames = frames,

//						FileOffset = ofs,
//					};
//				}
//				else if (flag == 2)
//				{
//					string type = reader.ReadTString();

//					uint graphics = reader.ReadUInt32();

//					uint i1 = reader.ReadUInt32();

//					byte b1 = reader.ReadByte();
//					byte b2 = reader.ReadByte();

//					uint i2 = reader.ReadUInt32();
//					uint i3 = reader.ReadUInt32();
//					uint i4 = reader.ReadUInt32();
//					uint i5 = reader.ReadUInt32();

//					return new TileData()
//					{
//						Type = type,
//						GraphicsIndex = (int)graphics
//					};
//				}
//				else
//				{
//					throw new Exception("Unknown tile type");
//				}
//			}
//		}

//		class ObjectData
//		{
//			public static ObjectData Read(BinaryReader reader)
//			{
//				uint i1 = reader.ReadUInt32(); //As float
//				uint i2 = reader.ReadUInt32(); //As float

//				uint i3 = reader.ReadUInt32();
//				uint i4 = reader.ReadUInt32();
//				uint i5 = reader.ReadUInt32();
//				uint i6 = reader.ReadUInt32();

//				string s1 = reader.ReadTString();

//				int i7 = reader.ReadInt32();
//				if (i7 > 0) //Load some kind of array
//				{
//					uint[] ar1 = new uint[i7];

//					for(int x = 0; x < i7; x++)
//					{
//						ar1[x] = reader.ReadUInt32();
//					}
//				}

//				uint i8 = reader.ReadUInt32();
//				if (i8 > 0) //Load some kind of array
//				{
//					uint[] ar2 = new uint[i8];

//					for (int x = 0; x < i8; x++)
//					{
//						ar2[x] = reader.ReadUInt32();
//					}
//				}

//				//Num tiles?
//				uint numTiles = reader.ReadUInt32();

//				TileData[] tiles = new TileData[numTiles];
//				for (uint tx = 0; tx < numTiles; tx++)
//				{
//					tiles[tx] = TileData.Read(reader);
//				}

//				uint i9 = reader.ReadUInt32(); //Function num?
//				if (i9 > 0)
//				{
//					//0x004375A6
//					for (int fx = 0; fx < i9; fx++)
//					{
//						Function func = Function.ReadFunction(reader);
//					}
//				}

//				return null;
//			}
//		}

//		class Function
//		{
//			public string Name;

//			public Action[] Actions;

//			public static Function ReadFunction(BinaryReader reader)
//			{
//				Function func = new Function();

//				func.Name = reader.ReadTString();

//				int i1_1 = reader.ReadInt32();
//				int i1_2 = reader.ReadInt32();

//				if (i1_2 > 0)
//				{
//					func.Actions = new Action[i1_2];
//					for (int x = 0; x < i1_2; x++)
//					{
//						func.Actions[x] = Action.ReadAction(reader);
//					}
//				}

//				return func;
//			}
//		}

//		class Action
//		{
//			//0x21 -> 0x422870

//			//Exec table around: 0x49A3B8
//			public static Action ReadAction(BinaryReader reader)
//			{
//				uint type = reader.ReadUInt32();

//				if (type > 0x73)
//				{
//					throw new Exception("Invalid type?");
//				}

//				uint i1;
//				uint i2;
//				uint i3;
//				uint i4;
//				uint i5;

//				string s1;
//				string s2;
//				string s3;
//				string s4;
//				string s5;
//				string s6;
//				string s7;

//				byte b1;
//				byte b2;
//				byte b3;

//				float f1;
//				float f2;

//				switch (type)
//				{
//					//0x00-0x0F
//					case 0x00: //0
//						s1 = reader.ReadTString();
//						break;
//					case 0x01: //1
//						s1 = reader.ReadTString();

//						i1 = reader.ReadUInt32();
//						break;
//					case 0x02: //2
//						s1 = reader.ReadTString();
//						break;
//					case 0x03: //3
//						i1 = reader.ReadUInt32();
//						i2 = reader.ReadUInt32();

//						for (int x = 0; x < i2; x++)
//						{
//							s1 = reader.ReadTString();
//						}
//						break;
//					case 0x04: //4
//						s1 = reader.ReadTString();
//						break;
//					case 0x05: //5
//						s1 = reader.ReadTString();
//						break;
//					case 0x06: //6
//						s1 = reader.ReadTString();
//						s2 = reader.ReadTString();

//						b1 = reader.ReadByte();

//						s3 = reader.ReadTString();
//						s4 = reader.ReadTString();
//						break;
//					case 0x07: //7
//						s1 = reader.ReadTString();

//						i1 = reader.ReadUInt32();
//						break;
//					case 0x08: //8
//						i1 = reader.ReadUInt32();

//						for (uint x = 0; x < i1; x++)
//						{
//							s1 = reader.ReadTString();
//						}

//						i2 = reader.ReadUInt32();

//						s2 = reader.ReadTString();
//						break;
//					case 0x09: //9
//						s1 = reader.ReadTString();
//						break;
//					case 0x0A: //10
//						s1 = reader.ReadTString();
//						s2 = reader.ReadTString();
//						break;
//					case 0x0B: //11
//						i1 = reader.ReadUInt32();
//						i2 = reader.ReadUInt32();
//						for (uint x = 0; x < i2; x++)
//						{
//							s1 = reader.ReadTString();
//						}
//						break;
//					case 0x0C: //12
//						s1 = reader.ReadTString();
//						break;
//					case 0x0D: //13
//						s1 = reader.ReadTString();
//						break;
//					case 0x0E: //14
//						s1 = reader.ReadTString();

//						i1 = reader.ReadUInt32();
//						break;
//					case 0x0F: //15
//						s1 = reader.ReadTString();
//						s2 = reader.ReadTString();

//						i1 = reader.ReadUInt32();
//						break;



//					//0x10-0x1F
//					case 0x10: //16
//						i1 = reader.ReadUInt32();

//						for (uint x = 0; x < i1; x++)
//						{
//							s1 = reader.ReadTString();
//						}

//						s2 = reader.ReadTString();
//						break;
//					case 0x11: //17
//						s1 = reader.ReadTString();
//						i1 = reader.ReadUInt32();
//						break;
//					case 0x12: //18
//						s1 = reader.ReadTString();
//						i1 = reader.ReadUInt32();
//						break;
//					case 0x13: //19
//						s1 = reader.ReadTString();
//						i1 = reader.ReadUInt32();
//						break;
//					case 0x14: //20
//						s1 = reader.ReadTString();
//						s2 = reader.ReadTString();
//						i1 = reader.ReadUInt32();
//						i2 = reader.ReadUInt32();
//						break;
//					case 0x15: //21
//						s1 = reader.ReadTString();
//						s2 = reader.ReadTString();
//						s3 = reader.ReadTString();

//						b1 = reader.ReadByte();
//						b2 = reader.ReadByte();
//						b3 = reader.ReadByte();
//						break;
//					case 0x16: //22
//						s1 = reader.ReadTString();
//						s2 = reader.ReadTString();

//						b1 = reader.ReadByte();

//						i1 = reader.ReadUInt32();

//						b2 = reader.ReadByte();
//						b3 = reader.ReadByte();

//						s2 = reader.ReadTString();
//						break;
//					case 0x17: //23
//						s1 = reader.ReadTString();

//						i1 = reader.ReadUInt32(); //Particle index?
//						i2 = reader.ReadUInt32();
//						i3 = reader.ReadUInt32(); //as float
//						i4 = reader.ReadUInt32(); //as float

//						b1 = reader.ReadByte();

//						s2 = reader.ReadTString();
//						break;
//					case 0x18: //24
//						s1 = reader.ReadTString();

//						i1 = reader.ReadUInt32(); //Particle index?

//						s2 = reader.ReadTString();
//						s3 = reader.ReadTString();
//						s4 = reader.ReadTString();
//						break;
//					case 0x19: //25
//						s1 = reader.ReadTString();

//						b1 = reader.ReadByte();

//						i1 = reader.ReadUInt32(); //as float
//						i2 = reader.ReadUInt32(); //as float

//						f1 = reader.ReadSingle();
//						f2 = reader.ReadSingle();
//						break;
//					case 0x1A: //26
//						s1 = reader.ReadTString();

//						b1 = reader.ReadByte();

//						i1 = reader.ReadUInt32(); //as float

//						f1 = reader.ReadSingle();
//						f2 = reader.ReadSingle();
//						break;
//					case 0x1B: //27
//						s1 = reader.ReadTString();

//						i1 = reader.ReadUInt32();
//						i2 = reader.ReadUInt32();

//						for (uint x = 0; x < i2; x++)
//						{
//							s2 = reader.ReadTString();
//						}
//						break;
//					case 0x1C: //28
//						s1 = reader.ReadTString();

//						i1 = reader.ReadUInt32();

//						s2 = reader.ReadTString();
//						break;
//					case 0x1D: //29
//						s1 = reader.ReadTString();
//						break;
//					case 0x1E: //30
//						i1 = reader.ReadUInt32();
//						i2 = reader.ReadUInt32();
//						i3 = reader.ReadUInt32();
//						break;
//					case 0x1F: //31
//						s1 = reader.ReadTString();
//						s2 = reader.ReadTString();
//						break;



//					//0x20-0x2F
//					case 0x20: //32
//						i1 = reader.ReadUInt32();

//						for (int x = 0; x < i1; x++)
//						{
//							s1 = reader.ReadTString();
//						}

//						i2 = reader.ReadUInt32();

//						s2 = reader.ReadTString();
//						break;
//					case 0x21: //33
//						i1 = reader.ReadUInt32();

//						for (int i = 0; i < i1; i++)
//						{
//							s1 = reader.ReadTString();
//						}

//						i2 = reader.ReadUInt32();
//						s2 = reader.ReadTString();

//						i3 = reader.ReadUInt32();
//						for (int x = 0; x < i3; x++)
//						{
//							Action.ReadAction(reader);
//						}

//						i4 = reader.ReadUInt32();
//						for (int x = 0; x < i4; x++)
//						{
//							Action.ReadAction(reader);
//						}
//						break;
//					case 0x22: //34
//						s1 = reader.ReadTString();

//						b1 = reader.ReadByte();

//						i1 = reader.ReadUInt32(); //as float

//						f1 = reader.ReadSingle();
//						f2 = reader.ReadSingle();

//						s1 = reader.ReadTString();
//						break;
//					case 0x23: //35
//						s1 = reader.ReadTString();

//						b1 = reader.ReadByte();

//						i1 = reader.ReadUInt32(); //as float

//						i2 = reader.ReadUInt32();

//						s2 = reader.ReadTString();
//						break;
//					case 0x24: //36
//						s1 = reader.ReadTString();

//						i1 = reader.ReadUInt32();
//						i2 = reader.ReadUInt32();
//						break;
//					case 0x25: //37
//						s1 = reader.ReadTString();
//						s2 = reader.ReadTString();
//						break;
//					case 0x26: //38
//						s1 = reader.ReadTString();
//						s2 = reader.ReadTString();

//						i1 = reader.ReadUInt32(); //as float
//						i2 = reader.ReadUInt32(); //as float

//						b1 = reader.ReadByte();

//						i3 = reader.ReadUInt32();
//						break;
//					case 0x27: //39
//						s1 = reader.ReadTString();

//						b1 = reader.ReadByte();

//						i1 = reader.ReadUInt32(); //as float? * (float)(0x000000E0)

//						s2 = reader.ReadTString();
//						break;
//					case 0x28: //40
//						s1 = reader.ReadTString();
//						break;
//					case 0x29: //41
//						s1 = reader.ReadTString();

//						i1 = reader.ReadUInt32();
//						i2 = reader.ReadUInt32();

//						f1 = reader.ReadSingle();
//						//?? logics based on this value (>0)

//						f2 = reader.ReadSingle();
//						//?? logics based on this value (>0)

//						s2 = reader.ReadTString();
//						break;
//					case 0x2A: //42
//						//Doesnt read, inits struct ?
//						break;
//					case 0x2B: //43
//						s1 = reader.ReadTString();
//						break;
//					case 0x2C: //44
//						s1 = reader.ReadTString();

//						i1 = reader.ReadUInt32();

//						s2 = reader.ReadTString();
//						break;
//					case 0x2D: //45
//						s1 = reader.ReadTString();

//						i1 = reader.ReadUInt32();

//						i2 = reader.ReadUInt32();
//						for (uint x = 0; x < i2; x++)
//						{
//							s1 = reader.ReadTString();
//						}
//						break;
//					case 0x2E: //46
//						s1 = reader.ReadTString();

//						f1 = reader.ReadSingle();
//						f2 = reader.ReadSingle();
//						break;
//					case 0x2F: //47
//						i1 = reader.ReadUInt32();

//						s1 = reader.ReadTString();
//						s2 = reader.ReadTString();
//						break;



//					//0x30-0x3F
//					case 0x30: //48
//						i1 = reader.ReadUInt32();
//						break;
//					case 0x31: //49
//						s1 = reader.ReadTString();
//						i1 = reader.ReadUInt32();

//						s2 = reader.ReadTString();

//						i2 = reader.ReadUInt32(); //as float
//						i3 = reader.ReadUInt32(); //as float

//						s3 = reader.ReadTString();

//						//Load particle data: i1
//						break;
//					case 0x32: //50
//						s1 = reader.ReadTString();

//						i1 = reader.ReadUInt32(); //as float * (sum number)

//						i2 = reader.ReadUInt32();
//						i3 = reader.ReadUInt32();
//						i4 = reader.ReadUInt32();
//						i5 = reader.ReadUInt32();

//						s2 = reader.ReadTString();
//						break;
//					case 0x33: //51
//						i1 = reader.ReadUInt32(); //Sound index?

//						//Load sound with index
//						break;
//					case 0x34: //52
//						s1 = reader.ReadTString();

//						i1 = reader.ReadUInt32();
//						break;
//					case 0x35: //53
//						s1 = reader.ReadTString();
//						i1 = reader.ReadUInt32();
//						s2 = reader.ReadTString();

//						i2 = reader.ReadUInt32(); //as float
//						i3 = reader.ReadUInt32(); //as float
//						i4 = reader.ReadUInt32(); //as float
//						break;
//					case 0x36: //54
//						s1 = reader.ReadTString();
//						i1 = reader.ReadUInt32();
//						break;
//					case 0x37: //55
//						s1 = reader.ReadTString();

//						i1 = reader.ReadUInt32();
//						i2 = reader.ReadUInt32();
//						i3 = reader.ReadUInt32();
//						i4 = reader.ReadUInt32();

//						s2 = reader.ReadTString();
//						break;
//					case 0x38: //56
//						s1 = reader.ReadTString();
//						s2 = reader.ReadTString();
//						break;
//					case 0x39: //57: Named func: LoadCursor
//						i1 = reader.ReadUInt32();

//						i2 = reader.ReadUInt32();
//						for (int x = 0; x < i2; x++)
//						{
//							s1 = reader.ReadTString();
//						}
//						break;
//					case 0x3A: //58
//						s1 = reader.ReadTString();
//						break;
//					case 0x3B: //59
//						s1 = reader.ReadTString();

//						i1 = reader.ReadUInt32();
//						i2 = reader.ReadUInt32();
//						break;
//					case 0x3C: //60
//						s1 = reader.ReadTString();
//						s2 = reader.ReadTString();
//						s3 = reader.ReadTString();
//						break;
//					case 0x3D: //61
//						s1 = reader.ReadTString();
//						s2 = reader.ReadTString();
//						break;
//					case 0x3E: //62
//						s1 = reader.ReadTString();
//						i1 = reader.ReadUInt32(); //Sound index
//						b1 = reader.ReadByte();
//						s2 = reader.ReadTString();

//						//Load sound with index
//						break;
//					case 0x3F: //63
//						i1 = reader.ReadUInt32(); //as float
//						i2 = reader.ReadUInt32(); //as float

//						s1 = reader.ReadTString();
//						break;



//					//0x40-0x4F
//					case 0x40: //64
//						i1 = reader.ReadUInt32(); //as float
//						s1 = reader.ReadTString();
//						break;
//					case 0x41: //65
//						i1 = reader.ReadUInt32();
//						i2 = reader.ReadUInt32();
//						i3 = reader.ReadUInt32();

//						for (uint x = 0; x < i3; x++)
//						{
//							s1 = reader.ReadTString();
//						}
//						break;
//					case 0x42: //66
//						i1 = reader.ReadUInt32();

//						i2 = reader.ReadUInt32();
//						for (uint x = 0; x < i2; x++)
//						{
//							s1 = reader.ReadTString();
//						}
//						break;
//					case 0x43: //67
//						s1 = reader.ReadTString();
//						i1 = reader.ReadUInt32();
//						break;
//					case 0x44: //68
//						s1 = reader.ReadTString();
//						break;
//					case 0x45: //69
//						s1 = reader.ReadTString();
//						s2 = reader.ReadTString();

//						i1 = reader.ReadUInt32();
//						i2 = reader.ReadUInt32(); //as float
//						i2 = reader.ReadUInt32(); //as float
//						break;
//					case 0x46: //70
//						i1 = reader.ReadUInt32();

//						i2 = reader.ReadUInt32();
//						for (uint x = 0; x < i2; x++)
//						{
//							s1 = reader.ReadTString();
//						}

//						s2 = reader.ReadTString();
//						break;
//					case 0x47: //71
//						s1 = reader.ReadTString();

//						i1 = reader.ReadUInt32();

//						i2 = reader.ReadUInt32();
//						for (uint x = 0; x < i2; x++)
//						{
//							s1 = reader.ReadTString();
//						}
//						break;
//					case 0x48: //72
//						s1 = reader.ReadTString();
//						break;
//					case 0x49: //73
//						s1 = reader.ReadTString();
//						s2 = reader.ReadTString();
//						s3 = reader.ReadTString();
//						break;
//					case 0x4A: //74
//						s1 = reader.ReadTString();
//						i1 = reader.ReadUInt32(); //as float * (sum num)
//						break;
//					case 0x4B: //75
//						i1 = reader.ReadUInt32();

//						i2 = reader.ReadUInt32();
//						for (int x = 0; x < i2; x++)
//						{
//							s1 = reader.ReadTString();
//						}
//						break;
//					case 0x4C: //76
//						i1 = reader.ReadUInt32();

//						i2 = reader.ReadUInt32();
//						for (int x = 0; x < i2; x++)
//						{
//							s1 = reader.ReadTString();
//						}
//						break;
//					case 0x4D: //77
//						s1 = reader.ReadTString();
//						s2 = reader.ReadTString();
//						s3 = reader.ReadTString();
//						break;
//					case 0x4E: //78
//						i1 = reader.ReadUInt32();

//						i2 = reader.ReadUInt32();
//						for (int x = 0; x < i2; x++)
//						{
//							s1 = reader.ReadTString();
//						}
//						break;
//					case 0x4F: //79
//						s1 = reader.ReadTString();
//						s2 = reader.ReadTString();

//						i1 = reader.ReadUInt32();

//						f1 = reader.ReadSingle();
//						break;



//					//0x50-0x5F
//					case 0x50: //80
//						s1 = reader.ReadTString();
//						s2 = reader.ReadTString();
//						s3 = reader.ReadTString();
//						break;
//					case 0x51: //81
//						s1 = reader.ReadTString();

//						i1 = reader.ReadUInt32();
//						i2 = reader.ReadUInt32();
//						i3 = reader.ReadUInt32();
//						i4 = reader.ReadUInt32();

//						s2 = reader.ReadTString();
//						s3 = reader.ReadTString();
//						break;
//					case 0x52: //82
//						s1 = reader.ReadTString();

//						i1 = reader.ReadUInt32();
//						i2 = reader.ReadUInt32();

//						s2 = reader.ReadTString();
//						s3 = reader.ReadTString();
//						s4 = reader.ReadTString();
//						break;
//					case 0x53: //83
//						s1 = reader.ReadTString();

//						i1 = reader.ReadUInt32();
//						i2 = reader.ReadUInt32();
//						i3 = reader.ReadUInt32();

//						f1 = reader.ReadSingle();

//						s2 = reader.ReadTString();
//						s3 = reader.ReadTString();
//						s4 = reader.ReadTString();
//						break;
//					case 0x54: //84
//						s1 = reader.ReadTString();
//						break;
//					case 0x55: //85
//						s1 = reader.ReadTString();

//						b1 = reader.ReadByte();

//						i1 = reader.ReadUInt32(); //as foat

//						i2 = reader.ReadUInt32();
//						i3 = reader.ReadUInt32();
//						i4 = reader.ReadUInt32();

//						s2 = reader.ReadTString();
//						break;
//					case 0x56: //86
//						s1 = reader.ReadTString();
//						s2 = reader.ReadTString();
//						break;
//					case 0x57: //87
//						s1 = reader.ReadTString();
//						i1 = reader.ReadUInt32(); 
//						break;
//					case 0x58: //88
//						i1 = reader.ReadUInt32();

//						//Load sound with index i1
//						break;
//					case 0x59: //89
//						s1 = reader.ReadTString();
//						i1 = reader.ReadUInt32();
//						s2 = reader.ReadTString();
//						break;
//					case 0x5A: //90
//						s1 = reader.ReadTString();

//						i1 = reader.ReadUInt32();
//						i2 = reader.ReadUInt32();

//						s2 = reader.ReadTString();
//						s3 = reader.ReadTString();
//						s4 = reader.ReadTString();
//						break;
//					case 0x5B: //91
//						s1 = reader.ReadTString();
//						s2 = reader.ReadTString();
//						break;
//					case 0x5C: //92
//						s1 = reader.ReadTString();

//						i1 = reader.ReadUInt32();
//						for (uint x = 0; x < i1; x++)
//						{
//							s2 = reader.ReadTString();
//						}
//						break;
//					case 0x5D: //93
//						s1 = reader.ReadTString();
//						s2 = reader.ReadTString();
//						break;
//					case 0x5E: //94
//						s1 = reader.ReadTString();

//						i1 = reader.ReadUInt32();

//						i2 = reader.ReadUInt32();
//						for (int x = 0; x < i2; x++)
//						{
//							s1 = reader.ReadTString();
//						}
//						break;
//					case 0x5F: //95
//						s1 = reader.ReadTString();

//						i1 = reader.ReadUInt32();
//						i2 = reader.ReadUInt32();

//						s2 = reader.ReadTString();
//						break;



//					//0x60-0x6F
//					case 0x60: //96
//						//Reads nothing, inits struct
//						break;
//					case 0x61: //97
//						s1 = reader.ReadTString();

//						i1 = reader.ReadUInt32(); //Object index

//						s2 = reader.ReadTString();
//						s3 = reader.ReadTString();
//						s4 = reader.ReadTString();
//						s5 = reader.ReadTString();

//						//Load object
//						break;
//					case 0x62: //98
//						//Reads nothing, inits struct
//						break;
//					case 0x63: //99
//						s1 = reader.ReadTString();

//						b1 = reader.ReadByte();

//						i1 = reader.ReadUInt32(); //as float

//						f1 = reader.ReadSingle();

//						s2 = reader.ReadTString();
//						break;
//					case 0x64: //100
//						s1 = reader.ReadTString();

//						i1 = reader.ReadUInt32(); //as float
//						i2 = reader.ReadUInt32(); //as float

//						f1 = reader.ReadSingle();
//						f2 = reader.ReadSingle();
//						break;
//					case 0x65: //101
//						s1 = reader.ReadTString();

//						i1 = reader.ReadUInt32();
//						for (uint x = 0; x < i1; x++)
//						{
//							s2 = reader.ReadTString();
//						}
//						break;
//					case 0x66: //102
//						s1 = reader.ReadTString();
//						break;
//					case 0x67: //103
//						i1 = reader.ReadUInt32();
//						i2 = reader.ReadUInt32();

//						for (uint x = 0; x < i2; x++)
//						{
//							s1 = reader.ReadTString();
//						}
//						break;
//					case 0x68: //104
//						s1 = reader.ReadTString();
//						i1 = reader.ReadUInt32();
//						s2 = reader.ReadTString();
//						break;
//					case 0x69: //105
//						s1 = reader.ReadTString();
//						break;
//					case 0x6A: //106
//						s1 = reader.ReadTString();
//						break;
//					case 0x6B: //107
//						s1 = reader.ReadTString();
//						s2 = reader.ReadTString();

//						i1 = reader.ReadUInt32(); //Object index
//						i2 = reader.ReadUInt32();
//						i3 = reader.ReadUInt32();
//						i4 = reader.ReadUInt32(); //as float
//						i5 = reader.ReadUInt32(); //as float

//						//Load object with index
//						break;
//					case 0x6C: //108
//						//???????????????
//						//Prints "Unable to create action with index" to log
//						throw new NotImplementedException();

//					case 0x6D: //109 - called "6C" in code
//						s1 = reader.ReadTString();
//						s2 = reader.ReadTString();
//						s3 = reader.ReadTString();
//						break;
//					case 0x6E: //110 - called "6D" in code
//						s1 = reader.ReadTString();
//						s2 = reader.ReadTString();
//						s3 = reader.ReadTString();

//						b1 = reader.ReadByte();
//						break;
//					case 0x6F: //111 - called "6E" in code
//						s1 = reader.ReadTString();
//						s2 = reader.ReadTString();

//						i1 = reader.ReadUInt32();

//						s3 = reader.ReadTString();
//						s4 = reader.ReadTString();
//						s5 = reader.ReadTString();
//						s6 = reader.ReadTString();
//						s7 = reader.ReadTString();
//						break;



//					//0x70-0x73
//					case 0x70: //112
//						//???????????????
//						//Prints "Unable to create action with index" to log
//						throw new NotImplementedException();

//					case 0x71: //113 - called "6F" in code
//						s1 = reader.ReadTString();
//						s2 = reader.ReadTString();

//						i1 = reader.ReadUInt32(); //as float
//						i2 = reader.ReadUInt32(); //as float
//						break;
//					case 0x72: //114 - called "70" in code
//						s1 = reader.ReadTString();
//						s2 = reader.ReadTString();
//						break;
//					case 0x73: //115 - called "71" in code
//						s1 = reader.ReadTString();
//						i1 = reader.ReadUInt32();

//						i2 = reader.ReadUInt32();
//						for (int x = 0; x < i2; x++)
//						{
//							s1 = reader.ReadTString();
//						}
//						break;

//					default:
//						throw new Exception();
//				}

//				return null;
//			}
//		}

//		class CollisionData
//		{
//			public uint ColliderGraphics;

//			public static CollisionData Read(BinaryReader reader)
//			{
//				uint colliderGraphics = reader.ReadUInt32();

//				uint i1 = reader.ReadUInt32();
//				uint i2 = reader.ReadUInt32();
//				uint i3 = reader.ReadUInt32();

//				return new CollisionData()
//				{
//					ColliderGraphics = colliderGraphics
//				};
//			}
//		}

//		struct Vec2
//		{
//			public float X;
//			public float Y;

//			public Vec2(float x, float y)
//			{
//				X = x;
//				Y = y;
//			}
//		}

//		class ParticleData
//		{
//			public static ParticleData Read(BinaryReader reader)
//			{
//				byte b1 = reader.ReadByte();
//				float f1 = reader.ReadSingle();

//				uint i1 = reader.ReadUInt32();
//				for (int x = 0; x < i1; x++)
//				{
//					uint graphicsIndex = reader.ReadUInt32();

//					uint i2 = reader.ReadUInt32();
//					uint i3 = reader.ReadUInt32(); //as float
//					uint i4 = reader.ReadUInt32(); //as float
//					uint i5 = reader.ReadUInt32();
//					uint i6 = reader.ReadUInt32();

//					float f2 = reader.ReadSingle();

//					byte b2 = reader.ReadByte();
//					byte b3 = reader.ReadByte();
//					byte b4 = reader.ReadByte();
//					byte b5 = reader.ReadByte();

//					for (int y = 0; y < 35; y++)
//					{
//						Vec2[] ar = ReadArray(reader);
//					}
//				}

//				return null;
//			}

//			private static Vec2[] ReadArray(BinaryReader reader)
//			{
//				int len = reader.ReadInt32();

//				Vec2[] ar = new Vec2[len];

//				for (int x = 0; x < len; x++)
//				{
//					ar[x] = new Vec2(reader.ReadSingle(), reader.ReadSingle());
//				}

//				return ar;
//			}
//		}

//		class LevelData
//		{
//			public string Name;

//			public Layer[] Layers;

//			public class Layer
//			{
//				public string Name;

//				public uint TilesX;
//				public uint TilesY;
//			}

//			public class LayerType0 : Layer
//			{
//				public int[] TileData;

//				public static LayerType0 Read(BinaryReader reader)
//				{
//					LayerType0 layer = new LayerType0();

//					layer.Name = reader.ReadTString();

//					uint i1 = reader.ReadUInt32();
//					uint i2 = reader.ReadUInt32();

//					layer.TilesX = reader.ReadUInt32(); //as float
//					layer.TilesY = reader.ReadUInt32(); //as float

//					layer.TileData = new int[layer.TilesX * layer.TilesY];

//					for (int x = 0; x < layer.TileData.Length; x++)
//					{
//						byte data = reader.ReadByte();
//						if (data == 0)
//						{
//							layer.TileData[x] = -1;
//						}
//						else
//						{
//							layer.TileData[x] = reader.ReadInt32();
//						}
//					}

//					byte b1 = reader.ReadByte();
//					byte b2 = reader.ReadByte();

//					float f1 = reader.ReadSingle();
//					float f2 = reader.ReadSingle();
//					float f3 = reader.ReadSingle();
//					float f4 = reader.ReadSingle();

//					return layer;
//				}
//			}

//			public class LayerType1 : Layer
//			{
//				public int[] TileData;

//				public static LayerType1 Read(BinaryReader reader)
//				{
//					LayerType1 layer = new LayerType1();

//					layer.Name = reader.ReadTString();

//					uint i1 = reader.ReadUInt32();
//					uint i2 = reader.ReadUInt32();

//					layer.TilesX = reader.ReadUInt32();
//					layer.TilesY = reader.ReadUInt32();

//					layer.TileData = new int[layer.TilesX * layer.TilesY];

//					for (int x = 0; x < layer.TileData.Length; x++)
//					{
//						byte data = reader.ReadByte();
//						if (data == 0)
//						{
//							layer.TileData[x] = -1;
//						}
//						else
//						{
//							layer.TileData[x] = reader.ReadInt32();
//						}
//					}

//					return layer;
//				}
//			}

//			public class LayerType2 : Layer
//			{
//				public int[] TileData;

//				public static LayerType2 Read(BinaryReader reader)
//				{
//					LayerType2 layer = new LayerType2();

//					layer.Name = reader.ReadTString();

//					uint i1 = reader.ReadUInt32(); //as float
//					uint i2 = reader.ReadUInt32(); //as float

//					layer.TilesX = reader.ReadUInt32();
//					layer.TilesY = reader.ReadUInt32();

//					layer.TileData = new int[layer.TilesX * layer.TilesY];

//					for (int x = 0; x < layer.TileData.Length; x++)
//					{
//						byte data = reader.ReadByte();
//						if (data == 0)
//						{
//							layer.TileData[x] = -1;
//						}
//						else
//						{
//							layer.TileData[x] = reader.ReadInt32();

//							uint i3 = reader.ReadUInt32();
//						}
//					}

//					return layer;
//				}
//			}

//			public class LayerType3 : Layer
//			{
//				public static LayerType3 Read(BinaryReader reader)
//				{
//					LayerType3 layer = new LayerType3();

//					layer.Name = reader.ReadTString();

//					uint i1 = reader.ReadUInt32();
//					uint i2 = reader.ReadUInt32();

//					uint numData = reader.ReadUInt32();
//					for (uint x = 0; x < numData; x++)
//					{
//						uint objIndex = reader.ReadUInt32();

//						int i3 = reader.ReadInt32();
//						int i4 = reader.ReadInt32();
//						int i5 = reader.ReadInt32();

//						byte b1 = reader.ReadByte();
//						if (b1 > 0)
//						{
//							uint i6 = reader.ReadUInt32();
//							uint i7 = reader.ReadUInt32();
//						}

//						b1 = reader.ReadByte();
//						if (b1 > 0)
//						{
//							uint i8 = reader.ReadUInt32();
//							uint i9 = reader.ReadUInt32();
//							uint i10 = reader.ReadUInt32();
//							uint i11 = reader.ReadUInt32();
//						}

//						uint numActions = reader.ReadUInt32();
//						for (uint y = 0; y < numActions; y++)
//						{
//							Function func = Function.ReadFunction(reader);
//						}
//					}

//					return layer;
//				}
//			}

//			public static LevelData Read(BinaryReader reader)
//			{
//				LevelData level = new LevelData();

//				level.Name = reader.ReadTString();

//				uint dataLength = reader.ReadUInt32();
//				long dataEnd = reader.BaseStream.Position + dataLength;

//				//Read level
//				{
//					uint widthPixel = reader.ReadUInt32();
//					uint heightPixel = reader.ReadUInt32();

//					uint i1 = reader.ReadUInt32();

//					uint layerCount = reader.ReadUInt32();
//					level.Layers = new Layer[layerCount];

//					for (uint x = 0; x < layerCount; x++)
//					{
//						uint type = reader.ReadUInt32();

//						Layer layer = null;
//						switch (type)
//						{
//							case 0:
//								layer = LayerType0.Read(reader);
//								break;
//							case 1:
//								layer = LayerType1.Read(reader);
//								break;
//							case 2:
//								layer = LayerType2.Read(reader);
//								break;
//							case 3:
//								layer = LayerType3.Read(reader);
//								break;

//							default:
//								throw new Exception();
//						}

//						level.Layers[x] = layer;
//					}
//				}

//				uint levelActions = reader.ReadUInt32();
//				for (uint x = 0; x < levelActions; x++)
//				{
//					Function func = Function.ReadFunction(reader);
//				}

//				if (reader.BaseStream.Position != dataEnd)
//				{
//					throw new Exception();
//				}

//				return level;
//			}
//		}

//		class GUIData
//		{
//			public string Name;

//			private static void ReadRectAndFuncs(BinaryReader reader) //GUI_load_rect_and_funcs
//			{
//				uint i3 = reader.ReadUInt32();

//				byte b1 = reader.ReadByte();

//				uint i4 = reader.ReadUInt32(); //as float
//				uint i5 = reader.ReadUInt32(); //as float
//				uint i6 = reader.ReadUInt32(); //as float
//				uint i7 = reader.ReadUInt32(); //as float

//				uint funcCount = reader.ReadUInt32();
//				for (uint x = 0; x < funcCount; x++)
//				{
//					Function f = Function.ReadFunction(reader);
//				}
//			}

//			private static void ReadGUIType1Internal(BinaryReader reader)
//			{
//				ReadRectAndFuncs(reader);

//				uint i9 = reader.ReadUInt32();
//				if (i9 > 0)
//				{
//					for (uint z = 0; z < i9; z++)
//					{
//						uint i20 = reader.ReadUInt32();

//						uint graphicsIndex = reader.ReadUInt32();

//						//Read rect?
//						{
//							float f1 = reader.ReadSingle();
//							float f2 = reader.ReadSingle();
//							float f3 = reader.ReadSingle();
//							float f4 = reader.ReadSingle();
//						}

//						uint i22 = reader.ReadUInt32();

//						for (uint w = 0; w < i22; w++)
//						{
//							uint i13 = reader.ReadUInt32();
//						}

//						uint i14 = reader.ReadUInt32(); //as float

//						byte b11 = reader.ReadByte();
//						byte b12 = reader.ReadByte();

//						uint i15 = reader.ReadUInt32(); //as float
//						uint i16 = reader.ReadUInt32(); //as float
//						uint i17 = reader.ReadUInt32(); //as float
//						uint i18 = reader.ReadUInt32(); //as float
//					}
//				}

//				uint i10 = reader.ReadUInt32();
//			}

//			private static void ReadGUIType3Internal(BinaryReader reader)
//			{
//				uint i1 = reader.ReadUInt32();
//				//Load font with index i1

//				uint i2 = reader.ReadUInt32();

//				uint i3 = reader.ReadUInt32();
//				for (uint z = 0; z < i3; z++)
//				{
//					string s1 = reader.ReadTString();
//				}

//				uint i4 = reader.ReadUInt32();
//			}

//			private static void ReadGUIElement(BinaryReader reader) //Create_GUI_element_with_index
//			{
//				uint type = reader.ReadUInt32();

//				switch (type)
//				{
//					case 0x00:
//						{
//							ReadRectAndFuncs(reader);

//							uint i8 = reader.ReadUInt32();

//							for (uint y = 0; y < i8; y++)
//							{
//								ReadGUIElement(reader);
//							}
//						}
//						break;
//					case 0x01:
//						{
//							ReadGUIType1Internal(reader);
//						}
//						break;
//					case 0x02:
//						{
//							ReadGUIType1Internal(reader);

//							uint i40 = reader.ReadUInt32();
//							uint i41 = reader.ReadUInt32();
//							uint i42 = reader.ReadUInt32();
//							uint i43 = reader.ReadUInt32();
//							uint i44 = reader.ReadUInt32();
//							uint i45 = reader.ReadUInt32();

//							byte b41 = reader.ReadByte();
//						}
//						break;
//					case 0x03:
//						{
//							ReadRectAndFuncs(reader);

//							ReadGUIType3Internal(reader);
//						}
//						break;
//					case 0x04:
//						{
//							ReadRectAndFuncs(reader);

//							uint i50 = reader.ReadUInt32();
//							//Load font: i50

//							uint i51 = reader.ReadUInt32();
//							uint i52 = reader.ReadUInt32();

//							uint i53 = reader.ReadUInt32();
//							//Read image: i53

//							uint i54 = reader.ReadUInt32();
//						}
//						break;
//					case 0x05:
//						{
//							ReadRectAndFuncs(reader);

//							ReadGUIType3Internal(reader);

//							uint i5 = reader.ReadUInt32(); //as float

//							byte b1 = reader.ReadByte();
//						}
//						break;
//					case 0x06:
//						{
//							ReadGUIType1Internal(reader);

//							uint i11 = reader.ReadUInt32();
//							uint i12 = reader.ReadUInt32();

//							float f5 = reader.ReadSingle();
//							float f6 = reader.ReadSingle();

//							byte b1 = reader.ReadByte();

//							float f7 = reader.ReadSingle();
//							float f8 = reader.ReadSingle();
//						}
//						break;
//					case 0x07:
//						{
//							ReadGUIType1Internal(reader);

//							uint u30 = reader.ReadUInt32();

//							uint u31 = reader.ReadUInt32(); //as float

//							uint u32 = reader.ReadUInt32();

//							uint u33 = reader.ReadUInt32(); //as float

//							uint u34 = reader.ReadUInt32();
//						}
//						break;
//					case 0x08:
//						{
//							ReadGUIType1Internal(reader);

//							uint i20 = reader.ReadUInt32();
//						}
//						break;

//					default:
//						throw new Exception();
//				}
//			}

//			public static GUIData Read(BinaryReader reader)
//			{
//				GUIData gui = new GUIData();

//				gui.Name = reader.ReadTString();

//				uint dataLength = reader.ReadUInt32();
//				long dataEnd = reader.BaseStream.Position + dataLength;

//				//Load_GUI
//				{
//					uint i1 = reader.ReadUInt32(); //Num something - inits array to NULLs

//					ReadGUIElement(reader);

//					uint numActions = reader.ReadUInt32();
//					for (uint x = 0; x < numActions; x++)
//					{
//						Function f = Function.ReadFunction(reader);
//					}
//				}

//				if (reader.BaseStream.Position != dataEnd)
//				{
//					throw new Exception();
//				}

//				return gui;
//			}
//		}

//		class DataIEX
//		{
//			public string GameFolderName;
//			public string GameSaveFileName;

//			public string[] Strings;

//			public Lookup[] GraphicsLookup;

//			public string[] SoundLookup;

//			public SoundData[] SoundDataArray;

//			public FontData[] FontDataArray;

//			public CursorData[] CursorDataArray;

//			public TileData[] TileDataArray;

//			public ObjectData[] ObjectDataArray;

//			public CollisionData[] CollisionDataArray;

//			public ParticleData[] ParticleDataArray;

//			public LevelData[] LevelDataArray;

//			public GUIData[] GUIDataArray;

//			public void Read(BinaryReader reader)
//			{
//				//Header (Done)
//				{
//					uint headerLength = reader.ReadUInt32();

//					GameFolderName = reader.ReadTString();
//					GameSaveFileName = reader.ReadTString();
//				}

//				//Strings (Done)
//				{
//					uint stringNum = reader.ReadUInt32();

//					Strings = new string[(int)stringNum];

//					for (uint x = 0; x < stringNum; x++)
//					{
//						Strings[x] = reader.ReadTString();
//					}
//				}

//				//Graphics lookup table (Done)
//				{
//					uint numEntries = reader.ReadUInt32();

//					GraphicsLookup = new Lookup[numEntries];

//					for (uint x = 0; x < numEntries; x++)
//					{
//						byte header = reader.ReadByte();
//						if (header != 0x01)
//						{
//							throw new Exception("Unexpected header!");
//						}

//						uint dataLength = reader.ReadUInt32();
//						long dataEnd = reader.BaseStream.Position + dataLength;

//						string file = reader.ReadTString();
//						string iex = reader.ReadTString();

//						GraphicsLookup[x] = new Lookup(file, iex);

//						if (reader.BaseStream.Position != dataEnd)
//						{
//							throw new Exception();
//						}
//					}
//				}

//				//Sound lookup - Waves data (Done)
//				{
//					uint numEntries = reader.ReadUInt32();

//					SoundLookup = new string[numEntries];

//					for (uint x = 0; x < numEntries; x++)
//					{
//						uint dataLength = reader.ReadUInt32();
//						long dataEnd = reader.BaseStream.Position + dataLength;

//						SoundLookup[x] = reader.ReadTString();

//						if (reader.BaseStream.Position != dataEnd)
//						{
//							throw new Exception();
//						}
//					}
//				}

//				//Sound data table (Data skipped, structure unknown)
//				{
//					uint numEntries = reader.ReadUInt32();

//					SoundDataArray = new SoundData[numEntries];

//					for (uint x = 0; x < numEntries; x++)
//					{
//						byte header = reader.ReadByte();
//						if (header == 0x00)
//						{
//							SoundDataArray[x] = null; //??

//							continue;
//						}
//						else if (header != 0x01)
//						{
//							throw new Exception("Unexpected header!");
//						}

//						uint dataLength = reader.ReadUInt32();
//						long dataEnd = reader.BaseStream.Position + dataLength;

//						SoundDataArray[x] = new SoundData()
//						{
//							Raw = reader.ReadBytes((int)dataLength)
//						};

//						if (reader.BaseStream.Position != dataEnd)
//						{
//							throw new Exception();
//						}
//					}
//				}

//				//Font data (Done)
//				{
//					uint numEntries = reader.ReadUInt32();

//					FontDataArray = new FontData[numEntries];

//					for (uint x = 0; x < numEntries; x++)
//					{
//						byte header = reader.ReadByte();
//						if (header == 0x00)
//						{
//							FontDataArray[x] = null; //??

//							continue;
//						}
//						else if (header != 0x01)
//						{
//							throw new Exception("Unexpected header!");
//						}

//						uint dataLength = reader.ReadUInt32();
//						long dataEnd = reader.BaseStream.Position + dataLength;

//						FontDataArray[x] = FontData.Read(reader);

//						if (reader.BaseStream.Position != dataEnd)
//						{
//							throw new Exception();
//						}
//					}
//				}

//				//Cursor data (Reads, has unknowns)
//				{
//					uint numEntries = reader.ReadUInt32();

//					CursorDataArray = new CursorData[numEntries];

//					for (uint x = 0; x < numEntries; x++)
//					{
//						byte header = reader.ReadByte();
//						if (header == 0x00)
//						{
//							CursorDataArray[x] = null; //??

//							continue;
//						}
//						else if (header != 0x01)
//						{
//							throw new Exception("Unexpected header!");
//						}

//						uint dataLength = reader.ReadUInt32();
//						long dataEnd = reader.BaseStream.Position + dataLength;

//						CursorDataArray[x] = CursorData.Read(reader);

//						if (reader.BaseStream.Position != dataEnd)
//						{
//							throw new Exception();
//						}
//					}
//				}

//				//Tile data (Reads, lots of unknowns)
//				{
//					uint numEntries = reader.ReadUInt32();

//					TileDataArray = new TileData[numEntries];

//					for (uint x = 0; x < numEntries; x++)
//					{
//						byte header = reader.ReadByte();
//						if (header == 0x00)
//						{
//							TileDataArray[x] = null; //??

//							continue;
//						}
//						else if (header != 0x01)
//						{
//							throw new Exception("Unexpected header!");
//						}

//						uint dataLength = reader.ReadUInt32();
//						long dataEnd = reader.BaseStream.Position + dataLength;

//						uint numData = reader.ReadUInt32(); //???
//						if (numData != 1)
//						{
//							throw new Exception();
//						}

//						TileDataArray[x] = TileData.Read(reader);

//						if (reader.BaseStream.Position != dataEnd)
//						{
//							throw new Exception();
//						}
//					}
//				}

//				//Object data (Reads, lots of unknowns)
//				{
//					uint numEntries = reader.ReadUInt32();

//					ObjectDataArray = new ObjectData[numEntries];

//					for (uint x = 0; x < numEntries; x++)
//					{
//						byte header = reader.ReadByte();
//						if (header == 0x00)
//						{
//							ObjectDataArray[x] = null; //??

//							continue;
//						}
//						else if (header != 0x01)
//						{
//							throw new Exception("Unexpected header!");
//						}

//						uint dataLength = reader.ReadUInt32();
//						long dataEnd = reader.BaseStream.Position + dataLength;

//						ObjectDataArray[x] = ObjectData.Read(reader);

//						if (reader.BaseStream.Position != dataEnd)
//						{
//							throw new Exception();
//						}
//					}
//				}

//				//Collision data (Reads, has unknowns)
//				{
//					uint numEntries = reader.ReadUInt32();

//					CollisionDataArray = new CollisionData[numEntries];

//					for (uint x = 0; x < numEntries; x++)
//					{
//						byte header = reader.ReadByte();
//						if (header == 0x00)
//						{
//							CollisionDataArray[x] = null; //??

//							continue;
//						}
//						else if (header != 0x01)
//						{
//							throw new Exception("Unexpected header!");
//						}

//						uint dataLength = reader.ReadUInt32();
//						long dataEnd = reader.BaseStream.Position + dataLength;

//						CollisionDataArray[x] = CollisionData.Read(reader);

//						if (reader.BaseStream.Position != dataEnd)
//						{
//							throw new Exception();
//						}
//					}
//				}

//				//Particles (Reads, lots of unknowns)
//				{
//					uint numEntries = reader.ReadUInt32();

//					ParticleDataArray = new ParticleData[numEntries];

//					for (uint x = 0; x < numEntries; x++)
//					{
//						byte header = reader.ReadByte();
//						if (header == 0x00)
//						{
//							ParticleDataArray[x] = null; //??

//							continue;
//						}
//						else if (header != 0x01)
//						{
//							throw new Exception("Unexpected header!");
//						}

//						uint dataLength = reader.ReadUInt32();
//						long dataEnd = reader.BaseStream.Position + dataLength;

//						ParticleDataArray[x] = ParticleData.Read(reader);

//						if (reader.BaseStream.Position != dataEnd)
//						{
//							throw new Exception();
//						}
//					}
//				}

//				//Read 3 lookups (#1 unknown, others read)
//				{
//					//#1
//					uint dataLength = reader.ReadUInt32();
//					long dataEnd = reader.BaseStream.Position + dataLength;

//					//Unknown

//					reader.BaseStream.Position = dataEnd;

//					//#2
//					dataLength = reader.ReadUInt32();
//					dataEnd = reader.BaseStream.Position + dataLength;

//					uint numActions = reader.ReadUInt32();
//					for (uint x = 0; x < numActions; x++)
//					{
//						Function func = Function.ReadFunction(reader);
//					}

//					if (reader.BaseStream.Position != dataEnd)
//					{
//						throw new Exception();
//					}

//					//#3
//					dataLength = reader.ReadUInt32();
//					dataEnd = reader.BaseStream.Position + dataLength;

//					numActions = reader.ReadUInt32();
//					for (uint x = 0; x < numActions; x++)
//					{
//						Function func = Function.ReadFunction(reader);
//					}

//					if (reader.BaseStream.Position != dataEnd)
//					{
//						throw new Exception();
//					}
//				}

//				//Level data (Reads, has unknowns)
//				{
//					uint numEntries = reader.ReadUInt32();

//					LevelDataArray = new LevelData[numEntries];

//					for (int x = 0; x < numEntries; x++)
//					{
//						LevelDataArray[x] = LevelData.Read(reader);
//					}
//				}

//				//GUI data (Reads, has lots of unknowns)
//				{
//					uint numEntries = reader.ReadUInt32();

//					GUIDataArray = new GUIData[numEntries];

//					for (int x = 0; x < numEntries; x++)
//					{
//						GUIDataArray[x] = GUIData.Read(reader);
//					}
//				}

//				if (reader.BaseStream.Position != reader.BaseStream.Length)
//				{
//					throw new Exception();
//				}
//			}
//		}
//	}
//
//	static class Ext
//	{
//		public static string ReadTString(this BinaryReader reader)
//		{
//			byte len = reader.ReadByte();
//			byte[] ascii = reader.ReadBytes(len);

//			return Encoding.ASCII.GetString(ascii);
//		}
//	}
//}
