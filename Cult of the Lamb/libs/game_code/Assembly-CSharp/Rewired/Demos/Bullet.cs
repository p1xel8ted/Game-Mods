// Decompiled with JetBrains decompiler
// Type: Rewired.Demos.Bullet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Rewired.Demos;

[AddComponentMenu("")]
public class Bullet : MonoBehaviour
{
  public float lifeTime = 3f;
  public bool die;
  public float deathTime;

  public void Start()
  {
    if ((double) this.lifeTime <= 0.0)
      return;
    this.deathTime = Time.time + this.lifeTime;
    this.die = true;
  }

  public void Update()
  {
    if (!this.die || (double) Time.time < (double) this.deathTime)
      return;
    Object.Destroy((Object) this.gameObject);
  }
}
