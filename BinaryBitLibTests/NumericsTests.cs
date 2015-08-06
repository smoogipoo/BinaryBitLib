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
            const int SHORTS_COUNT = 65535;

            using (MemoryStream ms = new MemoryStream())
            using (BinaryBitWriter bw = new BinaryBitWriter(ms))
            using (BinaryBitReader br = new BinaryBitReader(ms))
            {
                Random rand = new Random();
                ushort[] vals = new ushort[SHORTS_COUNT];
                int[] bitLengths = new int[SHORTS_COUNT];
                for (int i = 0; i < SHORTS_COUNT; i++)
                {
                    vals[i] = (ushort)(rand.NextDouble() * ushort.MaxValue);
                    bitLengths[i] = (int)(rand.Next(16, 33));
                    bw.WriteUInt(vals[i], bitLengths[i]);
                }

                bw.Flush();

                ms.Position = 0;

                for (int i = 0; i < SHORTS_COUNT; i++)
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
            const int SHORTS_COUNT = 65535;

            using (MemoryStream ms = new MemoryStream())
            using (BinaryBitWriter bw = new BinaryBitWriter(ms))
            using (BinaryBitReader br = new BinaryBitReader(ms))
            {
                Random rand = new Random();
                short[] vals = new short[SHORTS_COUNT * 2];
                int[] bitLengths = new int[SHORTS_COUNT * 2];
                for (int i = 0; i < SHORTS_COUNT * 2; i += 2)
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

                for (int i = 0; i < SHORTS_COUNT * 2; i++)
                    Assert.AreEqual(br.ReadInt(bitLengths[i]), vals[i]);
            }
        }
    }
}
