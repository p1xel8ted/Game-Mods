// Decompiled with JetBrains decompiler
// Type: BloodSplatterPrefab
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BloodSplatterPrefab : BaseMonoBehaviour
{
  public static BloodSplatterPrefab Instance;
  public ParticleSystem Ps;
  public ParticleSystem.Particle[] particles;
  public bool usedRoom;

  public void InitializeIfNeeded()
  {
    if ((Object) this.Ps == (Object) null)
      this.Ps = this.GetComponent<ParticleSystem>();
    ParticleSystem.MainModule main;
    if (this.particles != null)
    {
      int length = this.particles.Length;
      main = this.Ps.main;
      int maxParticles = main.maxParticles;
      if (length >= maxParticles)
        return;
    }
    main = this.Ps.main;
    this.particles = new ParticleSystem.Particle[main.maxParticles];
  }

  public void OnEnable()
  {
    this.InitializeIfNeeded();
    BloodSplatterPrefab.Instance = this;
    if (this.particles == null)
      return;
    this.Ps.SetParticles(this.particles);
  }

  public void NewParticle()
  {
    this.usedRoom = true;
    this.Ps.GetParticles(this.particles);
  }

  public void OnDisable()
  {
    if (this.usedRoom && (Object) this.Ps != (Object) null)
      this.Ps.main.playOnAwake = false;
    if (this.particles != null)
      this.Ps.GetParticles(this.particles);
    if (!((Object) BloodSplatterPrefab.Instance == (Object) this))
      return;
    BloodSplatterPrefab.Instance = (BloodSplatterPrefab) null;
  }

  public void OnDestroy()
  {
    this.Ps.Clear();
    this.particles = (ParticleSystem.Particle[]) null;
  }
}
