using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Li.Text
{
    public class TextFile
    {
        public readonly static string textHead = "#layer_type	name	left	top	width	height	type	opacity	visible	layer_id	group_layer_id	base	images	";
        public string FileHander
        {
            get;
            set;
        }

        public PictureInfo Fglarge
        {
            get;
            set;
        }

        public List<PictureInfo> TextData
        {
            get;
            set;
        }

        public TextFile()
        {
            FileHander = string.Empty;
            Fglarge = new PictureInfo();
            TextData = new List<PictureInfo>();
        }

        public TextFile(string ruleFile)
        {
            if (Path.GetExtension(ruleFile).ToLower() == ".json")
            {
                JsonFile(ruleFile);
                return;
            }
            List<string> list = new List<string>();
            FileStream fileStream = new FileStream(ruleFile, FileMode.Open, FileAccess.Read, FileShare.None);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            byte[] array = binaryReader.ReadBytes(5);
            if (System.Linq.Enumerable.SequenceEqual(array, new byte[] { 0xFE, 0XFE, 0X02, 0XFF, 0XFE }))
            {
                int num = binaryReader.ReadInt32();
                fileStream.Position = fileStream.Length - num+2;
                using(var deStream = new DeflateStream(new MemoryStream(binaryReader.ReadBytes(num)),CompressionMode.Decompress,true))
                {
                    using(var sr = new StreamReader(deStream,Encoding.Unicode))
                    {
                        while (sr.Peek()>=0)
                        {
                            var line = sr.ReadLine();
                            if(!string.IsNullOrEmpty(line))
                            {
                                list.Add(line);
                            }
                        }
                    }
                }
            }
            else
            {
                Encoding encoding = (array[1] == 0) ? Encoding.Unicode : Encoding.UTF8;
                fileStream.Position = 0L;
                using (StreamReader streamReader = new StreamReader(fileStream, encoding))
                {
                    while (streamReader.Peek() >= 0)
                    {
                        string text2 = streamReader.ReadLine();
                        if (!string.IsNullOrEmpty(text2))
                        {
                            list.Add(text2);
                        }
                    }
                }
            }
            binaryReader.Close();
            fileStream.Close();
            FileHander = list[0];
            Fglarge = new PictureInfo(list[1]);
            TextData = new List<PictureInfo>();
            for (int i = 2; i < list.Count; i++)
            {
                TextData.Add(new PictureInfo(list[i]));
            }
        }

        private void JsonFile(string ruleFile)
        {
            PictureInfo[] array = JsonConvert.DeserializeObject<PictureInfo[]>(File.ReadAllText(ruleFile).Replace("\t", ""));
            FileHander = "";
            Fglarge = array[0];
            TextData = new List<PictureInfo>();
            for (int i = 1; i < array.Length; i++)
            {
                TextData.Add(array[i]);
            }
        }
    }
}
