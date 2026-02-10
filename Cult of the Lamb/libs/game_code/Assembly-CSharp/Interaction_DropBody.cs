// Decompiled with JetBrains decompiler
// Type: Interaction_DropBody
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using UnityEngine;

#nullable disable
public class Interaction_DropBody : Interaction
{
  public bool Activating;
  public string sString;

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sString = ScriptLocalization.Interactions.Drop;
  }

  public void Start()
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

  public override void Update()
  {
    base.Update();
    if (!((Object) this.playerFarming != (Object) null))
      return;
    this.gameObject.transform.position = this.playerFarming.transform.position;
    this.GetLabel();
  }
}
