// Decompiled with JetBrains decompiler
// Type: FollowerExhaustionWarning
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class FollowerExhaustionWarning : MonoBehaviour, IUpdateManually
{
  public Follower Follower;
  public GameObject Container;
  public Image ProgressBar;
  private bool IsPlaying;
  public Gradient Gradient;

  private void OnEnable()
  {
    this.StartCoroutine((IEnumerator) this.Init());
    FollowerBrainStats.OnExhaustionStateChanged += new FollowerBrainStats.StatStateChangedEvent(this.ToggleWarning);
  }

  private void Awake() => this.Container.SetActive(false);

  private void Start()
  {
    this.Follower.OnFollowerBrainAssigned += new System.Action(this.OnBrainAssigned);
    if (this.Follower.Brain == null)
      return;
    this.OnBrainAssigned();
  }

  private void OnBrainAssigned()
  {
    this.Follower.OnFollowerBrainAssigned -= new System.Action(this.OnBrainAssigned);
    this.Follower.Brain.OnStateChanged += new Action<FollowerState, FollowerState>(this.OnStateChanged);
  }

  private void OnStateChanged(FollowerState newState, FollowerState oldState)
  {
    if (newState == null || newState.Type != FollowerStateType.Exhausted)
      return;
    this.StartCoroutine((IEnumerator) this.Init());
  }

  private IEnumerator Init()
  {
    yield return (object) new WaitForEndOfFrame();
    if ((UnityEngine.Object) this.Follower != (UnityEngine.Object) null && this.Follower.Brain != null && this.Follower.Brain.Stats != null)
    {
      FollowerState currentState = this.Follower.Brain.CurrentState;
      if ((currentState != null ? (currentState.Type == FollowerStateType.Exhausted ? 1 : 0) : 0) != 0)
        this.Show();
    }
  }

  private void OnDestroy()
  {
    FollowerBrainStats.OnExhaustionStateChanged -= new FollowerBrainStats.StatStateChangedEvent(this.ToggleWarning);
    if (!((UnityEngine.Object) this.Follower != (UnityEngine.Object) null) || this.Follower.Brain == null)
      return;
    this.Follower.Brain.OnStateChanged -= new Action<FollowerState, FollowerState>(this.OnStateChanged);
  }

  private void OnDisable()
  {
    FollowerBrainStats.OnExhaustionStateChanged -= new FollowerBrainStats.StatStateChangedEvent(this.ToggleWarning);
    if (!((UnityEngine.Object) this.Follower != (UnityEngine.Object) null) || this.Follower.Brain == null)
      return;
    this.Follower.Brain.OnStateChanged -= new Action<FollowerState, FollowerState>(this.OnStateChanged);
  }

  private void ToggleWarning(
    int followerid,
    FollowerStatState newstate,
    FollowerStatState oldstate)
  {
    if ((UnityEngine.Object) this.Follower != (UnityEngine.Object) null && this.Follower.Brain != null)
    {
      FollowerState currentState = this.Follower.Brain.CurrentState;
      if ((currentState != null ? (currentState.Type == FollowerStateType.Exhausted ? 1 : 0) : 0) != 0)
      {
        this.Show();
        return;
      }
    }
    this.Hide();
  }

  public void UpdateManually() => this.Update();

  private void Update()
  {
    if (!((UnityEngine.Object) this.Follower != (UnityEngine.Object) null) || this.Follower.Brain == null || this.Follower.Brain.Stats == null)
      return;
    if (this.IsPlaying && (UnityEngine.Object) this.ProgressBar != (UnityEngine.Object) null)
    {
      this.ProgressBar.fillAmount = Mathf.Max(this.Follower.Brain.Stats.Exhaustion / 100f, 0.1f);
      this.ProgressBar.color = this.Gradient.Evaluate(this.ProgressBar.fillAmount);
    }
    else
    {
      if (this.IsPlaying || (double) this.Follower.Brain.Stats.Exhaustion <= 0.0)
        return;
      this.Show();
    }
  }

  private void Show()
  {
    if ((UnityEngine.Object) this == (UnityEngine.Object) null || this.Container.activeInHierarchy)
      return;
    this.Container.SetActive(true);
    this.Container.transform.localScale = Vector3.zero;
    this.Container.transform.DOKill();
    this.Container.transform.DOScale(Vector3.one * 0.01f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.Follower.Interaction_FollowerInteraction.UpdateLayoutContent()));
    this.IsPlaying = true;
  }

  public void Hide()
  {
    if ((UnityEngine.Object) this == (UnityEngine.Object) null || !this.Container.activeInHierarchy)
      return;
    this.Container.transform.localScale = Vector3.one * 0.01f;
    this.Container.transform.DOKill();
    this.Container.transform.DOScale(Vector3.zero, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.Container.SetActive(false))).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
    {
      this.Container.SetActive(false);
      this.Follower.Interaction_FollowerInteraction.UpdateLayoutContent();
    }));
    this.IsPlaying = false;
  }
}
