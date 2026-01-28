// Decompiled with JetBrains decompiler
// Type: StaticColors
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class StaticColors : BaseMonoBehaviour
{
  public static Color GoatPurple = new Color(0.415f, 0.176f, 1f, 1f);
  public static Color RedColor = new Color(0.9921569f, 0.1137255f, 0.01176471f, 1f);
  public static Color DarkRedColor = new Color(0.47f, 0.11f, 0.18f, 1f);
  public static Color GreenColor = new Color(0.003921569f, 0.8352941f, 0.6352941f, 1f);
  public static Color DarkGreenColor = new Color(0.019f, 0.313f, 0.294f, 1f);
  public static Color OrangeColor = new Color(1f, 0.6156863f, 0.003921569f, 1f);
  public static Color OffWhiteColor = new Color(0.9607843f, 0.9294118f, 0.8352941f, 1f);
  public static Color LightGreyColor = new Color(0.8f, 0.8f, 0.8f, 1f);
  public static Color GreyColor = new Color(0.43f, 0.427f, 0.411f);
  public static Color DarkGreyColor = new Color(0.27f, 0.25f, 0.26f, 1f);
  public static Color TwitchPurple = new Color(0.4588235f, 0.294117f, 0.9058823f, 1f);
  public static Color DLC1Blue = new Color(0.021569f, 0.537255f, 0.9117647f, 1f);
  public static string GreyColorHex = "<color=#6E6666>";
  public static string OffWhiteHex = "<color=#F5EDD5>";
  public static string YellowColorHex = "#FFD201";
  public static string BlueColourHex = "#039ca1";
  public static string DarkBlueColourHex = "#2561af";
  public static string PayAttentionYellowHex = "#FFD201";
  public static string GreenColorHex = "<color=#03BF84>";
  public static Color BlueColor = StaticColors.BlueColourHex.ColourFromHex();
  public const float LOW_THRESHOLD = 0.25f;

  public static Color DarkBlueColor => StaticColors.DarkBlueColourHex.ColourFromHex();

  public Color DisplayGoatPurple => StaticColors.GoatPurple;

  public Color DisplayRedColor => StaticColors.RedColor;

  public Color DisplayDarkRedColor => StaticColors.DarkRedColor;

  public Color DisplayGreenColor => StaticColors.GreenColor;

  public Color DisplayDarkGreenColor => StaticColors.DarkGreenColor;

  public Color DisplayOrangeColor => StaticColors.OrangeColor;

  public Color DisplayOffWhiteColor => StaticColors.OffWhiteColor;

  public Color DisplayLightGreyColor => StaticColors.LightGreyColor;

  public Color DisplayGreyColor => StaticColors.GreyColor;

  public Color DisplayDarkGreyColor => StaticColors.DarkGreyColor;

  public Color DisplayTwitchPurple => StaticColors.TwitchPurple;

  public Color DisplayBlueColor => StaticColors.BlueColor;

  public Color DisplayDarkBlueColor => StaticColors.DarkBlueColor;

  public Color DisplayDLC1Blue => StaticColors.DLC1Blue;

  public string DisplayGreyColorHex => StaticColors.GreyColorHex;

  public string DisplayOffWhiteHex => StaticColors.OffWhiteHex;

  public string DisplayYellowColorHex => StaticColors.YellowColorHex;

  public string DisplayBlueColourHex => StaticColors.BlueColourHex;

  public string DisplayDarkBlueColourHex => StaticColors.DarkBlueColourHex;

  public static Color ColorForThreshold(float value)
  {
    if ((double) value >= 0.0 && (double) value < 0.25)
      return StaticColors.RedColor;
    return (double) value >= 0.25 && (double) value < 0.45 ? StaticColors.OrangeColor : StaticColors.GreenColor;
  }
}
