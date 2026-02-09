// Decompiled with JetBrains decompiler
// Type: AnimationAdorationUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

#nullable disable
public class AnimationAdorationUI : MonoBehaviour
{
  [SerializeField]
  public TMP_Text levelText;
  public bool IsPlaying;
  public Interaction_Ranchable animal;
  public GameObject BarContainer;
  public GameObject CompleteContainer;
  public BarControllerNonUI bc;
  public BarControllerNonUI BarController;
  public DG.Tweening.Sequence Sequence;

  public void Awake() => this.BarContainer.gameObject.SetActive(false);

  public void Start()
  {
    this.BarController.SetBarSize(this.animal.Adoration / 100f, false);
    this.transform.localScale = Vector3.zero;
    this.AddListener();
  }

  public void AddListener()
  {
    this.levelText.text = this.animal.Level.ToNumeral();
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
  }

  public void Show(bool animate = true)
  {
    if (this.IsPlaying || !DataManager.Instance.ShowLoyaltyBars)
      return;
    this.EnableBarContainerGameobject();
    this.levelText.text = this.animal.Level.ToNumeral();
    this.transform.DOKill();
    this.BarController.SetBarSize(this.animal.Adoration / 100f, false);
    if (animate)
    {
      this.transform.localScale = Vector3.zero;
      this.transform.DOScale(Vector3.one * 0.7f, 0.3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    }
    else
      this.transform.localScale = Vector3.one * 0.7f;
    this.SetObjects();
  }

  public void SetObjects()
  {
    this.EnableBarContainerGameobject();
    this.BarContainer.SetActive(true);
  }

  public void Hide(bool animate = true)
  {
    if (this.IsPlaying || this.transform.localScale == Vector3.zero)
      return;
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

  public IEnumerator IncreaseAdorationIE()
  {
    AnimationAdorationUI animationAdorationUi = this;
    int currentLevel = animationAdorationUi.animal.Level;
    float currentAdoration = animationAdorationUi.animal.Adoration;
    if (animationAdorationUi.transform.localScale != Vector3.one * 0.7f)
    {
      animationAdorationUi.transform.localScale = Vector3.zero;
      animationAdorationUi.transform.DOScale(Vector3.one * 0.7f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    }
    animationAdorationUi.EnableBarContainerGameobject();
    animationAdorationUi.IsPlaying = true;
    yield return (object) new WaitForSeconds(1f);
    animationAdorationUi.BarController.SetBarSize(currentAdoration / 100f, true, true);
    yield return (object) new WaitForSeconds(2f);
    if ((double) currentAdoration >= 100.0)
    {
      animationAdorationUi.levelText.text = (currentLevel + 1).ToNumeral();
      animationAdorationUi.levelText.transform.DOPunchScale(Vector3.one * 0.2f, 0.2f);
      BiomeConstants.Instance.EmitHeartPickUpVFX(animationAdorationUi.animal.transform.position, 0.0f, "red", "burst_big");
      yield return (object) new WaitForSeconds(1f);
      animationAdorationUi.BarController.SetBarSize(0.0f, true, true);
      yield return (object) new WaitForSeconds(1f);
    }
    yield return (object) new WaitForSeconds(1f);
    animationAdorationUi.transform.DOScale(Vector3.zero, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack);
    yield return (object) new WaitForSeconds(1f);
    animationAdorationUi.IsPlaying = false;
    animationAdorationUi.Hide(false);
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
  public void \u003CHide\u003Eb__12_0() => this.BarContainer.gameObject.SetActive(false);
}
