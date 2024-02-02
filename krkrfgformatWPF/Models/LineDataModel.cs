using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Li.Krkr.krkrfgformatWPF.Models
{
    public class LineDataModel : IKRKRImageData
    {
        [JsonProperty(PropertyName = "layer_type")]
        public string LayerType
        {
            get;
            private set;
        }

        [JsonProperty(PropertyName = "name")]
        public string Name
        {
            get;
            private set;
        }

        [JsonProperty(PropertyName = "left")]
        public string Left
        {
            get;
            private set;
        }

        [JsonProperty(PropertyName = "top")]
        public string Top
        {
            get;
            private set;
        }

        [JsonProperty(PropertyName = "width")]
        public string Width
        {
            get;
            private set;
        }

        [JsonProperty(PropertyName = "height")]
        public string Height
        {
            get;
            private set;
        }

        [JsonProperty(PropertyName = "type")]
        public string Type
        {
            get;
            private set;
        }

        [JsonProperty(PropertyName = "opacity")]
        public string Opacity
        {
            get;
            private set;
        }

        [JsonProperty(PropertyName = "visible")]
        public string Visible
        {
            get;
            private set;
        }

        [JsonProperty(PropertyName = "layer_id")]
        public string LayerId
        {
            get;
            private set;
        }

        [JsonProperty(PropertyName = "group_layer_id")]
        public string GroupLayerId
        {
            get;
            private set;
        }

        [JsonProperty(PropertyName = "base")]
        public string Base
        {
            get;
            private set;
        }

        [JsonProperty(PropertyName = "images")]
        public string Images
        {
            get;
            private set;
        }
        public LineDataModel()
        {
            LayerType = "";
            Name = "";
            Left = "";
            Top = "";
            Width = "";
            Height = "";
            Type = "";
            Opacity = "";
            Visible = "";
            LayerId = "";
            GroupLayerId = "";
            Base = "";
            Images = "";
        }

        public static LineDataModel CreatFromLineString(string line)
        {

            if (string.IsNullOrEmpty(line))
            {
                return null;
            }
            var array = line.Split('\t');
            //if (array.Length != 13)
            //{
            //    throw new Exception("txt文件内容格式错误");
            //}
            return new LineDataModel()
            {
                LayerType = array[0],
                Name = array[1],
                Left = array[2],
                Top = array[3],
                Width = array[4],
                Height = array[5],
                Type = array[6],
                Opacity = array[7],
                Visible = array[8],
                LayerId = array[9],
                GroupLayerId = array[10],
                Base = array[11],
                Images = array[12]
            };
        }
        public override string ToString()
        {
            StringBuilder tmp = new StringBuilder()
            .Append(this.LayerType + "\t")
            .Append(this.Name + "\t")
            .Append(this.Left + "\t")
            .Append(this.Top + "\t")
            .Append(this.Width + "\t")
            .Append(this.Height + "\t")
            .Append(this.Type + "\t")
            .Append(this.Opacity + "\t")
            .Append(this.Visible + "\t")
            .Append(this.LayerId + "\t")
            .Append(this.GroupLayerId + "\t")
            .Append(this.Base + "\t")
            .Append(this.Images + "\t");
            return tmp.ToString();
        }
        public Rect ToRect()
        {
            Rect rect = new Rect
            {
                X = Convert.ToInt32(this.Left),
                Y = Convert.ToInt32(this.Top),
                Width = Convert.ToInt32(this.Width),
                Height = Convert.ToInt32(this.Height)
            };
            return rect;
        }
    }
}
