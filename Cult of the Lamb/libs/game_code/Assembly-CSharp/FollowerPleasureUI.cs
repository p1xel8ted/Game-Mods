// Decompiled with JetBrains decompiler
// Type: FollowerPleasureUI
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
public class FollowerPleasureUI : MonoBehaviour
{
  [SerializeField]
  public Transform barContainer;
  public bool IsPlaying;
  public Follower follower;
  public GameObject BarContainer;
  public GameObject CompleteContainer;
  public BarControllerNonUI BarController;
  public DG.Tweening.Sequence Sequence;
  public Follower Follower;

  public void Awake() => this.BarContainer.gameObject.SetActive(false);

  public void Start()
  {
    if ((Object) this.follower != (Object) null && this.follower.Brain != null && this.Follower.Brain.Info != null && (Object) this.BarController != (Object) null)
      this.BarController.SetBarSize((float) this.Follower.Brain.Info.Pleasure / 65f, false);
    this.barContainer.localScale = Vector3.zero;
  }

  public void OnDestroy()
  {
    if (this.Sequence != null && this.Sequence.active)
    {
      this.Sequence.Kill();
      this.Sequence = (DG.Tweening.Sequence) null;
    }
    this.transform.DOKill();
  }

  public void Show(bool animate = true)
  {
    if (this.IsPlaying || !DataManager.Instance.ShowLoyaltyBars)
      return;
    this.EnableBarContainerGameobject();
    this.transform.DOKill();
    this.BarController.SetBarSize((float) this.Follower.Brain.Info.Pleasure / 65f, false);
    if (animate)
    {
      this.barContainer.localScale = Vector3.zero;
      this.barContainer.DOScale(Vector3.one * 0.7f, 0.3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    }
    else
      this.barContainer.localScale = Vector3.one * 0.7f;
  }

  public void Hide(bool animate = true)
  {
    if (this.IsPlaying || this.barContainer.localScale == Vector3.zero)
      return;
    this.barContainer.DOKill();
    if (animate)
    {
      this.barContainer.DOScale(Vector3.zero, 0.3f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.BarContainer.gameObject.SetActive(false)));
    }
    else
    {
      this.barContainer.localScale = Vector3.zero;
      this.BarContainer.gameObject.SetActive(false);
    }
  }

  public void HideCompleted() => this.CompleteContainer.gameObject.SetActive(false);

  public IEnumerator IncreasePleasure()
  {
    FollowerPleasureUI followerPleasureUi = this;
    if (DataManager.Instance.PleasureEnabled)
    {
      followerPleasureUi.EnableBarContainerGameobject();
      followerPleasureUi.IsPlaying = true;
      yield return (object) new WaitForSeconds(0.01f);
      if (followerPleasureUi.Sequence != null && followerPleasureUi.Sequence.active)
        followerPleasureUi.Sequence.Kill();
      float num1 = 0.0f;
      followerPleasureUi.Sequence = DOTween.Sequence();
      followerPleasureUi.Sequence.SetUpdate<DG.Tweening.Sequence>(true);
      if (followerPleasureUi.barContainer.localScale != Vector3.one * 0.7f)
      {
        followerPleasureUi.barContainer.localScale = Vector3.zero;
        followerPleasureUi.Sequence.Append((Tween) followerPleasureUi.barContainer.DOScale(Vector3.one * 0.7f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true));
        num1 += 0.5f;
      }
      followerPleasureUi.Sequence.AppendCallback(new TweenCallback(followerPleasureUi.\u003CIncreasePleasure\u003Eb__14_0));
      followerPleasureUi.Sequence.AppendInterval(1f).SetUpdate<DG.Tweening.Sequence>(true);
      float num2 = num1 + 1f;
      followerPleasureUi.Sequence.Append((Tween) followerPleasureUi.barContainer.DOScale(Vector3.zero, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true));
      float time = num2 + 0.5f;
      followerPleasureUi.Sequence.Play<DG.Tweening.Sequence>();
      yield return (object) new WaitForSecondsRealtime(time);
      followerPleasureUi.BarContainer.gameObject.SetActive(false);
      followerPleasureUi.IsPlaying = false;
    }
  }

  public void EnableBarContainerGameobject()
  {
    if ((Object) this.BarContainer != (Object) null)
      this.BarContainer.gameObject.SetActive(true);
    FaithCanvasOptimization componentInParent = this.GetComponentInParent<FaithCanvasOptimization>();
    if (!((Object) componentInParent != (Object) null))
      return;
    componentInParent.ActivateCanvas();
  }

  [CompilerGenerated]
  public void \u003CHide\u003Eb__9_0() => this.BarContainer.gameObject.SetActive(false);

  [CompilerGenerated]
  public void \u003CIncreasePleasure\u003Eb__14_0()
  {
    this.BarController.SetBarSize((float) this.Follower.Brain.Info.Pleasure / 65f, true, true);
  }
}
