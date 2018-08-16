using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIex
{
	public class Lookup
	{
		public string File;
		public string IEX;

		public Lookup(string file, string iex)
		{
			File = file;
			IEX = iex;
		}
	}
}
