// Decompiled with JetBrains decompiler
// Type: src.Extensions.RectTransformExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace src.Extensions;

public static class RectTransformExtensions
{
  public static void SetRect(
    this RectTransform trs,
    float left,
    float top,
    float right,
    float bottom)
  {
    trs.offsetMin = new Vector2(left, bottom);
    trs.offsetMax = new Vector2(-right, -top);
  }
}
