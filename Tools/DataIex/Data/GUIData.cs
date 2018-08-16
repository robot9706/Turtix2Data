using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIex
{
	public class GUIData
	{
		public string Name;

		private static void ReadRectAndFuncs(BinaryReader reader) //GUI_load_rect_and_funcs
		{
			uint i3 = reader.ReadUInt32();

			byte b1 = reader.ReadByte();

			uint i4 = reader.ReadUInt32(); //as float
			uint i5 = reader.ReadUInt32(); //as float
			uint i6 = reader.ReadUInt32(); //as float
			uint i7 = reader.ReadUInt32(); //as float

			uint funcCount = reader.ReadUInt32();
			for (uint x = 0; x < funcCount; x++)
			{
				GameFunction f = GameFunction.ReadFunction(reader);
			}
		}

		private static void ReadGUIType1Internal(BinaryReader reader)
		{
			ReadRectAndFuncs(reader);

			uint i9 = reader.ReadUInt32();
			if (i9 > 0)
			{
				for (uint z = 0; z < i9; z++)
				{
					uint i20 = reader.ReadUInt32();

					uint graphicsIndex = reader.ReadUInt32();

					//Read rect?
					{
						float f1 = reader.ReadSingle();
						float f2 = reader.ReadSingle();
						float f3 = reader.ReadSingle();
						float f4 = reader.ReadSingle();
					}

					uint i22 = reader.ReadUInt32();

					for (uint w = 0; w < i22; w++)
					{
						uint i13 = reader.ReadUInt32();
					}

					uint i14 = reader.ReadUInt32(); //as float

					byte b11 = reader.ReadByte();
					byte b12 = reader.ReadByte();

					uint i15 = reader.ReadUInt32(); //as float
					uint i16 = reader.ReadUInt32(); //as float
					uint i17 = reader.ReadUInt32(); //as float
					uint i18 = reader.ReadUInt32(); //as float
				}
			}

			uint i10 = reader.ReadUInt32();
		}

		private static void ReadGUIType3Internal(BinaryReader reader)
		{
			uint i1 = reader.ReadUInt32();
			//Load font with index i1

			uint i2 = reader.ReadUInt32();

			uint i3 = reader.ReadUInt32();
			for (uint z = 0; z < i3; z++)
			{
				string s1 = reader.ReadTString();
			}

			uint i4 = reader.ReadUInt32();
		}

		private static void ReadGUIElement(BinaryReader reader) //Create_GUI_element_with_index
		{
			uint type = reader.ReadUInt32();

			switch (type)
			{
				case 0x00:
					{
						ReadRectAndFuncs(reader);

						uint i8 = reader.ReadUInt32();

						for (uint y = 0; y < i8; y++)
						{
							ReadGUIElement(reader);
						}
					}
					break;
				case 0x01:
					{
						ReadGUIType1Internal(reader);
					}
					break;
				case 0x02:
					{
						ReadGUIType1Internal(reader);

						uint i40 = reader.ReadUInt32();
						uint i41 = reader.ReadUInt32();
						uint i42 = reader.ReadUInt32();
						uint i43 = reader.ReadUInt32();
						uint i44 = reader.ReadUInt32();
						uint i45 = reader.ReadUInt32();

						byte b41 = reader.ReadByte();
					}
					break;
				case 0x03:
					{
						ReadRectAndFuncs(reader);

						ReadGUIType3Internal(reader);
					}
					break;
				case 0x04:
					{
						ReadRectAndFuncs(reader);

						uint i50 = reader.ReadUInt32();
						//Load font: i50

						uint i51 = reader.ReadUInt32();
						uint i52 = reader.ReadUInt32();

						uint i53 = reader.ReadUInt32();
						//Read image: i53

						uint i54 = reader.ReadUInt32();
					}
					break;
				case 0x05:
					{
						ReadRectAndFuncs(reader);

						ReadGUIType3Internal(reader);

						uint i5 = reader.ReadUInt32(); //as float

						byte b1 = reader.ReadByte();
					}
					break;
				case 0x06:
					{
						ReadGUIType1Internal(reader);

						uint i11 = reader.ReadUInt32();
						uint i12 = reader.ReadUInt32();

						float f5 = reader.ReadSingle();
						float f6 = reader.ReadSingle();

						byte b1 = reader.ReadByte();

						float f7 = reader.ReadSingle();
						float f8 = reader.ReadSingle();
					}
					break;
				case 0x07:
					{
						ReadGUIType1Internal(reader);

						uint u30 = reader.ReadUInt32();

						uint u31 = reader.ReadUInt32(); //as float

						uint u32 = reader.ReadUInt32();

						uint u33 = reader.ReadUInt32(); //as float

						uint u34 = reader.ReadUInt32();
					}
					break;
				case 0x08:
					{
						ReadGUIType1Internal(reader);

						uint i20 = reader.ReadUInt32();
					}
					break;

				default:
					throw new Exception();
			}
		}

		public static GUIData Read(BinaryReader reader)
		{
			GUIData gui = new GUIData();

			gui.Name = reader.ReadTString();

			uint dataLength = reader.ReadUInt32();
			long dataEnd = reader.BaseStream.Position + dataLength;

			//Load_GUI
			{
				uint i1 = reader.ReadUInt32(); //Num something - inits array to NULLs

				ReadGUIElement(reader);

				uint numActions = reader.ReadUInt32();
				for (uint x = 0; x < numActions; x++)
				{
					GameFunction f = GameFunction.ReadFunction(reader);
				}
			}

			if (reader.BaseStream.Position != dataEnd)
			{
				throw new Exception();
			}

			return gui;
		}
	}
}
