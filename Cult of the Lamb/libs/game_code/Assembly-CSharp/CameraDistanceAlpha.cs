// Decompiled with JetBrains decompiler
// Type: CameraDistanceAlpha
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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
