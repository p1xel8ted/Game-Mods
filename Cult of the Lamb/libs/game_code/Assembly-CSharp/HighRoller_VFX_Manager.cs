// Decompiled with JetBrains decompiler
// Type: HighRoller_VFX_Manager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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
