// Decompiled with JetBrains decompiler
// Type: src.Interactions.Interaction_SoldOutSign
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;

#nullable disable
namespace src.Interactions;

public class Interaction_SoldOutSign : Interaction
{
  public string sSoldOut;

  public void Start() => this.UpdateLocalisation();

  public override void GetLabel() => this.Label = this.sSoldOut;

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sSoldOut = ScriptLocalization.Interactions.SoldOut;
  }

  public override void OnBecomeCurrent(PlayerFarming playerFarming)
  {
    base.OnBecomeCurrent(playerFarming);
    this.Interactable = false;
  }
}
