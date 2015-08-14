using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using BinaryBitLib;
using System.Runtime.InteropServices;
using System.Threading;

namespace BinaryBitLibTests
{
    [TestClass]
    public class ResourcesTest
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
        public void TestResourceRelease()
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryBitWriter bw = new BinaryBitWriter(ms))
            { }
            Assert.IsFalse(ms.CanRead || ms.CanWrite);

            ms = new MemoryStream();
            using (BinaryBitReader br = new BinaryBitReader(ms))
            { }
            Assert.IsFalse(ms.CanRead || ms.CanWrite);

            ms = new MemoryStream();
            createReader(ms);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Assert.IsFalse(ms.CanRead || ms.CanWrite);

            ms = new MemoryStream();
            createWriter(ms);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Assert.IsFalse(ms.CanRead || ms.CanWrite);
        }

        private void createReader(MemoryStream ms)
        {
            BinaryBitReader reader = new BinaryBitReader(ms);
        }

        private void createWriter(MemoryStream ms)
        {
            BinaryBitWriter writer = new BinaryBitWriter(ms);
        }
    }
}
