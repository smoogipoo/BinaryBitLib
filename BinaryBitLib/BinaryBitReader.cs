using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace BinaryBitLib
{
    public class BinaryBitReader : IDisposable
    {
        /// <summary>
        /// The stream being read from.
        /// </summary>
        public Stream BaseStream { get; private set; }

        /// <summary>
        /// The encoding of characters in the underlying stream.
        /// </summary>
        public Encoding Encoding;

        /// <summary>
        /// The data buffer.
        /// </summary>
        private byte[] buffer;

        /// <summary>
        /// Amount of available bits in the buffer.
        /// </summary>
        private int bufferAvailableBits;

        /// <summary>
        /// The current bit in the buffer.
        /// </summary>
        private int _bufferCurrentPosition;
        private int bufferCurrentPosition
        {
            get { return _bufferCurrentPosition; }
            set
            {
                _bufferCurrentPosition = value;
                if (_bufferCurrentPosition >= BufferSize * 8)
                    _bufferCurrentPosition = 0;
            }
        }

        /// <summary>
        /// Total number of bits read from the stream.
        /// </summary>
        public long BitsRead { get; private set; }

        /// <summary>
        /// Total number of bytes read from the stream.
        /// </summary>
        public long BytesRead { get { return BitsRead / 8; } }

        private int bufferSize = 0;
        /// <summary>
        /// Number of bytes to read from the stream into the local buffer.
        /// </summary>
        public int BufferSize
        {
            get { return bufferSize; }
            set
            {
                //Clamp to range [minBufferSize, intMax]
                value = Math.Max(1, value);

                if (bufferSize == value)
                    return;
                int oldBufferSize = bufferSize;
                bufferSize = value;

                //Reset the buffer
                BaseStream.Position -= bufferAvailableBits / 8;
                if (oldBufferSize > 0)
                    bufferCurrentPosition %= oldBufferSize;
                bufferAvailableBits = 0;

                Array.Resize(ref buffer, bufferSize);
            }
        }

        public BinaryBitReader()
            : this(new MemoryStream())
        { }

        public BinaryBitReader(Stream stream)
            : this(stream, new UTF8Encoding(false, true))
        { }

        public BinaryBitReader(Stream stream, Encoding encoding)
        {
            this.Encoding = encoding;

            BaseStream = stream;
            BufferSize = 16;
        }

        /// <summary>
        /// Reads a bit from the stream.
        /// </summary>
        /// <returns>1 or 0.</returns>
        public byte ReadBit()
        {
            EnsureCapacity(1);

            bool res = (buffer[bufferCurrentPosition / 8] & (1 << bufferCurrentPosition % 8)) != 0;
            
            bufferCurrentPosition++;
            bufferAvailableBits--;
            BitsRead++;

            return res ? (byte)1 : (byte)0;
        }

        /// <summary>
        /// Reads a sequence of bits from the stream.
        /// </summary>
        /// <param name="count">The number of bits to read.</param>
        /// <returns>Bits ORed together into a sequence of bytes.</returns>
        public byte[] ReadBits(int count)
        {
            byte[] bytes = new byte[(int)Math.Ceiling(count / 8f)];
            int bit = 0;

            while (bit < count)
            {
                bool read = false;
                //Todo: Reimplement this
                //if (bufferCurrentPosition % 8 == 0)
                //{
                //    //Optimization when byte-aligned, read directly from buffer
                //    while (count - bit >= 8)
                //    {
                //        EnsureCapacity(8);

                //        bytes[bit / 8] = buffer[bufferCurrentPosition / 8];
                //        bufferCurrentPosition += 8;
                //        bufferAvailableBits -= 8;
                //        BitsRead += 8;
                //        bit += 8;
                //        read = true;
                //    }
                //}

                if (!read)
                {
                    bytes[bit / 8] |= (byte)(ReadBit() << (bit % 8));
                    bit++;
                }
            }

            return bytes;
        }

        /// <summary>
        /// Reads a byte from the stream.
        /// </summary>
        /// <returns>The byte.</returns>
        public byte ReadByte()
        {
            return ReadBits(8)[0];
        }

        /// <summary>
        /// Reads a sequence of bytes from the stream.
        /// </summary>
        /// <param name="count">The number of bytes to read.</param>
        /// <returns>The bytes.</returns>
        public byte[] ReadBytes(int count)
        {
            return ReadBits(count * 8);
        }

        /// <summary>
        /// Reads a one-bit-encoded boolean from the stream.
        /// </summary>
        /// <returns>The bool.</returns>
        public bool ReadBool()
        {
            return ReadBit() == 1;
        }

        /// <summary>
        /// Reads an unsigned integer of a specific bit-length from the stream.
        /// </summary>
        /// <param name="numBits">The number of bits to read.</param>
        /// <returns>The unsigned integer.</returns>
        public uint ReadUInt(int numBits = 32)
        {
            if (numBits > 32 || numBits < 1)
                throw new ArgumentException("numBits");

            byte[] bytes = ReadBits(numBits);

            int retVal = 0;
            for (int i = 0; i < bytes.Length; i++)
                retVal |= bytes[i] << i * 8;

            return (uint)retVal;
        }

        /// <summary>
        /// Reads an unsigned long of a specific bit-length from the stream.
        /// </summary>
        /// <param name="numBits">The number of bits to read.</param>
        /// <returns>The unsigned long.</returns>
        public ulong ReadULong(int numBits = 64)
        {
            if (numBits > 64 || numBits < 1)
                throw new ArgumentException("numBits");

            byte[] bytes = ReadBits(numBits);

            long retVal = 0;
            for (int i = 0; i < bytes.Length; i++)
                retVal |= (long)bytes[i] << i * 8;

            return (ulong)retVal;
        }

        /// <summary>
        /// Reads a signed integer of a specific bit-length from the stream.
        /// </summary>
        /// <param name="numBits">The number of bits to read.</param>
        /// <returns>The signed integer.</returns>
        public int ReadInt(int numBits = 32)
        {
            if (numBits > 32 || numBits < 1)
                throw new ArgumentException("numBits");

            byte msb = ReadBit();
            numBits--;

            int ret = msb == 1 ? ~0 : 0;

            for (int i = 0; i < numBits; i++)
            {
                if (msb == 1)
                    ret &= ~(1 << i);
                byte r = ReadBit();
                ret |= r << i;
            }

            return ret;
        }

        /// <summary>
        /// Reads a signed long of a specific bit-length from the stream.
        /// </summary>
        /// <param name="numBits">The number of bits to read.</param>
        /// <returns>The signed long.</returns>
        public long ReadLong(long numBits = 64)
        {
            if (numBits > 64 || numBits < 1)
                throw new ArgumentException("numBits");

            byte msb = ReadBit();
            numBits--;

            long ret = msb == 1 ? ~0 : 0;

            for (int i = 0; i < numBits; i++)
            {
                if (msb == 1)
                    ret &= ~(1 << i);
                byte r = ReadBit();
                ret |= (long)r << i;
            }

            return ret;
        }

        /// <summary>
        /// Reads a float of a specific bit-length from the stream.
        /// </summary>
        /// <param name="numBits">The number of bits to read.</param>
        /// <returns>The float.</returns>
        public unsafe float ReadFloat(int numBits = 32)
        {
            if (numBits > 32 || numBits < 1)
                throw new ArgumentException("numBits");

            uint tmp = ReadUInt(numBits);
            return *(float*)&tmp;
        }

        /// <summary>
        /// Reads a double of a specific bit-length from the stream.
        /// </summary>
        /// <param name="numBits">The number of bits to read.</param>
        /// <returns>The double.</returns>
        public unsafe double ReadDouble(int numBits = 64)
        {
            if (numBits > 64 || numBits < 1)
                throw new ArgumentException("numBits");

            ulong tmp = ReadULong(numBits);
            return *(double*)&tmp;
        }

        /// <summary>
        /// Reads a decimal of a specific bit-length from the stream.
        /// </summary>
        /// <param name="numBits">The number of bits to read.</param>
        /// <returns>The decimal.</returns>
        public decimal ReadDecimal(int numBits = 128)
        {
            if (numBits > 128 || numBits < 1)
                throw new ArgumentException("numBits");

            int[] bits = new int[4];
            for (int i = 0; i < numBits; i++)
                bits[i / 32] |= ReadBit() << i % 32;

            return new decimal(bits);
        }

        /// <summary>
        /// Reads one character from the stream.
        /// </summary>
        /// <returns>The char.</returns>
        public char ReadChar()
        {
            if (Encoding.GetType() == typeof(ASCIIEncoding))
                return (char)ReadUInt(8);
            else if (Encoding.GetType() == typeof(UTF8Encoding))
            {
                byte firstByte = ReadByte();
                if (firstByte >> 7 == 0)
                    return Encoding.GetString(new[] { firstByte })[0];

                int bytesToRead = 0;
                while ((firstByte & (1 << (6 - bytesToRead))) > 0 && bytesToRead < 7)
                    bytesToRead++;

                if (bytesToRead == 0)
                    throw new Exception("Malformed unicode format.");

                byte[] bytes = new byte[bytesToRead + 1];
                bytes[0] = firstByte;

                int ptr = 1;
                while (ptr <= bytesToRead)
                {
                    bytes[ptr] = ReadByte();
                    ptr++;
                }

                return Encoding.GetString(bytes)[0];
            }
            else if (Encoding.GetType() == typeof(UnicodeEncoding))
            {
                int w1 = (int)ReadUInt(16);
                if (w1 < 0xD800 || w1 > 0xDFFF)
                    return Encoding.GetString(BitConverter.GetBytes(w1))[0];
                int w2 = (int)ReadUInt(16);
                if (w2 < 0xDC00 || w2 > 0xDFFF)
                    throw new Exception("Malformed unicode format.");
                
                byte[] w1Bytes = BitConverter.GetBytes(w1);
                byte[] w2Bytes = BitConverter.GetBytes(w2);

                string s = Encoding.GetString(new[] { w1Bytes[0], w1Bytes[1], w2Bytes[0], w2Bytes[1] });
                return '0';
            }
            else if (Encoding.GetType() == typeof(UTF32Encoding))
                return Encoding.GetString(ReadBytes(4))[0];
            return '\0';
        }

        /// <summary>
        /// Reads a sequence of characters from the stream.
        /// </summary>
        /// <param name="count">The number of characters to read.</param>
        /// <returns>The chars.</returns>
        public char[] ReadChars(int count)
        {
            //Todo: Inefficient
            char[] chars = new char[count];
            for (int i = 0; i < count; i++)
                chars[i] = ReadChar();

            return chars;
        }

        /// <summary>
        /// Reads a variable-length encoded string from the stream.
        /// </summary>
        /// <returns>The string.</returns>
        public string ReadString()
        {
            return new string(ReadChars(Read7BitEncodedInt()));
        }

        protected void EnsureCapacity(long bitCount)
        {
            if (bufferAvailableBits - bitCount < 0)
                bufferAvailableBits = BaseStream.Read(buffer, 0, BufferSize - bufferAvailableBits / 8) * 8;
        }

        /// <summary>
        /// See http://referencesource.microsoft.com/#mscorlib/system/io/binaryreader.cs,569
        /// </summary>
        protected int Read7BitEncodedInt()
        {
            int count = 0;
            int shift = 0;
            byte b;
            do
            {
                b = ReadByte();
                count |= (b & 0x7F) << shift;
                shift += 7;
            } while ((b & 0x80) != 0);
            return count;
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

        ~BinaryBitReader()
        {
            Dispose(false);
        }

        #endregion
    }
}
