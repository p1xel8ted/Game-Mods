// Decompiled with JetBrains decompiler
// Type: FollowerAdorationUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

#nullable disable
public class FollowerAdorationUI : MonoBehaviour
{
  [SerializeField]
  public TMP_Text levelText;
  public bool IsPlaying;
  public Follower follower;
  public GameObject BarContainer;
  public GameObject CompleteContainer;
  public BarControllerNonUI bc;
  public BarControllerNonUI BarController;
  public DG.Tweening.Sequence Sequence;
  public Follower Follower;

  public void Awake() => this.BarContainer.gameObject.SetActive(false);

  public void Start()
  {
    if ((UnityEngine.Object) this.follower != (UnityEngine.Object) null && this.follower.Brain != null && this.follower.Brain.Stats != null && (UnityEngine.Object) this.BarController != (UnityEngine.Object) null)
      this.BarController.SetBarSize(this.Follower.Brain.Stats.Adoration / this.Follower.Brain.Stats.MAX_ADORATION, false);
    this.transform.localScale = Vector3.zero;
    if (this.follower.Brain == null)
      this.follower.OnFollowerBrainAssigned += new System.Action(this.AddListener);
    else
      this.AddListener();
  }

  public void AddListener()
  {
    this.follower.Interaction_FollowerInteraction.OnGivenRewards += new System.Action(this.SetObjects);
    this.levelText.text = this.follower.Brain.Info.XPLevel.ToNumeral();
    this.levelText.isRightToLeftText = LocalizeIntegration.IsArabic();
  }

  public void OnDestroy()
  {
    if (this.Sequence != null && this.Sequence.active)
    {
      this.Sequence.Kill();
      this.Sequence = (DG.Tweening.Sequence) null;
    }
    this.transform.DOKill();
    if (!((UnityEngine.Object) this.follower != (UnityEngine.Object) null))
      return;
    if ((UnityEngine.Object) this.follower.Interaction_FollowerInteraction != (UnityEngine.Object) null)
      this.follower.Interaction_FollowerInteraction.OnGivenRewards -= new System.Action(this.SetObjects);
    this.follower.OnFollowerBrainAssigned -= new System.Action(this.AddListener);
  }

