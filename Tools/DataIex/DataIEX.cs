using System;
using System.IO;
using System.Text;

namespace DataIex
{
	public class DataIEX
	{
		//Header
		public string GameFolderName; //Folder name in %localappdata%, ex: C:\Users\[Username]\AppData\Local\[Folder name]
		public string GameSaveFileName; //In the %localappdata% folder

		//Lookups and file names
		public string[] Strings; //No idea what these are
		public Lookup[] GraphicsLookup; //A filename mapped to an IEX (Graphics.iex or Language/Localization.iex) which contains that file
		public string[] SoundLookup; //Sound filenames mapped to sound indexes
		
		//Data
		public IEXEntry[] SoundEntries;
		public IEXEntry[] FontEntires;
		public IEXEntry[] CursorEntries;
		public IEXEntry[] TileEntries; //Has a number before the actual data, it needs to be "1"
		public IEXEntry[] ObjectEntries;
		public IEXEntry[] CollisionEntries;
		public IEXEntry[] ParticleEntries;
		public IEXEntry[] LevelEntries;
		public IEXEntry[] GUIEntries;

		//Other data
		public IEXEntry UnknownEntry;
		public IEXEntry FunctonTable1;
		public IEXEntry FunctonTable2;

		//Parsing
		private BinaryReader _reader;

		#region Parse file structure
		public void Read(BinaryReader reader)
		{
			_reader = reader;

			ReadHeader(reader);

			ReadStrings(reader);
			ReadGraphicsLookup(reader);
			ReadSoundLookup(reader);

			SoundEntries = ReadEntryTable(reader);
			FontEntires = ReadEntryTable(reader);
			CursorEntries = ReadEntryTable(reader);
			TileEntries = ReadEntryTable(reader);
			ObjectEntries = ReadEntryTable(reader);
			CollisionEntries = ReadEntryTable(reader);
			ParticleEntries = ReadEntryTable(reader);

			UnknownEntry = ReadEntry(reader);
			FunctonTable1 = ReadEntry(reader);
			FunctonTable2 = ReadEntry(reader);

			LevelEntries = ReadNamedEntryTable(reader);
			GUIEntries = ReadNamedEntryTable(reader);

			if (reader.BaseStream.Position != reader.BaseStream.Length)
			{
				throw new Exception("Failed to parse IEX successfully!");
			}
		}

		//Header stuff
		private void ReadHeader(BinaryReader reader)
		{
			uint headerLength = reader.ReadUInt32();
			long headerEnd = reader.BaseStream.Position + headerLength;

			GameFolderName = reader.ReadTString();
			GameSaveFileName = reader.ReadTString();

			if (reader.BaseStream.Position != headerEnd)
			{
				throw new Exception("Failed to read header");
			}
		}

		private void ReadStrings(BinaryReader reader)
		{
			uint stringNum = reader.ReadUInt32();

			Strings = new string[(int)stringNum];

			for (uint x = 0; x < stringNum; x++)
			{
				Strings[x] = reader.ReadTString();
			}
		}

		private void ReadGraphicsLookup(BinaryReader reader)
		{
			uint numEntries = reader.ReadUInt32();

			GraphicsLookup = new Lookup[numEntries];

			for (uint x = 0; x < numEntries; x++)
			{
				byte header = reader.ReadByte();
				if (header == 0x00)
				{
					GraphicsLookup[x] = null; //The entry is null
				}
				else if (header != 0x01)
				{
					throw new Exception("Unexpected header!");
				}

				uint dataLength = reader.ReadUInt32();
				long dataEnd = reader.BaseStream.Position + dataLength;

				string file = reader.ReadTString();
				string iex = reader.ReadTString();

				GraphicsLookup[x] = new Lookup(file, iex);

				if (reader.BaseStream.Position != dataEnd)
				{
					throw new Exception();
				}
			}
		}

		private void ReadSoundLookup(BinaryReader reader)
		{
			uint numEntries = reader.ReadUInt32();

			SoundLookup = new string[numEntries];

			for (uint x = 0; x < numEntries; x++)
			{
				uint dataLength = reader.ReadUInt32();
				long dataEnd = reader.BaseStream.Position + dataLength;

				SoundLookup[x] = reader.ReadTString();

				if (reader.BaseStream.Position != dataEnd)
				{
					throw new Exception();
				}
			}
		}

