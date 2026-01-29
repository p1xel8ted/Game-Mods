// Decompiled with JetBrains decompiler
// Type: I2.Loc.LanguageData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;

#nullable disable
namespace I2.Loc;

[Serializable]
public class LanguageData
{
  public string Name;
  public string Code;
  public byte Flags;
  [NonSerialized]
  public bool Compressed;

  public bool IsEnabled() => ((int) this.Flags & 1) == 0;

  public void SetEnabled(bool bEnabled)
  {
    if (bEnabled)
      this.Flags &= (byte) 254;
    else
      this.Flags |= (byte) 1;
  }

  public bool IsLoaded() => ((int) this.Flags & 4) == 0;

  public bool CanBeUnloaded() => ((int) this.Flags & 2) == 0;

  public void SetLoaded(bool loaded)
  {
    if (loaded)
      this.Flags &= (byte) 251;
    else
      this.Flags |= (byte) 4;
  }

  public void SetCanBeUnLoaded(bool allowUnloading)
  {
    if (allowUnloading)
      this.Flags &= (byte) 253;
    else
      this.Flags |= (byte) 2;
  }
}
