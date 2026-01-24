// Decompiled with JetBrains decompiler
// Type: Structures_Crypt
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class Structures_Crypt : StructureBrain
{
  public override bool IsFull => this.Data.MultipleFollowerIDs.Count >= this.DEAD_BODY_SLOTS;

  public int DEAD_BODY_SLOTS => Structures_Crypt.GetCapacity(this.Data.Type);

  public static int GetCapacity(StructureBrain.TYPES Type)
  {
    if (Type == StructureBrain.TYPES.CRYPT_2)
      return 8;
    return Type == StructureBrain.TYPES.CRYPT_3 ? 12 : 5;
  }

  public void DepositBody(int followerID)
  {
    if (this.Data.MultipleFollowerIDs.Contains(followerID))
      return;
    this.Data.MultipleFollowerIDs.Add(followerID);
  }

  public void WithdrawBody(int followerID)
  {
    if (!this.Data.MultipleFollowerIDs.Contains(followerID))
      return;
    this.Data.MultipleFollowerIDs.Remove(followerID);
  }

  public override int SoulMax => 10 * this.FollowersFuneralCount();

  public float TimeBetweenSouls => 360f;

  public int FollowersFuneralCount()
  {
    int num = 0;
    for (int index = 0; index < this.Data.MultipleFollowerIDs.Count; ++index)
    {
      FollowerInfo infoById = FollowerInfo.GetInfoByID(this.Data.MultipleFollowerIDs[index], true);
      if (infoById != null && infoById.HadFuneral)
        ++num;
    }
    return num;
  }
}
