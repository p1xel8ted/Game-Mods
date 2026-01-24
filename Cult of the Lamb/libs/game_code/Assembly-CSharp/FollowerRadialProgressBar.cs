// Decompiled with JetBrains decompiler
// Type: FollowerRadialProgressBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FollowerRadialProgressBar : BaseMonoBehaviour
{
  public bool _shown;
  public SpriteRenderer RadialProgress;
  public SpriteRenderer RadialProgressFlashWhite;
  public List<GameObject> Images = new List<GameObject>();
  public Follower follower;
  public Coroutine cFlashRoutine;

  public void Awake()
  {
    this.RadialProgress.material.SetFloat("_Angle", 90f);
    this.SetVisibility();
  }

  public void Hide()
  {
    this._shown = false;
    this.SetVisibility();
  }

  public void Show()
  {
    this._shown = true;
    this.SetVisibility();
  }

  public void SetVisibility()
  {
    this.RadialProgress.gameObject.SetActive(this._shown);
    foreach (GameObject image in this.Images)
      image.SetActive(this._shown);
    this.RadialProgressFlashWhite.color = new Color(1f, 1f, 1f, 0.0f);
  }

  public void UpdateBar(float normalisedValue)
  {
    if (this.follower.Brain.Info == null || this.follower.Brain.CurrentTask == null)
      return;
    this.RadialProgress.material.SetFloat("_Arc1", (float) (360.0 - (double) normalisedValue * 360.0));
    this.RadialProgress.material.SetFloat("_Arc2", 0.0f);
  }

  public void Flash()
  {
    if (this.cFlashRoutine != null)
      this.StopCoroutine(this.cFlashRoutine);
    this.cFlashRoutine = this.StartCoroutine((IEnumerator) this.FlashRoutine());
  }

  public IEnumerator FlashRoutine()
  {
    float Progress = 0.0f;
    float Duration = 0.5f;
    Color Transparent = new Color(1f, 1f, 1f, 0.0f);
    this.RadialProgressFlashWhite.color = Color.white;
    yield return (object) new WaitForSeconds(0.2f);
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      this.RadialProgressFlashWhite.color = Color.Lerp(Color.white, Transparent, Progress / Duration);
      yield return (object) null;
    }
  }
}
