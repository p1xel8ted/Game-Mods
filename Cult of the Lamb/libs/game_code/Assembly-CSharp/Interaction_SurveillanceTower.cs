// Decompiled with JetBrains decompiler
// Type: Interaction_SurveillanceTower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using UnityEngine;

#nullable disable
public class Interaction_SurveillanceTower : Interaction
{
  public GameObject SurveillanceScreenPrefab;
  public Structure Structure;
  public string sSurveillance;

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

  public void OnStructureRemoved(StructuresData structure)
  {
    if (structure.Type != StructureBrain.TYPES.SURVEILLANCE)
      return;
    DataManager.Instance.HasBuiltSurveillance = false;
  }

  public void OnBrainAssigned() => DataManager.Instance.HasBuiltSurveillance = true;

  public void Start() => this.UpdateLocalisation();

  public override void OnInteract(StateMachine state) => base.OnInteract(state);

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sSurveillance = ScriptLocalization.Interactions.Surveillance;
  }

  public override void GetLabel() => this.Label = this.sSurveillance;
}
