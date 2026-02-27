// Decompiled with JetBrains decompiler
// Type: RandomFrame
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RandomFrame : BaseMonoBehaviour
{
  public List<Sprite> frames = new List<Sprite>();
  public int frame = -1;
  public Material mat;

  public void Start()
  {
    SpriteRenderer component = this.GetComponent<SpriteRenderer>();
    if ((Object) this.mat != (Object) null)
      component.material = this.mat;
    if (this.frame == -1)
      component.sprite = this.frames[Random.Range(0, this.frames.Count)];
    else
      component.sprite = this.frames[this.frame];
  }
}
