// Decompiled with JetBrains decompiler
// Type: StaticColors
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class StaticColors : BaseMonoBehaviour
{
  public static Color RedColor = new Color(0.9921569f, 0.1137255f, 0.01176471f, 1f);
  public static Color DarkRedColor = new Color(0.47f, 0.11f, 0.18f, 1f);
  public static Color GreenColor = new Color(0.003921569f, 0.8352941f, 0.6352941f, 1f);
  public static Color DarkGreenColor = new Color(0.019f, 0.313f, 0.294f, 1f);
  public static Color OrangeColor = new Color(1f, 0.6156863f, 0.003921569f, 1f);
  public static Color OffWhiteColor = new Color(0.9607843f, 0.9294118f, 0.8352941f, 1f);
  public static Color LightGreyColor = new Color(0.8f, 0.8f, 0.8f, 1f);
  public static Color GreyColor = new Color(0.43f, 0.427f, 0.411f);
  public static string GreyColorHex = "<color=#6E6666>";
  public static string OffWhiteHex = "<color=#F5EDD5>";
  public static string YellowColorHex = "#FFD201";
  public static Color TwitchPurple = new Color(0.4588235f, 0.294117f, 0.9058823f, 1f);
  public static Color DarkGreyColor = new Color(0.27f, 0.25f, 0.26f, 1f);
  public const float LOW_THRESHOLD = 0.25f;

  public static Color ColorForThreshold(float value)
  {
    if ((double) value >= 0.0 && (double) value < 0.25)
      return StaticColors.RedColor;
    return (double) value >= 0.25 && (double) value < 0.45 ? StaticColors.OrangeColor : StaticColors.GreenColor;
  }
}
