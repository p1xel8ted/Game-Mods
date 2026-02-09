// Decompiled with JetBrains decompiler
// Type: CodeStage.AdvancedFPSCounter.UIUtils
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace CodeStage.AdvancedFPSCounter;

public class UIUtils
{
  public static void ResetRectTransform(RectTransform rectTransform)
  {
    rectTransform.localRotation = Quaternion.identity;
    rectTransform.localScale = Vector3.one;
    rectTransform.pivot = new Vector2(0.5f, 0.5f);
    rectTransform.anchorMin = Vector2.zero;
    rectTransform.anchorMax = Vector2.one;
    rectTransform.anchoredPosition3D = Vector3.zero;
    rectTransform.offsetMin = Vector2.zero;
    rectTransform.offsetMax = Vector2.zero;
  }
}
