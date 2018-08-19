using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIex
{
	public class ParticleData
	{
		public byte UnknownByte1;

		public float UnknownFloat1;

		public ParticleInternalData[] DataArray;

		public class ParticleInternalData
		{
			public uint GraphicsIndex;

			public uint GraphicsTileIndex;

			public uint UnknownUInt2;
			public uint UnknownUInt3;
			public uint UnknownUInt4;
			public uint UnknownUInt5;

			public float UnknownFloat1;

			public byte AdditiveBlending;
			public byte UseXSizesForYSizes;
			public byte LoopAndNoDespawn;
			public byte UnknownByte4;

			public ParticleParam[] TimeScaleParamArray;
			public ParticleParam[] TimeScaleRandomizerParamArray;
			public ParticleParam[] SpawnParamArray; //Time: Timestamp, Value: Number of particles to spawn
			public ParticleParam[] SpawnScaleParamArray; //Probably
			public ParticleParam[] SizeXParamArray;
			public ParticleParam[] UnknownParamArray6;
			public ParticleParam[] ScaleXParamArray;
			public ParticleParam[] SizeYParamArray;
			public ParticleParam[] UnknownParamArray9;
			public ParticleParam[] ScaleYParamArray;
			public ParticleParam[] UnknownParamArray11;
			public ParticleParam[] UnknownParamArray12;
			public ParticleParam[] UnknownParamArray13;
			public ParticleParam[] SomethingRotationParamArray;
			public ParticleParam[] UnknownParamArray15;
			public ParticleParam[] UnknownParamArray16;
			public ParticleParam[] UnknownParamArray17;
			public ParticleParam[] UnknownParamArray18;
			public ParticleParam[] UnknownParamArray19;
			public ParticleParam[] UnknownParamArray20;
			public ParticleParam[] UnknownParamArray21;
			public ParticleParam[] UnknownParamArray22;
			public ParticleParam[] UnknownParamArray23;
			public ParticleParam[] UnknownParamArray24;
			public ParticleParam[] UnknownParamArray25;
			public ParticleParam[] UnknownParamArray26;
			public ParticleParam[] UnknownParamArray27;
			public ParticleParam[] UnknownParamArray28;
			public ParticleParam[] UnknownParamArray29;
			public ParticleParam[] UnknownParamArray30;
			public ParticleParam[] UnknownParamArray31;
			public ParticleParam[] UnknownParamArray32;
			public ParticleParam[] UnknownParamArray33;
			public ParticleParam[] UnknownParamArray34;
			public ParticleParam[] AlphaParamArray;
		}

		public static ParticleData Read(BinaryReader reader)
		{
			ParticleData data = new ParticleData();

			data.UnknownByte1 = reader.ReadByte();
			data.UnknownFloat1 = reader.ReadSingle();

			uint arrayLength = reader.ReadUInt32();
			data.DataArray = new ParticleInternalData[arrayLength];
			for (int x = 0; x < arrayLength; x++)
			{
				ParticleInternalData el = new ParticleInternalData();

				el.GraphicsIndex = reader.ReadUInt32();

				el.GraphicsTileIndex = reader.ReadUInt32();
				el.UnknownUInt2 = reader.ReadUInt32(); //as float
				el.UnknownUInt3 = reader.ReadUInt32(); //as float
				el.UnknownUInt4 = reader.ReadUInt32();
				el.UnknownUInt5 = reader.ReadUInt32();

				el.UnknownFloat1 = reader.ReadSingle();

				el.AdditiveBlending = reader.ReadByte();
				el.UseXSizesForYSizes = reader.ReadByte();
				el.LoopAndNoDespawn = reader.ReadByte();
				el.UnknownByte4 = reader.ReadByte();

				el.TimeScaleParamArray = ReadArray(reader);
				el.TimeScaleRandomizerParamArray = ReadArray(reader);
				el.SpawnParamArray = ReadArray(reader);
				el.SpawnScaleParamArray = ReadArray(reader);
				el.SizeXParamArray = ReadArray(reader);
				el.UnknownParamArray6 = ReadArray(reader);
				el.ScaleXParamArray = ReadArray(reader);
				el.SizeYParamArray = ReadArray(reader);
				el.UnknownParamArray9 = ReadArray(reader);
				el.ScaleYParamArray = ReadArray(reader);
				el.UnknownParamArray11 = ReadArray(reader);
				el.UnknownParamArray12 = ReadArray(reader);
				el.UnknownParamArray13 = ReadArray(reader);
				el.SomethingRotationParamArray = ReadArray(reader);
				el.UnknownParamArray15 = ReadArray(reader);
				el.UnknownParamArray16 = ReadArray(reader);
				el.UnknownParamArray17 = ReadArray(reader);
				el.UnknownParamArray18 = ReadArray(reader);
				el.UnknownParamArray19 = ReadArray(reader);
				el.UnknownParamArray20 = ReadArray(reader);
				el.UnknownParamArray21 = ReadArray(reader);
				el.UnknownParamArray22 = ReadArray(reader);
				el.UnknownParamArray23 = ReadArray(reader);
				el.UnknownParamArray24 = ReadArray(reader);
				el.UnknownParamArray25 = ReadArray(reader);
				el.UnknownParamArray26 = ReadArray(reader);
				el.UnknownParamArray27 = ReadArray(reader);
				el.UnknownParamArray28 = ReadArray(reader);
				el.UnknownParamArray29 = ReadArray(reader);
				el.UnknownParamArray30 = ReadArray(reader);
				el.UnknownParamArray31 = ReadArray(reader);
				el.UnknownParamArray32 = ReadArray(reader);
				el.UnknownParamArray33 = ReadArray(reader);
				el.UnknownParamArray34 = ReadArray(reader);
				el.AlphaParamArray = ReadArray(reader);

				data.DataArray[x] = el;
			}

			return data;
		}

		private static ParticleParam[] ReadArray(BinaryReader reader)
		{
			int len = reader.ReadInt32();

			ParticleParam[] ar = new ParticleParam[len];

			for (int x = 0; x < len; x++)
			{
				ar[x] = new ParticleParam(reader.ReadSingle(), reader.ReadSingle());
			}

			return ar;
		}

		public static void Write(ParticleData data, BinaryWriter writer)
		{
			writer.Write(data.UnknownByte1);

			writer.Write(data.UnknownFloat1);

			writer.Write(data.DataArray.Length);

			for (int x = 0; x < data.DataArray.Length; x++)
			{
				ParticleInternalData el = data.DataArray[x];

				writer.Write(el.GraphicsIndex);

				writer.Write(el.GraphicsTileIndex);
				writer.Write(el.UnknownUInt2);
				writer.Write(el.UnknownUInt3);
				writer.Write(el.UnknownUInt4);
				writer.Write(el.UnknownUInt5);

				writer.Write(el.UnknownFloat1);

				writer.Write(el.AdditiveBlending);
				writer.Write(el.UseXSizesForYSizes);
				writer.Write(el.LoopAndNoDespawn);
				writer.Write(el.UnknownByte4);

				WriteArray(el.TimeScaleParamArray, writer);
				WriteArray(el.TimeScaleRandomizerParamArray, writer);
				WriteArray(el.SpawnParamArray, writer);
				WriteArray(el.SpawnScaleParamArray, writer);
				WriteArray(el.SizeXParamArray, writer);
				WriteArray(el.UnknownParamArray6, writer);
				WriteArray(el.ScaleXParamArray, writer);
				WriteArray(el.SizeYParamArray, writer);
				WriteArray(el.UnknownParamArray9, writer);
				WriteArray(el.ScaleYParamArray, writer);
				WriteArray(el.UnknownParamArray11, writer);
				WriteArray(el.UnknownParamArray12, writer);
				WriteArray(el.UnknownParamArray13, writer);
				WriteArray(el.SomethingRotationParamArray, writer);
				WriteArray(el.UnknownParamArray15, writer);
				WriteArray(el.UnknownParamArray16, writer);
				WriteArray(el.UnknownParamArray17, writer);
				WriteArray(el.UnknownParamArray18, writer);
				WriteArray(el.UnknownParamArray19, writer);
				WriteArray(el.UnknownParamArray20, writer);
				WriteArray(el.UnknownParamArray21, writer);
				WriteArray(el.UnknownParamArray22, writer);
				WriteArray(el.UnknownParamArray23, writer);
				WriteArray(el.UnknownParamArray24, writer);
				WriteArray(el.UnknownParamArray25, writer);
				WriteArray(el.UnknownParamArray26, writer);
				WriteArray(el.UnknownParamArray27, writer);
				WriteArray(el.UnknownParamArray28, writer);
				WriteArray(el.UnknownParamArray29, writer);
				WriteArray(el.UnknownParamArray30, writer);
				WriteArray(el.UnknownParamArray31, writer);
				WriteArray(el.UnknownParamArray32, writer);
				WriteArray(el.UnknownParamArray33, writer);
				WriteArray(el.UnknownParamArray34, writer);
				WriteArray(el.AlphaParamArray, writer);
			}
		}

		private static void WriteArray(ParticleParam[] array, BinaryWriter writer)
		{
			writer.Write(array.Length);

			for (int x = 0; x < array.Length; x++)
			{
				writer.Write(array[x].Time);
				writer.Write(array[x].Value);
			}
		}
	}
}
