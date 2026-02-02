// Decompiled with JetBrains decompiler
// Type: Interaction_OutpostTemple
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Interaction_OutpostTemple : Interaction
{
  public GameObject AssignMenu;
  public string sAssignFollowers;

  public void Start()
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
