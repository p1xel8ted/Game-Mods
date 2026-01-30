// Decompiled with JetBrains decompiler
// Type: KnucklebonesModel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;

#nullable disable
public class KnucklebonesModel
{
  public const int NumDice = 3;
  public const int MaxOpponentDifficulty = 10;

  public static string GetLocalizedString(string str)
  {
    return LocalizationManager.GetTranslation("UI/Knucklebones/" + str);
  }
}
