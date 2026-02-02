// Decompiled with JetBrains decompiler
// Type: ScaleBounce
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  public float scaleSpeedX;
  public float scaleSpeedY;
  public float scaleX = 1f;
  public float scaleY = 1f;
  public float StartScaleX = 1f;
  public float StartScaleY = 1f;

  public void OnEnable()
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

  public void Update()
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
