using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using BinaryBitLib;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;

namespace BinaryBitLibTests
{
    [TestClass]
    public class PrimitiveTests
    {
        [TestMethod]
        public void TestOverflow()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryBitWriter bw = new BinaryBitWriter(ms))
            using (BinaryBitReader br = new BinaryBitReader(ms))
            {
                br.ReadBit();
                br.ReadBits(10);
                br.ReadBool();
                br.ReadByte();
                br.ReadBytes(10);
                br.ReadChar();
                br.ReadChars(10);
                br.ReadInt();
                br.ReadLong();
                br.ReadUInt();
                br.ReadULong();
                br.ReadString();

                bw.Flush();
            }
        }

        [TestMethod]
        public void TestBit()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryBitWriter bw = new BinaryBitWriter(ms))
            using (BinaryBitReader br = new BinaryBitReader(ms))
            {
                for (int i = 0; i < 256; i++)
                    bw.WriteBit((byte)i);
                bw.Flush();

                ms.Position = 0;

                for (int i = 0; i < 256; i++)
                {
                    if (i == 0)
                        Assert.AreEqual(br.ReadBit(), 0);
                    else
                        Assert.AreEqual(br.ReadBit(), 1);
                }
            }
        }

        [TestMethod]
        public void TestBits()
        {
            const int BITS_COUNT = 65536;

            using (MemoryStream ms = new MemoryStream())
            using (BinaryBitWriter bw = new BinaryBitWriter(ms))
            using (BinaryBitReader br = new BinaryBitReader(ms))
            {
                byte[] writeBytes = new byte[BITS_COUNT];
                for (int i = 0; i < BITS_COUNT; i++)
                    writeBytes[i] = 1;
                bw.WriteBits(writeBytes);
                bw.Flush();

                ms.Position = 0;

                byte[] readBytes = br.ReadBits(BITS_COUNT);
                for (int i = 0; i < BITS_COUNT; i++)
                    Assert.IsTrue((readBytes[i / 8] & (1 << i % 8)) > 0);
            }
        }

        [TestMethod]
        public void TestByte()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryBitWriter bw = new BinaryBitWriter(ms))
            using (BinaryBitReader br = new BinaryBitReader(ms))
            {
                for (int i = 0; i < 256; i++)
                    bw.WriteByte((byte)i);
                bw.Flush();

                ms.Position = 0;

                for (int i = 0; i < 256; i++)
                    Assert.AreEqual(br.ReadByte(), (byte)i);
            }
        }

        [TestMethod]
        public void TestBytes()
        {
            const int BYTES_COUNT = 65536;

            using (MemoryStream ms = new MemoryStream())
            using (BinaryBitWriter bw = new BinaryBitWriter(ms))
            using (BinaryBitReader br = new BinaryBitReader(ms))
            {
                Random rand = new Random();

                byte[] writeBytes = new byte[BYTES_COUNT];
                rand.NextBytes(writeBytes);
                bw.WriteBytes(writeBytes);
                bw.Flush();

                ms.Position = 0;

                byte[] readBytes = br.ReadBytes(BYTES_COUNT);
                for (int i = 0; i < readBytes.Length; i++)
                    Assert.AreEqual(readBytes[i], writeBytes[i]);
            }
        }
    }
}
