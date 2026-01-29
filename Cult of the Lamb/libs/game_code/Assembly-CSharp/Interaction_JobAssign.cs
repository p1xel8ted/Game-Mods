// Decompiled with JetBrains decompiler
// Type: Interaction_JobAssign
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (WorkPlace))]
public class Interaction_JobAssign : Interaction
{
  public string Name;
  public string Description;
  public WorkPlace workPlace;

  public void Start() => this.workPlace = this.GetComponent<WorkPlace>();

  public override void GetLabel()
  {
    if (DataManager.Instance.Followers.Count < 1 || !((Object) this.workPlace != (Object) null))
      return;
    int num = (Object) Worshipper.GetWorshipperByJobID(this.workPlace.ID) != (Object) null ? 1 : 0;
  }

  public override void OnInteract(StateMachine state)
  {
    if (!(this.Label != ""))
      return;
    GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasMenuList>().JobAssignMenu.GetComponent<JobAssignMenu>().Show(this.Name, this.Description, this.workPlace);
  }
}
