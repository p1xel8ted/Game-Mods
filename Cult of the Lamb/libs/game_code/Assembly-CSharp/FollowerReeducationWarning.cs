// Decompiled with JetBrains decompiler
// Type: FollowerReeducationWarning
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FollowerReeducationWarning : MonoBehaviour, IUpdateManually
{
  public Follower Follower;
  public GameObject Container;
  public SpriteRenderer ProgressBar;
  public Gradient Gradient;
  public const float DELAY_BETWEEN_UPDATES = 0.6f;
  public float delayTimer;
  public bool IsPlaying;
  public Vector3 originalScale;

  public void OnEnable() => this.StartCoroutine(this.Init());

  public void Awake()
  {
    this.originalScale = this.Container.transform.localScale;
    this.Container.SetActive(false);
    this.DisableCanvasAnimations();
  }

  public IEnumerator Init()
  {
    FollowerReeducationWarning reeducationWarning = this;
    yield return (object) new WaitForEndOfFrame();
    if ((UnityEngine.Object) reeducationWarning.Follower != (UnityEngine.Object) null && reeducationWarning.Follower.Brain != null && reeducationWarning.Follower.Brain.Stats != null && reeducationWarning.Follower.Brain.Info.CursedState == Thought.Dissenter)
      reeducationWarning.Show();
    else
      reeducationWarning.Hide();
    if ((UnityEngine.Object) reeducationWarning.Follower != (UnityEngine.Object) null && reeducationWarning.Follower.Brain != null)
    {
      reeducationWarning.Follower.Brain.OnBecomeDissenter += new System.Action(reeducationWarning.ToggleWarning);
      reeducationWarning.Follower.Brain.Stats.OnReeducationComplete += new System.Action(reeducationWarning.ToggleWarning);
    }
  }

  public void OnDisable()
  {
    if (!((UnityEngine.Object) this.Follower != (UnityEngine.Object) null) || this.Follower.Brain == null)
      return;
    this.Follower.Brain.OnBecomeDissenter -= new System.Action(this.ToggleWarning);
    this.Follower.Brain.Stats.OnReeducationComplete -= new System.Action(this.ToggleWarning);
  }

  public void DisableCanvasAnimations()
  {
  }

  public void ToggleWarning()
  {
    if ((UnityEngine.Object) this.Follower != (UnityEngine.Object) null && this.Follower.Brain != null && this.Follower.Brain.Stats != null && (double) this.Follower.Brain.Stats.Reeducation < 100.0)
      this.Show();
    else
      this.Hide();
  }

  public void UpdateManually() => this.Update();

  public void Update()
  {
    if (!((UnityEngine.Object) this.Follower != (UnityEngine.Object) null) || this.Follower.Brain == null || this.Follower.Brain.Stats == null)
      return;
    if (this.IsPlaying && (UnityEngine.Object) this.ProgressBar != (UnityEngine.Object) null && (double) (this.delayTimer -= Time.deltaTime) <= 0.0)
    {
      this.delayTimer -= Time.deltaTime;
      if ((double) this.delayTimer > 0.0)
        return;
      this.delayTimer = 0.6f;
      float num = Mathf.Max(this.Follower.Brain.Stats.Reeducation / 100f, 0.1f);
      Color color = this.Gradient.Evaluate(num);
      this.ProgressBar.color = new Color(color.r, color.g, color.b, num);
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
