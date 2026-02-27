// Decompiled with JetBrains decompiler
// Type: Interaction_Builder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_Builder : Interaction
{
  public List<StructuresData> StructuresList = new List<StructuresData>();
  public Structure structure;

  public void Start() => this.structure = this.GetComponent<Structure>();

  public override void OnInteract(StateMachine state)
  {
    GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasMenuList>();
  }
}
