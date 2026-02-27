// Decompiled with JetBrains decompiler
// Type: Interaction_DropBody
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using UnityEngine;

#nullable disable
public class Interaction_DropBody : Interaction
{
  private bool Activating;
  private string sString;

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sString = ScriptLocalization.Interactions.Drop;
  }

  private void Start()
  {
    this.IgnoreTutorial = true;
    this.UpdateLocalisation();
    this.GetLabel();
  }

  public override void GetLabel() => this.Label = this.sString;

  public override void OnInteract(StateMachine state)
  {
    if (this.Activating)
      return;
    base.OnInteract(state);
    this.enabled = false;
    this.Activating = true;
  }

  private new void Update()
  {
    if (!((Object) PlayerFarming.Instance != (Object) null))
      return;
    this.gameObject.transform.position = PlayerFarming.Instance.transform.position;
    this.GetLabel();
  }
}
