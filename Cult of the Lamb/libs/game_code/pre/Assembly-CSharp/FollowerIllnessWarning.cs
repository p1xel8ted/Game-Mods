// Decompiled with JetBrains decompiler
// Type: FollowerIllnessWarning
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class FollowerIllnessWarning : MonoBehaviour, IUpdateManually
{
  public Follower Follower;
  public GameObject Container;
  public Image ProgressBar;
  private bool IsPlaying;
  public Gradient Gradient;

  private void OnEnable() => this.StartCoroutine((IEnumerator) this.Init());

  private void Awake() => this.Container.SetActive(false);

  private IEnumerator Init()
  {
    FollowerIllnessWarning followerIllnessWarning = this;
    yield return (object) new WaitForEndOfFrame();
    if ((Object) followerIllnessWarning.Follower != (Object) null && followerIllnessWarning.Follower.Brain != null && followerIllnessWarning.Follower.Brain.Stats != null && followerIllnessWarning.Follower.Brain.Info.CursedState == Thought.Ill)
      followerIllnessWarning.Show();
    FollowerBrainStats.OnIllnessStateChanged += new FollowerBrainStats.StatStateChangedEvent(followerIllnessWarning.ToggleWarning);
  }

  private void OnDisable()
  {
    FollowerBrainStats.OnIllnessStateChanged -= new FollowerBrainStats.StatStateChangedEvent(this.ToggleWarning);
  }

  private void ToggleWarning(
    int followerid,
    FollowerStatState newstate,
    FollowerStatState oldstate)
  {
    if ((Object) this.Follower != (Object) null && this.Follower.Brain != null && this.Follower.Brain.Stats != null && this.Follower.Brain.Info.CursedState == Thought.Ill)
      this.Show();
    else
      this.Hide();
  }

  public void UpdateManually() => this.Update();

  private void Update()
  {
    if (!((Object) this.Follower != (Object) null) || this.Follower.Brain == null || this.Follower.Brain.Stats == null)
      return;
    if (this.IsPlaying && (Object) this.ProgressBar != (Object) null)
    {
      this.ProgressBar.fillAmount = Mathf.Max(this.Follower.Brain.Stats.Illness / 100f, 0.1f);
      this.ProgressBar.color = this.Gradient.Evaluate(this.ProgressBar.fillAmount);
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
    this.Container.transform.DOScale(Vector3.one * 0.01f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.Follower.Interaction_FollowerInteraction.UpdateLayoutContent()));
    this.IsPlaying = true;
  }

  public void Hide()
  {
    if (!this.Container.activeInHierarchy)
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