  public void Show(bool animate = true)
  {
    if (this.IsPlaying || !DataManager.Instance.ShowLoyaltyBars)
      return;
    this.EnableBarContainerGameobject();
    this.levelText.text = this.follower.Brain.CanLevelUp() ? (this.follower.Brain.Info.XPLevel + 1).ToNumeral() : this.follower.Brain.Info.XPLevel.ToNumeral();
    this.transform.DOKill();
    this.BarController.SetBarSize(this.Follower.Brain.Stats.Adoration / this.Follower.Brain.Stats.MAX_ADORATION, false);
    if (this.follower.Brain.Stats.HasLevelledUp)
      this.transform.localScale = Vector3.one * 0.7f;
    else if (animate)
    {
      this.transform.localScale = Vector3.zero;
      this.transform.DOScale(Vector3.one * 0.7f, 0.3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    }
    else
      this.transform.localScale = Vector3.one * 0.7f;
    this.SetObjects();
  }

  public IEnumerator FlashLevelUpIcon()
  {
    while (this.follower.Brain.Stats.HasLevelledUp)
    {
      this.CompleteContainer.transform.DOKill();
      this.CompleteContainer.transform.DOPunchScale(new Vector3(0.15f, 0.15f), 0.5f);
      yield return (object) new WaitForSeconds(2f);
    }
    this.CompleteContainer.transform.DOKill();
  }

  public void SetObjects()
  {
    if (this.follower.Brain.Location == FollowerLocation.Church)
      return;
    this.EnableBarContainerGameobject();
    this.BarContainer.SetActive(!this.follower.Brain.Stats.HasLevelledUp);
    this.CompleteContainer.transform.DOKill();
    this.CompleteContainer.transform.DOPunchScale(new Vector3(0.15f, 0.15f), 0.5f);
    this.CompleteContainer.SetActive(this.follower.Brain.Stats.HasLevelledUp);
    if (this.follower.Brain.Stats.HasLevelledUp && this.transform.localScale != Vector3.one * 0.7f)
      this.transform.DOScale(Vector3.one * 0.7f, 0.3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    if (!this.follower.Brain.Stats.HasLevelledUp)
      return;
    this.bc.enabled = false;
  }

  public void Hide(bool animate = true)
  {
    if (this.IsPlaying || this.transform.localScale == Vector3.zero)
      return;
    if (this.follower.Brain.Stats.HasLevelledUp)
    {
      this.SetObjects();
    }
    else
    {
      this.transform.DOKill();
      if (animate)
      {
        this.transform.DOScale(Vector3.zero, 0.3f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.BarContainer.gameObject.SetActive(false)));
      }
      else
      {
        this.transform.localScale = Vector3.zero;
        this.BarContainer.gameObject.SetActive(false);
      }
    }
  }

  public void Test(FollowerBrain.AdorationActions Action)
  {
    this.follower.Brain.AddAdoration(Action, (System.Action) null);
  }

  public IEnumerator IncreaseAdorationIE()
  {
    FollowerAdorationUI followerAdorationUi = this;
    if (DataManager.Instance.ShowLoyaltyBars)
    {
      followerAdorationUi.EnableBarContainerGameobject();
      followerAdorationUi.IsPlaying = true;
      yield return (object) new WaitForSeconds(0.01f);
      Debug.Log((object) $"INCREASE ADORATION!  {followerAdorationUi.Follower.Brain.Stats.Adoration.ToString()}  {followerAdorationUi.Follower.Brain.Stats.MAX_ADORATION.ToString()}");
      if (followerAdorationUi.Sequence != null && followerAdorationUi.Sequence.active)
        followerAdorationUi.Sequence.Kill();
      float num1 = 0.0f;
      followerAdorationUi.Sequence = DOTween.Sequence();
      followerAdorationUi.Sequence.SetUpdate<DG.Tweening.Sequence>(true);
      if (followerAdorationUi.transform.localScale != Vector3.one * 0.7f)
      {
        followerAdorationUi.transform.localScale = Vector3.zero;
        followerAdorationUi.Sequence.Append((Tween) followerAdorationUi.transform.DOScale(Vector3.one * 0.7f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true));
        num1 += 0.5f;
      }
      followerAdorationUi.Sequence.AppendCallback(new TweenCallback(followerAdorationUi.\u003CIncreaseAdorationIE\u003Eb__18_0));
      followerAdorationUi.Sequence.AppendInterval(1f).SetUpdate<DG.Tweening.Sequence>(true);
      float num2 = num1 + 1f;
      if (!followerAdorationUi.follower.Brain.Stats.HasLevelledUp)
      {
        followerAdorationUi.Sequence.Append((Tween) followerAdorationUi.transform.DOScale(Vector3.zero, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true));
        num2 += 0.5f;
      }
      followerAdorationUi.Sequence.Play<DG.Tweening.Sequence>();
      yield return (object) new WaitForSecondsRealtime(num2 + 0.25f);
      if ((double) followerAdorationUi.follower.Brain.Stats.Adoration >= (double) followerAdorationUi.follower.Brain.Stats.MAX_ADORATION)
      {
        followerAdorationUi.levelText.text = (followerAdorationUi.follower.Brain.Info.XPLevel + 1).ToNumeral();
        followerAdorationUi.levelText.transform.DOPunchScale(Vector3.one * 0.2f, 0.2f).SetUpdate<Tweener>(true);
        yield return (object) new WaitForSecondsRealtime(1f);
      }
      followerAdorationUi.BarContainer.gameObject.SetActive(false);
      followerAdorationUi.IsPlaying = false;
    }
  }

  public void EnableBarContainerGameobject()
  {
    if ((UnityEngine.Object) this.BarContainer != (UnityEngine.Object) null)
      this.BarContainer.gameObject.SetActive(true);
    FaithCanvasOptimization componentInParent = this.GetComponentInParent<FaithCanvasOptimization>();
    if (!((UnityEngine.Object) componentInParent != (UnityEngine.Object) null))
      return;
    componentInParent.ActivateCanvas();
  }

  [CompilerGenerated]
  public void \u003CHide\u003Eb__13_0() => this.BarContainer.gameObject.SetActive(false);

  [CompilerGenerated]
  public void \u003CIncreaseAdorationIE\u003Eb__18_0()
  {
    this.BarController.SetBarSize(this.Follower.Brain.Stats.Adoration / this.Follower.Brain.Stats.MAX_ADORATION, true, true);
  }
}
