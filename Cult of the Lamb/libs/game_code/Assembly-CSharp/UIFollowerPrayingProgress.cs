// Decompiled with JetBrains decompiler
// Type: UIFollowerPrayingProgress
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class UIFollowerPrayingProgress : BaseMonoBehaviour
{
  [SerializeField]
  public SpriteRenderer _radialProgress;
  [SerializeField]
  public Follower _follower;
  public bool _shown;
  public bool _flashing;

  public void Awake()
  {
    this.transform.localScale = Vector3.zero;
    this.gameObject.SetActive(false);
  }

  public void Show()
  {
    if (this._shown)
      return;
    this.gameObject.SetActive(true);
    this._shown = true;
    this.SetSpriteAtlasArcCenterOffset();
    this.transform.DOKill();
    this.transform.localScale = Vector3.zero;
    this.transform.DOScale(Vector3.one, 0.3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
  }

  public void Hide()
  {
    if (!this._shown)
      return;
    this._shown = false;
    this.transform.DOKill();
    this.transform.localScale = Vector3.one;
    this.transform.DOScale(Vector3.zero, 0.3f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.gameObject.SetActive(false)));
  }

  public void Update()
  {
    if (!this._shown || this._flashing || (Object) this._follower == (Object) null || this._follower.Brain == null || this._follower.Brain.Info == null || this._follower.Brain.CurrentTask == null || Time.frameCount % TimeManager.SIMULATION_FRAME_SPREAD != this._follower.Brain.Info.ID % TimeManager.SIMULATION_FRAME_SPREAD)
      return;
    float num = 0.0f;
    if (this._follower.Brain.CurrentTask is FollowerTask_Pray currentTask2)
      num = currentTask2.GetDurationPerDevotion(this._follower);
    else if (this._follower.Brain.CurrentTask is FollowerTask_PrayPassive currentTask1)
      num = currentTask1.GetDurationPerDevotion(this._follower);
    this._radialProgress.material.SetFloat("_Arc2", (float) (360.0 * (1.0 - (double) this._follower.Brain.Info.PrayProgress / (double) num)));
  }

  public void Flash()
  {
    if (!this.gameObject.activeInHierarchy)
      this.gameObject.gameObject.SetActive(true);
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.DoFlash());
  }

  public IEnumerator DoFlash()
  {
    this._flashing = true;
    Color transparent = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    this._radialProgress.color = Color.white;
    yield return (object) new WaitForSeconds(0.2f);
    this._radialProgress.DOColor(transparent, 0.5f);
    yield return (object) new WaitForSeconds(0.5f);
    this._radialProgress.material.SetFloat("_Arc2", 360f);
    this._radialProgress.color = StaticColors.RedColor;
    this._flashing = false;
  }

  public void SetSpriteAtlasArcCenterOffset()
  {
    Vector2 center = this._radialProgress.sprite.textureRect.center;
    this._radialProgress.material.SetVector("_ArcCenterOffset", (Vector4) (new Vector2(center.x / (float) this._radialProgress.sprite.texture.width, center.y / (float) this._radialProgress.sprite.texture.height) - Vector2.one * 0.5f));
  }

  [CompilerGenerated]
  public void \u003CHide\u003Eb__6_0() => this.gameObject.SetActive(false);
}
