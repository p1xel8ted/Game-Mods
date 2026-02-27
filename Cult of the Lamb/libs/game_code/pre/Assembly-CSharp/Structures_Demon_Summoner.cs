// Decompiled with JetBrains decompiler
// Type: Structures_Demon_Summoner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class Structures_Demon_Summoner : StructureBrain
{
  public int DemonSlots
  {
    get
    {
      if (this.Data.Type == StructureBrain.TYPES.DEMON_SUMMONER)
        return 1;
      if (this.Data.Type == StructureBrain.TYPES.DEMON_SUMMONER_2)
        return 2;
      return this.Data.Type == StructureBrain.TYPES.DEMON_SUMMONER_3 ? 3 : 1;
    }
  }

  public static void RemoveFollower(int ID)
  {
    List<Structures_Demon_Summoner> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_Demon_Summoner>();
    for (int index = 0; index < structuresOfType.Count; ++index)
    {
      if (structuresOfType[index].Data.MultipleFollowerIDs.Contains(ID))
        structuresOfType[index].Data.MultipleFollowerIDs.Remove(ID);
    }
    if (!DataManager.Instance.Followers_Demons_IDs.Contains(ID))
      return;
    int index1 = DataManager.Instance.Followers_Demons_IDs.IndexOf(ID);
    DataManager.Instance.Followers_Demons_IDs.Remove(ID);
    DataManager.Instance.Followers_Demons_Types.RemoveAt(index1);
  }
}
