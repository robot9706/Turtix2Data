using DataIex;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Data2Json
{
	class Program
	{
		enum ContentType
		{
			Invalid,

			Sound,
			Font,
			Cursor,
			Tile,
			Object,
			Collision,
			Particle,
			Level,
			GUI
		}

		private static Dictionary<ContentType, Type> _typeLookup = new Dictionary<ContentType, Type>()
		{
			// { ContentType.Sound, typeof(SoundData) },
			{ ContentType.Font, typeof(FontData) },
			//{ ContentType.Cursor, typeof(CursorData) },
			//{ ContentType.Tile, typeof(TileDataWrapper) },
			//{ ContentType.Object, typeof(ObjectData) },
			//{ ContentType.Collision, typeof(CollisionData) },
			//{ ContentType.Particle, typeof(ParticleData) },
			//{ ContentType.Level, typeof(LevelData) },
			//{ ContentType.GUI, typeof(GUIData) },
		};

		static void Main(string[] args)
		{
			if (args.Length == 3)
			{
				string binFile = args[2];
				if (File.Exists(binFile))
				{
					string outFile = Path.Combine(Path.GetDirectoryName(binFile), Path.GetFileNameWithoutExtension(binFile) + ".json");

					args = new string[]
					{
						args[0],
						args[1],
						args[2],
						outFile
					};
				}
			}

			if (args.Length != 4)
			{
				Console.WriteLine("Invalid params!");

				return;
			}

#if !DEBUG
			try
			{
#endif
			ContentType type = ParseType(args[1]);
			if (type == ContentType.Invalid)
			{
				Console.WriteLine("Invalid content type!");

				return;
			}

			switch (args[0])
			{
				case "p":
				case "pack":
					Serialize(args[2], type, args[3]);
					break;

				case "u":
				case "unpack":
					Deserialize(args[2], type, args[3]);
					break;

				default:
					throw new Exception("Unknown mode!");
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

		static ContentType ParseType(string name)
		{
			ContentType type = ContentType.Invalid;
			if (Enum.TryParse<ContentType>(name, true, out type))
			{
				return type;
			}

			//LOL
			switch (name)
			{
				case "s":
					return ContentType.Sound;
				case "f":
					return ContentType.Font;
				case "cu":
					return ContentType.Cursor;
				case "t:":
					return ContentType.Tile;
				case "o":
					return ContentType.Object;
				case "co":
					return ContentType.Collision;
				case "p":
					return ContentType.Particle;
				case "l":
					return ContentType.Level;
				case "g":
					return ContentType.GUI;
			}

			return ContentType.Invalid;
		}

		#region Serialize
		static void Serialize(string jsonFile, ContentType type, string outFile)
		{
			if (!_typeLookup.ContainsKey(type))
			{
				throw new NotImplementedException();
			}

			Type serializeType = _typeLookup[type];

			Console.WriteLine("Serializing \"" + jsonFile + "\" as \"" + type.ToString() + "\" to \"" + outFile + "\"");

			string json = File.ReadAllText(jsonFile);

			object data = JsonConvert.DeserializeObject(json, serializeType);

			WriteContentType(outFile, data, serializeType);
		}

		static void WriteContentType(string file, object data, Type type)
		{
			MethodInfo method = type.GetMethod("Write", BindingFlags.Static | BindingFlags.Public);
			if (method == null)
			{
				throw new Exception("No static Read method found for " + type.FullName);
			}

			using (FileStream fileStream = new FileStream(file, FileMode.Create))
			{
				using (BinaryWriter writer = new BinaryWriter(fileStream))
				{
					method.Invoke(null, new object[] { data, writer });
				}
			}
		}
		#endregion

		#region Deserialize
		static void Deserialize(string bin, ContentType type, string outFile)
		{
			if (!_typeLookup.ContainsKey(type))
			{
				throw new NotImplementedException();
			}

			Type deserializeType = _typeLookup[type];

			Console.WriteLine("Deserializing \"" + bin + "\" as \"" + type.ToString() + "\" to \"" + outFile + "\"");

			object data = ReadContentType(bin, deserializeType);
			if (data == null)
			{
				throw new Exception("Failed to read data!");
			}

			string json = JsonConvert.SerializeObject(data, Formatting.Indented);

			File.WriteAllText(outFile, json);
		}

		static object ReadContentType(string file, Type type)
		{
			MethodInfo method = type.GetMethod("Read", BindingFlags.Static | BindingFlags.Public);
			if (method == null)
			{
				throw new Exception("No static Read method found for " + type.FullName);
			}

			object obj = null;
			using (FileStream fileStream = new FileStream(file, FileMode.Open))
			{
				using (BinaryReader reader = new BinaryReader(fileStream))
				{
					obj = method.Invoke(null, new object[] { reader });

					if (fileStream.Position != fileStream.Length)
					{
						throw new Exception("Data not fully parsed for " + type.FullName);
					}
				}
			}

			return obj;
		}
		#endregion
	}
}
