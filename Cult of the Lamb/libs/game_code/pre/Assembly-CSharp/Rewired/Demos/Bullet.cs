// Decompiled with JetBrains decompiler
// Type: Rewired.Demos.Bullet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Rewired.Demos;

[AddComponentMenu("")]
public class Bullet : MonoBehaviour
{
  public float lifeTime = 3f;
  private bool die;
  private float deathTime;

  private void Start()
  {
    if ((double) this.lifeTime <= 0.0)
      return;
    this.deathTime = Time.time + this.lifeTime;
    this.die = true;
  }

  private void Update()
  {
    if (!this.die || (double) Time.time < (double) this.deathTime)
      return;
    Object.Destroy((Object) this.gameObject);
  }
}
