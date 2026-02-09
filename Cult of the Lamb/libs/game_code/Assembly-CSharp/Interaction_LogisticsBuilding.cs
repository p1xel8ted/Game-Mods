// Decompiled with JetBrains decompiler
// Type: Interaction_LogisticsBuilding
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using src.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class Interaction_LogisticsBuilding : Interaction
{
  public static List<Interaction_LogisticsBuilding> LogisticBuildings = new List<Interaction_LogisticsBuilding>();
  [SerializeField]
  public Structure structure;
  [SerializeField]
  public GameObject displayVisualsPosition;
  [SerializeField]
  public GameObject notes;
  public static Color[] colors = new Color[11]
  {
    StaticColors.BlueColor,
    StaticColors.RedColor,
    StaticColors.TwitchPurple,
    StaticColors.GoatPurple,
    StaticColors.GreenColor,
    StaticColors.OrangeColor,
    StaticColors.LightGreyColor,
    StaticColors.DarkGreenColor,
    StaticColors.DarkRedColor,
    StaticColors.DarkBlueColor,
    StaticColors.DarkGreyColor
  };
  public const float displayVisualsCooldown = 3f;
  public float displayVisualsTimer;

  public void Start()
  {
    Interaction_LogisticsBuilding.LogisticBuildings.Add(this);
    StructureManager.OnStructureRemoved += new StructureManager.StructureChanged(this.ClearInvalidConnections);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    Interaction_LogisticsBuilding.LogisticBuildings.Remove(this);
    StructureManager.OnStructureRemoved -= new StructureManager.StructureChanged(this.ClearInvalidConnections);
  }

  public override void Update()
  {
    base.Update();
    this.displayVisualsTimer += Time.deltaTime;
    this.notes.gameObject.SetActive(this.structure.Brain != null && this.structure.Brain.Data.LogisticSlots.Count > 0);
  }

  public override void GetLabel()
  {
    base.GetLabel();
    this.Label = ScriptLocalization.FollowerInteractions.MakeDemand;
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    GameManager.GetInstance().OnConversationNew();
    Time.timeScale = 0.0f;
    UILogisticsMenuController logisticsMenuController = MonoSingleton<UIManager>.Instance.LogisticsMenuTemplate.Instantiate<UILogisticsMenuController>();
    logisticsMenuController.Show(this.structure.Brain as Structures_Logistics);
    logisticsMenuController.OnHide = logisticsMenuController.OnHide + (System.Action) (() =>
    {
      GameManager.GetInstance().OnConversationEnd();
      Time.timeScale = 1f;
    });
    logisticsMenuController.OnHidden = logisticsMenuController.OnHidden + (System.Action) (() => Time.timeScale = 1f);
  }

  public override void IndicateHighlighted(PlayerFarming playerFarming = null)
  {
    base.IndicateHighlighted(playerFarming);
  }

  public static List<StructureBrain> GetStructuresIncludingBuildingSites(
    StructureBrain.TYPES structureType)
  {
    List<StructureBrain> list = StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.BUILD_SITE).Where<StructureBrain>((Func<StructureBrain, bool>) (b => b.Data.ToBuildType == structureType)).ToList<StructureBrain>();
    Debug.Log((object) $"Building sites for type {structureType}: {list.Count}");
    list.AddRange((IEnumerable<StructureBrain>) Interaction_LogisticsBuilding.GetStructures(structureType));
    return list;
  }

  public static List<StructureBrain> GetStructures(StructureBrain.TYPES structureType)
  {
    List<StructureBrain> structures = new List<StructureBrain>();
    switch (structureType)
    {
      case StructureBrain.TYPES.FARM_STATION:
        return StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.FARM_STATION_II);
      case StructureBrain.TYPES.SHRINE:
        List<Structures_Shrine> structuresOfType1 = StructureManager.GetAllStructuresOfType<Structures_Shrine>();
        for (int index = 0; index < structuresOfType1.Count; ++index)
        {
          if (structuresOfType1[index].Data.Type != StructureBrain.TYPES.SHRINE)
            structures.Add((StructureBrain) structuresOfType1[index]);
        }
        return structures;
      case StructureBrain.TYPES.BODY_PIT:
        List<Structures_Grave> structuresOfType2 = StructureManager.GetAllStructuresOfType<Structures_Grave>();
        for (int index = 0; index < structuresOfType2.Count; ++index)
          structures.Add((StructureBrain) structuresOfType2[index]);
        return structures;
      case StructureBrain.TYPES.LUMBERJACK_STATION:
        List<Structures_LumberjackStation> structuresOfType3 = StructureManager.GetAllStructuresOfType<Structures_LumberjackStation>();
        for (int index = 0; index < structuresOfType3.Count; ++index)
        {
          if (structuresOfType3[index].Data.Type == StructureBrain.TYPES.LUMBERJACK_STATION || structuresOfType3[index].Data.Type == StructureBrain.TYPES.LUMBERJACK_STATION_2)
            structures.Add((StructureBrain) structuresOfType3[index]);
        }
        return structures;
      case StructureBrain.TYPES.BLOODSTONE_MINE:
        List<Structures_MinerStation> structuresOfType4 = StructureManager.GetAllStructuresOfType<Structures_MinerStation>();
        for (int index = 0; index < structuresOfType4.Count; ++index)
          structures.Add((StructureBrain) structuresOfType4[index]);
        return structures;
      case StructureBrain.TYPES.OUTHOUSE:
        List<Structures_Outhouse> structuresOfType5 = StructureManager.GetAllStructuresOfType<Structures_Outhouse>();
        for (int index = 0; index < structuresOfType5.Count; ++index)
          structures.Add((StructureBrain) structuresOfType5[index]);
        return structures;
      case StructureBrain.TYPES.REFINERY:
        List<Structures_Refinery> structuresOfType6 = StructureManager.GetAllStructuresOfType<Structures_Refinery>();
        for (int index = 0; index < structuresOfType6.Count; ++index)
          structures.Add((StructureBrain) structuresOfType6[index]);
        return structures;
      case StructureBrain.TYPES.SCARECROW_2:
        List<Structures_Scarecrow> structuresOfType7 = StructureManager.GetAllStructuresOfType<Structures_Scarecrow>();
        for (int index = 0; index < structuresOfType7.Count; ++index)
        {
          if (structuresOfType7[index].Data.Type == structureType)
            structures.Add((StructureBrain) structuresOfType7[index]);
        }
        return structures;
      case StructureBrain.TYPES.MORGUE_1:
        List<Structures_Morgue> structuresOfType8 = StructureManager.GetAllStructuresOfType<Structures_Morgue>();
        for (int index = 0; index < structuresOfType8.Count; ++index)
          structures.Add((StructureBrain) structuresOfType8[index]);
        return structures;
      case StructureBrain.TYPES.CRYPT_1:
        List<Structures_Crypt> structuresOfType9 = StructureManager.GetAllStructuresOfType<Structures_Crypt>();
        for (int index = 0; index < structuresOfType9.Count; ++index)
          structures.Add((StructureBrain) structuresOfType9[index]);
        return structures;
      case StructureBrain.TYPES.PUB:
        List<Structures_Pub> structuresOfType10 = StructureManager.GetAllStructuresOfType<Structures_Pub>();
        for (int index = 0; index < structuresOfType10.Count; ++index)
          structures.Add((StructureBrain) structuresOfType10[index]);
        return structures;
      case StructureBrain.TYPES.RANCH:
        return StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.RANCH_2);
      case StructureBrain.TYPES.FURNACE_1:
        List<Structures_Furnace> structuresOfType11 = StructureManager.GetAllStructuresOfType<Structures_Furnace>();
        for (int index = 0; index < structuresOfType11.Count; ++index)
          structures.Add((StructureBrain) structuresOfType11[index]);
        return structures;
      case StructureBrain.TYPES.FURNACE_3:
        List<Structures_Furnace> structuresOfType12 = StructureManager.GetAllStructuresOfType<Structures_Furnace>();
        for (int index = 0; index < structuresOfType12.Count; ++index)
        {
          if (structuresOfType12[index].Data.Type == structureType)
            structures.Add((StructureBrain) structuresOfType12[index]);
        }
        return structures;
      case StructureBrain.TYPES.LIGHTNING_ROD_2:
        List<Structures_LightningRod> structuresOfType13 = StructureManager.GetAllStructuresOfType<Structures_LightningRod>();
        for (int index = 0; index < structuresOfType13.Count; ++index)
        {
          if (structuresOfType13[index].Data.Type == structureType)
            structures.Add((StructureBrain) structuresOfType13[index]);
        }
        return structures;
      case StructureBrain.TYPES.TOOLSHED:
        List<Structures_Toolshed> structuresOfType14 = StructureManager.GetAllStructuresOfType<Structures_Toolshed>();
        for (int index = 0; index < structuresOfType14.Count; ++index)
          structures.Add((StructureBrain) structuresOfType14[index]);
        return structures;
      case StructureBrain.TYPES.FARM_CROP_GROWER:
        List<Structures_FarmCropGrower> structuresOfType15 = StructureManager.GetAllStructuresOfType<Structures_FarmCropGrower>();
        for (int index = 0; index < structuresOfType15.Count; ++index)
          structures.Add((StructureBrain) structuresOfType15[index]);
        return structures;
      case StructureBrain.TYPES.ROTSTONE_MINE:
        List<Structures_RotstoneStation> structuresOfType16 = StructureManager.GetAllStructuresOfType<Structures_RotstoneStation>();
        for (int index = 0; index < structuresOfType16.Count; ++index)
          structures.Add((StructureBrain) structuresOfType16[index]);
        return structures;
      default:
        return StructureManager.GetAllStructuresOfType(structureType);
    }
  }

  public void ClearInvalidConnections(StructuresData removedStructure)
  {
    if (this.structure.Brain == null)
      return;
    for (int index = this.structure.Brain.Data.LogisticSlots.Count - 1; index >= 0; --index)
    {
      if (this.structure.Brain.Data.LogisticSlots[index].RootStructureType != StructureBrain.TYPES.NONE && (!StructureManager.IsAnyUpgradeBuiltOrBuildingSafe(this.structure.Brain.Data.LogisticSlots[index].RootStructureType) || !StructureManager.IsAnyUpgradeBuiltOrBuildingSafe(this.structure.Brain.Data.LogisticSlots[index].TargetStructureType)))
        this.structure.Brain.Data.LogisticSlots.RemoveAt(index);
    }
  }

  public Color GetLogisticColor()
  {
    return Interaction_LogisticsBuilding.colors[Interaction_LogisticsBuilding.LogisticBuildings.IndexOf(this) % Interaction_LogisticsBuilding.colors.Length];
  }

  public static Structure GetStructure(StructureBrain brain)
  {
    foreach (Structure structure in Structure.Structures)
    {
      if (structure.Brain == brain)
        return structure;
    }
    return (Structure) null;
  }
}
