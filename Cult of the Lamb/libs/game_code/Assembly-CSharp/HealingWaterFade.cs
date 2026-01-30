// Decompiled with JetBrains decompiler
// Type: HealingWaterFade
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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

  public IEnumerator CrossFadeRoutine()
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
