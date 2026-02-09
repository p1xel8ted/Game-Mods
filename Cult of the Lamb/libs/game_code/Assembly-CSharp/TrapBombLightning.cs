// Decompiled with JetBrains decompiler
// Type: TrapBombLightning
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class TrapBombLightning : TrapBomb
{
  public override void Explode(int Size)
  {
    LightningRingExplosion.CreateExplosion(this.transform.position, Health.Team.KillAll, this.health, Team2Damage: (float) this.Team2Damage);
  }
}
