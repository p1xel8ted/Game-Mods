// Decompiled with JetBrains decompiler
// Type: SpawnParticlesOnStart
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private SpriteRenderer spriteRenderer;

  private void Start()
  {
  }

  private void doParticles()
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
