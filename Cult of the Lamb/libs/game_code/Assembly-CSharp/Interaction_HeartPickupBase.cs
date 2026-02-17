// Decompiled with JetBrains decompiler
// Type: Interaction_HeartPickupBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class Interaction_HeartPickupBase : Interaction
{
  public void DoRevive(
    int health,
    Interaction_HeartPickupBase.HeartPickupType heartPickupType)
  {
    if (!this.playerFarming.IsKnockedOut)
      return;
    CoopManager.WakeKnockedOutPlayer(this.playerFarming, (float) health, heartType: heartPickupType, pauseTime: true);
  }

  public enum HeartPickupType
  {
    Red,
    Blue,
    Black,
    Fire,
    Ice,
  }
}
