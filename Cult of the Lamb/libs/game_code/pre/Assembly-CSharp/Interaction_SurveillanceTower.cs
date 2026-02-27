// Decompiled with JetBrains decompiler
// Type: Interaction_SurveillanceTower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using UnityEngine;

#nullable disable
public class Interaction_SurveillanceTower : Interaction
{
  public GameObject SurveillanceScreenPrefab;
  private Structure Structure;
  private string sSurveillance;

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.ActivateDistance = 2f;
    this.Structure = this.GetComponent<Structure>();
    this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    StructureManager.OnStructureRemoved += new StructureManager.StructureChanged(this.OnStructureRemoved);
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    StructureManager.OnStructureRemoved -= new StructureManager.StructureChanged(this.OnStructureRemoved);
  }

  private void OnStructureRemoved(StructuresData structure)
  {
    if (structure.Type != StructureBrain.TYPES.SURVEILLANCE)
      return;
    DataManager.Instance.HasBuiltSurveillance = false;
  }

  private void OnBrainAssigned() => DataManager.Instance.HasBuiltSurveillance = true;

  private void Start() => this.UpdateLocalisation();

  public override void OnInteract(StateMachine state) => base.OnInteract(state);

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sSurveillance = ScriptLocalization.Interactions.Surveillance;
  }

  public override void GetLabel() => this.Label = this.sSurveillance;
}
