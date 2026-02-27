// Decompiled with JetBrains decompiler
// Type: I2.Loc.GlobalParametersExample
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
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
