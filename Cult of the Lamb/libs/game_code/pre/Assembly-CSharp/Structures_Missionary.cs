// Decompiled with JetBrains decompiler
// Type: Structures_Missionary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