		//Read unnamed entries
		private IEXEntry ReadEntry(BinaryReader reader)
		{
			uint dataLength = reader.ReadUInt32();
			long dataEnd = reader.BaseStream.Position + dataLength;

			IEXEntry entry = new IEXEntry(dataLength, reader.BaseStream.Position);

			reader.BaseStream.Position = dataEnd;

			return entry;
		}

		private IEXEntry[] ReadEntryTable(BinaryReader reader)
		{
			uint numEntries = reader.ReadUInt32();
			IEXEntry[] array = new IEXEntry[numEntries];

			for (uint x = 0; x < numEntries; x++)
			{
				byte header = reader.ReadByte();
				if (header == 0x00)
				{
					array[x] = null;

					continue;
				}
				else if (header != 0x01)
				{
					throw new Exception("Unexpected header!");
				}

				array[x] = ReadEntry(reader);
			}

			return array;
		}

		//Read named entries
		private IEXEntry ReadNamedEntry(BinaryReader reader)
		{
			string name = reader.ReadTString();

			uint dataLength = reader.ReadUInt32();
			long dataEnd = reader.BaseStream.Position + dataLength;

			IEXEntry entry = new IEXEntry(dataLength, reader.BaseStream.Position, name);

			reader.BaseStream.Position = dataEnd;

			return entry;
		}

		private IEXEntry[] ReadNamedEntryTable(BinaryReader reader)
		{
			uint numEntries = reader.ReadUInt32();
			IEXEntry[] array = new IEXEntry[numEntries];

			for (uint x = 0; x < numEntries; x++)
			{
				array[x] = ReadNamedEntry(reader);
			}

			return array;
		}
		#endregion

		#region Read objects
		private void JumpToEntry(IEXEntry entry)
		{
			_reader.BaseStream.Position = entry.FileOffset;
		}

		private void CheckFilePosition(IEXEntry entry)
		{
			if (_reader.BaseStream.Position != entry.EntryEnd)
			{
				throw new Exception("Failed to parse entry!");
			}
		}

		public void CopyEntryToStream(IEXEntry entry, Stream target)
		{
			JumpToEntry(entry);

			byte[] buffer = _reader.ReadBytes(entry.Length);
			target.Write(buffer, 0, buffer.Length);

			CheckFilePosition(entry);
		}

		public SoundData ReadSoundData(IEXEntry entry)
		{
			JumpToEntry(entry);

			SoundData data = SoundData.Read(_reader);

			CheckFilePosition(entry);

			return data;
		}

		public FontData ReadFontData(IEXEntry entry)
		{
			JumpToEntry(entry);

			FontData font = FontData.Read(_reader);

			CheckFilePosition(entry);

			return font;
		}

		public CursorData ReadCursorData(IEXEntry entry)
		{
			JumpToEntry(entry);

			CursorData font = CursorData.Read(_reader);

			CheckFilePosition(entry);

			return font;
		}

		public TileData ReadTileData(IEXEntry entry)
		{
			JumpToEntry(entry);

			uint magicNumber = _reader.ReadUInt32(); //Tile count?
			if (magicNumber != 0x01)
			{
				throw new Exception("Unexpected tile count"); ;
			}

			TileData tile = TileData.Read(_reader);

			CheckFilePosition(entry);

			return tile;
		}

		public ObjectData ReadObjectData(IEXEntry entry)
		{
			JumpToEntry(entry);

			ObjectData obj = ObjectData.Read(_reader);

			CheckFilePosition(entry);

			return obj;
		}

		public CollisionData ReadCollisionData(IEXEntry entry)
		{
			JumpToEntry(entry);

			CollisionData collision = CollisionData.Read(_reader);

			CheckFilePosition(entry);

			return collision;
		}

		public ParticleData ReadParticleData(IEXEntry entry)
		{
			JumpToEntry(entry);

			ParticleData particle = ParticleData.Read(_reader);

			CheckFilePosition(entry);

			return particle;
		}

		public GameFunction[] ReadFunctionTable(IEXEntry entry)
		{
			JumpToEntry(entry);

			uint entries = _reader.ReadUInt32();
			GameFunction[] array = new GameFunction[entries];

			for (uint x = 0; x < entries; x++)
			{
				array[x] = GameFunction.ReadFunction(_reader);
			}

			CheckFilePosition(entry);

			return array;
		}

		public LevelData ReadLevelData(IEXEntry entry)
		{
			JumpToEntry(entry);

			LevelData level = LevelData.Read(_reader);

			CheckFilePosition(entry);

			return level;
		}

		public GUIData ReadGUIData(IEXEntry entry)
		{
			JumpToEntry(entry);

			GUIData gui = GUIData.Read(_reader);

			CheckFilePosition(entry);

			return gui;
		}
		#endregion

