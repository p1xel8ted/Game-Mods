// Decompiled with JetBrains decompiler
// Type: MA_DestroyFinishedParticle
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
public class MA_DestroyFinishedParticle : MonoBehaviour
{
  public ParticleSystem particles;

  public void Awake()
  {
    this.useGUILayout = false;
    this.particles = this.GetComponent<ParticleSystem>();
  }

  public void Update()
  {
    if (this.particles.IsAlive())
      return;
    Object.Destroy((Object) this.gameObject);
  }
}
