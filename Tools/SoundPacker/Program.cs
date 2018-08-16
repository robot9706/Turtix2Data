using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundPacker
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
			}
		}

		static void Pack(string iex, string dir)
		{
			Dictionary<int, string> sort = new Dictionary<int, string>();

			string[] files = Directory.GetFiles(dir);

			foreach (string f in files)
			{
				if (Path.GetExtension(f).ToLower() == ".ogg")
				{
					string name = Path.GetFileNameWithoutExtension(f);

					if (name.Contains("_"))
					{
						string[] parts = name.Split('_');

						int index = Convert.ToInt32(parts[1]);

						sort.Add(index, f);
					}
				}
			}

			List<string> oggFilesSorted = sort.OrderBy(x => x.Key).ToArray().Select(x => x.Value).ToList();

			using (FileStream fs = new FileStream(iex, FileMode.Create))
			{
				using (BinaryWriter bw = new BinaryWriter(fs))
				{
					for (uint index = 0; index < oggFilesSorted.Count; index++)
					{
						bw.Write((byte)0x01);

						bw.Write((uint)index);

						string filePath = oggFilesSorted[(int)index];
						FileInfo fif = new FileInfo(filePath);

						uint dataLength = (uint)fif.Length;
						uint entryLength = dataLength + 4;

						bw.Write((uint)entryLength);
						bw.Write((uint)dataLength);

						bw.Flush();

						Console.WriteLine("Packing " + Path.GetFileName(filePath) + " as entry " + index.ToString());

						using (FileStream data = File.OpenRead(filePath))
						{
							data.CopyTo(fs);
						}
					}

					bw.Write(0x00);
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
						uint fullLength = br.ReadUInt32();
						long nextChunkHeader = fs.Position + fullLength;

						uint dataLength = br.ReadUInt32();

						string outFile = Path.Combine(dir, "Sound_" + index.ToString() + ".ogg");

						Console.WriteLine("Extracting entry " + index.ToString() + " to " + Path.GetFileName(outFile));

						using (FileStream file = new FileStream(outFile, FileMode.Create))
						{
							byte[] buf = br.ReadBytes((int)dataLength);

							file.Write(buf, 0, buf.Length);
						}

						fs.Position = nextChunkHeader;
					}
				}
			}
		}
	}
}
