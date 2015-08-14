using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using BinaryBitLib;

namespace BinaryBitLibTests
{
    [TestClass]
    public class BufferedNumericsTests
    {
        [TestMethod]
        public void TestMultiFloat()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryBitWriter bw = new BinaryBitWriter(ms))
            using (BinaryBitReader br = new BinaryBitReader(ms))
            {
                br.BufferSize = bw.BufferSize = Config.MULTI_TEST_BUFFER_COUNT * sizeof(float);

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
                    Assert.AreEqual(vals[i], br.ReadDouble());
            }
        }

        [TestMethod]
        public void TestMultiDouble()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryBitWriter bw = new BinaryBitWriter(ms))
            using (BinaryBitReader br = new BinaryBitReader(ms))
            {
                br.BufferSize = bw.BufferSize = Config.MULTI_TEST_BUFFER_COUNT * sizeof(double);

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
                    Assert.AreEqual(vals[i], br.ReadDouble());
            }
        }

        [TestMethod]
        public void TestMultiDecimal()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryBitWriter bw = new BinaryBitWriter(ms))
            using (BinaryBitReader br = new BinaryBitReader(ms))
            {
                br.BufferSize = bw.BufferSize = Config.MULTI_TEST_BUFFER_COUNT * sizeof(decimal);

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
                    Assert.AreEqual(vals[i], br.ReadDecimal());
            }
        }
    }
}
