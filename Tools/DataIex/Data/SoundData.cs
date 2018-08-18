using System.IO;

namespace DataIex
{
	public class SoundData
	{
		public uint[] SoundIndexes;

		public static SoundData Read(BinaryReader reader)
		{
			SoundData data = new SoundData();

			uint arrLength = reader.ReadUInt32();
			data.SoundIndexes = new uint[arrLength];
			for (int x = 0; x < arrLength; x++)
			{
				data.SoundIndexes[x] = reader.ReadUInt32();
			}

			return data;
		}

		public static void Write(SoundData data, BinaryWriter writer)
		{
			writer.Write(data.SoundIndexes.Length);
			for(int x = 0; x < data.SoundIndexes.Length; x++)
			{
				writer.Write(data.SoundIndexes[x]);
			}
		}
	}
}
