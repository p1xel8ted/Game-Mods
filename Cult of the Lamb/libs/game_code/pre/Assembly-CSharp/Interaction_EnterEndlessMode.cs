// Decompiled with JetBrains decompiler
// Type: Interaction_EnterEndlessMode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;

#nullable disable
public class Interaction_EnterEndlessMode : Interaction
{
  private string ComingSoon;

  private void Start() => this.UpdateLocalisation();

  public override void UpdateLocalisation()
  {
    this.ComingSoon = ScriptLocalization.Interactions.ComingSoon;
  }

  public override void GetLabel() => this.Label = this.ComingSoon;
}
