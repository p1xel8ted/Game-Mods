// Decompiled with JetBrains decompiler
// Type: ParticlesMinIntensityAndDelete
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ParticlesMinIntensityAndDelete : MonoBehaviour
{
  [SerializeField]
  public List<ParticleSystem> particles;
  public bool do_delete;
  public float time_to_del;

  public void DoDelete(float time_to_del)
  {
    this.do_delete = true;
    this.time_to_del = time_to_del;
    foreach (ParticleSystem particle in this.particles)
      particle.main.loop = false;
  }

  public void Update()
  {
    if (!this.do_delete)
      return;
    this.time_to_del -= Time.deltaTime;
    if ((double) this.time_to_del >= 0.0)
      return;
    WorldGameObject componentInParent = this.GetComponentInParent<WorldGameObject>();
    if (!((Object) componentInParent != (Object) null))
      return;
    componentInParent.DestroyMe();
  }
}
