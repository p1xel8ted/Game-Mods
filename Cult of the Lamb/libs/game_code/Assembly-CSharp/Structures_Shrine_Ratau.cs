// Decompiled with JetBrains decompiler
// Type: Structures_Shrine_Ratau
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
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
