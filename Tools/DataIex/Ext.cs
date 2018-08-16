using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIex
{
	static class Ext
	{
		public static string ReadTString(this BinaryReader reader)
		{
			byte len = reader.ReadByte();
			byte[] ascii = reader.ReadBytes(len);

			return Encoding.ASCII.GetString(ascii);
		}

		public static void WriteTString(this BinaryWriter writer, string text)
		{
			byte[] ascii = Encoding.ASCII.GetBytes(text);
			if (ascii.Length > 255)
			{
				throw new ArgumentOutOfRangeException("Text too long!");
			}

			writer.Write((byte)ascii.Length);
			writer.Write(ascii);
		}
	}
}
