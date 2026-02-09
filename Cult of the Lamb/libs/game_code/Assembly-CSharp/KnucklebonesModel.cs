// Decompiled with JetBrains decompiler
// Type: KnucklebonesModel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
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
