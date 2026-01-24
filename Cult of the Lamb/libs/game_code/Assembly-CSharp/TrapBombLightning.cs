// Decompiled with JetBrains decompiler
// Type: TrapBombLightning
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class TrapBombLightning : TrapBomb
{
  public override void Explode(int Size)
  {
    LightningRingExplosion.CreateExplosion(this.transform.position, Health.Team.KillAll, this.health, Team2Damage: (float) this.Team2Damage);
  }
}
