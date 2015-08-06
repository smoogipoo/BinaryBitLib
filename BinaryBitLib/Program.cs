using BinaryBitLib.BinaryBitLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace BinaryBitLib
{
    class Program
    {
        static void Main(string[] args)
        {

            using (MemoryStream ms = new MemoryStream())
            using (BinaryBitWriter bw = new BinaryBitWriter(ms, new UTF32Encoding()))
            using (BinaryBitReader br = new BinaryBitReader(ms, new UTF32Encoding()))
            {
                bw.BufferSize = 4096;
                br.BufferSize = 4096;

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < 109384; i++)
                    sb.Append((char)i);

                string original = sb.ToString();


                bw.WriteString(original);
                bw.Flush();

                ms.Position = 0;
                string asdf = br.ReadString();

                if (original.Length != asdf.Length)
                    Console.WriteLine("Lengths not equal.");

                for (int i = 0; i < asdf.Length; i++)
                    if (original[i] != asdf[i])
                        throw new Exception(string.Format("Character differs. {0} -> {1}", original[i], asdf[i]));
            }

            Console.ReadKey();
        }
    }
}
