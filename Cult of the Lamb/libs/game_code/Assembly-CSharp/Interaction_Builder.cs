// Decompiled with JetBrains decompiler
// Type: Interaction_Builder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
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
