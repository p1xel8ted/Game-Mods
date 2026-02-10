// Decompiled with JetBrains decompiler
// Type: I2.Loc.GlobalParametersExample
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
namespace I2.Loc;

public class GlobalParametersExample : RegisterGlobalParameters
{
  public override string GetParameterValue(string ParamName)
  {
    switch (ParamName)
    {
      case "WINNER":
        return "Javier";
      case "NUM PLAYERS":
        return 5.ToString();
      default:
        return (string) null;
    }
  }
}
