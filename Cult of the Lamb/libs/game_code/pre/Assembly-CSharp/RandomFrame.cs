// Decompiled with JetBrains decompiler
// Type: RandomFrame
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
