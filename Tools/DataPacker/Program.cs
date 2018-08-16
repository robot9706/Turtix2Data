using DataIex;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace DataPacker
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

#if !DEBUG
			try
			{
#endif
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
#if !DEBUG
			}
			catch (Exception ex)
			{
				Console.WriteLine("An exception occured: " + ex.Message);
				Console.WriteLine(ex.StackTrace);
			}
#endif
		}

		#region Pack
		static void Pack(string iexPath, string dir)
		{
			//Find the info file
			string info = Path.Combine(dir, "IEX.json");
			if (!File.Exists(info))
			{
				throw new FileNotFoundException(info);
			}

			//Create the IEX
			DataIEX iex = new DataIEX();

			//Start loading stuff based on the IEX.json
			string infoData = File.ReadAllText(info);
			JToken infoJSON = (JToken)JsonConvert.DeserializeObject(infoData);

			//Header stuff
			{
				//File & folder
				{
					JToken saveInfo = infoJSON["header"];

					iex.GameFolderName = (string)saveInfo["folderName"];
					iex.GameSaveFileName = (string)saveInfo["saveFile"];
				}

				//Strings
				{
					JArray strings = (JArray)infoJSON["strings"];

					iex.Strings = new string[strings.Count];
					for (int x = 0; x < iex.Strings.Length; x++)
					{
						iex.Strings[x] = (string)strings[x];
					}
				}

				//Graphics lookup
				{
					JArray lookup = (JArray)infoJSON["graphicsLookup"];

					List<Lookup> lookups = new List<Lookup>();
					foreach (JObject pair in lookup.Children())
					{
						lookups.Add(new Lookup((string)pair["file"], (string)pair["iex"]));
					}

					iex.GraphicsLookup = lookups.ToArray();
				}

				//Sound lookup
				{
					JArray soundLookup = (JArray)infoJSON["soundLookup"];

					iex.SoundLookup = new string[soundLookup.Count];
					for (int x = 0; x < iex.SoundLookup.Length; x++)
					{
						iex.SoundLookup[x] = (string)soundLookup[x];
					}
				}
			}

			//Data
			{
				JToken fileInfo = infoJSON["Contents"];

				iex.SoundEntries = CreateEntryList(fileInfo, "Sound", dir);
				iex.FontEntires = CreateEntryList(fileInfo, "Font", dir);
				iex.CursorEntries = CreateEntryList(fileInfo, "Cursor", dir);
				iex.TileEntries = CreateEntryList(fileInfo, "Tile", dir);
				iex.ObjectEntries = CreateEntryList(fileInfo, "Object", dir);
				iex.CollisionEntries = CreateEntryList(fileInfo, "Collision", dir);
				iex.ParticleEntries = CreateEntryList(fileInfo, "Particle", dir);

				//etc
				{
					JToken etc = fileInfo["etc"];

					iex.UnknownEntry = CreateEntry(dir, (string)etc["UnknownEntry"]);
					iex.FunctonTable1 = CreateEntry(dir, (string)etc["FunctionTable1"]);
					iex.FunctonTable2 = CreateEntry(dir, (string)etc["FunctionTable2"]);
				}

				iex.LevelEntries = CreateEntryList(fileInfo, "Level", dir);
				iex.GUIEntries = CreateEntryList(fileInfo, "GUI", dir);
			}

			//Write the iex to a file
			using (FileStream fs = new FileStream(iexPath, FileMode.Create))
			{
				using (BinaryWriter bw = new BinaryWriter(fs))
				{
					iex.Write(bw);
				}
			}
		}

		private static IEXEntry CreateEntry(string dir, string filePath)
		{
			string realPath = Path.Combine(dir, filePath);

			return new IEXEntry(realPath);
		}

		private static IEXEntry[] CreateEntryList(JToken fileInfo, string extractName, string dir)
		{
			string collectionName = extractName + "s";

			JArray array = (JArray)fileInfo[collectionName];
			IEXEntry[] entryList = new IEXEntry[array.Count];

			for (int x = 0; x < array.Count; x++)
			{
				if ((string)array[x] == null)
				{
					entryList[x] = null;
					continue;
				}

				string filePath = (string)array[x];

				entryList[x] = CreateEntry(dir, filePath);
			}

			return entryList;
		}

		#endregion

		#region Unpack
		static void Unpack(string iexDir, string dir)
		{
			using (FileStream fs = File.OpenRead(iexDir))
			{
				using (BinaryReader br = new BinaryReader(fs))
				{
					DataIEX iex = new DataIEX();
					iex.Read(br);

					//Extract data
					{
						JToken infoJSON = new JObject();

						//Header and stuff
						{
							//Save info
							{
								JToken saveInfo = new JObject();

								saveInfo["folderName"] = iex.GameFolderName;
								saveInfo["saveFile"] = iex.GameSaveFileName;

								infoJSON["header"] = saveInfo;
							}

							//Strings
							{
								infoJSON["strings"] = new JArray(iex.Strings);
							}

							//Graphics lookup
							{
								JArray lookup = new JArray();

								foreach (Lookup l in iex.GraphicsLookup)
								{
									JObject pair = new JObject();
									pair["file"] = l.File;
									pair["iex"] = l.IEX;

									lookup.Add(pair);
								}

								infoJSON["graphicsLookup"] = lookup;
							}

							//Sound lookup
							{
								infoJSON["soundLookup"] = new JArray(iex.SoundLookup);
							}
						}

						//Extract entries
						{
							JToken fileInfo = new JObject();

							ExtractIndexedEntryArray(iex, iex.SoundEntries, dir, "Sound", fileInfo);
							ExtractIndexedEntryArray(iex, iex.FontEntires, dir, "Font", fileInfo);
							ExtractIndexedEntryArray(iex, iex.CursorEntries, dir, "Cursor", fileInfo);
							ExtractIndexedEntryArray(iex, iex.TileEntries, dir, "Tile", fileInfo);
							ExtractIndexedEntryArray(iex, iex.ObjectEntries, dir, "Object", fileInfo);
							ExtractIndexedEntryArray(iex, iex.CollisionEntries, dir, "Collision", fileInfo);
							ExtractIndexedEntryArray(iex, iex.ParticleEntries, dir, "Particle", fileInfo);

							JToken etc = new JObject();
							{
								etc["UnknownEntry"] = "UnknownEntry.bin";
								etc["FunctionTable1"] = "FunctionTable1.bin";
								etc["FunctionTable2"] = "FunctionTable2.bin";

								ExtractEntry(iex, iex.UnknownEntry, Path.Combine(dir, (string)etc["UnknownEntry"]));
								ExtractEntry(iex, iex.FunctonTable1, Path.Combine(dir, (string)etc["FunctionTable1"]));
								ExtractEntry(iex, iex.FunctonTable2, Path.Combine(dir, (string)etc["FunctionTable2"]));
							}
							fileInfo["etc"] = etc;

							ExtractNamedEntryArray(iex, iex.LevelEntries, dir, "Levels", fileInfo);
							ExtractNamedEntryArray(iex, iex.GUIEntries, dir, "GUIs", fileInfo);

							infoJSON["Contents"] = fileInfo;
						}

						//Save info json
						string headerData = JsonConvert.SerializeObject(infoJSON, Formatting.Indented);
						File.WriteAllText(Path.Combine(dir, "IEX.json"), headerData);
					}

					Console.WriteLine("Done extracting.");
				}
			}
		}

		static void ExtractEntry(DataIEX iex, IEXEntry entry, string file)
		{
			using (FileStream target = new FileStream(file, FileMode.Create))
			{
				iex.CopyEntryToStream(entry, target);
			}
		}

		static void ExtractIndexedEntryArray(DataIEX iex, IEXEntry[] list, string directory, string extractName, JToken infoJSON)
		{
			string collectionName = extractName + "s";

			Console.WriteLine("Extracting \"" + collectionName + "\", x" + list.Length.ToString());

			Directory.CreateDirectory(Path.Combine(directory, collectionName));

			string[] fileArray = new string[list.Length];

			for (int x = 0; x < list.Length; x++)
			{
				if (list[x] == null)
				{
					fileArray[x] = null;
				}
				else
				{
					string filePath = Path.Combine(collectionName, extractName + x.ToString() + ".bin");

					fileArray[x] = filePath;

					filePath = Path.Combine(directory, filePath);

					ExtractEntry(iex, list[x], filePath);
				}

				Console.WriteLine("\t" + (x + 1).ToString() + "/" + list.Length.ToString());
			}

			infoJSON[collectionName] = new JArray(fileArray);
		}

		static void ExtractNamedEntryArray(DataIEX iex, IEXEntry[] list, string directory, string collectionName, JToken infoJSON)
		{
			Directory.CreateDirectory(Path.Combine(directory, collectionName));

			Console.WriteLine("Extracting \"" + collectionName + "\", x" + list.Length.ToString());

			string[] fileArray = new string[list.Length];

			for (int x = 0; x < list.Length; x++)
			{
				if (list[x] == null)
				{
					fileArray[x] = null;
				}
				else
				{
					string filePath = Path.Combine(collectionName, list[x].Name);

					fileArray[x] = filePath;

					filePath = Path.Combine(directory, filePath);

					ExtractEntry(iex, list[x], filePath);
				}

				Console.WriteLine("\t" + (x + 1).ToString() + "/" + list.Length.ToString());
			}

			infoJSON[collectionName] = new JArray(fileArray);
		}
		#endregion
	}
}