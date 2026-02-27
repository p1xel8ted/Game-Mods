// Decompiled with JetBrains decompiler
// Type: Interaction_WorshipperPickUp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Interaction_WorshipperPickUp : Interaction
{
  private WorshipperInfoManager character;
  private Worshipper worshipper;
  private new StateMachine state;

  private void Start()
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
