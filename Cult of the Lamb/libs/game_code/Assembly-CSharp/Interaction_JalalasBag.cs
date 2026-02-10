// Decompiled with JetBrains decompiler
// Type: Interaction_JalalasBag
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using UnityEngine;

#nullable disable
public class Interaction_JalalasBag : Interaction
{
  public override void GetLabel()
  {
    base.GetLabel();
    this.Label = ScriptLocalization.Interactions.PickUp;
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    BiomeConstants.Instance.EmitSmokeExplosionVFX(this.transform.position);
    DataManager.Instance.IsJalalaBag = false;
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.FindJalalaBag);
    Object.Destroy((Object) this.gameObject);
  }
}
