// Decompiled with JetBrains decompiler
// Type: KnucklebonesModel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
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
