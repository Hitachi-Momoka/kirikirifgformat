using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Windows;
using Li.Krkr.krkrfgformatWPF.Models;
using Newtonsoft.Json;

namespace Li.Krkr.krkrfgformatWPF.Serves
{
    public static class RuleDataServes
    {
        public static RuleDataModel CreateFromFile(string file)
        {
            if (string.IsNullOrEmpty(file))
                return null;

            if (Path.GetExtension(file).Equals(".json"))
            {
                return CreateFromJsonFile(file);
            }
            var fs = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read);
            var br = new BinaryReader(fs);
            var header = br.ReadBytes(5);
            var index = 0;
            var signature = (uint)(
                header[index]
                | (header[index + 1] << 8)
                | (header[index + 2] << 16)
                | (header[index + 3] << 24)
            );
            if ((signature & 0xFF00FFFFu) == 0xFF00FEFEu && header[2] < 3 && 0xFE == header[4])
            {
                switch (header[2])
                {
                    case 2:
                        return CreateFromZlibFile(fs, br);
                    case 1:
                        //br.Close();
                        return CreateFromCryptFile(fs);
                }
            }
            var encoding = (header[1] == 0) ? Encoding.Unicode : Encoding.UTF8;

            return CreateFromTextFile(fs, encoding);
        }

        /// <summary>
        /// 解析被简单算法加密的txt文档
        /// </summary>
        /// <param name="fs"></param>
        /// <returns></returns>
        private static RuleDataModel CreateFromCryptFile(Stream fs)
        {
            var reader = new BinaryReader(fs, Encoding.Unicode, true);
            var outsr = new MemoryStream((int)fs.Length + 2);
            var writer = new BinaryWriter(outsr, Encoding.Unicode, true);
            writer.Write('\xFEFF');
            int c;
            while ((c = reader.Read()) != -1)
            {
                c = (c & 0xAAAA) >> 1 | (c & 0x5555) << 1;
                writer.Write((char)c);
            }
            reader.Close();
            writer.Close();
            fs.Close();
            return CreateFromTextFile(outsr, Encoding.Unicode);
        }

        /// <summary>
        /// 解析普通文本文档
        /// </summary>
        /// <param name="fs"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        private static RuleDataModel CreateFromTextFile(Stream fs, Encoding encoding)
        {
            var lines = new List<string>();
            fs.Position = 0L;
            using (var streamReader = new StreamReader(fs, encoding))
            {
                while (streamReader.Peek() >= 0)
                {
                    var str = streamReader.ReadLine();
                    if (!string.IsNullOrEmpty(str))
                    {
                        lines.Add(str);
                    }
                }
            }

            var data = new RuleDataModel()
            {
                OriginalFilePath = "",
                FileHead = lines[0],
                FgLarge = LineDataModel.CreateFromLineString(lines[1]),
                TextData = lines.Skip(2).Select(LineDataModel.CreateFromLineString).ToList()
            };
            fs.Close();
            return data;
        }

        /// <summary>
        /// 解析被ZLIB压缩的文本内容
        /// </summary>
        /// <param name="fs"></param>
        /// <param name="br"></param>
        /// <returns></returns>
        private static RuleDataModel CreateFromZlibFile(FileStream fs, BinaryReader br)
        {
            var lines = new List<string>();
            var dataLength = br.ReadInt32();
            fs.Position = fs.Length - dataLength;
            int b1 = br.ReadByte();
            int b2 = br.ReadByte();
            if ((0x78 != b1 && 0x58 != b1) || 0 != (b1 << 8 | b2) % 31)
            {
                throw new InvalidDataException("Data not recoginzed as zlib-compressed stream");
            }

            using (
                var deStream = new DeflateStream(
                    new MemoryStream(br.ReadBytes(dataLength - 2)),
                    CompressionMode.Decompress,
                    true
                )
            )
            using (var sr = new StreamReader(deStream, Encoding.Unicode))
            {
                while (sr.Peek() >= 0)
                {
                    var str = sr.ReadLine();
                    if (!string.IsNullOrEmpty(str))
                        lines.Add(str);
                }
            }

            var data = new RuleDataModel()
            {
                OriginalFilePath = fs.Name,
                FileHead = lines[0],
                FgLarge = LineDataModel.CreateFromLineString(lines[1]),
                TextData = lines.Skip(2).Select(LineDataModel.CreateFromLineString).ToList()
            };
            br.Close();
            fs.Close();
            return data;
        }

        /// <summary>
        /// 解析JSON文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private static RuleDataModel CreateFromJsonFile(string file)
        {
            var array = JsonConvert.DeserializeObject<List<LineDataModel>>(
                File.ReadAllText(file).Replace("\t", "")
            );
            var data = new RuleDataModel
            {
                OriginalFilePath = file,
                FileHead = string.Empty,
                FgLarge = array[0],
                TextData = array.Skip(1).ToList()
            };
            return data;
        }
    }
}
