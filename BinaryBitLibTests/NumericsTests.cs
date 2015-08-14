using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BinaryBitLib;
using System.IO;

namespace BinaryBitLibTests
{
    [TestClass]
    public class NumericsTests
    {
        [TestMethod]
        public void TestUnsignedShort()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryBitWriter bw = new BinaryBitWriter(ms))
            using (BinaryBitReader br = new BinaryBitReader(ms))
            {
                ushort[] vals = new ushort[Config.MULTI_TEST_COUNT];
                byte[] bytes = new byte[4];

                Random rand = new Random();
                for (int i = 0; i < Config.MULTI_TEST_COUNT; i++)
                {
                    rand.NextBytes(bytes);

                    vals[i] = BitConverter.ToUInt16(bytes, 0);
                    bw.WriteUInt(vals[i], 16);
                }

                bw.Flush();

                ms.Position = 0;

                for (int i = 0; i < Config.MULTI_TEST_COUNT; i++)
                    Assert.AreEqual(vals[i], br.ReadUInt(16));
            }
        }

        [TestMethod]
        public void TestSignedShort()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryBitWriter bw = new BinaryBitWriter(ms))
            using (BinaryBitReader br = new BinaryBitReader(ms))
            {
                short[] vals = new short[Config.MULTI_TEST_COUNT];
                byte[] bytes = new byte[4];

                Random rand = new Random();
                for (int i = 0; i < Config.MULTI_TEST_COUNT; i++)
                {
                    rand.NextBytes(bytes);

                    vals[i] = BitConverter.ToInt16(bytes, 0);
                    bw.WriteInt(vals[i], 16);
                }

                bw.Flush();

                ms.Position = 0;

                for (int i = 0; i < Config.MULTI_TEST_COUNT; i++)
                    Assert.AreEqual(vals[i], br.ReadInt(16));
            }
        }

        [TestMethod]
        public void TestUnsignedInt()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryBitWriter bw = new BinaryBitWriter(ms))
            using (BinaryBitReader br = new BinaryBitReader(ms))
            {
                uint[] vals = new uint[Config.MULTI_TEST_COUNT];
                byte[] bytes = new byte[4];

                Random rand = new Random();
                for (int i = 0; i < Config.MULTI_TEST_COUNT; i++)
                {
                    rand.NextBytes(bytes);

                    vals[i] = BitConverter.ToUInt32(bytes, 0);
                    bw.WriteUInt(vals[i], 32);
                }

                bw.Flush();

                ms.Position = 0;

                for (int i = 0; i < Config.MULTI_TEST_COUNT; i++)
                    Assert.AreEqual(vals[i], br.ReadUInt(32));
            }
        }

        [TestMethod]
        public void TestSignedInt()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryBitWriter bw = new BinaryBitWriter(ms))
            using (BinaryBitReader br = new BinaryBitReader(ms))
            {
                int[] vals = new int[Config.MULTI_TEST_COUNT];
                byte[] bytes = new byte[4];

                Random rand = new Random();
                for (int i = 0; i < Config.MULTI_TEST_COUNT; i++)
                {
                    rand.NextBytes(bytes);

                    vals[i] = BitConverter.ToInt32(bytes, 0);
                    bw.WriteInt(vals[i], 32);
                }

                bw.Flush();

                ms.Position = 0;

                for (int i = 0; i < Config.MULTI_TEST_COUNT; i++)
                    Assert.AreEqual(vals[i], br.ReadInt(32));
            }
        }

        [TestMethod]
        public void TestUnsignedLong()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryBitWriter bw = new BinaryBitWriter(ms))
            using (BinaryBitReader br = new BinaryBitReader(ms))
            {
                ulong[] vals = new ulong[Config.MULTI_TEST_COUNT];
                byte[] bytes = new byte[8];

                Random rand = new Random();
                for (int i = 0; i < Config.MULTI_TEST_COUNT; i++)
                {
                    rand.NextBytes(bytes);

                    vals[i] = BitConverter.ToUInt64(bytes, 0);
                    bw.WriteULong(vals[i], 64);
                }

                bw.Flush();

                ms.Position = 0;

                for (int i = 0; i < Config.MULTI_TEST_COUNT; i++)
                    Assert.AreEqual(vals[i], br.ReadULong(64));
            }
        }

        [TestMethod]
        public void TestSignedLong()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryBitWriter bw = new BinaryBitWriter(ms))
            using (BinaryBitReader br = new BinaryBitReader(ms))
            {
                long[] vals = new long[Config.MULTI_TEST_COUNT];
                byte[] bytes = new byte[8];

                Random rand = new Random();
                for (int i = 0; i < Config.MULTI_TEST_COUNT; i++)
                {
                    rand.NextBytes(bytes);

                    vals[i] = BitConverter.ToInt64(bytes, 0);
                    bw.WriteLong(vals[i], 64);
                }

                bw.Flush();

                ms.Position = 0;

                for (int i = 0; i < Config.MULTI_TEST_COUNT; i++)
                    Assert.AreEqual(vals[i], br.ReadLong(64));
            }
        }

        [TestMethod]
        public void TestFloat()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryBitWriter bw = new BinaryBitWriter(ms))
            using (BinaryBitReader br = new BinaryBitReader(ms))
            {
                float[] vals = new float[Config.MULTI_TEST_COUNT];
                byte[] bytes = new byte[4];

                Random rand = new Random();
                for (int i = 0; i < Config.MULTI_TEST_COUNT; i++)
                {
                    rand.NextBytes(bytes);

                    vals[i] = BitConverter.ToSingle(bytes, 0);
                    bw.WriteFloat(vals[i], 32);
                }

                bw.Flush();

                ms.Position = 0;

                for (int i = 0; i < Config.MULTI_TEST_COUNT; i++)
                    Assert.AreEqual(vals[i], br.ReadFloat(32));
            }
        }

        [TestMethod]
        public void TestDouble()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryBitWriter bw = new BinaryBitWriter(ms))
            using (BinaryBitReader br = new BinaryBitReader(ms))
            {
                double[] vals = new double[Config.MULTI_TEST_COUNT];
                byte[] bytes = new byte[8];

                Random rand = new Random();
                for (int i = 0; i < Config.MULTI_TEST_COUNT; i++)
                {
                    rand.NextBytes(bytes);

                    vals[i] = BitConverter.ToDouble(bytes, 0);
                    bw.WriteDouble(vals[i], 64);
                }

                bw.Flush();

                ms.Position = 0;

                for (int i = 0; i < Config.MULTI_TEST_COUNT; i++)
                    Assert.AreEqual(vals[i], br.ReadDouble(64));
            }
        }

        [TestMethod]
        public void TestDecimal()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryBitWriter bw = new BinaryBitWriter(ms))
            using (BinaryBitReader br = new BinaryBitReader(ms))
            {
                decimal[] vals = new decimal[Config.MULTI_TEST_COUNT];

                Random rand = new Random();
                for (int i = 0; i < Config.MULTI_TEST_COUNT; i++)
                {
                    vals[i] = new decimal(rand.Next(0, int.MaxValue), rand.Next(0, int.MaxValue), rand.Next(0, int.MaxValue), rand.Next(0, 2) == 1, (byte)rand.Next(0, 29));
                    bw.WriteDecimal(vals[i], 128);
                }

                bw.Flush();

                ms.Position = 0;

                for (int i = 0; i < Config.MULTI_TEST_COUNT; i++)
                    Assert.AreEqual(vals[i], br.ReadDecimal(128));
            }
        }
    }
}
