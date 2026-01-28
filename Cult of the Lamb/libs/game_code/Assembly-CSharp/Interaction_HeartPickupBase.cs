// Decompiled with JetBrains decompiler
// Type: Interaction_HeartPickupBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
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
