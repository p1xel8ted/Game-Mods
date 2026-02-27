// Decompiled with JetBrains decompiler
// Type: ParticleSystemAnimEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ParticleSystemAnimEvent : BaseMonoBehaviour
{
  public ParticleSystem ParticleSystem;
  public SpriteRenderer sprite;

  public void Play()
  {
    if (!((Object) this.ParticleSystem != (Object) null))
      return;
    this.ParticleSystem.Play();
  }
}
