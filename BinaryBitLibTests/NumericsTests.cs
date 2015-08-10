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
        public void TestSingleUnsignedInt()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryBitWriter bw = new BinaryBitWriter(ms))
            using (BinaryBitReader br = new BinaryBitReader(ms))
            {
                Random rand = new Random();
                ushort val = (ushort)(rand.NextDouble() * ushort.MaxValue);
                bw.WriteUInt(val, 22);
                bw.Flush();

                ms.Position = 0;

                Assert.AreEqual(br.ReadUInt(22), val);
            }
        }

        [TestMethod]
        public void TestMultiUnsignedInt()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryBitWriter bw = new BinaryBitWriter(ms))
            using (BinaryBitReader br = new BinaryBitReader(ms))
            {
                Random rand = new Random();
                ushort[] vals = new ushort[Config.MULTI_TEST_COUNT];
                int[] bitLengths = new int[Config.MULTI_TEST_COUNT];
                for (int i = 0; i < Config.MULTI_TEST_COUNT; i++)
                {
                    vals[i] = (ushort)(rand.NextDouble() * ushort.MaxValue);
                    bitLengths[i] = (int)(rand.Next(16, 33));
                    bw.WriteUInt(vals[i], bitLengths[i]);
                }

                bw.Flush();

                ms.Position = 0;

                for (int i = 0; i < Config.MULTI_TEST_COUNT; i++)
                    Assert.AreEqual(br.ReadUInt(bitLengths[i]), vals[i]);
            }
        }

        [TestMethod]
        public void TestSingleSignedInt()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryBitWriter bw = new BinaryBitWriter(ms))
            using (BinaryBitReader br = new BinaryBitReader(ms))
            {
                Random rand = new Random();
                short pVal = (short)(rand.NextDouble() * short.MaxValue);
                short mVal = (short)(rand.NextDouble() * short.MinValue);
                bw.WriteInt(pVal, 22);
                bw.WriteInt(mVal, 22);
                bw.Flush();

                ms.Position = 0;

                Assert.AreEqual(br.ReadInt(22), pVal);
                Assert.AreEqual(br.ReadInt(22), mVal);
            }
        }

        [TestMethod]
        public void TestMultiSignedInt()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryBitWriter bw = new BinaryBitWriter(ms))
            using (BinaryBitReader br = new BinaryBitReader(ms))
            {
                Random rand = new Random();
                short[] vals = new short[Config.MULTI_TEST_COUNT * 2];
                int[] bitLengths = new int[Config.MULTI_TEST_COUNT * 2];
                for (int i = 0; i < Config.MULTI_TEST_COUNT * 2; i += 2)
                {
                    vals[i] = (short)(rand.NextDouble() * short.MaxValue);
                    vals[i + 1] = (short)(rand.NextDouble() * short.MinValue);
                    bitLengths[i] = (int)rand.Next(16, 33);
                    bitLengths[i + 1] = (int)rand.Next(16, 33);

                    bw.WriteInt(vals[i], bitLengths[i]);
                    bw.WriteInt(vals[i + 1], bitLengths[i + 1]);
                }

                bw.Flush();

                ms.Position = 0;

                for (int i = 0; i < Config.MULTI_TEST_COUNT * 2; i++)
                    Assert.AreEqual(br.ReadInt(bitLengths[i]), vals[i]);
            }
        }

        [TestMethod]
        public void TestSingleFloat()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryBitWriter bw = new BinaryBitWriter(ms))
            using (BinaryBitReader br = new BinaryBitReader(ms))
            {
                Random rand = new Random();
                float pVal = (float)(rand.NextDouble() * float.MaxValue);
                float mVal = (float)(rand.NextDouble() * float.MinValue);
                bw.WriteFloat(pVal);
                bw.WriteFloat(mVal);
                bw.Flush();

                ms.Position = 0;

                Assert.AreEqual(br.ReadFloat(), pVal);
                Assert.AreEqual(br.ReadFloat(), mVal);
            }
        }

        [TestMethod]
        public void TestMultiFloat()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryBitWriter bw = new BinaryBitWriter(ms))
            using (BinaryBitReader br = new BinaryBitReader(ms))
            {
                Random rand = new Random();
                float[] vals = new float[Config.MULTI_TEST_COUNT];
                for (int i = 0; i < Config.MULTI_TEST_COUNT; i++)
                {
                    vals[i] = (float)(rand.NextDouble() * float.MaxValue);
                    bw.WriteDouble(vals[i]);
                }

                bw.Flush();

                ms.Position = 0;

                for (int i = 0; i < Config.MULTI_TEST_COUNT; i++)
                    Assert.AreEqual(br.ReadDouble(), vals[i]);
            }
        }

        [TestMethod]
        public void TestSingleDouble()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryBitWriter bw = new BinaryBitWriter(ms))
            using (BinaryBitReader br = new BinaryBitReader(ms))
            {
                Random rand = new Random();
                double pVal = (double)(rand.NextDouble() * double.MaxValue);
                double mVal = (double)(rand.NextDouble() * double.MinValue);
                bw.WriteDouble(pVal);
                bw.WriteDouble(mVal);
                bw.Flush();

                ms.Position = 0;

                Assert.AreEqual(br.ReadDouble(), pVal);
                Assert.AreEqual(br.ReadDouble(), mVal);
            }
        }

        [TestMethod]
        public void TestMultiDouble()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryBitWriter bw = new BinaryBitWriter(ms))
            using (BinaryBitReader br = new BinaryBitReader(ms))
            {
                Random rand = new Random();
                double[] vals = new double[Config.MULTI_TEST_COUNT];
                for (int i = 0; i < Config.MULTI_TEST_COUNT; i++)
                {
                    vals[i] = (double)(rand.NextDouble() * double.MaxValue);
                    bw.WriteDouble(vals[i]);
                }

                bw.Flush();

                ms.Position = 0;

                for (int i = 0; i < Config.MULTI_TEST_COUNT; i++)
                    Assert.AreEqual(br.ReadDouble(), vals[i]);
            }
        }

        [TestMethod]
        public void TestSingleDecimal()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryBitWriter bw = new BinaryBitWriter(ms))
            using (BinaryBitReader br = new BinaryBitReader(ms))
            {
                Random rand = new Random();
                decimal pVal = (decimal)((decimal)rand.NextDouble() * decimal.MaxValue);
                decimal mVal = (decimal)((decimal)rand.NextDouble() * decimal.MinValue);
                bw.WriteDecimal(pVal);
                bw.WriteDecimal(mVal);
                bw.Flush();

                ms.Position = 0;

                Assert.AreEqual(br.ReadDecimal(), pVal);
                Assert.AreEqual(br.ReadDecimal(), mVal);
            }
        }

        [TestMethod]
        public void TestMultiDecimal()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryBitWriter bw = new BinaryBitWriter(ms))
            using (BinaryBitReader br = new BinaryBitReader(ms))
            {
                Random rand = new Random();
                decimal[] vals = new decimal[Config.MULTI_TEST_COUNT];
                for (int i = 0; i < Config.MULTI_TEST_COUNT; i++)
                {
                    vals[i] = (decimal)((decimal)rand.NextDouble() * decimal.MaxValue);
                    bw.WriteDecimal(vals[i]);
                }

                bw.Flush();

                ms.Position = 0;

                for (int i = 0; i < Config.MULTI_TEST_COUNT; i++)
                    Assert.AreEqual(br.ReadDecimal(), vals[i]);
            }
        }
    }
}
