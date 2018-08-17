using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsPacker
{
	class Program
	{
		static void Main(string[] args)
		{
			if (args.Length != 3)
			{
				Console.WriteLine("Invalid params!");

				return;
			}

			switch (args[0])
			{
				case "p":
				case "pack":
					if (!Directory.Exists(args[2]))
					{
						Console.WriteLine("Input directory not found!");
					}

					Pack(args[1], args[2]);
					break;

				case "u":
				case "unpack":
					if (!File.Exists(args[1]))
					{
						Console.WriteLine("Input file not found!");

						return;
					}

					string dir = args[2];
					Directory.CreateDirectory(dir);

					Unpack(args[1], dir);
					break;

				default:
					throw new Exception("Unknown mode!");
			}
		}

		static void Pack(string iex, string dir)
		{
			Dictionary<int, Tuple<string, string, string>> sort = new Dictionary<int, Tuple<string, string, string>>();

			string[] files = Directory.GetFiles(dir);

			foreach (string f in files)
			{
				if (Path.GetExtension(f).ToLower() == ".csv")
				{
					string name = Path.GetFileNameWithoutExtension(f);

					if (name.Contains("_"))
					{
						string[] parts = name.Split('_');

						int index = Convert.ToInt32(parts[1]);

						string fDir = Path.GetDirectoryName(f);

						string colorFile = Path.Combine(fDir, "Entry_" + index.ToString() + "_color.jpg");
						string sheetFile = Path.Combine(fDir, "Entry_" + index.ToString() + "_spritesheet.csv");
						string alphaFile = Path.Combine(fDir, "Entry_" + index.ToString() + "_alpha.jpg");

						if (!File.Exists(colorFile))
						{
							Console.WriteLine("Unable to find color jpg: \"" + Path.GetFileName(colorFile) + "\"");

							continue;
						}

						if (!File.Exists(sheetFile))
						{
							Console.WriteLine("Unable to find spritesheet: \"" + Path.GetFileName(sheetFile) + "\"");

							continue;
						}

						if (!File.Exists(alphaFile))
						{
							Console.WriteLine("Unable to find alpha jpg: \"" + Path.GetFileName(alphaFile) + "\"");

							continue;
						}

						sort.Add(index, new Tuple<string, string, string>(colorFile, sheetFile, alphaFile));
					}
				}
			}

			Tuple<string, string, string>[] entries = new Tuple<string, string, string>[sort.Keys.Max() + 1];

			foreach (int index in sort.Keys.OrderBy(x => x))
			{
				entries[index] = sort[index];
			}

			using (FileStream fs = new FileStream(iex, FileMode.Create))
			{
				using (BinaryWriter bw = new BinaryWriter(fs))
				{
					for (int x = 0; x < entries.Length; x++)
					{
						if (entries[x] == null)
							continue;

						bw.Write((byte)0x01);

						WriteEntry(bw, x, entries[x]);
					}

					bw.Write((byte)0x00);
				}
			}
		}

		static void WriteEntry(BinaryWriter writer, int index, Tuple<string, string, string> data)
		{
			Console.WriteLine("Packing entry " + index.ToString());

			MemoryStream sheetDataStream = new MemoryStream();
			WriteSheetData(sheetDataStream, data.Item2);
			sheetDataStream.Position = 0;

			FileInfo colorFileInfo = new FileInfo(data.Item1);
			FileInfo alphaFileInfo = new FileInfo(data.Item3);

			long fullEntrySize = colorFileInfo.Length + sheetDataStream.Length + alphaFileInfo.Length + 4; //(+4 is the color data length)

			writer.Write((uint)index);
			writer.Write((uint)fullEntrySize);

			writer.Write((uint)colorFileInfo.Length);

			writer.Flush();

			using (FileStream colorFile = File.OpenRead(data.Item1))
			{
				colorFile.CopyTo(writer.BaseStream);
			}

			sheetDataStream.CopyTo(writer.BaseStream);

			using (FileStream alphaFile = File.OpenRead(data.Item3))
			{
				alphaFile.CopyTo(writer.BaseStream);
			}
		}

		static void WriteSheetData(MemoryStream target, string csv)
		{
			int numCells = -1;
			int numUsedCells = -1;

			Rect?[] cellData = null;

			using (StreamReader reader = new StreamReader(csv))
			{
				string line;

				while(!reader.EndOfStream)
				{
					line = reader.ReadLine();
					if (string.IsNullOrEmpty(line) || line.StartsWith("#"))
					{
						continue;
					}

					string[] lineData = line.Trim().Split(';');

					if (lineData.Length != 5)
					{
						Console.WriteLine("Error in csv line: \"" + line + "\"");

						continue;
					}

					int index = Convert.ToInt32(lineData[0]);

					int x = Convert.ToInt32(lineData[1]);
					int y = Convert.ToInt32(lineData[2]);

					int w = Convert.ToInt32(lineData[3]);
					int h = Convert.ToInt32(lineData[4]);

					if (index == -1) //Special case, contains cell data
					{
						numCells = x;
						numUsedCells = y;

						cellData = new Rect?[numCells];
					}
					else
					{
						cellData[index] = new Rect(x, y, w, h);
					}
				}
			}

			using (BinaryWriter w = new BinaryWriter(target, Encoding.ASCII, true))
			{
				w.Write((uint)numCells);
				w.Write((uint)numUsedCells);

				int cellsWritten = 0;
				for (int x = 0; x < cellData.Length; x++)
				{
					Rect? cell = cellData[x];

					if (cell.HasValue)
					{
						w.Write((uint)x);

						w.Write((uint)cell.Value.X);
						w.Write((uint)cell.Value.Y);

						w.Write((uint)cell.Value.Width);
						w.Write((uint)cell.Value.Height);

						cellsWritten++;
					}
				}

				if (cellsWritten != numUsedCells)
				{
					Console.WriteLine("Invalid cell data! Used cells != Actual cell data");

					throw new Exception();
				}
			}
		}

		static void Unpack(string iex, string dir)
		{
			using (FileStream fs = File.OpenRead(iex))
			{
				using (BinaryReader br = new BinaryReader(fs))
				{
					while (br.ReadByte() == 0x01)
					{
						uint index = br.ReadUInt32();
						Console.WriteLine("Extracting entry " + index.ToString());

						uint entryLength = br.ReadUInt32();
						uint colorDataLength = br.ReadUInt32();

						uint dataLengthNoColor = entryLength - colorDataLength - 4; //-4 is the color data length

						//Read color data
						string outPath = Path.Combine(dir, "Entry_" + index.ToString() + "_color.jpg");
						using (FileStream cd = new FileStream(outPath, FileMode.Create))
						{
							byte[] buf = br.ReadBytes((int)colorDataLength);

							cd.Write(buf, 0, buf.Length);
						}

						//Read middle data
						uint middleDataLength;

						outPath = Path.Combine(dir, "Entry_" + index.ToString() + "_spritesheet.csv");
						using (StreamWriter mid = new StreamWriter(outPath))
						{
							uint len1 = br.ReadUInt32(); //Cell count
							uint len2 = br.ReadUInt32(); //Used cells

							mid.WriteLine("#Index,X,Y,Width,Height");
							mid.WriteLine("#Cell slots " + len1.ToString());
							mid.WriteLine("#Used cells " + len2.ToString());
							mid.WriteLine("-1;" + len1.ToString() + ";" + len2.ToString() + ";-1;-1");

							middleDataLength = 4 + 4 + (len2 * (4 * 5));

							Rect?[] cells = new Rect?[len1];

							for (uint idx = 0; idx < len2; idx++)
							{
								uint cellIndex = br.ReadUInt32();
								uint x = br.ReadUInt32();
								uint y = br.ReadUInt32();
								uint w = br.ReadUInt32();
								uint h = br.ReadUInt32();

								cells[cellIndex] = new Rect((int)x, (int)y, (int)w, (int)h);
							}

							mid.WriteLine("#Cell data");
							for (int x = 0; x < cells.Length; x++)
							{
								Rect? rv = cells[x];

								if (!rv.HasValue)
									continue;

								Rect r = rv.Value;

								mid.WriteLine(x.ToString() + ";" + r.X.ToString() + ";" + r.Y.ToString() + ";" + r.Width.ToString() + ";" + r.Height.ToString());
							}
						}

						//Alpha data stars here
						uint alphaDataLength = dataLengthNoColor - middleDataLength;

						//Read alpha data
						outPath = Path.Combine(dir, "Entry_" + index.ToString() + "_alpha.jpg");
						using (FileStream cd = new FileStream(outPath, FileMode.Create))
						{
							byte[] buf = br.ReadBytes((int)alphaDataLength);

							cd.Write(buf, 0, buf.Length);
						}
					}
				}
			}
		}
	}
}
