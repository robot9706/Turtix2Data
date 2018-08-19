using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIex
{
	public struct ParticleParam
	{
		public float Time;
		public float Value;

		public ParticleParam(float t, float v)
		{
			Time = t;
			Value = v;
		}
	}
}
