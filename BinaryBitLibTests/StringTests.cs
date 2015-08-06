using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using BinaryBitLib;
using System.Text;

namespace BinaryBitLibTests
{
    [TestClass]
    public class StringTests
    {
        [TestMethod]
        public void TestASCIIString()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryBitWriter bw = new BinaryBitWriter(ms, new ASCIIEncoding()))
            using (BinaryBitReader br = new BinaryBitReader(ms, new ASCIIEncoding()))
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < 128; i++)
                    sb.Append((char)i);

                string result = sb.ToString();

                bw.WriteString(result);
                bw.Flush();

                ms.Position = 0;

                Assert.AreEqual(br.ReadString(), result);
            }
        }

        [TestMethod]
        public void TestUTF8String()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryBitWriter bw = new BinaryBitWriter(ms, new UTF8Encoding()))
            using (BinaryBitReader br = new BinaryBitReader(ms, new UTF8Encoding()))
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < 55296; i++)
                    sb.Append((char)i);

                string result = sb.ToString();

                bw.WriteString(result);
                bw.Flush();

                ms.Position = 0;

                Assert.AreEqual(br.ReadString(), result);
            }
        }

        [TestMethod]
        public void TestUTF16String()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryBitWriter bw = new BinaryBitWriter(ms, new UnicodeEncoding()))
            using (BinaryBitReader br = new BinaryBitReader(ms, new UnicodeEncoding()))
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < 55296; i++)
                    sb.Append((char)i);

                string result = sb.ToString();

                bw.WriteString(result);
                bw.Flush();

                ms.Position = 0;

                Assert.AreEqual(br.ReadString(), result);
            }
        }

        [TestMethod]
        public void TestUTF32String()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryBitWriter bw = new BinaryBitWriter(ms, new UTF32Encoding()))
            using (BinaryBitReader br = new BinaryBitReader(ms, new UTF32Encoding()))
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < 55296; i++)
                    sb.Append((char)i);

                string result = sb.ToString() + sb.ToString() + sb.ToString();

                bw.WriteString(result);
                bw.Flush();

                ms.Position = 0;

                Assert.AreEqual(br.ReadString(), result);
            }
        }
    }
}
