// Decompiled with JetBrains decompiler
// Type: SpawnParticlesOnDeath
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (Health))]
public class SpawnParticlesOnDeath : BaseMonoBehaviour
{
  [Range(0.0f, 1f)]
  public float BreakCameraShake;
  public Health health;
  public int maxParticles = 10;
  public List<Sprite> ParticleChunks;
  public GameObject particleSpawn;
  public float zSpawn;
  public Material particleMaterial;
  public bool scaleParticlesOut = true;

  public void OnEnable()
  {
    if ((Object) this.health == (Object) null)
      this.health = this.GetComponent<Health>();
    if (!((Object) this.health != (Object) null))
      return;
    this.health.OnDie += new Health.DieAction(this.OnDie);
  }

  public void OnDisable()
  {
    if (!((Object) this.health != (Object) null))
      return;
    this.health.OnDie -= new Health.DieAction(this.OnDie);
  }

  public void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    if ((Object) Attacker == (Object) null || (Object) this.transform == (Object) null)
      return;
    CameraManager.shakeCamera(this.BreakCameraShake, Utils.GetAngle(Attacker.transform.position, this.transform.position));
    int num = -1;
    if (this.ParticleChunks.Count > 0)
    {
      while (++num < this.maxParticles)
      {
        if ((Object) Attacker == (Object) null || (Object) this.transform == (Object) null)
          return;
        if ((Object) this.particleMaterial == (Object) null)
          Particle_Chunk.AddNew(this.transform.position, Utils.GetAngle(Attacker.transform.position, this.transform.position) + (float) Random.Range(-20, 20), this.ParticleChunks, scaleObjectOut: this.scaleParticlesOut);
        else
          Particle_Chunk.AddNewMat(this.transform.position, Utils.GetAngle(Attacker.transform.position, this.transform.position) + (float) Random.Range(-20, 20), this.ParticleChunks, mat: this.particleMaterial, scaleObjectOut: this.scaleParticlesOut);
      }
    }
    if (!((Object) this.particleSpawn != (Object) null))
      return;
    Object.Instantiate<GameObject>(this.particleSpawn, new Vector3(this.transform.position.x, this.transform.position.y, this.zSpawn), Quaternion.identity);
  }
}
