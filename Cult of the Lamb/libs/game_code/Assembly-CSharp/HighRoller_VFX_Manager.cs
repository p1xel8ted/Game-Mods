// Decompiled with JetBrains decompiler
// Type: HighRoller_VFX_Manager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class HighRoller_VFX_Manager : MonoBehaviour
{
  [SerializeField]
  public ParticleSystem blurVFX;
  [SerializeField]
  public ParticleSystem iceBlastVFX;

  public void Play()
  {
    this.blurVFX.Play();
    this.iceBlastVFX.Play();
  }

  public void Stop()
  {
    this.blurVFX.Stop();
    this.iceBlastVFX.Stop();
  }
}
