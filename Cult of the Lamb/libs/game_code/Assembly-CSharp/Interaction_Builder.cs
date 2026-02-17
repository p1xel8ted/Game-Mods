// Decompiled with JetBrains decompiler
// Type: Interaction_Builder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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
