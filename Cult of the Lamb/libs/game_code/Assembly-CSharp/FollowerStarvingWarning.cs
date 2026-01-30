// Decompiled with JetBrains decompiler
// Type: FollowerStarvingWarning
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FollowerStarvingWarning : MonoBehaviour, IUpdateManually
{
  public Follower Follower;
  public GameObject Container;
  public SpriteRenderer ProgressBar;
  public Gradient Gradient;
  public const float DELAY_BETWEEN_UPDATES = 0.6f;
  public float delayTimer;
  public bool IsPlaying;
  public Vector3 originalScale;

  public void OnEnable() => this.StartCoroutine((IEnumerator) this.Init());

  public void Awake()
  {
    this.originalScale = this.Container.transform.localScale;
    this.Container.SetActive(false);
    this.DisableCanvasAnimations();
  }

  public void DisableCanvasAnimations()
  {
  }

  public IEnumerator Init()
  {
    FollowerStarvingWarning followerStarvingWarning = this;
    yield return (object) new WaitForEndOfFrame();
    if ((Object) followerStarvingWarning.Follower != (Object) null && followerStarvingWarning.Follower.Brain != null && followerStarvingWarning.Follower.Brain.Stats != null && followerStarvingWarning.Follower.Brain.Info.CursedState == Thought.BecomeStarving)
      followerStarvingWarning.Show();
    else
      followerStarvingWarning.Hide();
    FollowerBrainStats.OnStarvationStateChanged += new FollowerBrainStats.StatStateChangedEvent(followerStarvingWarning.ToggleWarning);
  }

  public void OnDisable()
  {
    FollowerBrainStats.OnStarvationStateChanged -= new FollowerBrainStats.StatStateChangedEvent(this.ToggleWarning);
  }

  public void ToggleWarning(int followerid, FollowerStatState newstate, FollowerStatState oldstate)
  {
    if ((Object) this.Follower != (Object) null && this.Follower.Brain != null && this.Follower.Brain.Stats != null && this.Follower.Brain.Info.CursedState == Thought.BecomeStarving)
      this.Show();
    else
      this.Hide();
  }

  public void Update()
  {
    if (!((Object) this.Follower != (Object) null) || this.Follower.Brain == null || this.Follower.Brain.Stats == null)
      return;
    if (this.IsPlaying && (Object) this.ProgressBar != (Object) null)
    {
      this.delayTimer -= Time.deltaTime;
      if ((double) this.delayTimer > 0.0)
        return;
      this.delayTimer = 0.6f;
      float num = Mathf.Max(this.Follower.Brain.Stats.Starvation / 75f, 0.1f);
      Color color = this.Gradient.Evaluate(num);
      this.ProgressBar.color = new Color(color.r, color.g, color.b, num);
    }
    else
    {
      if (this.IsPlaying || this.Follower.Brain.Info.CursedState != Thought.BecomeStarving)
        return;
      this.Show();
    }
  }

  public void Show()
  {
    if (this.Container.activeInHierarchy)
      return;
    this.Container.SetActive(true);
    this.Container.transform.localScale = Vector3.zero;
    this.Container.transform.DOKill();
    this.Container.transform.DOScale(this.originalScale, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    this.IsPlaying = true;
  }

  public void Hide()
  {
    if (!this.Container.activeInHierarchy)
      return;
    this.Container.transform.localScale = this.originalScale;
    this.Container.transform.DOKill();
    this.Container.transform.DOScale(Vector3.zero, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.Container.SetActive(false)));
    this.IsPlaying = false;
  }

  public void UpdateManually() => this.Update();

  [CompilerGenerated]
  public void \u003CHide\u003Eb__16_0() => this.Container.SetActive(false);
}
