using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using BinaryBitLib;
using System.Text;

namespace BinaryBitLibTests
{
    [TestClass]
    public class CharTests
    {
        [TestMethod]
        public void TestASCIIChars()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryBitWriter bw = new BinaryBitWriter(ms, new ASCIIEncoding()))
            using (BinaryBitReader br = new BinaryBitReader(ms, new ASCIIEncoding()))
            {
                for (int i = 0; i < 128; i++)
                    bw.WriteChar((char)i);
                bw.Flush();

                ms.Position = 0;

                for (int i = 0; i < 128; i++)
                    Assert.AreEqual((char)i, br.ReadChar());
            }
        }

        [TestMethod]
        public void TestUTF8Chars()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryBitWriter bw = new BinaryBitWriter(ms, new UTF8Encoding()))
            using (BinaryBitReader br = new BinaryBitReader(ms, new UTF8Encoding()))
            {
                for (int i = 0; i < 55296; i++)
                    bw.WriteChar((char)i);
                bw.Flush();

                ms.Position = 0;

                for (int i = 0; i < 55296; i++)
                    Assert.AreEqual((char)i, br.ReadChar());
            }
        }

        [TestMethod]
        public void TestUTF16Chars()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryBitWriter bw = new BinaryBitWriter(ms, new UnicodeEncoding()))
            using (BinaryBitReader br = new BinaryBitReader(ms, new UnicodeEncoding()))
            {
                for (int i = 0; i < 55296; i++)
                    bw.WriteChar((char)i);
                bw.Flush();

                ms.Position = 0;

                for (int i = 0; i < 55296; i++)
                    Assert.AreEqual((char)i, br.ReadChar());
            }
        }

        [TestMethod]
        public void TestUTF32Chars()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryBitWriter bw = new BinaryBitWriter(ms, new UTF32Encoding()))
            using (BinaryBitReader br = new BinaryBitReader(ms, new UTF32Encoding()))
            {
                for (int i = 0; i < 55296; i++)
                    bw.WriteChar((char)i);
                bw.Flush();

                ms.Position = 0;

                for (int i = 0; i < 55296; i++)
                    Assert.AreEqual((char)i, br.ReadChar());
            }
        }
    }
}
