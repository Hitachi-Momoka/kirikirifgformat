using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Li.Krkr.InfoReader
{
    public interface IData
    {
        string DataType { get; }
        string Name { get; }
    }
    public interface IGroupSingalData : IData
    {
        List<string> NameParts { get; }
        LayerType LayerType { get; }
    }
    public class FgaliasLineData : IData
    {
        public string DataType { private set; get; } = "fgalias";
        public string Name { private set; get; }
        public List<string> Parts { private set; get; }

        public FgaliasLineData()
        {
            this.Init();
        }
        public FgaliasLineData(string line)
        {
            this.Init();
            var array = line.Split('\t');
            this.Name = array[1];
            this.Parts = new List<string>();
            for (int i = 2; i < array.Length; i++)
            {
                this.Parts.Add(array[i]);
            }
        }
        private void Init()
        {
            this.Name = "";
            this.Parts = new List<string>();
        }
    }

    public class DressLineData : IGroupSingalData
    {
        public string DataType { private set; get; } = "dress";
        public List<string> NameParts { private set; get; }
        public LayerType LayerType { private set; get; }
        public string Name { get => throw new NotSupportedException("This Class don't need this."); }
        public string LayerPath { private set; get; }

        public DressLineData()
        {
            this.Init();
        }
        public DressLineData(string line)
        {
            this.Init();
            var array = line.Split('\t');
            if(5 == array.Length && !array[3].Equals("dummy"))
            {
                this.NameParts = new List<string>() { array[1], array[3] };
                this.LayerType = array[2].Equals("base") ? LayerType.Base : LayerType.Diff;
                this.LayerPath = array[4];
            }
        }
        private void Init()
        {
            this.NameParts = new List<string>();
            this.LayerType = LayerType.Base;
            this.LayerPath = "";
        }
    }
    public class FacegroupData : IData
    {
        public string DataType { private set; get; } = "facegroup";
        public string Name { get => throw new NotSupportedException("This Class don't need this."); }
        public List<string> GroupName { private set; get; }
        public FacegroupData()
        {
            this.Init();
        }
        public FacegroupData(string line)
        {
            this.Init();
            var array = line.Split('\t');
            this.GroupName.Add(array[1]);
        }
        private void Init()
        {
            this.GroupName = new List<string>(); ;
        }
    }
    public class FgnameLineData : IData
    {
        public string DataType { get; } = "fgname";

        public string Name { private set; get; }
        public string LayerPath { private set; get; }
        public FgnameLineData()
        {
            this.Init();
        }
        public FgnameLineData(string line)
        {
            this.Init();
            var array = line.Split('\t');
            this.Name = array[1];
            this.LayerPath = array[2];
        }
        private void Init()
        {
            this.Name = "";
            this.LayerPath = "";
        }
    }
    public class FaceLineData : IGroupSingalData
    {
        public string DataType { get; } = "face";
        public List<string> NameParts { get => throw new NotSupportedException("This Class don't need this."); }

        public LayerType LayerType { private set; get; }
        public string Name { private set; get; }
        public string LayerPath { private set; get; }
        public FaceLineData()
        {
            this.Init();
        }
        public FaceLineData(string line)
        {
            this.Init();
            var array = line.Split('\t');
            this.Name = array[1];
            this.LayerType = array[2].Equals("base") ? LayerType.Base : LayerType.Diff;
            this.LayerPath = array[3];
        }
        private void Init()
        {
            this.LayerType = LayerType.Base;
            this.LayerPath = "";
            this.Name = "";
        }
    }

    public static class DataRegex
    {
        //dress	私服	diff	正面２	私服１/正面１
        public static Regex dressRegex = new Regex("^dress\t.+\t.+\t.+\t.*");
        //fgname	眉_笑１	表情/眉/笑１
        public static Regex fgnameRegex = new Regex("^fgname\t.+\t.+");
        //face	20h	base	困った　悲しい　困った
        public static Regex faceRegex = new Regex("^face\t.+\t.+\t.+");
        //fgalias	笑１	眉_笑１	目_笑１	口_笑１	汗_无	頬_頬１
        //public static Regex fgaliasRegex = new Regex("^fgalias(?<fgDisplayName>\t.*?\t)+(?<fgDisplayParts>.*)");
        public static Regex fgaliasRegex = new Regex("^fgalias(\t.+?\t)+.+");
        //facegroup	眉_
        public static Regex facegroupRegex = new Regex("^facegroup\t.+");
    }

    public static class TextEncoding
    {
        public static Encoding Shift_JIS = Encoding.GetEncoding(932);
    }
    public enum LayerType
    {
        Base = 0x00,
        Diff = 0x01
    }

    public static class ReaderHelper
    {
        public static bool AsciiEquals(this string dst,string str)
        {
            return dst.StartsWith(str);
        }
    }
}
