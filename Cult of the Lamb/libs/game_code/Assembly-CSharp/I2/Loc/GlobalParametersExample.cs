// Decompiled with JetBrains decompiler
// Type: I2.Loc.GlobalParametersExample
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
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
