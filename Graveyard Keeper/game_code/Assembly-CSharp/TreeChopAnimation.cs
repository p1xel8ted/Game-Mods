// Decompiled with JetBrains decompiler
// Type: TreeChopAnimation
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DG.Tweening;
using UnityEngine;

#nullable disable
public class TreeChopAnimation : InteractionAnimation
{
  public float duration = 0.5f;
  public float z_rotation = 4f;

  public override void DoAction()
  {
    this.transform.DOShakeRotation(this.duration, new Vector3(0.0f, 0.0f, this.z_rotation));
  }
}