		#region Write file
		private void WriteHeader(BinaryWriter writer)
		{
			using (MemoryStream header = new MemoryStream())
			{
				using (BinaryWriter headerWriter = new BinaryWriter(header, Encoding.ASCII, true))
				{
					headerWriter.WriteTString(GameFolderName);
					headerWriter.WriteTString(GameSaveFileName);
				}

				header.Position = 0;

				writer.Write((uint)header.Length);
				writer.Flush();

				header.CopyTo(writer.BaseStream);
			}
		}

		private void WriteStrings(BinaryWriter writer)
		{
			writer.Write((uint)Strings.Length);
			for (int x = 0; x < Strings.Length; x++)
			{
				writer.WriteTString(Strings[x]);
			}
		}

		private void WriteGraphicsLookup(BinaryWriter writer)
		{
			writer.Write((uint)GraphicsLookup.Length);

			for (int x = 0; x < GraphicsLookup.Length; x++)
			{
				if (GraphicsLookup[x] == null)
				{
					writer.Write((byte)0x00);
					continue;
				}

				writer.Write((byte)0x01);

				Lookup lk = GraphicsLookup[x];

				using (MemoryStream header = new MemoryStream())
				{
					using (BinaryWriter headerWriter = new BinaryWriter(header, Encoding.ASCII, true))
					{
						headerWriter.WriteTString(lk.File);
						headerWriter.WriteTString(lk.IEX);
					}

					header.Position = 0;

					writer.Write((uint)header.Length);
					writer.Flush();

					header.CopyTo(writer.BaseStream);
				}
			}
		}

		private void WriteSoundLookup(BinaryWriter writer)
		{
			writer.Write((uint)SoundLookup.Length);

			for (uint x = 0; x < SoundLookup.Length; x++)
			{
				using (MemoryStream header = new MemoryStream())
				{
					using (BinaryWriter headerWriter = new BinaryWriter(header, Encoding.ASCII, true))
					{
						headerWriter.WriteTString(SoundLookup[x]);
					}

					header.Position = 0;

					writer.Write((uint)header.Length);
					writer.Flush();

					header.CopyTo(writer.BaseStream);
				}
			}
		}

		private void WriteEntry(BinaryWriter writer, IEXEntry entry)
		{
			writer.Write((uint)entry.Length);

			writer.Flush();

			using (FileStream data = File.OpenRead(entry.SourceFile))
			{
				data.CopyTo(writer.BaseStream);
			}
		}

		private void WriteIndexedEntryList(BinaryWriter writer, IEXEntry[] list)
		{
			writer.Write((uint)list.Length);

			for (uint x = 0; x < list.Length; x++)
			{
				IEXEntry entry = list[x];

				if (entry == null)
				{
					writer.Write((byte)0x00);
					continue;
				}

				writer.Write((byte)0x01);

				WriteEntry(writer, entry);
			}
		}

		private void WriteNamedEntry(BinaryWriter writer, IEXEntry entry)
		{
			writer.Write(Path.GetFileName(entry.SourceFile));
			writer.Write((uint)entry.Length);

			writer.Flush();

			using (FileStream data = File.OpenRead(entry.SourceFile))
			{
				data.CopyTo(writer.BaseStream);
			}
		}

		private void WriteNamedEntryList(BinaryWriter writer, IEXEntry[] list)
		{
			writer.Write((uint)list.Length);

			for (uint x = 0; x < list.Length; x++)
			{
				IEXEntry entry = list[x];

				WriteNamedEntry(writer, entry);
			}
		}

		public void Write(BinaryWriter writer)
		{
			WriteHeader(writer);

			WriteStrings(writer);
			WriteGraphicsLookup(writer);
			WriteSoundLookup(writer);

			WriteIndexedEntryList(writer, SoundEntries);
			WriteIndexedEntryList(writer, FontEntires);
			WriteIndexedEntryList(writer, CursorEntries);
			WriteIndexedEntryList(writer, TileEntries);
			WriteIndexedEntryList(writer, ObjectEntries);
			WriteIndexedEntryList(writer, CollisionEntries);
			WriteIndexedEntryList(writer, ParticleEntries);

			WriteEntry(writer, UnknownEntry);
			WriteEntry(writer, FunctonTable1);
			WriteEntry(writer, FunctonTable2);

			WriteNamedEntryList(writer, LevelEntries);
			WriteNamedEntryList(writer, GUIEntries);
		}
		#endregion
	}
}
