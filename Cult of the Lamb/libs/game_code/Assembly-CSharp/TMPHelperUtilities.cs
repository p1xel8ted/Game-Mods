// Decompiled with JetBrains decompiler
// Type: TMPHelperUtilities
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
public static class TMPHelperUtilities
{
  public static void CopyTMPToTMP(TMP_Text source, TextMeshPro target)
  {
    if ((Object) source == (Object) null || (Object) target == (Object) null)
    {
      Debug.LogWarning((object) "Source or Target is null.");
    }
    else
    {
      target.text = source.text;
      target.font = source.font;
      target.fontSize = source.fontSize;
      target.fontStyle = source.fontStyle;
      target.enableAutoSizing = source.enableAutoSizing;
      target.fontSizeMin = source.fontSizeMin;
      target.fontSizeMax = source.fontSizeMax;
      target.alignment = source.alignment;
      target.characterSpacing = source.characterSpacing;
      target.wordSpacing = source.wordSpacing;
      target.lineSpacing = source.lineSpacing;
      target.paragraphSpacing = source.paragraphSpacing;
      target.lineSpacingAdjustment = source.lineSpacingAdjustment;
      target.color = source.color;
      target.enableVertexGradient = source.enableVertexGradient;
      target.colorGradient = source.colorGradient;
      target.overrideColorTags = source.overrideColorTags;
      target.richText = source.richText;
      target.enableWordWrapping = source.enableWordWrapping;
      target.wordWrappingRatios = source.wordWrappingRatios;
      target.overflowMode = source.overflowMode;
      target.firstVisibleCharacter = source.firstVisibleCharacter;
      target.maxVisibleCharacters = source.maxVisibleCharacters;
      target.margin = source.margin;
      target.extraPadding = source.extraPadding;
      target.characterWidthAdjustment = source.characterWidthAdjustment;
      target.fontMaterial = source.fontMaterial;
    }
  }
}
