// Decompiled with JetBrains decompiler
// Type: NewsItemData
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using LitJson;
using System;
using System.Globalization;
using System.Text;

#nullable disable
public class NewsItemData
{
  public const string PREFIX = "";
  public const string SUFFIX = "\n";
  public string version;
  public bool is_upcoming;
  public string items;
  public DateTime date;
  public int progress = -1;
  public bool visible = true;

  public NewsItemData(string version, JsonData data)
  {
    if (version == "info")
    {
      NewsGUI.update_url = (string) data["url"];
      this.visible = false;
    }
    else
    {
      this.is_upcoming = version == "upcoming";
      if (data.Keys.Contains(nameof (visible)))
        this.visible = (int) data[nameof (visible)] == 1;
      if (this.is_upcoming)
      {
        this.version = (string) data[nameof (version)];
        this.progress = (int) data[nameof (progress)];
      }
      else
      {
        this.version = version;
        if (this.visible)
        {
          float num = float.Parse(version, (IFormatProvider) CultureInfo.InvariantCulture);
          if ((double) num > (double) NewsGUI.last_ver)
            NewsGUI.last_ver = num;
        }
      }
      this.date = DateTime.Parse((string) data[nameof (date)], (IFormatProvider) new CultureInfo("ru-RU"));
      JsonData jsonData = data[nameof (items)];
      StringBuilder stringBuilder = new StringBuilder();
      int count = jsonData.Count;
      for (int index = 0; index < count; ++index)
      {
        string str1 = (string) jsonData[index];
        stringBuilder.Append("");
        string str2 = str1.Replace("(!)", "[ff0000][!][-]").Replace("(+)", "[ffff00][+][-]");
        stringBuilder.Append(str2);
        stringBuilder.Append("\n");
      }
      this.items = stringBuilder.ToString();
    }
  }
}
