// Decompiled with JetBrains decompiler
// Type: Interaction_DwellingAssign
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (Dwelling))]
public class Interaction_DwellingAssign : Interaction
{
  public string Name;
  public string Description;
  private Dwelling dwelling;

  private void Start() => this.dwelling = this.GetComponent<Dwelling>();

  public override void GetLabel()
  {
    if (DataManager.Instance.Followers.Count < 1)
    {
      this.Label = "";
    }
    else
    {
      if (!((Object) this.dwelling != (Object) null))
        return;
      int num = (Object) null != (Object) null ? 1 : 0;
    }
  }

  public override void OnInteract(StateMachine state)
  {
    if (!(this.Label != ""))
      return;
    GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasMenuList>().DwellingAssignMenu.GetComponent<DwellingAssignMenu>().Show(this.Name, this.Description, this.dwelling);
  }
}
