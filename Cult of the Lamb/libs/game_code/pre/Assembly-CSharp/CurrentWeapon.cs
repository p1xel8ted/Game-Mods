// Decompiled with JetBrains decompiler
// Type: CurrentWeapon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private EquipmentType Weapon = EquipmentType.None;
  [SerializeField]
  private Image icon;
  [SerializeField]
  private Image Background;
  [SerializeField]
  private TextMeshProUGUI LevelText;
  private bool playing;

  public void OnEnable()
  {
    if (DataManager.Instance.CurrentWeapon == EquipmentType.None || !GameManager.IsDungeon(PlayerFarming.Location) || PlayerFarming.Location == FollowerLocation.IntroDungeon)
      this.transform.localScale = Vector3.zero;
    PlayerWeapon.OnWeaponChanged += new PlayerWeapon.WeaponEvent(this.SetWeapon);
    Interaction_WeaponSelectionPodium.OnHighlightWeapon += new Action<bool>(this.HighlightWeapon);
    this.Background.enabled = false;
  }

  private void HighlightWeapon(bool Toggle)
  {
    if (this.Weapon == EquipmentType.None)
      return;
    this.Background.transform.DOKill();
    this.transform.DOKill();
    this.Background.enabled = Toggle;
    if (this.Background.enabled)
    {
      this.Background.transform.localScale = Vector3.one * 1.2f;
      this.Background.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
      this.transform.DOScale(Vector3.one * 1.1f, 0.0f);
      if (this.playing)
        return;
      this.StartCoroutine((IEnumerator) this.Highlighting());
    }
    else
    {
      this.transform.DOKill();
      this.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    }
  }

  private IEnumerator Highlighting()
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

  private void OnDisable()
  {
    PlayerWeapon.OnWeaponChanged -= new PlayerWeapon.WeaponEvent(this.SetWeapon);
    Interaction_WeaponSelectionPodium.OnHighlightWeapon -= new Action<bool>(this.HighlightWeapon);
  }

  private void SetWeapon(EquipmentType weapon, int Level) => this.SetWeapon(weapon, Level, true);

  private void SetWeapon(EquipmentType weapon, int Level, bool punch)
  {
    this.Weapon = weapon;
    this.icon.enabled = weapon != EquipmentType.None;
    this.Background.enabled = weapon != EquipmentType.None;
    this.LevelText.text = Level.ToNumeral();
    if (weapon == EquipmentType.None || (UnityEngine.Object) EquipmentManager.GetWeaponData(weapon) == (UnityEngine.Object) null)
      return;
    this.icon.sprite = PlayerFarming.Instance.playerWeapon.GetCurrentIcon(weapon);
    this.transform.localScale = Vector3.one;
    if (!punch)
      return;
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
}
