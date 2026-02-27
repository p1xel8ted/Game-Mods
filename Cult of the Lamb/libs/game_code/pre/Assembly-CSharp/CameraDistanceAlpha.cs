// Decompiled with JetBrains decompiler
// Type: CameraDistanceAlpha
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CameraDistanceAlpha : BaseMonoBehaviour
{
  public List<SpriteRenderer> Renderers = new List<SpriteRenderer>();
  public float Multiplier = 1f;
  private Color color;

  private void Update()
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
