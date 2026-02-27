// Decompiled with JetBrains decompiler
// Type: Particle_ScaleUpAndFadeOut
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Particle_ScaleUpAndFadeOut : BaseMonoBehaviour
{
  public float scaleSpeed;
  public float scale;
  public float timer;
  public float InitScale;

  public void Start() => this.transform.localScale = new Vector3(1f, this.InitScale, 1f);

  public void Update()
  {
    if ((double) (this.timer += Time.deltaTime) > 0.20000000298023224)
    {
      this.scale -= 0.1f;
      this.transform.localScale = new Vector3(this.scale, this.scale, 1f);
      if ((double) this.scale <= 0.0)
        Object.Destroy((Object) this.gameObject);
    }
    else
    {
      this.scaleSpeed += (float) ((1.0 - (double) this.scale) * 0.30000001192092896);
      this.scale += (this.scaleSpeed *= 0.7f);
      this.transform.localScale = new Vector3(1f, this.scale, 1f);
    }
    this.transform.eulerAngles = new Vector3(-60f, 0.0f, 0.0f);
  }
}
