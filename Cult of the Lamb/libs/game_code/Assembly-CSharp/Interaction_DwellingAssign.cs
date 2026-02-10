// Decompiled with JetBrains decompiler
// Type: Interaction_DwellingAssign
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (Dwelling))]
public class Interaction_DwellingAssign : Interaction
{
  public string Name;
  public string Description;
  public Dwelling dwelling;

  public void Start() => this.dwelling = this.GetComponent<Dwelling>();

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
