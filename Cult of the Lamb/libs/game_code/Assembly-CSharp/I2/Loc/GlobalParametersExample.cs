// Decompiled with JetBrains decompiler
// Type: I2.Loc.GlobalParametersExample
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
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
