// Decompiled with JetBrains decompiler
// Type: SpawnParticlesOnStart
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SpawnParticlesOnStart : BaseMonoBehaviour
{
  [Range(0.0f, 1f)]
  public int maxParticles = 10;
  public List<Sprite> ParticleChunks;
  public GameObject particleSpawn;
  public float zSpawn;
  public SpriteRenderer spriteRenderer;

  public void Start()
  {
  }

  public void doParticles()
  {
    this.spriteRenderer = this.GetComponent<SpriteRenderer>();
    int num = -1;
    if (this.ParticleChunks.Count > 0)
    {
      while (++num < this.maxParticles)
        Particle_Chunk.AddNew(this.transform.position, Utils.GetAngle(this.transform.position, this.transform.position) + (float) Random.Range(-20, 20), this.ParticleChunks);
    }
    if (!((Object) this.particleSpawn != (Object) null))
      return;
    Object.Instantiate<GameObject>(this.particleSpawn, new Vector3(this.transform.position.x, this.transform.position.y, this.zSpawn), Quaternion.identity);
  }
}
