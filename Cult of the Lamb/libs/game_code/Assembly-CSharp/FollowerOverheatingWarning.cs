// Decompiled with JetBrains decompiler
// Type: FollowerOverheatingWarning
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FollowerOverheatingWarning : MonoBehaviour, IUpdateManually
{
  public Follower Follower;
  public GameObject Container;
  public SpriteRenderer ProgressBar;
  public Gradient Gradient;
  public bool IsPlaying;
  public Vector3 originalScale;
  public const float DELAY_BETWEEN_UPDATES = 0.6f;
  public float delayTimer;

  public void OnEnable()
  {
    this.StartCoroutine((IEnumerator) this.Init());
    FollowerBrainStats.OnOverheatingStateChanged += new FollowerBrainStats.StatStateChangedEvent(this.ToggleWarning);
  }

  public void Awake()
  {
    this.originalScale = this.Container.transform.localScale;
    this.Container.SetActive(false);
    this.DisableCanvasAnimations();
  }

  public void Start()
  {
    this.Follower.OnFollowerBrainAssigned += new System.Action(this.OnBrainAssigned);
    if (this.Follower.Brain == null)
      return;
    this.OnBrainAssigned();
  }

  public void OnBrainAssigned()
  {
    this.Follower.OnFollowerBrainAssigned -= new System.Action(this.OnBrainAssigned);
    this.Follower.Brain.OnStateChanged += new Action<FollowerState, FollowerState>(this.OnStateChanged);
  }

  public void OnStateChanged(FollowerState newState, FollowerState oldState)
  {
    if (newState == null || newState.Type != FollowerStateType.Overheating)
      return;
    this.StartCoroutine((IEnumerator) this.Init());
  }

  public IEnumerator Init()
  {
    yield return (object) new WaitForEndOfFrame();
    if ((UnityEngine.Object) this.Follower != (UnityEngine.Object) null && this.Follower.Brain != null && this.Follower.Brain.Stats != null)
    {
      FollowerState currentState = this.Follower.Brain.CurrentState;
      if ((currentState != null ? (currentState.Type == FollowerStateType.Overheating ? 1 : 0) : 0) != 0)
        this.Show();
    }
  }

  public void OnDestroy()
  {
    FollowerBrainStats.OnOverheatingStateChanged -= new FollowerBrainStats.StatStateChangedEvent(this.ToggleWarning);
    if (!((UnityEngine.Object) this.Follower != (UnityEngine.Object) null) || this.Follower.Brain == null)
      return;
    this.Follower.Brain.OnStateChanged -= new Action<FollowerState, FollowerState>(this.OnStateChanged);
  }

  public void OnDisable()
  {
    FollowerBrainStats.OnOverheatingStateChanged -= new FollowerBrainStats.StatStateChangedEvent(this.ToggleWarning);
    if (!((UnityEngine.Object) this.Follower != (UnityEngine.Object) null) || this.Follower.Brain == null)
      return;
    this.Follower.Brain.OnStateChanged -= new Action<FollowerState, FollowerState>(this.OnStateChanged);
  }

  public void DisableCanvasAnimations()
  {
  }

  public void ToggleWarning(int followerid, FollowerStatState newstate, FollowerStatState oldstate)
  {
    if ((UnityEngine.Object) this.Follower != (UnityEngine.Object) null && this.Follower.Brain != null && this.Follower.Brain.Info.CursedState == Thought.Overheating)
      this.Show();
    else
      this.Hide();
  }

  public void UpdateManually() => this.Update();

  public void Update()
  {
    if (!((UnityEngine.Object) this.Follower != (UnityEngine.Object) null) || this.Follower.Brain == null || this.Follower.Brain.Stats == null)
      return;
    if (this.IsPlaying && (UnityEngine.Object) this.ProgressBar != (UnityEngine.Object) null)
    {
      this.delayTimer -= Time.deltaTime;
      if ((double) this.delayTimer > 0.0)
        return;
      this.delayTimer = 0.6f;
      float num = Mathf.Max(this.Follower.Brain.Stats.Overheating / 100f, 0.1f);
      Color color = this.Gradient.Evaluate(num);
      this.ProgressBar.color = new Color(color.r, color.g, color.b, num);
    }
    else
    {
      if (this.IsPlaying || (double) this.Follower.Brain.Stats.Overheating <= 0.0)
        return;
      this.Show();
    }
  }

  public void Show()
  {
    if ((UnityEngine.Object) this == (UnityEngine.Object) null || this.Container.activeInHierarchy)
      return;
    this.Container.SetActive(true);
    this.Container.transform.localScale = Vector3.zero;
    this.Container.transform.DOKill();
    this.Container.transform.DOScale(this.originalScale, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    this.IsPlaying = true;
  }

  public void Hide()
  {
    if ((UnityEngine.Object) this == (UnityEngine.Object) null || !this.Container.activeInHierarchy)
      return;
    this.Container.transform.localScale = this.originalScale;
    this.Container.transform.DOKill();
    this.Container.transform.DOScale(Vector3.zero, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.Container.SetActive(false))).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.Container.SetActive(false)));
    this.IsPlaying = false;
  }

  [CompilerGenerated]
  public void \u003CHide\u003Eb__21_0() => this.Container.SetActive(false);

  [CompilerGenerated]
  public void \u003CHide\u003Eb__21_1() => this.Container.SetActive(false);
}
