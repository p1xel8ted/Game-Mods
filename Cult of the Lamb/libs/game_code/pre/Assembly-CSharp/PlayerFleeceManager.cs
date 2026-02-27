// Decompiled with JetBrains decompiler
// Type: PlayerFleeceManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Lamb.UI.DeathScreen;

#nullable disable
public class PlayerFleeceManager
{
  private static float damageMultiplier = 1f;
  public static PlayerFleeceManager.DamageEvent OnDamageMultiplierModified;

  public static float GetCursesDamageMultiplier()
  {
    switch (DataManager.Instance.PlayerFleece)
    {
      case 1:
        return PlayerFleeceManager.damageMultiplier;
      case 2:
        return 1f;
      default:
        return 1f;
    }
  }

  public static float GetCursesFeverMultiplier()
  {
    return DataManager.Instance.PlayerFleece == 2 ? 0.5f : 1f;
  }

  public static float GetWeaponDamageMultiplier()
  {
    switch (DataManager.Instance.PlayerFleece)
    {
      case 1:
        return PlayerFleeceManager.damageMultiplier;
      case 2:
        return -0.5f;
      default:
        return 0.0f;
    }
  }

  public static float GetDamageReceivedMultiplier()
  {
    return DataManager.Instance.PlayerFleece == 1 ? 1f : 0.0f;
  }

  public static float GetHealthMultiplier() => DataManager.Instance.PlayerFleece == 2 ? 0.5f : 1f;

  public static float GetLootMultiplier(UIDeathScreenOverlayController.Results _result)
  {
    return DataManager.Instance.PlayerFleece == 3 && _result == UIDeathScreenOverlayController.Results.Killed ? -100f : 0.0f;
  }

  public static void OnTarotCardPickedUp()
  {
    if (DataManager.Instance.PlayerFleece != 3 || (double) PlayerFarming.Instance.health.BlackHearts > 0.0)
      return;
    PlayerFarming.Instance.health.BlackHearts = 2f;
  }

  public static int GetFreeTarotCards() => DataManager.Instance.PlayerFleece == 4 ? 4 : 0;

  public static void IncrementDamageModifier()
  {
    if (DataManager.Instance.PlayerFleece != 1)
      return;
    PlayerFleeceManager.damageMultiplier += 0.05f;
    PlayerFleeceManager.DamageEvent multiplierModified = PlayerFleeceManager.OnDamageMultiplierModified;
    if (multiplierModified == null)
      return;
    multiplierModified(PlayerFleeceManager.damageMultiplier);
  }

  public static void ResetDamageModifier()
  {
    PlayerFleeceManager.damageMultiplier = 0.0f;
    PlayerFleeceManager.DamageEvent multiplierModified = PlayerFleeceManager.OnDamageMultiplierModified;
    if (multiplierModified == null)
      return;
    multiplierModified(PlayerFleeceManager.damageMultiplier);
  }

  public delegate void DamageEvent(float damageMultiplier);
}
