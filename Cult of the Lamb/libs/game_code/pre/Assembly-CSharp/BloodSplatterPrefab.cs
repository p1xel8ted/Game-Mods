// Decompiled with JetBrains decompiler
// Type: BloodSplatterPrefab
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BloodSplatterPrefab : BaseMonoBehaviour
{
  public static BloodSplatterPrefab Instance;
  public ParticleSystem Ps;
  public ParticleSystem.Particle[] particles;
  private bool usedRoom;

  private void InitializeIfNeeded()
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

  private void OnEnable()
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

  private void OnDisable()
  {
    if (this.usedRoom && (Object) this.Ps != (Object) null)
      this.Ps.main.playOnAwake = false;
    if (this.particles != null)
      this.Ps.GetParticles(this.particles);
    if (!((Object) BloodSplatterPrefab.Instance == (Object) this))
      return;
    BloodSplatterPrefab.Instance = (BloodSplatterPrefab) null;
  }

  private void OnDestroy()
  {
    this.Ps.Clear();
    this.particles = (ParticleSystem.Particle[]) null;
  }
}
