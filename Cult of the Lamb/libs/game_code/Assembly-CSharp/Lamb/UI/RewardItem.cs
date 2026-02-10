// Decompiled with JetBrains decompiler
// Type: Lamb.UI.RewardItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class RewardItem : MonoBehaviour
{
  [SerializeField]
  public RectTransform _container;
  [SerializeField]
  public GameObject _dropShadow;
  [Header("Selected")]
  [SerializeField]
  public CanvasGroup _selectedContainerCanvasGroup;
  [SerializeField]
  public RectTransform _selectedContainer;
  [SerializeField]
  public GameObject _tick;
  [Header("Available")]
  [SerializeField]
  public GameObject _availableContainer;
  [Header("Unlocked")]
  [SerializeField]
  public GameObject _godTear;
  [SerializeField]
  public GameObject _unlockedContainer;
  [Header("Locked")]
  [SerializeField]
  public GameObject _lockedContainer;
  [Header("Incognito")]
  [SerializeField]
  public Image _godTearImage;
  [SerializeField]
  public GameObject _incognitoContainer;
  public RewardItem.RewardState _state;
  public MMUILineRenderer.Branch _progressionBranch;
  public Tween _lineTween;

  public void Configure(
    ScenarioData scenarioData,
    DungeonSandboxManager.ProgressionSnapshot progressionSnapshot,
    bool left)
  {
    this._selectedContainerCanvasGroup.alpha = 0.0f;
    this.RemoveIncognitoMode();
    this._tick.SetActive(false);
    if (DataManager.Instance.UnlockedFleeces.Contains((int) progressionSnapshot.FleeceType))
    {
      if (progressionSnapshot.FleeceType != PlayerFleeceManager.FleeceType.Default && !DungeonSandboxManager.HasFinishedAnyWithDefaultFleece())
        this.SetAsHidden();
      else if (progressionSnapshot.CompletedRuns == scenarioData.ScenarioIndex)
        this.SetAsAvailable();
      else if (progressionSnapshot.CompletedRuns > scenarioData.ScenarioIndex)
        this.SetAsUnlocked();
      else
        this.SetAsHidden();
    }
    else if (scenarioData.ScenarioIndex == 0 && DungeonSandboxManager.HasFinishedAnyWithDefaultFleece())
      this.SetAsLocked();
    else
      this.SetAsHidden();
    if (left)
      this._container.pivot = new Vector2(1f, 0.5f);
    else
      this._container.pivot = new Vector2(0.0f, 0.5f);
  }

  public void ConfigureLine(MMUILineRenderer.Branch branch)
  {
    if (this._state == RewardItem.RewardState.Locked)
      branch.Color = StaticColors.DarkGreyColor;
    else if (this._state == RewardItem.RewardState.Unlocked)
      branch.Color = StaticColors.RedColor;
    else if (this._state == RewardItem.RewardState.Hidden)
      branch.Color = new Color(1f, 1f, 1f, 0.0f);
    else
      branch.Color = StaticColors.GreenColor;
  }

  public void ConfigureSecondaryLine(MMUILineRenderer.Branch branch)
  {
    this._progressionBranch = branch;
    this._progressionBranch.Color = StaticColors.OffWhiteColor;
    this._progressionBranch.Fill = 0.0f;
  }

  public void SetAsLocked()
  {
    this._state = RewardItem.RewardState.Locked;
    this._availableContainer.SetActive(false);
    this._unlockedContainer.SetActive(false);
    this._lockedContainer.SetActive(true);
  }

  public void SetAsAvailable()
  {
    this._state = RewardItem.RewardState.Unlocked;
    this._availableContainer.SetActive(true);
    this._unlockedContainer.SetActive(false);
    this._lockedContainer.SetActive(false);
  }

  public void SetAsHidden()
  {
    this._dropShadow.SetActive(false);
    this._state = RewardItem.RewardState.Hidden;
    this._godTear.SetActive(false);
    this._availableContainer.SetActive(false);
    this._unlockedContainer.SetActive(false);
    this._lockedContainer.SetActive(false);
  }

  public void SetAsUnlocked()
  {
    this._state = RewardItem.RewardState.Obtained;
    this._availableContainer.SetActive(false);
    this._unlockedContainer.SetActive(true);
    this._lockedContainer.SetActive(false);
    this._godTear.SetActive(false);
    this._tick.SetActive(true);
  }

  public void SetIncognitoMode()
  {
    if (this._state == RewardItem.RewardState.Hidden)
      return;
    this._godTearImage.color = new Color(0.0f, 1f, 1f, 1f);
    this._incognitoContainer.SetActive(true);
  }

  public void RemoveIncognitoMode()
  {
    this._godTearImage.color = new Color(1f, 1f, 1f, 1f);
    this._incognitoContainer.SetActive(false);
  }

  public void Highlight()
  {
    if (this._lineTween != null)
      this._lineTween.Kill();
    this._lineTween = (Tween) DOTween.To((DOGetter<float>) (() => this._progressionBranch.Fill), (DOSetter<float>) (f => this._progressionBranch.Fill = f), 0.65f, 0.15f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.Linear).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    this._container.DOKill();
    this._container.DOScale(1.5f, 0.3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(0.15f);
    this._selectedContainerCanvasGroup.DOKill();
    this._selectedContainerCanvasGroup.DOFade(1f, 0.3f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.Linear).SetUpdate<TweenerCore<float, float, FloatOptions>>(true).SetDelay<TweenerCore<float, float, FloatOptions>>(0.15f);
  }

  public void UnHighlight()
  {
    this._container.DOKill();
    this._container.DOScale(1f, 0.3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    this._selectedContainerCanvasGroup.DOKill();
    this._selectedContainerCanvasGroup.DOFade(0.0f, 0.3f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.Linear).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    if (this._lineTween != null)
      this._lineTween.Kill();
    this._lineTween = (Tween) DOTween.To((DOGetter<float>) (() => this._progressionBranch.Fill), (DOSetter<float>) (f => this._progressionBranch.Fill = f), 0.0f, 0.15f).SetDelay<TweenerCore<float, float, FloatOptions>>(0.3f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.Linear).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
  }

  [CompilerGenerated]
  public float \u003CHighlight\u003Eb__24_0() => this._progressionBranch.Fill;

  [CompilerGenerated]
  public void \u003CHighlight\u003Eb__24_1(float f) => this._progressionBranch.Fill = f;

  [CompilerGenerated]
  public float \u003CUnHighlight\u003Eb__25_0() => this._progressionBranch.Fill;

  [CompilerGenerated]
  public void \u003CUnHighlight\u003Eb__25_1(float f) => this._progressionBranch.Fill = f;

  public enum RewardState
  {
    Locked,
    Unlocked,
    Obtained,
    Hidden,
  }
}
