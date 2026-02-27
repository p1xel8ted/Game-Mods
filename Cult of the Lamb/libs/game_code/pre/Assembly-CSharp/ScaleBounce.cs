// Decompiled with JetBrains decompiler
// Type: ScaleBounce
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ScaleBounce : BaseMonoBehaviour
{
  public float TargetX = 1f;
  public float TargetY = 1f;
  public float DampingX = 0.3f;
  public float ElasticX = 0.7f;
  public float DampingY = 0.3f;
  public float ElasticY = 0.7f;
  private float scaleSpeedX;
  private float scaleSpeedY;
  private float scaleX = 1f;
  private float scaleY = 1f;
  public float StartScaleX = 1f;
  public float StartScaleY = 1f;

  private void OnEnable()
  {
    this.scaleX = this.StartScaleX;
    this.scaleY = this.StartScaleY;
    this.gameObject.transform.localScale = new Vector3(this.scaleX, this.scaleY);
  }

  public void SquishMe(float _scaleSpeedX, float _scaleSpeedY)
  {
    this.scaleSpeedX = _scaleSpeedX;
    this.scaleSpeedY = _scaleSpeedY;
  }

  private void Update()
  {
    if ((double) Time.timeScale == 0.0)
      return;
    this.scaleX = this.gameObject.transform.localScale.x;
    this.scaleY = this.gameObject.transform.localScale.y;
    this.scaleSpeedX += (this.TargetX - this.scaleX) * this.DampingX / Time.deltaTime;
    this.scaleX += (this.scaleSpeedX *= this.ElasticX) * Time.deltaTime;
    this.scaleSpeedY += (this.TargetY - this.scaleY) * this.DampingY / Time.deltaTime;
    this.scaleY += (this.scaleSpeedY *= this.ElasticY) * Time.deltaTime;
    this.gameObject.transform.localScale = new Vector3(this.scaleX, this.scaleY);
  }
}
