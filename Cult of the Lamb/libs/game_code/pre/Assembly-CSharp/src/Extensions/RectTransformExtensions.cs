// Decompiled with JetBrains decompiler
// Type: src.Extensions.RectTransformExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
