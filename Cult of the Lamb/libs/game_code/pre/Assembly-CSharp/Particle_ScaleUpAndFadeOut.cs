// Decompiled with JetBrains decompiler
// Type: Particle_ScaleUpAndFadeOut
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Particle_ScaleUpAndFadeOut : BaseMonoBehaviour
{
  private float scaleSpeed;
  private float scale;
  private float timer;
  public float InitScale;

  private void Start() => this.transform.localScale = new Vector3(1f, this.InitScale, 1f);

  private void Update()
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
