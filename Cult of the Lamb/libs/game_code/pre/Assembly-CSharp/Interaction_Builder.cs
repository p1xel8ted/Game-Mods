// Decompiled with JetBrains decompiler
// Type: Interaction_Builder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_Builder : Interaction
{
  private List<StructuresData> StructuresList = new List<StructuresData>();
  private Structure structure;

  private void Start() => this.structure = this.GetComponent<Structure>();

  public override void OnInteract(StateMachine state)
  {
    GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasMenuList>();
  }
}
