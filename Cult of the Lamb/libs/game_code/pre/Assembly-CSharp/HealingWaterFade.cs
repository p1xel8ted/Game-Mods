// Decompiled with JetBrains decompiler
// Type: HealingWaterFade
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class HealingWaterFade : BaseMonoBehaviour
{
  public SpriteRenderer BlueWater;
  public SpriteRenderer BlackWater;
  public BaseMonoBehaviour HealingComponent;
  public BaseMonoBehaviour SplashComponent;
  public GameObject HeartGem;
  public GameObject HeartGemBroken;

  public void CrossFade() => this.StartCoroutine((IEnumerator) this.CrossFadeRoutine());

  private IEnumerator CrossFadeRoutine()
  {
    this.HeartGem.SetActive(false);
    this.HeartGemBroken.SetActive(true);
    CameraManager.shakeCamera(0.5f, (float) Random.Range(0, 360));
    this.BlueWater.gameObject.SetActive(false);
    this.BlackWater.gameObject.SetActive(true);
    Color CurrentColor = new Color(1f, 0.0f, 0.0f, 0.0f);
    Color TargetColor = this.BlackWater.color;
    float Progress = 0.0f;
    float Duration = 2f;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      this.BlackWater.color = Color.Lerp(CurrentColor, TargetColor, Progress / Duration);
      yield return (object) null;
    }
    this.HealingComponent.enabled = false;
    this.SplashComponent.enabled = true;
  }
}
