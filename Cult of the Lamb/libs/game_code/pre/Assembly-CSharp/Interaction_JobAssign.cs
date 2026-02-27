// Decompiled with JetBrains decompiler
// Type: Interaction_JobAssign
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (WorkPlace))]
public class Interaction_JobAssign : Interaction
{
  public string Name;
  public string Description;
  private WorkPlace workPlace;

  private void Start() => this.workPlace = this.GetComponent<WorkPlace>();

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
