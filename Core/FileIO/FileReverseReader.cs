using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace andy250.CaseLog.Core.FileIO
{
    internal class FileReverseReader : IDisposable
    {
        private const int ReadLength = 4096;

        private bool disposed;
        private readonly FileStream file;
        private readonly Encoding encoding;
        private Func<long, byte, bool> characterStartDetector;

        public FileReverseReader(string path)
        {
            disposed = false;
            file = new FileStream(path, FileMode.Open, FileAccess.Read);
            encoding = FindEncoding(file);
            SetupCharacterStartDetector();
        }

        public List<string> Read(int amountOfLines)
        {
            long position = file.Length;
            if (encoding is UnicodeEncoding && (position & 1) != 0)
            {
                throw new InvalidDataException("UTF-16 encoding provided, but stream has odd length.");
            }

            // Allow up to two bytes for data from the start of the previous
            // read which didn't quite make it as full characters
            byte[] buffer = new byte[ReadLength + 2];
            char[] charBuffer = new char[encoding.GetMaxCharCount(buffer.Length)];
            int leftOverData = 0;
            string previousEnd = null;

            // TextReader doesn't return an empty string if there's line break at the end
            // of the data. Therefore we don't return an empty string if it's our *first*
            // return.
            bool firstLine = true;

            // A line-feed at the start of the previous buffer means we need to swallow
            // the carriage-return at the end of this buffer - hence this needs declaring
            // way up here!
            bool swallowCarriageReturn = false;

            var result = new List<string>();

            while (position > 0 && result.Count < amountOfLines)
            {
                int bytesToRead = Math.Min(position > int.MaxValue ? ReadLength : (int)position, ReadLength);

                position -= bytesToRead;
                file.Position = position;
                ReadExactly(buffer, bytesToRead);

                // If we haven't read a full buffer, but we had bytes left
                // over from before, copy them to the end of the buffer
                if (leftOverData > 0 && bytesToRead != ReadLength)
                {
                    // Buffer.BlockCopy doesn't document its behaviour with respect
                    // to overlapping data: we *might* just have read 7 bytes instead of
                    // 8, and have two bytes to copy...
                    Array.Copy(buffer, ReadLength, buffer, bytesToRead, leftOverData);
                }

                // We've now *effectively* read this much data.
                bytesToRead += leftOverData;

                int firstCharPosition = 0;
                while (!characterStartDetector(position + firstCharPosition, buffer[firstCharPosition]))
                {
                    firstCharPosition++;
                    // Bad UTF-8 sequences could trigger this. For UTF-8 we should always
                    // see a valid character start in every 3 bytes, and if this is the start of the file
                    // so we've done a short read, we should have the character start
                    // somewhere in the usable buffer.
                    if (firstCharPosition == 3 || firstCharPosition == bytesToRead)
                    {
                        throw new InvalidDataException("Invalid UTF-8 data");
                    }
                }
                leftOverData = firstCharPosition;

                int charsRead = encoding.GetChars(buffer, firstCharPosition, bytesToRead - firstCharPosition, charBuffer, 0);
                int endExclusive = charsRead;

                for (int i = charsRead - 1; i >= 0; i--)
                {
                    char lookingAt = charBuffer[i];
                    if (swallowCarriageReturn)
                    {
                        swallowCarriageReturn = false;
                        if (lookingAt == '\r')
                        {
                            endExclusive--;
                            continue;
                        }
                    }

                    // Anything non-line-breaking, just keep looking backwards
                    if (lookingAt != '\n' && lookingAt != '\r')
                    {
                        continue;
                    }

                    // End of CRLF? Swallow the preceding CR
                    if (lookingAt == '\n')
                    {
                        swallowCarriageReturn = true;
                    }

                    int start = i + 1;
                    string bufferContents = new string(charBuffer, start, endExclusive - start);
                    endExclusive = i;
                    string finalString = previousEnd == null ? bufferContents : bufferContents + previousEnd;
                    if (!firstLine || finalString.Length != 0)
                    {
                        result.Add(finalString);
                    }
                    firstLine = false;
                    previousEnd = null;
                }

                previousEnd = endExclusive == 0 ? null : (new string(charBuffer, 0, endExclusive) + previousEnd);

                // If we didn't decode the start of the array, put it at the end for next time
                if (leftOverData != 0)
                {
                    Buffer.BlockCopy(buffer, 0, buffer, ReadLength, leftOverData);
                }
            }

            if (leftOverData != 0)
            {
                // At the start of the final buffer, we had the end of another character.
                throw new InvalidDataException("Invalid UTF-8 data at start of stream");
            }

            if (!firstLine || !string.IsNullOrEmpty(previousEnd))
            {
                result.Add(previousEnd ?? "");
            }

            return result;
        } 

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;
            if (disposing)
            {
                file?.Dispose();
            }
            disposed = true;
        }

        private Encoding FindEncoding(FileStream fs)
        {
            // Read the BOM
            var bom = new byte[4];
            fs.Read(bom, 0, 4);

            // Analyze the BOM
            if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return Encoding.UTF7;
            if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return Encoding.UTF8;
            if (bom[0] == 0xff && bom[1] == 0xfe) return Encoding.Unicode; //UTF-16LE
            if (bom[0] == 0xfe && bom[1] == 0xff) return Encoding.BigEndianUnicode; //UTF-16BE
            if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return Encoding.UTF32;
            return Encoding.ASCII;
        }

        private void SetupCharacterStartDetector()
        {
            if (encoding.IsSingleByte)
            {
                // For a single byte encoding, every byte is the start (and end) of a character
                characterStartDetector = (pos, data) => true;
            }
            else if (encoding is UnicodeEncoding)
            {
                // For UTF-16, even-numbered positions are the start of a character
                characterStartDetector = (pos, data) => (pos & 1) == 0;
            }
            else if (encoding is UTF8Encoding)
            {
                // For UTF-8, bytes with the top bit clear or the second bit set are the start of a character
                // See http://www.cl.cam.ac.uk/~mgk25/unicode.html
                characterStartDetector = (pos, data) => (data & 0x80) == 0 || (data & 0x40) != 0;
            }
            else
            {
                throw new ArgumentException("Only single byte, UTF-8 and Unicode encodings are permitted");
            }
        }

        private void ReadExactly(byte[] buffer, int bytesToRead)
        {
            int index = 0;
            while (index < bytesToRead)
            {
                int read = file.Read(buffer, index, bytesToRead - index);
                if (read == 0)
                {
                    throw new EndOfStreamException($"End of stream reached with {bytesToRead - index} byte{(bytesToRead - index == 1 ? "s" : "")} left to read.");
                }
                index += read;
            }
        }
    }
}
