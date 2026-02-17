// Decompiled with JetBrains decompiler
// Type: CurrentWeapon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class CurrentWeapon : MonoBehaviour
{
  public EquipmentType Weapon = EquipmentType.None;
  public int Level;
  [SerializeField]
  public Image icon;
  [SerializeField]
  public Image Background;
  [SerializeField]
  public TextMeshProUGUI LevelText;
  [SerializeField]
  public float scaleMultiplier = 2f;
  public PlayerFarming playerFarming;
  public bool playing;

  public void OnEnable()
  {
  }

  public void Awake()
  {
    PlayerWeapon.OnWeaponChanged += new PlayerWeapon.WeaponEvent(this.SetWeapon);
  }

  public void Init(PlayerFarming playerFarmingVar)
  {
    this.playerFarming = playerFarmingVar;
    if ((UnityEngine.Object) this.playerFarming == (UnityEngine.Object) null || this.playerFarming.currentWeapon == EquipmentType.None || !GameManager.IsDungeon(PlayerFarming.Location) || PlayerFarming.Location == FollowerLocation.IntroDungeon)
      this.transform.localScale = Vector3.zero;
    else
      this.transform.localScale = Vector3.one;
    Interaction_WeaponSelectionPodium.OnHighlightWeapon += new Action<bool, PlayerFarming>(this.HighlightWeapon);
    Singleton<AccessibilityManager>.Instance.OnRomanNumeralsChanged += new Action<bool>(this.OnRomanNumeralSettingChanged);
    this.Background.enabled = false;
    this.SetWeapon(this.playerFarming.currentWeapon, this.playerFarming.currentWeaponLevel, this.playerFarming);
  }

  public void HighlightWeapon(bool Toggle, PlayerFarming p)
  {
    if ((UnityEngine.Object) this.playerFarming != (UnityEngine.Object) p || this.Weapon == EquipmentType.None || (UnityEngine.Object) this.Background == (UnityEngine.Object) null)
      return;
    this.Background.transform.DOKill();
    this.transform.DOKill();
    this.Background.enabled = Toggle;
    if (this.Background.enabled)
    {
      this.Background.transform.localScale = Vector3.one * 1.2f;
      this.Background.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
      this.transform.DOScale(Vector3.one * 1.1f * this.scaleMultiplier, 0.0f);
      if (this.playing)
        return;
      this.StartCoroutine((IEnumerator) this.Highlighting());
    }
    else
    {
      this.transform.DOKill();
      this.transform.DOScale(Vector3.one * this.scaleMultiplier, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    }
  }

  public IEnumerator Highlighting()
  {
    CurrentWeapon currentWeapon = this;
    while (currentWeapon.Background.enabled)
    {
      currentWeapon.playing = true;
      currentWeapon.transform.DOKill();
      currentWeapon.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.0f), 0.5f);
      yield return (object) new WaitForSeconds(1f);
    }
    currentWeapon.playing = false;
  }

  public void OnDisable()
  {
    Interaction_WeaponSelectionPodium.OnHighlightWeapon -= new Action<bool, PlayerFarming>(this.HighlightWeapon);
    Singleton<AccessibilityManager>.Instance.OnRomanNumeralsChanged -= new Action<bool>(this.OnRomanNumeralSettingChanged);
  }

  public void OnDestroy()
  {
    PlayerWeapon.OnWeaponChanged -= new PlayerWeapon.WeaponEvent(this.SetWeapon);
    Interaction_WeaponSelectionPodium.OnHighlightWeapon -= new Action<bool, PlayerFarming>(this.HighlightWeapon);
    Singleton<AccessibilityManager>.Instance.OnRomanNumeralsChanged -= new Action<bool>(this.OnRomanNumeralSettingChanged);
  }

  public void SetWeapon(EquipmentType weapon, int level, PlayerFarming p)
  {
    if ((UnityEngine.Object) this.playerFarming == (UnityEngine.Object) null)
    {
      Debug.Log((object) "PlayerFarming == null");
    }
    else
    {
      if ((UnityEngine.Object) p != (UnityEngine.Object) this.playerFarming)
        return;
      bool punch = weapon != this.playerFarming.currentWeapon || level != this.playerFarming.currentWeaponLevel;
      weapon = this.playerFarming.currentWeapon;
      level = this.playerFarming.currentWeaponLevel;
      this.SetWeapon(weapon, level, punch);
    }
  }

  public void OnRomanNumeralSettingChanged(bool state)
  {
    this.LevelText.text = this.Level.ToNumeral();
  }

  public void SetWeapon(EquipmentType weapon, int level, bool punch)
  {
    this.Weapon = weapon;
    this.Level = level;
    if ((UnityEngine.Object) this.icon != (UnityEngine.Object) null)
      this.icon.enabled = weapon != EquipmentType.None;
    this.Background.enabled = weapon != EquipmentType.None;
    this.LevelText.text = level.ToNumeral();
    this.LevelText.isRightToLeftText = LocalizeIntegration.IsArabic();
    if (weapon == EquipmentType.None || (UnityEngine.Object) EquipmentManager.GetWeaponData(weapon) == (UnityEngine.Object) null)
      return;
    this.icon.sprite = PlayerFarming.Instance.playerWeapon.GetCurrentIcon(weapon);
    this.transform.localScale = Vector3.one * this.scaleMultiplier;
    if (punch)
    {
      this.icon.transform.localScale = Vector3.one * 2f;
      this.icon.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
      this.Background.transform.DOKill();
      this.Background.enabled = true;
      this.Background.transform.localScale = Vector3.one * 0.9f;
      this.Background.transform.DOScale(Vector3.one * 2f, 0.5f);
      Color c = this.Background.color;
      DOTweenModuleUI.DOFade(this.Background, 0.0f, 0.5f).OnComplete<TweenerCore<Color, Color, ColorOptions>>((TweenCallback) (() =>
      {
        this.Background.color = c;
        this.Background.enabled = false;
      }));
    }
    else
      this.Background.enabled = false;
  }
}
