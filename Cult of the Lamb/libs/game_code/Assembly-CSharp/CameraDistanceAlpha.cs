// Decompiled with JetBrains decompiler
// Type: CameraDistanceAlpha
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CameraDistanceAlpha : BaseMonoBehaviour
{
  public List<SpriteRenderer> Renderers = new List<SpriteRenderer>();
  public float Multiplier = 1f;
  public Color color;

  public void Update()
  {
    if ((Object) Camera.main == (Object) null)
      return;
    foreach (SpriteRenderer renderer in this.Renderers)
    {
      this.color = renderer.color;
      this.color.a = Mathf.Lerp(0.0f, 1f, Mathf.Abs(Camera.main.transform.position.x - renderer.transform.position.x) * this.Multiplier);
      renderer.color = this.color;
    }
  }
}
