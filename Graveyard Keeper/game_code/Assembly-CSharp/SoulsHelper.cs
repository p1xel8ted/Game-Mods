// Decompiled with JetBrains decompiler
// Type: SoulsHelper
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

#nullable disable
public static class SoulsHelper
{
  public const string SOULS_SINS_COUNT_RES = "sins_count";
  public const float BASE_SOUL_REWARD = 5f;
  public const float BASE_SIN_REWARD = 5f;

  public static float CalculatePointsAfterSoulRelease(Item healed_soul)
  {
    float num1 = healed_soul.durability;
    float num2 = healed_soul.GetParam("sins_count");
    if ((double) num1 > 0.89999997615814209)
      num1 = 1f;
    return (float) ((double) num1 * 5.0 + (double) num2 * 5.0);
  }
}
