// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Rituals.RitualItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI.Alerts;
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

public class RitualItem : MonoBehaviour
{
  public const string kUnlockedLayer = "Unlocked";
  public const string kLockedLayer = "Locked";
  public Action<UpgradeSystem.Type> OnRitualItemSelected;
  [SerializeField]
  public RectTransform _rectTransform;
  [SerializeField]
  public MMButton _button;
  [SerializeField]
  public RitualAlert _alert;
  [SerializeField]
  public Animator _animator;
  [SerializeField]
  public Image _ritualImage;
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
  public RitualIconMapping _iconMapping;
  public UpgradeSystem.Type _ritualType;
  public bool _locked;
  public bool _maxed;
  public bool _cooldown;
  public bool _canAfford;
  public bool _available = true;
  public bool _removedCost;
  public Vector2 _anchoredOrigin;
  [CompilerGenerated]
  public bool \u003CFree\u003Ek__BackingField;

  public UpgradeSystem.Type RitualType => this._ritualType;

  public bool Locked => this._locked;

  public bool Maxed => this._maxed;

  public bool Cooldown => this._cooldown;

  public bool Available => this._available;

  public RectTransform RectTransform => this._rectTransform;

  public MMButton Button => this._button;

  public bool Free
  {
    get => this.\u003CFree\u003Ek__BackingField;
    set => this.\u003CFree\u003Ek__BackingField = value;
  }

