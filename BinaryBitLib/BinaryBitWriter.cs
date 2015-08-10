using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BinaryBitLib
{
    public class BinaryBitWriter : IDisposable
    {
        /// <summary>
        /// The stream being written to.
        /// </summary>
        public Stream BaseStream { get; private set; }

        /// <summary>
        /// The encoding of characters in the underlying stream.
        /// </summary>
        public Encoding Encoding;

        private byte currentByte;
        private int currentBit;
        private bool rewriteByte;

        private int newBytesWritten;

        private int bufferSize;
        /// <summary>
        /// Number of bytes to buffer before writing to the stream.
        /// </summary>
        public int BufferSize
        {
            get { return bufferSize; }
            set { bufferSize = Math.Max(1, value); }
        }

        public BinaryBitWriter()
            : this(new MemoryStream())
        { }

        public BinaryBitWriter(Stream stream)
            : this(stream, new UTF8Encoding())
        { }

        public BinaryBitWriter(Stream stream, Encoding encoding)
        {
            this.Encoding = encoding;

            BaseStream = stream;
            BufferSize = 16;
        }

        /// <summary>
        /// Writes a bit to the stream.
        /// </summary>
        /// <param name="value">The bit to write.</param>
        public void WriteBit(byte value)
        {
            EnsureCapacity();

            if (value != 0)
                currentByte |= (byte)(1 << currentBit);

            currentBit++;
        }

        /// <summary>
        /// Writes a sequence of bits to the stream.
        /// </summary>
        /// <param name="value">The bits to write.</param>
        public void WriteBits(byte[] value)
        {
            for (int i = 0; i < value.Length; i++)
                WriteBit(value[i]);
        }

        /// <summary>
        /// Writes a byte to the stream.
        /// </summary>
        /// <param name="value">The byte to write.</param>
        public void WriteByte(byte value)
        {
            for (int i = 0; i < 8; i++)
                WriteBit((byte)(value & (1 << i)));
        }

        /// <summary>
        /// Writes a sequence of bytes to the stream.
        /// </summary>
        /// <param name="value">The bytes to write.</param>
        public void WriteBytes(byte[] value)
        {
            for (int i = 0; i < value.Length; i++)
                WriteByte(value[i]);
        }
        
        /// <summary>
        /// Writes a one-bit-encoded boolean to the stream.
        /// </summary>
        /// <param name="value">The boolean to write.</param>
        public void WriteBool(bool value)
        {
            WriteBit(value ? (byte)1 : (byte)0);
        }

        /// <summary>
        /// Writes an unsigned integer of a specific bit-length to the stream.
        /// </summary>
        /// <param name="value">The integer to write.</param>
        /// <param name="numBits">The bit-length of the integer.</param>
        public void WriteUInt(uint value, int numBits = 32)
        {
            if (numBits > 32 || numBits < 1)
                throw new ArgumentException("numBits");

            for (int i = 0; i < numBits; i++)
                WriteBit((value & ((uint)1 << i)) > 0 ? (byte)1 : (byte)0);
        }

        /// <summary>
        /// Writes an unsigned long of a specific bit-length to the stream.
        /// </summary>
        /// <param name="value">The long to write.</param>
        /// <param name="numBits">The bit-length of the long.</param>
        public void WriteULong(ulong value, int numBits = 64)
        {
            if (numBits > 64 || numBits < 1)
                throw new ArgumentException("numBits");

            for (int i = 0; i < numBits; i++)
                WriteBit((value & ((ulong)1 << i)) > 0 ? (byte)1 : (byte)0);
        }

        /// <summary>
        /// Writes a signed integer of a specific bit-length to the stream.
        /// </summary>
        /// <param name="value">The integer to write.</param>
        /// <param name="numBits">The bit-length of the integer.</param>
        public unsafe void WriteInt(int value, int numBits = 32)
        {
            if (numBits > 32 || numBits < 1)
                throw new ArgumentException("numBits");

            uint tmp = *(uint*)&value;
            
            WriteBit((tmp & ((uint)1 << numBits - 1)) > 0 ? (byte)1 : (byte)0);
            numBits--;

            WriteUInt(tmp, numBits);
        }

        /// <summary>
        /// Writes a signed long of a specific bit-length to the stream.
        /// </summary>
        /// <param name="value">The long to write.</param>
        /// <param name="numBits">The bit-length of the integer.</param>
        public unsafe void WriteLong(long value, int numBits = 64)
        {
            if (numBits > 64 || numBits < 1)
                throw new ArgumentException("numBits");

            ulong tmp = *(ulong*)&value;
            
            WriteBit((tmp & ((ulong)1 << numBits - 1)) > 0 ? (byte)1 : (byte)0);
            numBits--;

            WriteULong(tmp, numBits);
        }

        /// <summary>
        /// Writes a float of a specific bit-length to the stream.
        /// </summary>
        /// <param name="value">The float to write.</param>
        /// <param name="numBits">The bit-length of the float.</param>
        public unsafe void WriteFloat(float value, int numBits = 32)
        {
            if (numBits > 32 || numBits < 1)
                throw new ArgumentException("numBits");

            uint tmp = *(uint*)&value;
            WriteUInt(tmp, numBits);
        }

        /// <summary>
        /// Writes a double of a specific bit-length to the stream.
        /// </summary>
        /// <param name="value">The double to write.</param>
        /// <param name="numBits">The bit-length of the double.</param>
        public unsafe void WriteDouble(double value, int numBits = 64)
        {
            if (numBits > 64 || numBits < 1)
                throw new ArgumentException("numBits");

            ulong tmp = *(ulong*)&value;
            WriteULong(tmp, numBits);
        }

        public void WriteDecimal(decimal value, int numBits = 128)
        {
            if (numBits > 128 || numBits < 1)
                throw new ArgumentException("numBits");

            int[] bits = Decimal.GetBits(value);
            for (int i = 0; i < numBits; i++)
                WriteBit((bits[i / 32] & ((uint)1 << i % 32)) > 0 ? (byte)1 : (byte)0);
        }

        /// <summary>
        /// Writes a character to the stream.
        /// </summary>
        /// <param name="value">The character to write.</param>
        public void WriteChar(char value)
        {
            byte[] bytes = Encoding.GetBytes(new[] { value });
            WriteBytes(bytes);
        }

        /// <summary>
        /// Writes a sequence of characters to the stream.
        /// </summary>
        /// <param name="value">The characters to write.</param>
        public void WriteChars(char[] value)
        {
            for (int i = 0; i < value.Length; i++)
                WriteChar(value[i]);
        }

        /// <summary>
        /// Writes a variable-length encoded string to the stream.
        /// </summary>
        /// <param name="value">The string to write.</param>
        public void WriteString(string value)
        {
            Write7BitEncodedInt(value.Length);
            for (int i = 0; i < value.Length; i++)
                WriteChar(value[i]);
        }

        /// <summary>
        /// Causes the BinaryBitWriter to flush any remaining data to the underlying stream.
        /// </summary>
        public void Flush()
        {
            writeCurrentByte();
            BaseStream.Flush();
            
            rewriteByte = currentBit != 8;

            if (!rewriteByte)
                currentBit = 0;
            newBytesWritten = 0;
        }

        protected void EnsureCapacity()
        {
            if (currentBit == 8)
            {
                writeCurrentByte();
                currentByte = 0;
                currentBit = 0;
            }

            if (newBytesWritten >= BufferSize)
                BaseStream.Flush();
        }

        private void writeCurrentByte()
        {
            if (rewriteByte)
            {
                BaseStream.Position--;
                rewriteByte = false;
            }

            BaseStream.WriteByte(currentByte);
            newBytesWritten++;
        }

        /// <summary>
        /// See http://referencesource.microsoft.com/#mscorlib/system/io/binarywriter.cs,407
        /// </summary>
        protected void Write7BitEncodedInt(int value)
        {
            uint v = (uint)value;
            while (v >= 0x80)
            {
                WriteByte((byte)(v | 0x80));
                v >>= 7;
            }
            WriteByte((byte)v);
        }

        #region Disposal

        private bool disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                    BaseStream.Dispose();

                BaseStream = null;
                
                disposed = true;
            }
        }

        ~BinaryBitWriter()
        {
            Dispose(false);
        }

        #endregion
    }
}
