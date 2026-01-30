// Decompiled with JetBrains decompiler
// Type: src.Interactions.Interaction_SoldOutSign
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
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
