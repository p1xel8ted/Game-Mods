// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Rituals.CultUpgradeItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI.Assets;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI.Rituals;

public class CultUpgradeItem : MonoBehaviour
{
  public const string kUnlockedLayer = "Unlocked";
  public const string kLockedLayer = "Locked";
  public Action<CultUpgradeData.TYPE> OnCultUpgradeItemSelected;
  [SerializeField]
  public MMButton _button;
  [SerializeField]
  public Animator _animator;
  [SerializeField]
  public Image _borderImage;
  [SerializeField]
  public Image _ritualImage;
  [SerializeField]
  public Image _defaultImage;
  [SerializeField]
  public Image _currentBorder;
  [SerializeField]
  public TextMeshProUGUI _costText;
  [SerializeField]
  public RectTransform _iconContainer;
  [SerializeField]
  public GameObject _cooldownContainer;
  [SerializeField]
  public Image _cooldownFill;
  [SerializeField]
  public Image _flashIcon;
  [SerializeField]
  public CultUpgradeIconMapping _iconMapping;
  [SerializeField]
  public GameObject _alert;
  [SerializeField]
  public FollowerLocation _requiredDungeon;
  public CultUpgradeData.TYPE _cultUpgradeType;
  public bool _locked;
  public bool _unlocked;
  public bool _maxed;
  public bool _cooldown;
  public bool _canAfford;
  public bool selectionAlwaysInvalid;
  public Vector2 _anchoredOrigin;
  public bool isBorderSelectionItem;
  public bool DoingUnlock;

  public CultUpgradeData.TYPE CultUpgradeType => this._cultUpgradeType;

  public bool Locked => this._locked;

  public bool Unlocked => this._unlocked;

  public bool Maxed => this._maxed;

  public bool Cooldown => this._cooldown;

  public MMButton Button => this._button;

  public void Configure(CultUpgradeData.TYPE cultUpgradeType)
  {
    this._button.onClick.AddListener(new UnityAction(this.OnRitualItemClicked));
    this._button.OnSelected += new System.Action(this.OnSelected);
    this._button.OnConfirmDenied += new System.Action(this.Shake);
    this.Init(cultUpgradeType, true);
  }

  public void FadeIconImages(float fadeValue = 0.25f)
  {
    this._ritualImage.color = this._ritualImage.color with
    {
      a = fadeValue
    };
  }

  public void Init(CultUpgradeData.TYPE cultUpgradeType, bool faded = false)
  {
    this._cultUpgradeType = cultUpgradeType;
    if ((UnityEngine.Object) this._cooldownContainer != (UnityEngine.Object) null)
      this._cooldownContainer.SetActive(false);
    this._locked = !CheatConsole.UnlockAllRituals && !CultUpgradeData.IsUnlocked(this._cultUpgradeType);
    if (!this._locked && CultUpgradeData.IsBorder(this._cultUpgradeType))
      this._unlocked = true;
    if (this._requiredDungeon != FollowerLocation.None && DataManager.Instance.DungeonCompleted(this._requiredDungeon))
      this._unlocked = true;
    if (!CultUpgradeData.IsBorder(this._cultUpgradeType))
    {
      this._maxed = CultUpgradeData.IsUpgradeMaxed();
      if (this._maxed)
        this._cultUpgradeType = CultUpgradeData.TYPE.None;
    }
    else
      this._maxed = false;
    this._cooldown = (double) CultUpgradeData.GetCoolDownNormalised(this._cultUpgradeType) > 0.0;
    this._canAfford = CultUpgradeData.UserCanAffordUpgrade(this._cultUpgradeType);
    this._anchoredOrigin = this._iconContainer.anchoredPosition;
    this._flashIcon.gameObject.SetActive(false);
    this._alert.gameObject.SetActive(false);
    bool flag = !this._locked;
    if (this._locked & faded)
    {
      this.gameObject.SetActive(true);
      flag = true;
    }
    if (flag)
    {
      if ((UnityEngine.Object) this._borderImage != (UnityEngine.Object) null)
      {
        this._defaultImage.enabled = false;
        this._ritualImage.enabled = true;
        int a = Mathf.Min((int) cultUpgradeType, 10);
        int num1 = Mathf.Max(a, 1);
        int num2 = num1;
        Debug.Log((object) $"Icon display level is {a.ToString()} {num1.ToString()} {num2.ToString()}");
        this._ritualImage.sprite = this._iconMapping.GetImage((CultUpgradeData.TYPE) num2);
        this._borderImage.enabled = DataManager.Instance.TempleBorder + 1 >= 100;
        this._borderImage.sprite = this._iconMapping.GetImage((CultUpgradeData.TYPE) Mathf.Max(DataManager.Instance.TempleBorder, 100));
      }
      else
        this._ritualImage.sprite = this._iconMapping.GetImage(this._cultUpgradeType);
      if (this._unlocked)
      {
        if ((UnityEngine.Object) this._cooldownContainer != (UnityEngine.Object) null)
          this._cooldownContainer.SetActive(false);
        this._costText.text = "";
        if (!this.isBorderSelectionItem)
          this._ritualImage.color = new Color(1f, 1f, 1f, 1f);
      }
      else if (this._maxed)
        this._costText.text = ScriptLocalization.UI_Generic.Max.Colour(StaticColors.RedColor);
      else if (this._cooldown)
      {
        this._costText.text = "";
        if ((UnityEngine.Object) this._cooldownContainer != (UnityEngine.Object) null)
          this._cooldownContainer.SetActive(true);
        if ((bool) (UnityEngine.Object) this._cooldownFill)
          this._cooldownFill.fillAmount = CultUpgradeData.GetCoolDownNormalised(this._cultUpgradeType);
        this._ritualImage.color = new Color(0.0f, 1f, 1f, 1f);
      }
      else
        this._costText.text = "";
    }
    else
      this.SetAsLocked();
    if ((UnityEngine.Object) this._currentBorder != (UnityEngine.Object) null)
      this._currentBorder.enabled = (UnityEngine.Object) this._borderImage == (UnityEngine.Object) null && this._cultUpgradeType == (CultUpgradeData.TYPE) DataManager.Instance.TempleBorder;
    this._button.Confirmable = !this._locked && this._canAfford && !this._cooldown;
    if (!this._unlocked)
      return;
    this._button.Confirmable = true;
  }

