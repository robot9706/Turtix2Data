using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsPacker
{
	static class Ext
	{
		public static string ToDecHex(uint value)
		{
			return (value.ToString() + " (0x" + value.ToString("X2") + ")");
		}
	}
}
