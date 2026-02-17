// Decompiled with JetBrains decompiler
// Type: KnucklebonesModel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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
