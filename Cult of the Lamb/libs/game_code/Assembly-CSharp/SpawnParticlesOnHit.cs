// Decompiled with JetBrains decompiler
// Type: SpawnParticlesOnHit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (Health))]
public class SpawnParticlesOnHit : BaseMonoBehaviour
{
  [Range(0.0f, 1f)]
  public float BreakCameraShake;
  public Health health;
  public int maxParticles = 10;
  public List<Sprite> ParticleChunks;
  public GameObject particleSpawn;
  public float zSpawn;
  public SpriteRenderer spriteRenderer;

  public void Start()
  {
    this.health = this.GetComponent<Health>();
    this.health.OnHit += new Health.HitAction(this.OnHit);
    this.spriteRenderer = this.GetComponent<SpriteRenderer>();
  }

  public void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    if ((Object) this.spriteRenderer != (Object) null)
    {
      if (this.spriteRenderer.isVisible)
        CameraManager.shakeCamera(this.BreakCameraShake, Utils.GetAngle(Attacker.transform.position, this.transform.position));
    }
    else
      CameraManager.shakeCamera(this.BreakCameraShake, Utils.GetAngle(Attacker.transform.position, this.transform.position));
    int num = -1;
    if (this.ParticleChunks.Count > 0)
    {
      while (++num < this.maxParticles)
        Particle_Chunk.AddNew(this.transform.position, Utils.GetAngle(Attacker.transform.position, this.transform.position) + (float) Random.Range(-20, 20), this.ParticleChunks);
    }
    if (!((Object) this.particleSpawn != (Object) null))
      return;
    Object.Instantiate<GameObject>(this.particleSpawn, new Vector3(this.transform.position.x, this.transform.position.y, this.zSpawn), Quaternion.identity);
  }
}
