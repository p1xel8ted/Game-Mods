// Decompiled with JetBrains decompiler
// Type: Interaction_WeaponChoiceChest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_WeaponChoiceChest : Interaction_WeaponSelectionPodium
{
  public static List<Interaction_WeaponSelectionPodium> otherWeaponOptions = new List<Interaction_WeaponSelectionPodium>();
  public bool hasTriedFallback;

  public new void Awake()
  {
    Interaction_WeaponChoiceChest.otherWeaponOptions.Add((Interaction_WeaponSelectionPodium) this);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    Interaction_WeaponChoiceChest.otherWeaponOptions.Remove((Interaction_WeaponSelectionPodium) this);
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    for (int i = Interaction_WeaponChoiceChest.otherWeaponOptions.Count - 1; i >= 0; i--)
    {
      if ((Object) Interaction_WeaponChoiceChest.otherWeaponOptions[i] != (Object) this && Interaction_WeaponChoiceChest.otherWeaponOptions[i].gameObject.activeSelf && this.IsPodiumInSameRoom(Interaction_WeaponChoiceChest.otherWeaponOptions[i]))
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

  public override void SetWeapon(int ForceLevel = -1)
  {
    if (PlayerFleeceManager.FleeceSwapsWeaponForCurse())
    {
      this.SetCurse(-1);
    }
    else
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
              this.SetCurse(-1);
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
      if (this.playerFarming.currentWeapon == EquipmentType.None)
        this.WeaponLevel += DataManager.StartingEquipmentLevel;
      this.WeaponLevel += Mathf.Clamp(this.LevelIncreaseAmount - 1, 0, this.LevelIncreaseAmount);
    }
  }

  public override void SetRelic()
  {
    this.IconSpriteRenderer.material = this.WeaponMaterial;
    this.IconSpriteRenderer.transform.localScale = Vector3.one * 0.41f;
    this.IconSpriteRenderer.transform.parent.localPosition = new Vector3(this.IconSpriteRenderer.transform.parent.localPosition.x, this.IconSpriteRenderer.transform.parent.localPosition.y, -1.5f);
    this.TypeOfRelic = EquipmentManager.GetRandomRelicData(false, this.playerFarming).RelicType;
    if (this.TypeOfRelic.ToString().Contains("Blessed") && DataManager.Instance.ForceBlessedRelic)
      DataManager.Instance.ForceBlessedRelic = false;
    else if (this.TypeOfRelic.ToString().Contains("Dammed") && DataManager.Instance.ForceDammedRelic)
      DataManager.Instance.ForceDammedRelic = false;
    this.Type = Interaction_WeaponSelectionPodium.Types.Relic;
    if (DataManager.Instance.SpawnedRelicsThisRun.Contains(this.TypeOfRelic))
      return;
    DataManager.Instance.SpawnedRelicsThisRun.Add(this.TypeOfRelic);
  }

  public override void SetCurse(int ForceLevel = -1)
  {
    if (PlayerFleeceManager.FleeceSwapsCurseForRelic())
    {
      this.SetRelic();
    }
    else
    {
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
            if (!this.hasTriedFallback && !PlayerFleeceManager.FleeceSwapsWeaponForCurse())
            {
              this.hasTriedFallback = true;
              this.SetWeapon(-1);
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
      if (this.playerFarming.currentCurse == EquipmentType.None)
        this.WeaponLevel += DataManager.StartingEquipmentLevel;
      this.WeaponLevel += Mathf.Clamp(this.LevelIncreaseAmount - 1, 0, this.LevelIncreaseAmount);
    }
  }
}