  public void ForceIncognitoState()
  {
    this._costText.text = "";
    this._ritualImage.color = new Color(0.0f, 1f, 1f, 1f);
    this._flashIcon.gameObject.SetActive(true);
    this._flashIcon.color = new Color(0.0f, 0.0f, 0.0f, 0.5f);
  }

  public void AnimateIncognitoOut()
  {
    this._ritualImage.DOKill();
    DOTweenModuleUI.DOColor(this._ritualImage, Color.white, 0.25f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    this._flashIcon.DOKill();
    DOTweenModuleUI.DOFade(this._flashIcon, 0.0f, 0.25f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true).OnComplete<TweenerCore<Color, Color, ColorOptions>>((TweenCallback) (() =>
    {
      if (this._maxed)
        this._costText.text = ScriptLocalization.UI_Generic.Max.Colour(StaticColors.RedColor);
      else if (this._cooldown)
      {
        this._costText.text = ScriptLocalization.UI_Generic.Cooldown.Colour(StaticColors.RedColor);
        if (!((UnityEngine.Object) this._cooldownContainer != (UnityEngine.Object) null))
          return;
        this._cooldownContainer.SetActive(true);
      }
      else
        this._costText.text = StructuresData.ItemCost.GetCostString(CultUpgradeData.GetCost(this._cultUpgradeType));
    }));
  }

  public void ForceLockedState() => this.SetAsLocked();

  public IEnumerator DoUnlock(CultUpgradeData.TYPE cultUpgradeType, float delay = 1.5f)
  {
    yield return (object) new WaitForSeconds(delay);
    this.DoingUnlock = true;
    this._iconContainer.DOKill();
    this._iconContainer.DOScale(Vector3.one * 0.75f, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuad).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.25f);
    this.Init(cultUpgradeType, true);
    this._animator.SetLayerWeight(this._animator.GetLayerIndex("Unlocked"), 1f);
    this._animator.SetLayerWeight(this._animator.GetLayerIndex("Locked"), 0.0f);
    this._ritualImage.color = new Color(1f, 1f, 1f, 1f);
    this._flashIcon.color = Color.white;
    this._flashIcon.gameObject.SetActive(true);
    UIManager.PlayAudio("event:/unlock_building/unlock");
    this._iconContainer.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.25f);
    DOTweenModuleUI.DOColor(this._flashIcon, new Color(1f, 1f, 1f, 0.0f), 0.25f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.25f);
    this.DoingUnlock = false;
  }

  public void SetMaxed()
  {
    this._maxed = true;
    this._costText.text = ScriptLocalization.UI_Generic.Max.Colour(StaticColors.RedColor);
  }

  public void OnRitualItemClicked()
  {
    if (this.DoingUnlock)
      return;
    if (!this._locked && !this._maxed && !this._cooldown)
    {
      Action<CultUpgradeData.TYPE> upgradeItemSelected = this.OnCultUpgradeItemSelected;
      if (upgradeItemSelected == null)
        return;
      upgradeItemSelected(this._cultUpgradeType);
    }
    else
    {
      if (!this._maxed)
        return;
      this.Shake();
    }
  }

  public void Shake()
  {
    this._iconContainer.DOKill();
    this._iconContainer.anchoredPosition = this._anchoredOrigin;
    this._iconContainer.DOShakePosition(1f, new Vector3(10f, 0.0f)).SetUpdate<Tweener>(true);
  }

  public void SetAsLocked()
  {
    if (CultUpgradeData.IsBorder(this._cultUpgradeType))
      this._costText.text = "";
    this._animator.SetLayerWeight(this._animator.GetLayerIndex("Unlocked"), 0.0f);
    this._animator.SetLayerWeight(this._animator.GetLayerIndex("Locked"), 1f);
  }

  public void OnSelected()
  {
    Debug.Log((object) ("Selected? " + Time.realtimeSinceStartup.ToString()));
  }

  [CompilerGenerated]
  public void \u003CAnimateIncognitoOut\u003Eb__42_0()
  {
    if (this._maxed)
      this._costText.text = ScriptLocalization.UI_Generic.Max.Colour(StaticColors.RedColor);
    else if (this._cooldown)
    {
      this._costText.text = ScriptLocalization.UI_Generic.Cooldown.Colour(StaticColors.RedColor);
      if (!((UnityEngine.Object) this._cooldownContainer != (UnityEngine.Object) null))
        return;
      this._cooldownContainer.SetActive(true);
    }
    else
      this._costText.text = StructuresData.ItemCost.GetCostString(CultUpgradeData.GetCost(this._cultUpgradeType));
  }
}
