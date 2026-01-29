// Decompiled with JetBrains decompiler
// Type: Interaction_WorshipperPickUp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Interaction_WorshipperPickUp : Interaction
{
  public WorshipperInfoManager character;
  public Worshipper worshipper;

  public void Start()
  {
    this.character = this.GetComponent<WorshipperInfoManager>();
    this.worshipper = this.GetComponent<Worshipper>();
    this.state = this.GetComponent<StateMachine>();
  }

  public override void GetLabel()
  {
    if ((Object) this.character == (Object) null)
      this.Label = "";
    else if (this.character.v_i == null)
      this.Label = "";
    else
      this.Label = this.character.v_i.Name;
  }

  public override void OnInteract(StateMachine state) => base.OnInteract(state);
}
