// Decompiled with JetBrains decompiler
// Type: FollowerState_Nude
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class FollowerState_Nude : FollowerState
{
  public override FollowerStateType Type => FollowerStateType.Nude;

  public override string OverrideIdleAnim
  {
    get
    {
      if (this.brain == null || this.brain.Info == null || !this.brain.Info.IsDrunk)
        return "idle";
      if (this.brain.HasTrait(FollowerTrait.TraitType.Argumentative) || this.brain.HasTrait(FollowerTrait.TraitType.Bastard))
        return "Drinking/idle-drunk-angry";
      return this.brain.HasTrait(FollowerTrait.TraitType.Scared) || this.brain.HasTrait(FollowerTrait.TraitType.Terrified) || this.brain.HasTrait(FollowerTrait.TraitType.CriminalScarred) || this.brain.HasTrait(FollowerTrait.TraitType.MissionaryTerrified) ? "Drinking/idle-drunk-sad" : "Drinking/idle-drunk-happy";
    }
  }

  public override string OverrideWalkAnim
  {
    get
    {
      return this.brain != null && this.brain.Info != null && this.brain.Info.IsDrunk ? "prance-drunk" : "prance";
    }
  }
}
