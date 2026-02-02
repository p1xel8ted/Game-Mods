// Decompiled with JetBrains decompiler
// Type: HighRoller_VFX_Manager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
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
