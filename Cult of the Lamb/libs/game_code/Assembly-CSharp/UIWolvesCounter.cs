// Decompiled with JetBrains decompiler
// Type: UIWolvesCounter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Spine.Unity;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

#nullable disable
public class UIWolvesCounter : MonoBehaviour
{
  [SerializeField]
  public CanvasGroup canvasGroup;
  [SerializeField]
  public Transform container;
  [SerializeField]
  public GameObject countParent;
  [SerializeField]
  public TMP_Text count;
  [SerializeField]
  public SkeletonGraphic checkBox;

  public void Start()
  {
    this.gameObject.SetActive(false);
    Interaction_WolfBase.OnWolvesBegan += new Interaction_WolfBase.WolfEvent(this.Show);
    Interaction_WolfBase.OnWolvesSucceeded += new Interaction_WolfBase.WolfEvent(this.OnWolvesSucceeded);
    Interaction_WolfBase.OnWolvesFailed += new Interaction_WolfBase.WolfEvent(this.OnWolvesFailed);
    Interaction_WolfBase.OnWolfFled += new Interaction_WolfBase.WolfEvent(this.OnWolfFled);
    Interaction_WolfBase.OnWolfDied += new Interaction_WolfBase.WolfEvent(this.OnWolfDied);
  }

  public void OnDestroy()
  {
    Interaction_WolfBase.OnWolvesBegan -= new Interaction_WolfBase.WolfEvent(this.Show);
    Interaction_WolfBase.OnWolfFled -= new Interaction_WolfBase.WolfEvent(this.OnWolfFled);
    Interaction_WolfBase.OnWolfDied -= new Interaction_WolfBase.WolfEvent(this.OnWolfDied);
    Interaction_WolfBase.OnWolvesSucceeded -= new Interaction_WolfBase.WolfEvent(this.OnWolvesSucceeded);
    Interaction_WolfBase.OnWolvesFailed -= new Interaction_WolfBase.WolfEvent(this.OnWolvesFailed);
  }

  public void Show()
  {
    this.canvasGroup.alpha = 0.0f;
    this.gameObject.SetActive(true);
    this.countParent.gameObject.SetActive(true);
    this.checkBox.gameObject.SetActive(false);
    this.canvasGroup.DOFade(1f, 1f);
    this.container.transform.localScale = Vector3.one;
    this.container.transform.DOScale(1.2f, 1f).SetLoops<TweenerCore<Vector3, Vector3, VectorOptions>>(-1, DG.Tweening.LoopType.Yoyo);
    this.count.text = (Interaction_WolfBase.WolfTarget - Interaction_WolfBase.WolfFled - Interaction_WolfBase.WolfDied).ToString();
  }

  public void OnWolfFled()
  {
    this.transform.DOKill();
    this.transform.localScale = Vector3.one * 0.6f;
    this.transform.DOPunchScale(Vector3.one * 0.3f, 0.2f, 1);
    this.count.text = (Interaction_WolfBase.WolfTarget - Interaction_WolfBase.WolfFled - Interaction_WolfBase.WolfDied).ToString();
  }

  public void OnWolfDied()
  {
    this.transform.DOKill();
    this.transform.localScale = Vector3.one * 0.6f;
    this.transform.DOPunchScale(Vector3.one * 0.3f, 0.2f, 1);
    this.count.text = (Interaction_WolfBase.WolfTarget - Interaction_WolfBase.WolfFled - Interaction_WolfBase.WolfDied).ToString();
  }

  public void OnWolvesSucceeded()
  {
    this.checkBox.gameObject.SetActive(true);
    this.countParent.gameObject.SetActive(false);
    this.checkBox.AnimationState.SetAnimation(0, "turn-on", false);
    this.canvasGroup.DOFade(0.0f, 1f).SetDelay<TweenerCore<float, float, FloatOptions>>(3f).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => this.gameObject.SetActive(false)));
  }

  public void OnWolvesFailed()
  {
    this.checkBox.gameObject.SetActive(true);
    this.countParent.gameObject.SetActive(false);
    this.checkBox.AnimationState.SetAnimation(0, "turn-on-failed", false);
    this.canvasGroup.DOFade(0.0f, 1f).SetDelay<TweenerCore<float, float, FloatOptions>>(3f).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => this.gameObject.SetActive(false)));
  }

  [CompilerGenerated]
  public void \u003COnWolvesSucceeded\u003Eb__10_0() => this.gameObject.SetActive(false);

  [CompilerGenerated]
  public void \u003COnWolvesFailed\u003Eb__11_0() => this.gameObject.SetActive(false);
}
