using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Li.Krkr.krkrfgformatWPF.Models;

public record RuleDataModel
{
    public static readonly string TextHead = "#layer_type	name	left	top	width	height	type	opacity	visible	layer_id	group_layer_id	base	images	";

    public string OriginalFilePath { set; get; } = "";
    public string FileHead { get; set; } = "";
    public LineDataModel FgLarge { get; set; } = new();
    public List<LineDataModel> TextData { get; set; } = [];

    #region IRuleData
    public LineDataModel GetFgLarge()
    {
        return this.FgLarge;
    }

    public List<LineDataModel> GetTextData()
    {
        return this.TextData;
    }

    public LineDataModel GetLineDataById(int id)
    {
        return this.TextData.First(t => t.LayerId == id.ToString());
    }
    public LineDataModel GetLineDataBySize(int w,int h)
    {
        return this.TextData.First(t => Convert.ToInt32(t.Width) == w && Convert.ToInt32(t.Height) == h);
    }

    public List<LineDataModel> GetLineDataByGroupLayerId(int id)
    {
        return this.TextData.Where(t => t.GroupLayerId == id.ToString()).ToList();
    }

    public static int GetLineDataVisible(LineDataModel lineData)
    {
        return Convert.ToInt32(lineData.Visible);
    }
    #endregion
}