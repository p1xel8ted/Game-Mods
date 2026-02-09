// Decompiled with JetBrains decompiler
// Type: Structures_Shrine_Ratau
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class Structures_Shrine_Ratau : StructureBrain
{
  public override int SoulMax
  {
    get
    {
      switch (PlayerFarming.Location)
      {
        case FollowerLocation.Sozo:
        case FollowerLocation.Sozo_Cave:
          return 30;
        default:
          return 15;
      }
    }
  }

  public float TimeBetweenSouls => 600f;
}
