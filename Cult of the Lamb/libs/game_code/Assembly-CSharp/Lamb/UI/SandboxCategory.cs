// Decompiled with JetBrains decompiler
// Type: Lamb.UI.SandboxCategory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class SandboxCategory : UISubmenuBase
{
  public const float kTransitionDuration = 0.35f;
  public Action<PlayerFleeceManager.FleeceType> OnFleeceSelected;
  public Action<ScenarioData> OnScenarioChosen;
  [Header("Scenario")]
  [SerializeField]
  public DungeonSandboxManager.ScenarioType _scenarioType;
  [SerializeField]
  public TMP_Text _scenarioHeader;
  [SerializeField]
  public TMP_Text _scenarioDescription;
  [SerializeField]
  public RectTransform _rectTransform;
  [SerializeField]
  public Color _backgroundColor;
  [SerializeField]
  public RewardPath[] _rewardPaths;
  [SerializeField]
  public GameObject _dotsContainer;
  [Header("Fleeces")]
  [SerializeField]
  public TMP_Text _fleeceHeader;
  [SerializeField]
  public TMP_Text _fleeceDescription;
  [SerializeField]
  public TMP_Text _fleeceLocked;
  [Header("XP Bar")]
  [SerializeField]
  public UIGodTearBar xpbar;
  public SandboxCategory.TransitionType _transitionType;

  public void Start()
  {
    this._scenarioHeader.text = LocalizationManager.GetTranslation($"UI/SandboxMenu/{this._scenarioType}");
    this._scenarioDescription.text = LocalizationManager.GetTranslation($"UI/SandboxMenu/{this._scenarioType}/Description");
    this._fleeceHeader.text = string.Empty;
    this._fleeceDescription.text = string.Empty;
    this._fleeceLocked.gameObject.SetActive(false);
    this.xpbar.gameObject.SetActive(false);
    foreach (RewardPath rewardPath in this._rewardPaths)
    {
      rewardPath.OnSelected += (Action<PlayerFleeceManager.FleeceType>) (fleece =>
      {
        Action<PlayerFleeceManager.FleeceType> onFleeceSelected = this.OnFleeceSelected;
        if (onFleeceSelected != null)
          onFleeceSelected(fleece);
        int num = (int) fleece;
        if (DataManager.Instance.UnlockedFleeces.Contains(num))
        {
          this.xpbar.gameObject.SetActive(true);
          this.xpbar.Show((float) DataManager.Instance.CurrentChallengeModeXP);
          this._fleeceHeader.gameObject.SetActive(true);
          this._fleeceDescription.gameObject.SetActive(true);
          this._fleeceLocked.gameObject.SetActive(false);
          this._fleeceHeader.text = LocalizationManager.GetTranslation($"TarotCards/Fleece{num}/Name");
          this._fleeceDescription.text = LocalizationManager.GetTranslation($"TarotCards/Fleece{num}/Description");
          if (!SettingsManager.Settings.Accessibility.DyslexicFont)
            return;
          this._fleeceDescription.rectTransform.sizeDelta = this._fleeceDescription.rectTransform.sizeDelta with
          {
            x = 840f
          };
        }
        else
        {
          this.xpbar.gameObject.SetActive(true);
          this.xpbar.Hide();
          this._fleeceHeader.gameObject.SetActive(false);
          this._fleeceDescription.gameObject.SetActive(false);
          this._fleeceLocked.gameObject.SetActive(true);
        }
      });
      rewardPath.OnConfirmed += (Action<ScenarioData>) (scenario =>
      {
        Action<ScenarioData> onScenarioChosen = this.OnScenarioChosen;
        if (onScenarioChosen == null)
          return;
        onScenarioChosen(scenario);
      });
      rewardPath.Configure(DungeonSandboxManager.GetDataForScenarioType(this._scenarioType));
    }
  }

  public void SetIncognitoMode()
  {
    foreach (RewardPath rewardPath in this._rewardPaths)
      rewardPath.SetIncognitoMode();
  }

  public void RemoveIncognitoMode()
  {
    foreach (RewardPath rewardPath in this._rewardPaths)
      rewardPath.RemoveIncognitoMode();
  }

  public void ShowDots() => this._dotsContainer.SetActive(true);

  public void HideDots() => this._dotsContainer.SetActive(false);

  public void Show(SandboxCategory.TransitionType transitionType, bool immediate = false)
  {
    this._transitionType = transitionType;
    this.Show(immediate);
  }

  public void Hide(SandboxCategory.TransitionType transitionType, bool immediate = false)
  {
    this._transitionType = transitionType;
    this.Hide(immediate);
  }

  public override IEnumerator DoShowAnimation()
  {
    SandboxCategory sandboxCategory = this;
    sandboxCategory._canvasGroup.DOKill();
    sandboxCategory._rectTransform.DOKill();
    sandboxCategory._canvasGroup.alpha = 0.0f;
    sandboxCategory._canvasGroup.DOFade(1f, 0.35f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    if (sandboxCategory._transitionType == SandboxCategory.TransitionType.MoveLeft)
    {
      sandboxCategory._rectTransform.localPosition = (Vector3) new Vector2(100f, 0.0f);
      sandboxCategory._rectTransform.DOLocalMove(Vector3.zero, 0.35f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
    }
    else if (sandboxCategory._transitionType == SandboxCategory.TransitionType.MoveRight)
    {
      sandboxCategory._rectTransform.localPosition = (Vector3) new Vector2(-100f, 0.0f);
      sandboxCategory._rectTransform.DOLocalMove(Vector3.zero, 0.35f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
    }
    else
    {
      sandboxCategory._rectTransform.localScale = (Vector3) (Vector2.one * 1.15f);
      sandboxCategory._rectTransform.DOScale(1f, 0.35f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    }
    yield return (object) new WaitForSecondsRealtime(0.35f);
  }

  public override IEnumerator DoHideAnimation()
  {
    SandboxCategory sandboxCategory = this;
    sandboxCategory._canvasGroup.DOKill();
    sandboxCategory._rectTransform.DOKill();
    sandboxCategory._canvasGroup.DOFade(0.0f, 0.35f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    if (sandboxCategory._transitionType == SandboxCategory.TransitionType.MoveLeft)
    {
      sandboxCategory._rectTransform.localPosition = (Vector3) Vector2.zero;
      sandboxCategory._rectTransform.DOLocalMove((Vector3) new Vector2(-100f, 0.0f), 0.35f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine);
    }
    else if (sandboxCategory._transitionType == SandboxCategory.TransitionType.MoveRight)
    {
      sandboxCategory._rectTransform.localPosition = (Vector3) Vector2.zero;
      sandboxCategory._rectTransform.DOLocalMove((Vector3) new Vector2(100f, 0.0f), 0.35f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine);
    }
    else
      sandboxCategory._rectTransform.DOScale(0.85f, 0.35f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.35f);
  }

  public void SetCurrentSelectable(SandboxFleeceItem sandboxFleeceItem)
  {
    this.SetCurrentSelectable((PlayerFleeceManager.FleeceType) sandboxFleeceItem.FleeceIndex);
  }

  public void SetCurrentSelectable(PlayerFleeceManager.FleeceType fleeceType)
  {
    foreach (RewardPath rewardPath in this._rewardPaths)
    {
      if (rewardPath.Fleece == fleeceType)
      {
        this.OverrideDefault((Selectable) rewardPath.FleeceItem.Button);
        return;
      }
    }
    this.OverrideDefault((Selectable) this._rewardPaths[0].FleeceItem.Button);
  }

  public void Activate() => this.ActivateNavigation();

  [CompilerGenerated]
  public void \u003CStart\u003Eb__16_0(PlayerFleeceManager.FleeceType fleece)
  {
    Action<PlayerFleeceManager.FleeceType> onFleeceSelected = this.OnFleeceSelected;
    if (onFleeceSelected != null)
      onFleeceSelected(fleece);
    int num = (int) fleece;
    if (DataManager.Instance.UnlockedFleeces.Contains(num))
    {
      this.xpbar.gameObject.SetActive(true);
      this.xpbar.Show((float) DataManager.Instance.CurrentChallengeModeXP);
      this._fleeceHeader.gameObject.SetActive(true);
      this._fleeceDescription.gameObject.SetActive(true);
      this._fleeceLocked.gameObject.SetActive(false);
      this._fleeceHeader.text = LocalizationManager.GetTranslation($"TarotCards/Fleece{num}/Name");
      this._fleeceDescription.text = LocalizationManager.GetTranslation($"TarotCards/Fleece{num}/Description");
      if (!SettingsManager.Settings.Accessibility.DyslexicFont)
        return;
      this._fleeceDescription.rectTransform.sizeDelta = this._fleeceDescription.rectTransform.sizeDelta with
      {
        x = 840f
      };
    }
    else
    {
      this.xpbar.gameObject.SetActive(true);
      this.xpbar.Hide();
      this._fleeceHeader.gameObject.SetActive(false);
      this._fleeceDescription.gameObject.SetActive(false);
      this._fleeceLocked.gameObject.SetActive(true);
    }
  }

  [CompilerGenerated]
  public void \u003CStart\u003Eb__16_1(ScenarioData scenario)
  {
    Action<ScenarioData> onScenarioChosen = this.OnScenarioChosen;
    if (onScenarioChosen == null)
      return;
    onScenarioChosen(scenario);
  }

  public enum TransitionType
  {
    Scale,
    MoveLeft,
    MoveRight,
  }
}
