using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIex
{
	public class GameFunction
	{
		public string Name;

		public GameAction[] Actions;

		public static GameFunction ReadFunction(BinaryReader reader)
		{
			GameFunction func = new GameFunction();

			func.Name = reader.ReadTString();

			int i1_1 = reader.ReadInt32();

			int numActions = reader.ReadInt32();
			if (numActions > 0)
			{
				func.Actions = new GameAction[numActions];
				for (int x = 0; x < numActions; x++)
				{
					func.Actions[x] = GameAction.ReadAction(reader);
				}
			}

			return func;
		}
	}
}
