// Decompiled with JetBrains decompiler
// Type: Structures_Missionary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class Structures_Missionary : StructureBrain
{
  public static void RemoveFollower(int ID)
  {
    List<Structures_Missionary> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_Missionary>();
    for (int index = 0; index < structuresOfType.Count; ++index)
    {
      if (structuresOfType[index].Data.MultipleFollowerIDs.Contains(ID))
        structuresOfType[index].Data.MultipleFollowerIDs.Remove(ID);
    }
    if (!DataManager.Instance.Followers_OnMissionary_IDs.Contains(ID))
      return;
    DataManager.Instance.Followers_OnMissionary_IDs.Remove(ID);
  }
}
