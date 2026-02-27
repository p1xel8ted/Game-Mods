// Decompiled with JetBrains decompiler
// Type: Interaction_WeaponChoice
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class Interaction_WeaponChoice : Interaction_WeaponSelectionPodium
{
  [SerializeField]
  public UnityEvent onInteract;
  public bool hasTriedFallback;

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.onInteract?.Invoke();
  }

  public override void SetWeapon(int ForceLevel = -1)
  {
    Debug.Log((object) "SetWeapon()");
    this.IconSpriteRenderer.material = this.WeaponMaterial;
    if (PlayerFleeceManager.FleeceSwapsWeaponForCurse())
    {
      this.SetCurse(-1);
    }
    else
    {
      for (int index = 0; index < 50; ++index)
      {
        this.TypeOfWeapon = DataManager.Instance.GetRandomWeaponInPool();
        bool flag = false;
        foreach (Interaction_WeaponSelectionPodium otherWeaponOption in this.otherWeaponOptions)
        {
          if (otherWeaponOption.TypeOfWeapon == this.TypeOfWeapon)
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
              return;
            }
            this.gameObject.SetActive(false);
          }
        }
        else
          break;
      }
      this.Type = Interaction_WeaponSelectionPodium.Types.Weapon;
      this.WeaponLevel = DataManager.Instance.CurrentRunWeaponLevel + 1;
      this.weaponBetterIcon.enabled = false;
      if (DataManager.Instance.ForcedStartingWeapon != EquipmentType.None && (!PlayerFleeceManager.FleeceSwapsCurseForRelic() || DataManager.Instance.ForcedStartingWeapon != EquipmentType.Sword_Ratau))
      {
        this.TypeOfWeapon = DataManager.Instance.ForcedStartingWeapon;
        DataManager.Instance.ForcedStartingWeapon = EquipmentType.None;
      }
      if (this.playerFarming.currentWeapon != EquipmentType.None)
        return;
      this.WeaponLevel += DataManager.StartingEquipmentLevel;
    }
  }

  public override void SetCurse(int ForceLevel = -1)
  {
    if (PlayerFleeceManager.FleeceSwapsCurseForRelic())
    {
      this.SetWeapon(-1);
    }
    else
    {
      Debug.Log((object) "SetCurse()");
      this.IconSpriteRenderer.material = this.CurseMaterial;
      for (int index = 0; index < 50; ++index)
      {
        this.TypeOfWeapon = DataManager.Instance.GetRandomCurseInPool();
        bool flag = false;
        foreach (Interaction_WeaponSelectionPodium otherWeaponOption in this.otherWeaponOptions)
        {
          if (otherWeaponOption.TypeOfWeapon == this.TypeOfWeapon)
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
              return;
            }
            this.gameObject.SetActive(false);
          }
        }
        else
          break;
      }
      this.Type = Interaction_WeaponSelectionPodium.Types.Curse;
      this.WeaponLevel = DataManager.Instance.CurrentRunCurseLevel + 1;
      this.weaponBetterIcon.enabled = false;
      if (DataManager.Instance.ForcedStartingCurse != EquipmentType.None)
      {
        this.TypeOfWeapon = DataManager.Instance.ForcedStartingCurse;
        DataManager.Instance.ForcedStartingCurse = EquipmentType.None;
      }
      if (this.playerFarming.currentCurse != EquipmentType.None)
        return;
      this.WeaponLevel += DataManager.StartingEquipmentLevel;
    }
  }

  public override void SetRelic()
  {
    for (int index = 0; index < 50; ++index)
    {
      this.TypeOfRelic = EquipmentManager.GetRandomRelicData(false, this.playerFarming).RelicType;
      bool flag = false;
      foreach (Interaction_WeaponSelectionPodium otherWeaponOption in this.otherWeaponOptions)
      {
        if (otherWeaponOption.TypeOfRelic == this.TypeOfRelic)
          flag = true;
      }
      if (flag)
      {
        if (index >= 49)
          this.gameObject.SetActive(false);
      }
      else
        break;
    }
    this.SetRelic(this.TypeOfRelic);
  }
}
