// Decompiled with JetBrains decompiler
// Type: BluePrint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class BluePrint
{
  public BluePrint.BluePrintType type;

  public static StructuresData GetStructure(BluePrint.BluePrintType type)
  {
    switch (type)
    {
      case BluePrint.BluePrintType.TREE:
        return StructuresData.GetInfoByType(StructureBrain.TYPES.DECORATION_TREE, 0);
      case BluePrint.BluePrintType.STONE:
        return StructuresData.GetInfoByType(StructureBrain.TYPES.DECORATION_STONE, 0);
      case BluePrint.BluePrintType.PATH_DIRT:
        return StructuresData.GetInfoByType(StructureBrain.TYPES.PLANK_PATH, 0);
      default:
        return (StructuresData) null;
    }
  }

  public static BluePrint Create(BluePrint.BluePrintType type)
  {
    Debug.Log((object) ("CREATE CARD! " + (object) type));
    return new BluePrint() { type = type };
  }

  public static BluePrint.BluePrintType GiveNewBluePrint()
  {
    List<BluePrint.BluePrintType> bluePrintTypeList = new List<BluePrint.BluePrintType>();
    foreach (BluePrint.BluePrintType allBluePrint in DataManager.AllBluePrints)
    {
      bool flag = false;
      foreach (BluePrint playerBluePrint in DataManager.Instance.PlayerBluePrints)
      {
        if (playerBluePrint.type == allBluePrint)
          flag = true;
      }
      if (!flag)
        bluePrintTypeList.Add(allBluePrint);
    }
    return bluePrintTypeList[UnityEngine.Random.Range(0, bluePrintTypeList.Count)];
  }

  public enum BluePrintType
  {
    TREE,
    STONE,
    PATH_DIRT,
  }
}
