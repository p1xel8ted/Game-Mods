// Decompiled with JetBrains decompiler
// Type: Interaction_SimpleInteraction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;

#nullable disable
public class Interaction_SimpleInteraction : Interaction
{
  public string sLabel;
  public string sLabelDefault = "";

  public virtual void Start() => this.UpdateLocalisation();

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sLabel = string.IsNullOrEmpty(this.sLabelDefault) ? ScriptLocalization.FollowerInteractions.MakeDemand : this.sLabelDefault;
  }

  public override void GetLabel()
  {
    base.GetLabel();
    this.Label = this.sLabel;
  }
}
