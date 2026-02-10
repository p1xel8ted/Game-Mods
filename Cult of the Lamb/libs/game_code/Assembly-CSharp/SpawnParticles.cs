// Decompiled with JetBrains decompiler
// Type: SpawnParticles
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SpawnParticles : MonoBehaviour
{
  [Range(0.0f, 1f)]
  public float BreakCameraShake;
  public int maxParticles = 10;
  public List<Sprite> ParticleChunks;
  public GameObject particleSpawn;
  public float zSpawn;
  public Material particleMaterial;
  public bool scaleParticlesOut = true;

  public void Spawn(float facing, float angle)
  {
    if ((Object) this.transform == (Object) null)
      return;
    CameraManager.shakeCamera(this.BreakCameraShake, facing);
    int num = -1;
    if (this.ParticleChunks.Count > 0)
    {
      while (++num < this.maxParticles)
      {
        if ((Object) this.transform == (Object) null)
          return;
        if ((Object) this.particleMaterial == (Object) null)
          Particle_Chunk.AddNew(this.transform.position, facing + Random.Range((float) (-(double) angle * 0.5), angle * 0.5f), this.ParticleChunks, scaleObjectOut: this.scaleParticlesOut);
        else
          Particle_Chunk.AddNewMat(this.transform.position, facing + Random.Range((float) (-(double) angle * 0.5), angle * 0.5f), this.ParticleChunks, mat: this.particleMaterial, scaleObjectOut: this.scaleParticlesOut);
      }
    }
    if (!((Object) this.particleSpawn != (Object) null))
      return;
    Object.Instantiate<GameObject>(this.particleSpawn, new Vector3(this.transform.position.x, this.transform.position.y, this.zSpawn), Quaternion.identity);
  }
}
