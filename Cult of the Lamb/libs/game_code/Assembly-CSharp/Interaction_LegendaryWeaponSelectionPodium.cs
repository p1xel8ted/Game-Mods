// Decompiled with JetBrains decompiler
// Type: Interaction_LegendaryWeaponSelectionPodium
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_LegendaryWeaponSelectionPodium : Interaction_WeaponSelectionPodium
{
  public static List<Interaction_LegendaryWeaponSelectionPodium> LegendaryPodiums = new List<Interaction_LegendaryWeaponSelectionPodium>();
  [SerializeField]
  public Interaction_EntrySignPost entrySignToDestroy;

  public override void OnEnable()
  {
    base.OnEnable();
    Interaction_LegendaryWeaponSelectionPodium.LegendaryPodiums.Add(this);
  }

  public override void OnDisable()
  {
    base.OnDisable();
    Interaction_LegendaryWeaponSelectionPodium.LegendaryPodiums.Remove(this);
  }

  public void Start()
  {
    this.TypeOfWeapon = EquipmentType.None;
    List<Objectives_LegendaryWeaponRun> objectivesOfType = ObjectiveManager.GetObjectivesOfType<Objectives_LegendaryWeaponRun>();
    for (int index = 0; index < objectivesOfType.Count; ++index)
    {
      if (!objectivesOfType[index].IsComplete && DataManager.Instance.WeaponPool.Contains(objectivesOfType[index].LegendaryWeapon))
      {
        this.TypeOfWeapon = objectivesOfType[index].LegendaryWeapon;
        break;
      }
    }
    if (PlayerFleeceManager.FleeceSwapsWeaponForCurse() || DungeonSandboxManager.Active || GameManager.CurrentDungeonFloor > 1 || DataManager.Instance.HealingLeshyQuestActive || DataManager.Instance.HealingHeketQuestActive || DataManager.Instance.HealingKallamarQuestActive || DataManager.Instance.HealingShamuraQuestActive || this.TypeOfWeapon == EquipmentType.None)
    {
      Object.Destroy((Object) this.gameObject);
    }
    else
    {
      if ((Object) this.entrySignToDestroy != (Object) null)
        Object.Destroy((Object) this.entrySignToDestroy.gameObject);
      this.HasSecondaryInteraction = false;
      this.AllowResummonWeapon = false;
      if (DataManager.Instance.ForcedStartingWeapon != this.TypeOfWeapon)
        return;
      DataManager.Instance.ForcedStartingWeapon = EquipmentType.None;
      Debug.Log((object) "Swapped");
    }
  }

  public override void SetWeapon(int ForceLevel = -1)
  {
    this.IconSpriteRenderer.material = this.WeaponMaterial;
    this.Type = Interaction_WeaponSelectionPodium.Types.Weapon;
    this.WeaponLevel = PlayerFarming.Location != FollowerLocation.Boss_5 ? DataManager.Instance.CurrentRunWeaponLevel + 1 : 1;
    if (!this.increaseWeaponLevel && PlayerFarming.Location != FollowerLocation.Boss_5)
    {
      --this.WeaponLevel;
    }
    else
    {
      if ((Object) this.playerFarming == (Object) null || this.playerFarming.currentWeapon == EquipmentType.None)
        this.WeaponLevel += DataManager.StartingEquipmentLevel;
      if (ForceLevel == -1)
      {
        DataManager.Instance.CurrentRunWeaponLevel = this.WeaponLevel;
        this.WeaponLevel += Mathf.Clamp(this.LevelIncreaseAmount - 1, 0, this.LevelIncreaseAmount);
      }
      else
        this.WeaponLevel = DataManager.Instance.CurrentRunWeaponLevel = ForceLevel;
    }
  }
}
