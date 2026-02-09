// Decompiled with JetBrains decompiler
// Type: CurrentCurse
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class CurrentCurse : MonoBehaviour
{
  public EquipmentType Curse = EquipmentType.None;
  public int Level;
  [SerializeField]
  public Image icon;
  [SerializeField]
  public Image Background;
  [SerializeField]
  public TextMeshProUGUI LevelText;
  public float scaleMultiplier = 2f;
  public PlayerFarming playerFarming;

  public void Awake() => PlayerSpells.OnCurseChanged += new PlayerSpells.CurseEvent(this.SetCurse);

  public void Init(PlayerFarming playerFarmingVar)
  {
    this.playerFarming = playerFarmingVar;
    if (!DataManager.Instance.EnabledSpells)
      this.gameObject.SetActive(false);
    else if ((UnityEngine.Object) this.playerFarming == (UnityEngine.Object) null || this.playerFarming.currentCurse == EquipmentType.None || !GameManager.IsDungeon(PlayerFarming.Location) || PlayerFarming.Location == FollowerLocation.IntroDungeon)
      this.transform.localScale = Vector3.zero;
    else
      this.transform.localScale = Vector3.one;
  }

  public void OnEnable()
  {
    Interaction_WeaponSelectionPodium.OnHighlightCurse += new Action<bool, PlayerFarming>(this.HighlightCurse);
    Singleton<AccessibilityManager>.Instance.OnRomanNumeralsChanged += new Action<bool>(this.OnRomanNumeralSettingChanged);
  }

  public void OnDisable()
  {
    Interaction_WeaponSelectionPodium.OnHighlightCurse -= new Action<bool, PlayerFarming>(this.HighlightCurse);
    Singleton<AccessibilityManager>.Instance.OnRomanNumeralsChanged -= new Action<bool>(this.OnRomanNumeralSettingChanged);
  }

  public void OnDestroy()
  {
    PlayerSpells.OnCurseChanged -= new PlayerSpells.CurseEvent(this.SetCurse);
    Interaction_WeaponSelectionPodium.OnHighlightCurse -= new Action<bool, PlayerFarming>(this.HighlightCurse);
    Singleton<AccessibilityManager>.Instance.OnRomanNumeralsChanged -= new Action<bool>(this.OnRomanNumeralSettingChanged);
  }

  public void HighlightCurse(bool Toggle, PlayerFarming p)
  {
    if ((UnityEngine.Object) this.playerFarming != (UnityEngine.Object) p || this.Curse == EquipmentType.None)
      return;
    this.Background.transform.DOKill();
    this.transform.DOKill();
    this.Background.enabled = Toggle;
    if (this.Background.enabled)
    {
      this.Background.transform.localScale = Vector3.one * 1.1f;
      this.Background.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
      this.transform.DOScale(Vector3.one * 1.1f * this.scaleMultiplier, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    }
    else
      this.transform.DOScale(Vector3.one * this.scaleMultiplier, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
  }

  public void OnRomanNumeralSettingChanged(bool state)
  {
    this.LevelText.text = this.Level.ToNumeral();
  }

  public void SetCurse(EquipmentType curse, int level, PlayerFarming _playerFarming)
  {
    if ((UnityEngine.Object) this.playerFarming == (UnityEngine.Object) null || (UnityEngine.Object) this.playerFarming.gameObject == (UnityEngine.Object) null || (UnityEngine.Object) this.playerFarming != (UnityEngine.Object) _playerFarming)
      return;
    curse = this.playerFarming.currentCurse;
    level = this.playerFarming.currentCurseLevel;
    this.SetCurse(curse, level, true);
  }

  public void SetCurse(EquipmentType curse, int level, bool punch)
  {
    if (DataManager.Instance.EnabledSpells)
      this.gameObject.SetActive(true);
    this.Curse = curse;
    this.Level = level;
    this.icon.enabled = curse != EquipmentType.None;
    this.Background.enabled = curse != EquipmentType.None;
    this.LevelText.text = level.ToNumeral();
    this.LevelText.isRightToLeftText = LocalizeIntegration.IsArabic();
    if (curse == EquipmentType.None || (UnityEngine.Object) EquipmentManager.GetCurseData(curse) == (UnityEngine.Object) null)
      return;
    this.icon.sprite = EquipmentManager.GetCurseData(curse).UISprite;
    this.transform.localScale = Vector3.one * this.scaleMultiplier;
    if (!punch)
      return;
    this.icon.transform.localScale = Vector3.one * 2f;
    this.icon.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    this.Background.transform.DOKill();
    this.Background.enabled = true;
    this.Background.transform.localScale = Vector3.one * 0.9f;
    this.Background.transform.DOScale(Vector3.one * 3f, 0.5f);
    Color c = this.Background.color;
    DOTweenModuleUI.DOFade(this.Background, 0.0f, 0.5f).OnComplete<TweenerCore<Color, Color, ColorOptions>>((TweenCallback) (() =>
    {
      this.Background.color = c;
      this.Background.enabled = false;
    }));
  }
}
