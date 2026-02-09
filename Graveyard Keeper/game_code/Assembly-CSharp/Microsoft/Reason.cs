// Decompiled with JetBrains decompiler
// Type: Microsoft.Reason
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

#nullable disable
namespace Microsoft;

public class Reason
{
  public ushort code;
  public string reason;
  public bool isClose;

  public Reason(ushort code, string reason, bool isClose)
  {
    this.code = code;
    this.reason = reason;
    this.isClose = true;
  }
}
