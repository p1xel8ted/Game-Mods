// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Rituals.RitualItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI.Alerts;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI.Rituals;

public class RitualItem : MonoBehaviour
{
  private const string kUnlockedLayer = "Unlocked";
  private const string kLockedLayer = "Locked";
  public Action<UpgradeSystem.Type> OnRitualItemSelected;
  [SerializeField]
  private MMButton _button;
  [SerializeField]
  private RitualAlert _alert;
  [SerializeField]
  private Animator _animator;
  [SerializeField]
  private Image _ritualImage;
  [SerializeField]
  private TextMeshProUGUI _costText;
  [SerializeField]
  private RectTransform _iconContainer;
  [SerializeField]
  private GameObject _cooldownContainer;
  [SerializeField]
  private Image _cooldownFill;
  [SerializeField]
  private Image _flashIcon;
  private UpgradeSystem.Type _ritualType;
  private bool _locked;
  private bool _maxed;
  private bool _cooldown;
  private bool _canAfford;
  private Vector2 _anchoredOrigin;

  public UpgradeSystem.Type RitualType => this._ritualType;

  public bool Locked => this._locked;

  public bool Maxed => this._maxed;

  public bool Cooldown => this._cooldown;

  public MMButton Button => this._button;

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
    if ((UnityEngine.Object) this._alert != (UnityEngine.Object) null)
      this._alert.Configure(ritualType);
    if (!this._locked)
    {
      this._ritualImage.sprite = DoctrineUpgradeSystem.GetIconForRitual(this._ritualType);
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
    this._button.Confirmable = !this._locked && this._canAfford && !this._cooldown;
  }

  public void ForceIncognitoState()
  {
    this._alert.gameObject.SetActive(false);
    this._costText.text = "";
    this._ritualImage.color = new Color(0.0f, 1f, 1f, 1f);
    this._flashIcon.gameObject.SetActive(true);
    this._flashIcon.color = new Color(0.0f, 0.0f, 0.0f, 0.5f);
  }

  public void ForceLockedState()
  {
    this.SetAsLocked();
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
    this._alert.gameObject.SetActive(false);
    UIManager.PlayAudio("event:/unlock_building/unlock");
    this._iconContainer.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.25f);
    DOTweenModuleUI.DOColor(this._flashIcon, new Color(1f, 1f, 1f, 0.0f), 0.25f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.25f);
    yield return (object) new WaitForSecondsRealtime(0.1f);
    Vector3 one = Vector3.one;
    this._alert.transform.localScale = Vector3.zero;
    this._alert.transform.DOScale(one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBounce).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    this._alert.gameObject.SetActive(true);
    yield return (object) new WaitForSecondsRealtime(0.5f);
  }

  public void SetMaxed()
  {
    this._maxed = true;
    this._costText.text = ScriptLocalization.UI_Generic.Max.Colour(StaticColors.RedColor);
  }

  private void OnRitualItemClicked()
  {
    if (!this._locked && !this._maxed && this._canAfford && !this._cooldown)
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

  private void Shake()
  {
    this._iconContainer.DOKill();
    this._iconContainer.anchoredPosition = this._anchoredOrigin;
    this._iconContainer.DOShakePosition(1f, new Vector3(10f, 0.0f)).SetUpdate<Tweener>(true);
  }

  private void SetAsLocked()
  {
    this._costText.text = "";
    this._animator.SetLayerWeight(this._animator.GetLayerIndex("Unlocked"), 0.0f);
    this._animator.SetLayerWeight(this._animator.GetLayerIndex("Locked"), 1f);
  }

  private void OnSelected()
  {
    if (!((UnityEngine.Object) this._alert != (UnityEngine.Object) null))
      return;
    this._alert.TryRemoveAlert();
  }
}
