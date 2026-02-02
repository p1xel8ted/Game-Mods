// Decompiled with JetBrains decompiler
// Type: FollowerIllnessWarning
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FollowerIllnessWarning : MonoBehaviour, IUpdateManually
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

  public IEnumerator Init()
  {
    FollowerIllnessWarning followerIllnessWarning = this;
    yield return (object) new WaitForEndOfFrame();
    if ((Object) followerIllnessWarning.Follower != (Object) null && followerIllnessWarning.Follower.Brain != null && followerIllnessWarning.Follower.Brain.Stats != null && followerIllnessWarning.Follower.Brain.Info.CursedState == Thought.Ill)
      followerIllnessWarning.Show();
    else
      followerIllnessWarning.Hide();
    FollowerBrainStats.OnIllnessStateChanged += new FollowerBrainStats.StatStateChangedEvent(followerIllnessWarning.ToggleWarning);
  }

  public void OnDisable()
  {
    FollowerBrainStats.OnIllnessStateChanged -= new FollowerBrainStats.StatStateChangedEvent(this.ToggleWarning);
  }

  public void DisableCanvasAnimations()
  {
  }

  public void ToggleWarning(int followerid, FollowerStatState newstate, FollowerStatState oldstate)
  {
    if ((Object) this.Follower != (Object) null && this.Follower.Brain != null && this.Follower.Brain.Stats != null && this.Follower.Brain.Info.CursedState == Thought.Ill)
      this.Show();
    else
      this.Hide();
  }

  public void UpdateManually() => this.Update();

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
      float num = Mathf.Max(this.Follower.Brain.Stats.Illness / 100f, 0.1f);
      Color color = this.Gradient.Evaluate(num);
      this.ProgressBar.color = new Color(color.r, color.g, color.b, num);
    }
    else
    {
      if (this.IsPlaying || this.Follower.Brain.Info.CursedState != Thought.Ill)
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
    this.Container.transform.DOScale(Vector3.zero, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.Container.SetActive(false))).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.Container.SetActive(false)));
    this.IsPlaying = false;
  }

  [CompilerGenerated]
  public void \u003CHide\u003Eb__17_0() => this.Container.SetActive(false);

  [CompilerGenerated]
  public void \u003CHide\u003Eb__17_1() => this.Container.SetActive(false);
}
