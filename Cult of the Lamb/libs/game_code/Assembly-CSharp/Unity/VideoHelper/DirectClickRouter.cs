// Decompiled with JetBrains decompiler
// Type: Unity.VideoHelper.DirectClickRouter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;

#nullable disable
namespace Unity.VideoHelper;

public class DirectClickRouter : ClickRouter
{
  public float lastClickTime;
  public const float clickInterval = 0.3f;

  public override void OnPointerClick(PointerEventData eventData)
  {
  }

  public override void OnPointerDown(PointerEventData eventData)
  {
    if ((double) this.lastClickTime + 0.30000001192092896 > (double) Time.time)
      this.OnDoubleClick.Invoke();
    else
      this.OnClick.Invoke();
    this.lastClickTime = Time.time;
  }
}
