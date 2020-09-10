using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Li.Text
{
    public class PictureInfo
    {
        [JsonProperty(PropertyName = "layer_type")]
        public string LayerType
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "name")]
        public string Name
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "left")]
        public string Left
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "top")]
        public string Top
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "width")]
        public string Width
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "height")]
        public string Height
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "type")]
        public string Type
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "opacity")]
        public string Opacity
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "visible")]
        public string Visible
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "layer_id")]
        public string LayerId
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "group_layer_id")]
        public string GroupLayerId
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "base")]
        public string Base
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "images")]
        public string Images
        {
            get;
            set;
        }

        public PictureInfo()
        {
            LayerType = default;
            Name = default;
            Left = default;
            Top = default;
            Width = default;
            Height = default;
            Type = default;
            Opacity = default;
            Visible = default;
            LayerId = default;
            GroupLayerId = default;
            Base = default;
            Images = default;
        }


        public PictureInfo(string readline)
        {
            List<string> list = new List<string>();
            if (readline != string.Empty)
            {
                string text = string.Empty;
                for (int i = 0; i < readline.Length; i++)
                {
                    if (readline[i] != '\t')
                    {
                        text += readline[i].ToString();
                    }
                    else
                    {
                        if (text == string.Empty)
                        {
                            list.Add("0");
                            text = string.Empty;
                            continue;
                        }
                        list.Add(text);
                        text = string.Empty;
                    }
                    if (list.Count == 13)
                    {
                        break;
                    }
                }
            }
            if (list.Count != 13)
            {
                throw new Exception("txt文件内容格式错误");
            }
            LayerType = list[0];
            Name = list[1];
            Left = list[2];
            Top = list[3];
            Width = list[4];
            Height = list[5];
            Type = list[6];
            Opacity = list[7];
            Visible = list[8];
            LayerId = list[9];
            GroupLayerId = list[10];
            Base = list[11];
            Images = list[12];
        }

        public override string ToString()
        {
            return this.ToString("\t");
        }
        public string ToString(string str)
        {
            string tmp = "";
            tmp += (this.LayerType + str);
            tmp += (this.Name + str);
            tmp += (this.Left + str);
            tmp += (this.Top + str);
            tmp += (this.Width + str);
            tmp += (this.Height + str);
            tmp += (this.Type + str);
            tmp += (this.Opacity + str);
            tmp += (this.Visible + str);
            tmp += (this.LayerId + str);
            tmp += (this.GroupLayerId + str);
            tmp += (this.Base + str);
            tmp += (this.Images + str);
            return tmp;
        }
    }
}
