// Decompiled with JetBrains decompiler
// Type: FollowerReeducationWarning
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
public class FollowerReeducationWarning : MonoBehaviour, IUpdateManually
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
    FollowerReeducationWarning reeducationWarning = this;
    yield return (object) new WaitForEndOfFrame();
    if ((UnityEngine.Object) reeducationWarning.Follower != (UnityEngine.Object) null && reeducationWarning.Follower.Brain != null && reeducationWarning.Follower.Brain.Stats != null && reeducationWarning.Follower.Brain.Info.CursedState == Thought.Dissenter)
      reeducationWarning.Show();
    if ((UnityEngine.Object) reeducationWarning.Follower != (UnityEngine.Object) null && reeducationWarning.Follower.Brain != null)
    {
      reeducationWarning.Follower.Brain.OnBecomeDissenter += new System.Action(reeducationWarning.ToggleWarning);
      reeducationWarning.Follower.Brain.Stats.OnReeducationComplete += new System.Action(reeducationWarning.ToggleWarning);
    }
  }

  private void OnDisable()
  {
    if (!((UnityEngine.Object) this.Follower != (UnityEngine.Object) null) || this.Follower.Brain == null)
      return;
    this.Follower.Brain.OnBecomeDissenter -= new System.Action(this.ToggleWarning);
    this.Follower.Brain.Stats.OnReeducationComplete -= new System.Action(this.ToggleWarning);
  }

  public void ToggleWarning()
  {
    if ((UnityEngine.Object) this.Follower != (UnityEngine.Object) null && this.Follower.Brain != null && this.Follower.Brain.Stats != null && (double) this.Follower.Brain.Stats.Reeducation < 100.0)
      this.Show();
    else
      this.Hide();
  }

  public void UpdateManually() => this.Update();

  private void Update()
  {
    if (!((UnityEngine.Object) this.Follower != (UnityEngine.Object) null) || this.Follower.Brain == null || this.Follower.Brain.Stats == null)
      return;
    if (this.IsPlaying && (UnityEngine.Object) this.ProgressBar != (UnityEngine.Object) null)
    {
      this.ProgressBar.fillAmount = Mathf.Max(this.Follower.Brain.Stats.Reeducation / 100f, 0.1f);
      this.ProgressBar.color = this.Gradient.Evaluate(this.ProgressBar.fillAmount);
      if (this.Follower.Brain.Info.CursedState == Thought.Dissenter)
        return;
      this.Hide();
    }
    else
    {
      if (this.IsPlaying || this.Follower.Brain.Info.CursedState != Thought.Dissenter)
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
