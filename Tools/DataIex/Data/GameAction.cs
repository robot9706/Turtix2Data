using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIex
{
	public class GameAction
	{
		//Call addresses:
		//0x21 -> 0x422870

		public uint TypeID;

		//Exec table around: 0x49A3B8
		public static GameAction ReadAction(BinaryReader reader)
		{
			GameAction action = new GameAction();

			action.TypeID = reader.ReadUInt32();

			if (action.TypeID > 0x73)
			{
				throw new Exception("Invalid type?");
			}

			uint i1;
			uint i2;
			uint i3;
			uint i4;
			uint i5;

			string s1;
			string s2;
			string s3;
			string s4;
			string s5;
			string s6;
			string s7;

			byte b1;
			byte b2;
			byte b3;

			float f1;
			float f2;

			switch (action.TypeID)
			{
				//0x00-0x0F
				case 0x00: //0
					s1 = reader.ReadTString();
					break;
				case 0x01: //1
					s1 = reader.ReadTString();

					i1 = reader.ReadUInt32();
					break;
				case 0x02: //2
					s1 = reader.ReadTString();
					break;
				case 0x03: //3
					i1 = reader.ReadUInt32();
					i2 = reader.ReadUInt32();

					for (int x = 0; x < i2; x++)
					{
						s1 = reader.ReadTString();
					}
					break;
				case 0x04: //4
					s1 = reader.ReadTString();
					break;
				case 0x05: //5
					s1 = reader.ReadTString();
					break;
				case 0x06: //6
					s1 = reader.ReadTString();
					s2 = reader.ReadTString();

					b1 = reader.ReadByte();

					s3 = reader.ReadTString();
					s4 = reader.ReadTString();
					break;
				case 0x07: //7
					s1 = reader.ReadTString();

					i1 = reader.ReadUInt32();
					break;
				case 0x08: //8
					i1 = reader.ReadUInt32();

					for (uint x = 0; x < i1; x++)
					{
						s1 = reader.ReadTString();
					}

					i2 = reader.ReadUInt32();

					s2 = reader.ReadTString();
					break;
				case 0x09: //9
					s1 = reader.ReadTString();
					break;
				case 0x0A: //10
					s1 = reader.ReadTString();
					s2 = reader.ReadTString();
					break;
				case 0x0B: //11
					i1 = reader.ReadUInt32();
					i2 = reader.ReadUInt32();
					for (uint x = 0; x < i2; x++)
					{
						s1 = reader.ReadTString();
					}
					break;
				case 0x0C: //12
					s1 = reader.ReadTString();
					break;
				case 0x0D: //13
					s1 = reader.ReadTString();
					break;
				case 0x0E: //14
					s1 = reader.ReadTString();

					i1 = reader.ReadUInt32();
					break;
				case 0x0F: //15
					s1 = reader.ReadTString();
					s2 = reader.ReadTString();

					i1 = reader.ReadUInt32();
					break;



				//0x10-0x1F
				case 0x10: //16
					i1 = reader.ReadUInt32();

					for (uint x = 0; x < i1; x++)
					{
						s1 = reader.ReadTString();
					}

					s2 = reader.ReadTString();
					break;
				case 0x11: //17
					s1 = reader.ReadTString();
					i1 = reader.ReadUInt32();
					break;
				case 0x12: //18
					s1 = reader.ReadTString();
					i1 = reader.ReadUInt32();
					break;
				case 0x13: //19
					s1 = reader.ReadTString();
					i1 = reader.ReadUInt32();
					break;
				case 0x14: //20
					s1 = reader.ReadTString();
					s2 = reader.ReadTString();
					i1 = reader.ReadUInt32();
					i2 = reader.ReadUInt32();
					break;
				case 0x15: //21
					s1 = reader.ReadTString();
					s2 = reader.ReadTString();
					s3 = reader.ReadTString();

					b1 = reader.ReadByte();
					b2 = reader.ReadByte();
					b3 = reader.ReadByte();
					break;
				case 0x16: //22
					s1 = reader.ReadTString();
					s2 = reader.ReadTString();

					b1 = reader.ReadByte();

					i1 = reader.ReadUInt32();

					b2 = reader.ReadByte();
					b3 = reader.ReadByte();

					s2 = reader.ReadTString();
					break;
				case 0x17: //23
					s1 = reader.ReadTString();

					i1 = reader.ReadUInt32(); //Particle index?
					i2 = reader.ReadUInt32();
					i3 = reader.ReadUInt32(); //as float
					i4 = reader.ReadUInt32(); //as float

					b1 = reader.ReadByte();

					s2 = reader.ReadTString();
					break;
				case 0x18: //24
					s1 = reader.ReadTString();

					i1 = reader.ReadUInt32(); //Particle index?

					s2 = reader.ReadTString();
					s3 = reader.ReadTString();
					s4 = reader.ReadTString();
					break;
				case 0x19: //25
					s1 = reader.ReadTString();

					b1 = reader.ReadByte();

					i1 = reader.ReadUInt32(); //as float
					i2 = reader.ReadUInt32(); //as float

					f1 = reader.ReadSingle();
					f2 = reader.ReadSingle();
					break;
				case 0x1A: //26
					s1 = reader.ReadTString();

					b1 = reader.ReadByte();

					i1 = reader.ReadUInt32(); //as float

					f1 = reader.ReadSingle();
					f2 = reader.ReadSingle();
					break;
				case 0x1B: //27
					s1 = reader.ReadTString();

					i1 = reader.ReadUInt32();
					i2 = reader.ReadUInt32();

					for (uint x = 0; x < i2; x++)
					{
						s2 = reader.ReadTString();
					}
					break;
				case 0x1C: //28
					s1 = reader.ReadTString();

					i1 = reader.ReadUInt32();

					s2 = reader.ReadTString();
					break;
				case 0x1D: //29
					s1 = reader.ReadTString();
					break;
				case 0x1E: //30
					i1 = reader.ReadUInt32();
					i2 = reader.ReadUInt32();
					i3 = reader.ReadUInt32();
					break;
				case 0x1F: //31
					s1 = reader.ReadTString();
					s2 = reader.ReadTString();
					break;



				//0x20-0x2F
				case 0x20: //32
					i1 = reader.ReadUInt32();

					for (int x = 0; x < i1; x++)
					{
						s1 = reader.ReadTString();
					}

					i2 = reader.ReadUInt32();

					s2 = reader.ReadTString();
					break;
				case 0x21: //33
					i1 = reader.ReadUInt32();

					for (int i = 0; i < i1; i++)
					{
						s1 = reader.ReadTString();
					}

					i2 = reader.ReadUInt32();
					s2 = reader.ReadTString();

					i3 = reader.ReadUInt32();
					for (int x = 0; x < i3; x++)
					{
						GameAction.ReadAction(reader);
					}

					i4 = reader.ReadUInt32();
					for (int x = 0; x < i4; x++)
					{
						GameAction.ReadAction(reader);
					}
					break;
				case 0x22: //34
					s1 = reader.ReadTString();

					b1 = reader.ReadByte();

					i1 = reader.ReadUInt32(); //as float

					f1 = reader.ReadSingle();
					f2 = reader.ReadSingle();

					s1 = reader.ReadTString();
					break;
				case 0x23: //35
					s1 = reader.ReadTString();

					b1 = reader.ReadByte();

					i1 = reader.ReadUInt32(); //as float

					i2 = reader.ReadUInt32();

					s2 = reader.ReadTString();
					break;
				case 0x24: //36
					s1 = reader.ReadTString();

					i1 = reader.ReadUInt32();
					i2 = reader.ReadUInt32();
					break;
				case 0x25: //37
					s1 = reader.ReadTString();
					s2 = reader.ReadTString();
					break;
				case 0x26: //38
					s1 = reader.ReadTString();
					s2 = reader.ReadTString();

					i1 = reader.ReadUInt32(); //as float
					i2 = reader.ReadUInt32(); //as float

					b1 = reader.ReadByte();

					i3 = reader.ReadUInt32();
					break;
				case 0x27: //39
					s1 = reader.ReadTString();

					b1 = reader.ReadByte();

					i1 = reader.ReadUInt32(); //as float? * (float)(0x000000E0)

					s2 = reader.ReadTString();
					break;
				case 0x28: //40
					s1 = reader.ReadTString();
					break;
				case 0x29: //41
					s1 = reader.ReadTString();

					i1 = reader.ReadUInt32();
					i2 = reader.ReadUInt32();

					f1 = reader.ReadSingle();
					//?? logics based on this value (>0)

					f2 = reader.ReadSingle();
					//?? logics based on this value (>0)

					s2 = reader.ReadTString();
					break;
				case 0x2A: //42
						   //Doesnt read, inits struct ?
					break;
				case 0x2B: //43
					s1 = reader.ReadTString();
					break;
				case 0x2C: //44
					s1 = reader.ReadTString();

					i1 = reader.ReadUInt32();

					s2 = reader.ReadTString();
					break;
				case 0x2D: //45
					s1 = reader.ReadTString();

					i1 = reader.ReadUInt32();

					i2 = reader.ReadUInt32();
					for (uint x = 0; x < i2; x++)
					{
						s1 = reader.ReadTString();
					}
					break;
				case 0x2E: //46
					s1 = reader.ReadTString();

					f1 = reader.ReadSingle();
					f2 = reader.ReadSingle();
					break;
				case 0x2F: //47
					i1 = reader.ReadUInt32();

					s1 = reader.ReadTString();
					s2 = reader.ReadTString();
					break;



				//0x30-0x3F
				case 0x30: //48
					i1 = reader.ReadUInt32();
					break;
				case 0x31: //49
					s1 = reader.ReadTString();
					i1 = reader.ReadUInt32();

					s2 = reader.ReadTString();

					i2 = reader.ReadUInt32(); //as float
					i3 = reader.ReadUInt32(); //as float

					s3 = reader.ReadTString();

					//Load particle data: i1
					break;
				case 0x32: //50
					s1 = reader.ReadTString();

					i1 = reader.ReadUInt32(); //as float * (sum number)

					i2 = reader.ReadUInt32();
					i3 = reader.ReadUInt32();
					i4 = reader.ReadUInt32();
					i5 = reader.ReadUInt32();

					s2 = reader.ReadTString();
					break;
				case 0x33: //51
					i1 = reader.ReadUInt32(); //Sound index?

					//Load sound with index
					break;
				case 0x34: //52
					s1 = reader.ReadTString();

					i1 = reader.ReadUInt32();
					break;
				case 0x35: //53
					s1 = reader.ReadTString();
					i1 = reader.ReadUInt32();
					s2 = reader.ReadTString();

					i2 = reader.ReadUInt32(); //as float
					i3 = reader.ReadUInt32(); //as float
					i4 = reader.ReadUInt32(); //as float
					break;
				case 0x36: //54
					s1 = reader.ReadTString();
					i1 = reader.ReadUInt32();
					break;
				case 0x37: //55
					s1 = reader.ReadTString();

					i1 = reader.ReadUInt32();
					i2 = reader.ReadUInt32();
					i3 = reader.ReadUInt32();
					i4 = reader.ReadUInt32();

					s2 = reader.ReadTString();
					break;
				case 0x38: //56
					s1 = reader.ReadTString();
					s2 = reader.ReadTString();
					break;
				case 0x39: //57: Named func: LoadCursor
					i1 = reader.ReadUInt32();

					i2 = reader.ReadUInt32();
					for (int x = 0; x < i2; x++)
					{
						s1 = reader.ReadTString();
					}
					break;
				case 0x3A: //58
					s1 = reader.ReadTString();
					break;
				case 0x3B: //59
					s1 = reader.ReadTString();

					i1 = reader.ReadUInt32();
					i2 = reader.ReadUInt32();
					break;
				case 0x3C: //60
					s1 = reader.ReadTString();
					s2 = reader.ReadTString();
					s3 = reader.ReadTString();
					break;
				case 0x3D: //61
					s1 = reader.ReadTString();
					s2 = reader.ReadTString();
					break;
				case 0x3E: //62
					s1 = reader.ReadTString();
					i1 = reader.ReadUInt32(); //Sound index
					b1 = reader.ReadByte();
					s2 = reader.ReadTString();

					//Load sound with index
					break;
				case 0x3F: //63
					i1 = reader.ReadUInt32(); //as float
					i2 = reader.ReadUInt32(); //as float

					s1 = reader.ReadTString();
					break;



				//0x40-0x4F
				case 0x40: //64
					i1 = reader.ReadUInt32(); //as float
					s1 = reader.ReadTString();
					break;
				case 0x41: //65
					i1 = reader.ReadUInt32();
					i2 = reader.ReadUInt32();
					i3 = reader.ReadUInt32();

					for (uint x = 0; x < i3; x++)
					{
						s1 = reader.ReadTString();
					}
					break;
				case 0x42: //66
					i1 = reader.ReadUInt32();

					i2 = reader.ReadUInt32();
					for (uint x = 0; x < i2; x++)
					{
						s1 = reader.ReadTString();
					}
					break;
				case 0x43: //67
					s1 = reader.ReadTString();
					i1 = reader.ReadUInt32();
					break;
				case 0x44: //68
					s1 = reader.ReadTString();
					break;
				case 0x45: //69
					s1 = reader.ReadTString();
					s2 = reader.ReadTString();

					i1 = reader.ReadUInt32();
					i2 = reader.ReadUInt32(); //as float
					i2 = reader.ReadUInt32(); //as float
					break;
				case 0x46: //70
					i1 = reader.ReadUInt32();

					i2 = reader.ReadUInt32();
					for (uint x = 0; x < i2; x++)
					{
						s1 = reader.ReadTString();
					}

					s2 = reader.ReadTString();
					break;
				case 0x47: //71
					s1 = reader.ReadTString();

					i1 = reader.ReadUInt32();

					i2 = reader.ReadUInt32();
					for (uint x = 0; x < i2; x++)
					{
						s1 = reader.ReadTString();
					}
					break;
				case 0x48: //72
					s1 = reader.ReadTString();
					break;
				case 0x49: //73
					s1 = reader.ReadTString();
					s2 = reader.ReadTString();
					s3 = reader.ReadTString();
					break;
				case 0x4A: //74
					s1 = reader.ReadTString();
					i1 = reader.ReadUInt32(); //as float * (sum num)
					break;
				case 0x4B: //75
					i1 = reader.ReadUInt32();

					i2 = reader.ReadUInt32();
					for (int x = 0; x < i2; x++)
					{
						s1 = reader.ReadTString();
					}
					break;
				case 0x4C: //76
					i1 = reader.ReadUInt32();

					i2 = reader.ReadUInt32();
					for (int x = 0; x < i2; x++)
					{
						s1 = reader.ReadTString();
					}
					break;
				case 0x4D: //77
					s1 = reader.ReadTString();
					s2 = reader.ReadTString();
					s3 = reader.ReadTString();
					break;
				case 0x4E: //78
					i1 = reader.ReadUInt32();

					i2 = reader.ReadUInt32();
					for (int x = 0; x < i2; x++)
					{
						s1 = reader.ReadTString();
					}
					break;
				case 0x4F: //79
					s1 = reader.ReadTString();
					s2 = reader.ReadTString();

					i1 = reader.ReadUInt32();

					f1 = reader.ReadSingle();
					break;



				//0x50-0x5F
				case 0x50: //80
					s1 = reader.ReadTString();
					s2 = reader.ReadTString();
					s3 = reader.ReadTString();
					break;
				case 0x51: //81
					s1 = reader.ReadTString();

					i1 = reader.ReadUInt32();
					i2 = reader.ReadUInt32();
					i3 = reader.ReadUInt32();
					i4 = reader.ReadUInt32();

					s2 = reader.ReadTString();
					s3 = reader.ReadTString();
					break;
				case 0x52: //82
					s1 = reader.ReadTString();

					i1 = reader.ReadUInt32();
					i2 = reader.ReadUInt32();

					s2 = reader.ReadTString();
					s3 = reader.ReadTString();
					s4 = reader.ReadTString();
					break;
				case 0x53: //83
					s1 = reader.ReadTString();

					i1 = reader.ReadUInt32();
					i2 = reader.ReadUInt32();
					i3 = reader.ReadUInt32();

					f1 = reader.ReadSingle();

					s2 = reader.ReadTString();
					s3 = reader.ReadTString();
					s4 = reader.ReadTString();
					break;
				case 0x54: //84
					s1 = reader.ReadTString();
					break;
				case 0x55: //85
					s1 = reader.ReadTString();

					b1 = reader.ReadByte();

					i1 = reader.ReadUInt32(); //as foat

					i2 = reader.ReadUInt32();
					i3 = reader.ReadUInt32();
					i4 = reader.ReadUInt32();

					s2 = reader.ReadTString();
					break;
				case 0x56: //86
					s1 = reader.ReadTString();
					s2 = reader.ReadTString();
					break;
				case 0x57: //87
					s1 = reader.ReadTString();
					i1 = reader.ReadUInt32();
					break;
				case 0x58: //88
					i1 = reader.ReadUInt32();

					//Load sound with index i1
					break;
				case 0x59: //89
					s1 = reader.ReadTString();
					i1 = reader.ReadUInt32();
					s2 = reader.ReadTString();
					break;
				case 0x5A: //90
					s1 = reader.ReadTString();

					i1 = reader.ReadUInt32();
					i2 = reader.ReadUInt32();

					s2 = reader.ReadTString();
					s3 = reader.ReadTString();
					s4 = reader.ReadTString();
					break;
				case 0x5B: //91
					s1 = reader.ReadTString();
					s2 = reader.ReadTString();
					break;
				case 0x5C: //92
					s1 = reader.ReadTString();

					i1 = reader.ReadUInt32();
					for (uint x = 0; x < i1; x++)
					{
						s2 = reader.ReadTString();
					}
					break;
				case 0x5D: //93
					s1 = reader.ReadTString();
					s2 = reader.ReadTString();
					break;
				case 0x5E: //94
					s1 = reader.ReadTString();

					i1 = reader.ReadUInt32();

					i2 = reader.ReadUInt32();
					for (int x = 0; x < i2; x++)
					{
						s1 = reader.ReadTString();
					}
					break;
				case 0x5F: //95
					s1 = reader.ReadTString();

					i1 = reader.ReadUInt32();
					i2 = reader.ReadUInt32();

					s2 = reader.ReadTString();
					break;



				//0x60-0x6F
				case 0x60: //96
						   //Reads nothing, inits struct
					break;
				case 0x61: //97
					s1 = reader.ReadTString();

					i1 = reader.ReadUInt32(); //Object index

					s2 = reader.ReadTString();
					s3 = reader.ReadTString();
					s4 = reader.ReadTString();
					s5 = reader.ReadTString();

					//Load object
					break;
				case 0x62: //98
						   //Reads nothing, inits struct
					break;
				case 0x63: //99
					s1 = reader.ReadTString();

					b1 = reader.ReadByte();

					i1 = reader.ReadUInt32(); //as float

					f1 = reader.ReadSingle();

					s2 = reader.ReadTString();
					break;
				case 0x64: //100
					s1 = reader.ReadTString();

					i1 = reader.ReadUInt32(); //as float
					i2 = reader.ReadUInt32(); //as float

					f1 = reader.ReadSingle();
					f2 = reader.ReadSingle();
					break;
				case 0x65: //101
					s1 = reader.ReadTString();

					i1 = reader.ReadUInt32();
					for (uint x = 0; x < i1; x++)
					{
						s2 = reader.ReadTString();
					}
					break;
				case 0x66: //102
					s1 = reader.ReadTString();
					break;
				case 0x67: //103
					i1 = reader.ReadUInt32();
					i2 = reader.ReadUInt32();

					for (uint x = 0; x < i2; x++)
					{
						s1 = reader.ReadTString();
					}
					break;
				case 0x68: //104
					s1 = reader.ReadTString();
					i1 = reader.ReadUInt32();
					s2 = reader.ReadTString();
					break;
				case 0x69: //105
					s1 = reader.ReadTString();
					break;
				case 0x6A: //106
					s1 = reader.ReadTString();
					break;
				case 0x6B: //107
					s1 = reader.ReadTString();
					s2 = reader.ReadTString();

					i1 = reader.ReadUInt32(); //Object index
					i2 = reader.ReadUInt32();
					i3 = reader.ReadUInt32();
					i4 = reader.ReadUInt32(); //as float
					i5 = reader.ReadUInt32(); //as float

					//Load object with index
					break;
				case 0x6C: //108
						   //???????????????
						   //Prints "Unable to create action with index" to log
					throw new NotImplementedException();

				case 0x6D: //109 - called "6C" in code
					s1 = reader.ReadTString();
					s2 = reader.ReadTString();
					s3 = reader.ReadTString();
					break;
				case 0x6E: //110 - called "6D" in code
					s1 = reader.ReadTString();
					s2 = reader.ReadTString();
					s3 = reader.ReadTString();

					b1 = reader.ReadByte();
					break;
				case 0x6F: //111 - called "6E" in code
					s1 = reader.ReadTString();
					s2 = reader.ReadTString();

					i1 = reader.ReadUInt32();

					s3 = reader.ReadTString();
					s4 = reader.ReadTString();
					s5 = reader.ReadTString();
					s6 = reader.ReadTString();
					s7 = reader.ReadTString();
					break;



				//0x70-0x73
				case 0x70: //112
						   //???????????????
						   //Prints "Unable to create action with index" to log
					throw new NotImplementedException();

				case 0x71: //113 - called "6F" in code
					s1 = reader.ReadTString();
					s2 = reader.ReadTString();

					i1 = reader.ReadUInt32(); //as float
					i2 = reader.ReadUInt32(); //as float
					break;
				case 0x72: //114 - called "70" in code
					s1 = reader.ReadTString();
					s2 = reader.ReadTString();
					break;
				case 0x73: //115 - called "71" in code
					s1 = reader.ReadTString();
					i1 = reader.ReadUInt32();

					i2 = reader.ReadUInt32();
					for (int x = 0; x < i2; x++)
					{
						s1 = reader.ReadTString();
					}
					break;

				default:
					throw new Exception();
			}

			return action;
		}
	}
}