  public void Configure(UpgradeSystem.Type ritualType)
  {
    this._cooldownContainer.SetActive(false);
    this._ritualType = ritualType;
    this._locked = !CheatConsole.UnlockAllRituals && !DataManager.Instance.UnlockedUpgrades.Contains(this._ritualType);
    this._maxed = UpgradeSystem.IsUpgradeMaxed(this._ritualType);
    this._cooldown = (double) UpgradeSystem.GetCoolDownNormalised(this._ritualType) > 0.0;
    this._canAfford = UpgradeSystem.UserCanAffordUpgrade(this._ritualType);
    this._anchoredOrigin = this._iconContainer.anchoredPosition;
    this._button.onClick.AddListener(new UnityAction(this.OnRitualItemClicked));
    this._button.OnSelected += new System.Action(this.OnSelected);
    this._button.OnConfirmDenied += new System.Action(this.Shake);
    this._flashIcon.gameObject.SetActive(false);
    this._costText.isRightToLeftText = LocalizeIntegration.IsArabic();
    if (this.Free)
      this._canAfford = true;
    switch (ritualType)
    {
      case UpgradeSystem.Type.Ritual_BecomeDisciple:
        bool flag1 = false;
        foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
        {
          if (!FollowerManager.FollowerLocked(allBrain.Info.ID) && allBrain.Info.XPLevel >= 10 && !allBrain.Info.IsDisciple)
            flag1 = true;
        }
        this._available = flag1;
        break;
      case UpgradeSystem.Type.Ritual_Snowman:
        this._available = StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.SNOWMAN).Count > 0;
        break;
      case UpgradeSystem.Type.Ritual_Divorce:
        bool flag2 = false;
        foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
        {
          if (!FollowerManager.FollowerLocked(allBrain.Info.ID) && allBrain.Info.MarriedToLeader)
            flag2 = true;
        }
        this._available = flag2;
        break;
      case UpgradeSystem.Type.Ritual_RanchMeat:
      case UpgradeSystem.Type.Ritual_RanchHarvest:
        this._available = StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.RANCH).Count + StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.RANCH_2).Count > 0;
        break;
    }
    if ((UnityEngine.Object) this._alert != (UnityEngine.Object) null)
      this._alert.Configure(ritualType);
    if (!this._locked)
    {
      this._ritualImage.sprite = this._iconMapping.GetImage(this._ritualType);
      if (this._maxed)
        this._costText.text = ScriptLocalization.UI_Generic.Max.Colour(StaticColors.RedColor);
      else if (this._cooldown)
      {
        this._costText.text = ScriptLocalization.UI_Generic.Cooldown.Colour(StaticColors.RedColor);
        this._cooldownContainer.SetActive(true);
        this._cooldownFill.fillAmount = UpgradeSystem.GetCoolDownNormalised(this._ritualType);
        this._ritualImage.color = new Color(0.0f, 1f, 1f, 1f);
      }
      else
        this._costText.text = StructuresData.ItemCost.GetCostString(UpgradeSystem.GetCost(ritualType));
    }
    else
      this.SetAsLocked();
    this._button.Confirmable = !this._locked && this._canAfford && !this._cooldown && this._available;
    if (!this._removedCost)
      return;
    this._costText.text = "";
    this._button.Confirmable = true;
  }

  public void ForceIncognitoState()
  {
    if ((UnityEngine.Object) this._alert != (UnityEngine.Object) null)
      this._alert.gameObject.SetActive(false);
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
        this._cooldownContainer.SetActive(true);
      }
      else
        this._costText.text = StructuresData.ItemCost.GetCostString(UpgradeSystem.GetCost(this._ritualType));
    }));
  }

  public void ForceLockedState()
  {
    this.SetAsLocked();
    if (!((UnityEngine.Object) this._alert != (UnityEngine.Object) null))
      return;
    this._alert.gameObject.SetActive(false);
  }

  public IEnumerator DoUnlock()
  {
    this._iconContainer.DOScale(Vector3.one * 0.75f, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuad).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.25f);
    this.Configure(this._ritualType);
    this._animator.SetLayerWeight(this._animator.GetLayerIndex("Unlocked"), 1f);
    this._animator.SetLayerWeight(this._animator.GetLayerIndex("Locked"), 0.0f);
    this._ritualImage.color = new Color(1f, 1f, 1f, 1f);
    this._flashIcon.gameObject.SetActive(true);
    if ((UnityEngine.Object) this._alert != (UnityEngine.Object) null)
      this._alert.gameObject.SetActive(false);
    UIManager.PlayAudio("event:/unlock_building/unlock");
    this._iconContainer.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.25f);
    DOTweenModuleUI.DOColor(this._flashIcon, new Color(1f, 1f, 1f, 0.0f), 0.25f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.25f);
    yield return (object) new WaitForSecondsRealtime(0.1f);
    if ((UnityEngine.Object) this._alert != (UnityEngine.Object) null)
    {
      Vector3 one = Vector3.one;
      this._alert.transform.localScale = Vector3.zero;
      this._alert.transform.DOScale(one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBounce).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      this._alert.gameObject.SetActive(true);
      yield return (object) new WaitForSecondsRealtime(0.5f);
    }
  }

  public void SetMaxed()
  {
    this._maxed = true;
    this._costText.text = ScriptLocalization.UI_Generic.Max.Colour(StaticColors.RedColor);
  }

  public void OnRitualItemClicked()
  {
    if (!this._locked && !this._maxed && !this._cooldown && (this._canAfford || this.Free))
    {
      Action<UpgradeSystem.Type> ritualItemSelected = this.OnRitualItemSelected;
      if (ritualItemSelected == null)
        return;
      ritualItemSelected(this._ritualType);
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
    this._costText.text = "";
    this._animator.SetLayerWeight(this._animator.GetLayerIndex("Unlocked"), 0.0f);
    this._animator.SetLayerWeight(this._animator.GetLayerIndex("Locked"), 1f);
  }

  public void OnSelected()
  {
    if (!((UnityEngine.Object) this._alert != (UnityEngine.Object) null))
      return;
    this._alert.TryRemoveAlert();
  }

  public void RemoveCost()
  {
    this._costText.text = "";
    this._canAfford = true;
    this._button.Confirmable = true;
    this._removedCost = true;
  }

  [CompilerGenerated]
  public void \u003CAnimateIncognitoOut\u003Eb__42_0()
  {
    if (this._maxed)
      this._costText.text = ScriptLocalization.UI_Generic.Max.Colour(StaticColors.RedColor);
    else if (this._cooldown)
    {
      this._costText.text = ScriptLocalization.UI_Generic.Cooldown.Colour(StaticColors.RedColor);
      this._cooldownContainer.SetActive(true);
    }
    else
      this._costText.text = StructuresData.ItemCost.GetCostString(UpgradeSystem.GetCost(this._ritualType));
  }
}
