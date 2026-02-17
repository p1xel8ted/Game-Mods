// Decompiled with JetBrains decompiler
// Type: FollowerState_Drunk
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerState_Drunk : FollowerState
{
  public int StateVariation;

  public override FollowerStateType Type => FollowerStateType.Drunk;

  public override float XPMultiplierAddition => -0.5f;

  public override float MaxSpeed => 0.75f;

  public bool AngryDrunk
  {
    get
    {
      return this.brain != null && (this.brain.HasTrait(FollowerTrait.TraitType.Argumentative) || this.brain.HasTrait(FollowerTrait.TraitType.Bastard)) || this.StateVariation < 2;
    }
  }

  public bool SadDrunk
  {
    get
    {
      return this.brain != null && (this.brain.HasTrait(FollowerTrait.TraitType.Scared) || this.brain.HasTrait(FollowerTrait.TraitType.Terrified) || this.brain.HasTrait(FollowerTrait.TraitType.CriminalScarred) || this.brain.HasTrait(FollowerTrait.TraitType.MissionaryTerrified)) || this.StateVariation < 4;
    }
  }

  public FollowerState_Drunk() => this.StateVariation = Random.Range(0, 10);

  public override string OverrideIdleAnim
  {
    get
    {
      if (this.AngryDrunk)
        return "Drinking/idle-drunk-angry";
      return this.SadDrunk ? "Drinking/idle-drunk-sad" : "Drinking/idle-drunk-happy";
    }
  }

  public override string OverrideWalkAnim
  {
    get
    {
      if (this.AngryDrunk)
        return "Drinking/walk-drunk-angry";
      return this.SadDrunk ? "Drinking/walk-drunk-sad" : "Drinking/walk-drunk-happy";
    }
  }
}
