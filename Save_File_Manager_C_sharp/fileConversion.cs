using System.Collections.Generic;
using System.Text;
using System;
using NPrng.Generators;
using NPrng;
using System.Linq;

namespace Save_File_Manager
{
    public static class fileConversion
    {
        /// <summary>
        /// Creates a file that has been encoded by a seed.<br/>
        /// version numbers:<br/>
        /// - 1: normal: weak<br/>
        /// - 2: secure: stronger<br/>
        /// - 3: super-secure: strogest(only works, if opened on the same location, with the same name)<br/>
        /// - 4: stupid secure: v3 but encription "expires" on the next day
        /// </summary>
        /// <param name="fileLine">The line in the file.</param>
        /// <param name="seed">The ssed for encoding the file</param>
        /// <param name="fileName">The name of the file that will be created. If the name contains a *, it will be replaced with the seed.</param>
        /// <param name="saveExt">The extension of the file that will be created.</param>
        /// <param name="encoding">The encoding of the input lines.</param>
        /// <param name="version">The encription version.</param>
        public static void EncodeFile(string fileLine, long seed=1, string fileName="file", string saveExt="sav", Encoding encoding=null, int version=2)
        {
            EncodeFile(new List<string> { fileLine }, seed, fileName, saveExt, encoding, version);
        }

        /// <summary>
        /// Creates a file that has been encoded by a seed.<br/>
        /// version numbers:<br/>
        /// - 1: normal: weak<br/>
        /// - 2: secure: stronger<br/>
        /// - 3: super-secure: strogest(only works, if opened on the same location, with the same name)<br/>
        /// - 4: stupid secure: v3 but encription "expires" on the next day
        /// </summary>
        /// <param name="fileLines">The list of lines in the file.</param>
        /// <param name="seed">The ssed for encoding the file</param>
        /// <param name="fileName">The name of the file that will be created. If the name contains a *, it will be replaced with the seed.</param>
        /// <param name="saveExt">The extension of the file that will be created.</param>
        /// <param name="encoding">The encoding of the input lines. By default it uses the default encoding.</param>
        /// <param name="version">The encription version.</param>
        public static void EncodeFile(IEnumerable<string> fileLines, long seed=1, string fileName="file", string saveExt="sav", Encoding encoding=null, int version=2)
        {
            var r = new SplittableRandom((ulong)Math.Abs(seed));
            foreach (var line in fileLines)
            {
                EncodeLine(line, r, encoding);
            }
        }

        /// <summary>
        /// Encodes a line into a list of bytes.
        /// </summary>
        /// <param name="line">The line.</param>
        /// <param name="rand">A random number generator from NPrng.</param>
        /// <param name="encoding">The encoding that the text is in.</param>
        /// <returns>The encoded bytes.</returns>
        private static IEnumerable<byte> EncodeLine(string line, AbstractPseudoRandomGenerator rand, Encoding encoding)
        {
            var encode64 = rand.GenerateInRange(2, 5);
            // encoding into bytes
            var lineEnc = encoding.GetBytes(line);
            // change encoding to utf-8
            var lineUtf8 = Encoding.Convert(encoding, Encoding.UTF8, lineEnc);
            // encode to base64 x times
            var lineBase64 = lineUtf8;
            for (int x = 0; x < encode64; x++)
            {
                lineBase64 = Encoding.UTF8.GetBytes(Convert.ToBase64String(lineBase64));
            }
            // shufling bytes
            var lineEncoded = new List<byte>();
            foreach (var byteBase64 in lineBase64)
            {
                var modByte = (byte)(byteBase64 + (int)rand.GenerateInRange(-32, 134));
                lineEncoded.Add(modByte);
            }
            // + \n
            lineEncoded.Add(10);
            return lineEncoded;
        }

        /// <summary>
        /// Decodes a list of bytes into a line.
        /// </summary>
        /// <param name="bytes">The list of bytes.</param>
        /// <param name="rand">A random number generator from NPrng.</param>
        /// <param name="encoding">The encoding that the text is in.</param>
        /// <returns>The decoded line.</returns>
        private static string DecodeLine(IEnumerable<byte> bytes, AbstractPseudoRandomGenerator rand, Encoding encoding)
        {
            var encode64 = rand.GenerateInRange(2, 5);
            // deshufling bytes
            var lineDecoded = new List<byte>();
            foreach (var lineByte in bytes)
            {
                if (lineByte != 10)
                {
                    var modByte = (byte)(lineByte - (int)rand.GenerateInRange(-32, 134));
                    lineDecoded.Add(modByte);
                }
            }
            // encode to base64 x times
            var lineUtf8 = lineDecoded.ToArray();
            for (int x = 0; x < encode64; x++)
            {
                lineUtf8 = Convert.FromBase64String(Encoding.UTF8.GetString(lineUtf8.ToArray()));
            }
            // change encoding from utf-8
            var lineBytes = Encoding.Convert(Encoding.UTF8, encoding, lineUtf8);
            // decode into string
            return encoding.GetString(lineBytes);
        }
    }
}
