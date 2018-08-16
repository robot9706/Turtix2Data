using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIex
{
	public class IEXEntry
	{ 
		public int Length;
		public long FileOffset;

		public string Name;

		public string SourceFile;

		public long EntryEnd
		{
			get
			{
				return FileOffset + Length;
			}
		}

		public IEXEntry(uint length, long fileOffset)
		{
			Length = (int)length;
			FileOffset = fileOffset;
			Name = null;
		}

		public IEXEntry(uint length, long fileOffset, string name)
		{
			Length = (int)length;
			FileOffset = fileOffset;
			Name = name;
		}

		public IEXEntry(string sourceFile)
		{
			SourceFile = sourceFile;

			Length = (int)new FileInfo(SourceFile).Length;
		}
	}
}
