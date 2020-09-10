using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Li.Text;

namespace JsonToTxt
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length<=0)
            {
                Console.WriteLine("缺少参数.");
                Console.Read();
                Environment.Exit(0);
            }
            string file = args[0];
            TextFile text = new TextFile(file);
            string newfile = Path.GetDirectoryName(file) + "\\" + Path.GetFileNameWithoutExtension(file) + ".csv";
            FileStream fs = new FileStream(newfile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            using(StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine(TextFile.textHead);
                sw.WriteLine(text.Fglarge.ToString());
                foreach(var info in text.TextData)
                {
                    sw.WriteLine(info.ToString(","));
                }
            }
            fs.Close();
            Console.WriteLine(file + " is Done.");
        }
    }
}
