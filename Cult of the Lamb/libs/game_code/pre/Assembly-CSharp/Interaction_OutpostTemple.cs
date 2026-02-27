// Decompiled with JetBrains decompiler
// Type: Interaction_OutpostTemple
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Interaction_OutpostTemple : Interaction
{
  public GameObject AssignMenu;
  private string sAssignFollowers;

  private void Start()
  {
    this.ActivateDistance = 2f;
    this.UpdateLocalisation();
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sAssignFollowers = "Assign Follower to outpost";
  }

  public override void GetLabel() => this.Label = this.sAssignFollowers;

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    Object.Instantiate<GameObject>(this.AssignMenu, GameObject.FindGameObjectWithTag("Canvas").transform);
  }
}
