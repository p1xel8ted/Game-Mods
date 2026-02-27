// Decompiled with JetBrains decompiler
// Type: Interaction_WeaponChoiceChest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_WeaponChoiceChest : Interaction_WeaponSelectionPodium
{
  private static List<Interaction_WeaponSelectionPodium> otherWeaponOptions = new List<Interaction_WeaponSelectionPodium>();
  private bool hasTriedFallback;

  private void Awake()
  {
    Interaction_WeaponChoiceChest.otherWeaponOptions.Add((Interaction_WeaponSelectionPodium) this);
  }

  protected override void OnDestroy()
  {
    base.OnDestroy();
    Interaction_WeaponChoiceChest.otherWeaponOptions.Remove((Interaction_WeaponSelectionPodium) this);
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    for (int i = Interaction_WeaponChoiceChest.otherWeaponOptions.Count - 1; i >= 0; i--)
    {
      if ((Object) Interaction_WeaponChoiceChest.otherWeaponOptions[i] != (Object) this)
      {
        Interaction_WeaponChoiceChest.otherWeaponOptions[i].Interactable = false;
        Interaction_WeaponChoiceChest.otherWeaponOptions[i].Lighting.SetActive(false);
        Interaction_WeaponChoiceChest.otherWeaponOptions[i].weaponBetterIcon.enabled = false;
        Interaction_WeaponChoiceChest.otherWeaponOptions[i].IconSpriteRenderer.transform.DOScale(Vector3.zero, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => Interaction_WeaponChoiceChest.otherWeaponOptions[i].IconSpriteRenderer.enabled = false));
        Interaction_WeaponChoiceChest.otherWeaponOptions[i].podiumOn.SetActive(false);
        Interaction_WeaponChoiceChest.otherWeaponOptions[i].podiumOff.SetActive(true);
        Interaction_WeaponChoiceChest.otherWeaponOptions[i].particleEffect.Stop();
        Interaction_WeaponChoiceChest.otherWeaponOptions[i].AvailableGoop.Play("Hide");
        Interaction_WeaponChoiceChest.otherWeaponOptions[i].enabled = false;
      }
    }
    if (this.Type == Interaction_WeaponSelectionPodium.Types.Weapon)
      DataManager.Instance.CurrentRunWeaponLevel = this.WeaponLevel;
    if (this.Type != Interaction_WeaponSelectionPodium.Types.Curse)
      return;
    DataManager.Instance.CurrentRunCurseLevel = this.WeaponLevel;
  }

  protected override void SetWeapon()
  {
    Debug.Log((object) "SetWeapon()");
    this.IconSpriteRenderer.material = this.WeaponMaterial;
    for (int index = 0; index < 50; ++index)
    {
      this.TypeOfWeapon = DataManager.Instance.GetRandomWeaponInPool();
      bool flag = false;
      foreach (Interaction_WeaponSelectionPodium otherWeaponOption in Interaction_WeaponChoiceChest.otherWeaponOptions)
      {
        if ((Object) otherWeaponOption != (Object) this && otherWeaponOption.TypeOfWeapon == this.TypeOfWeapon)
          flag = true;
      }
      if (flag)
      {
        if (index >= 49)
        {
          if (!this.hasTriedFallback && DataManager.Instance.EnabledSpells)
          {
            this.hasTriedFallback = true;
            this.SetCurse();
          }
          else
          {
            this.gameObject.SetActive(false);
            return;
          }
        }
      }
      else
        break;
    }
    this.Type = Interaction_WeaponSelectionPodium.Types.Weapon;
    this.WeaponLevel = DataManager.Instance.CurrentRunWeaponLevel + 1;
    if (DataManager.Instance.ForcedStartingWeapon != EquipmentType.None)
    {
      this.TypeOfWeapon = DataManager.Instance.ForcedStartingWeapon;
      DataManager.Instance.ForcedStartingWeapon = EquipmentType.None;
    }
    if (DataManager.Instance.CurrentWeapon != EquipmentType.None)
      return;
    this.WeaponLevel += DataManager.StartingEquipmentLevel;
  }

  protected override void SetCurse()
  {
    Debug.Log((object) "SetCurse()");
    this.IconSpriteRenderer.material = this.CurseMaterial;
    for (int index = 0; index < 50; ++index)
    {
      this.TypeOfWeapon = DataManager.Instance.GetRandomCurseInPool();
      bool flag = false;
      foreach (Interaction_WeaponSelectionPodium otherWeaponOption in Interaction_WeaponChoiceChest.otherWeaponOptions)
      {
        if ((Object) otherWeaponOption != (Object) this && otherWeaponOption.TypeOfWeapon == this.TypeOfWeapon)
          flag = true;
      }
      if (flag)
      {
        if (index >= 49)
        {
          if (!this.hasTriedFallback)
          {
            this.hasTriedFallback = true;
            this.SetWeapon();
          }
          else
          {
            this.gameObject.SetActive(false);
            return;
          }
        }
      }
      else
        break;
    }
    this.Type = Interaction_WeaponSelectionPodium.Types.Curse;
    this.WeaponLevel = DataManager.Instance.CurrentRunCurseLevel + 1;
    if (DataManager.Instance.ForcedStartingCurse != EquipmentType.None)
    {
      this.TypeOfWeapon = DataManager.Instance.ForcedStartingCurse;
      DataManager.Instance.ForcedStartingCurse = EquipmentType.None;
    }
    if (DataManager.Instance.CurrentCurse != EquipmentType.None)
      return;
    this.WeaponLevel += DataManager.StartingEquipmentLevel;
  }
}
