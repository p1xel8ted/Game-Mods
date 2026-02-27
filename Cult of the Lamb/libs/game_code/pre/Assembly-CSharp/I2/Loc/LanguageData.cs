// Decompiled with JetBrains decompiler
// Type: I2.Loc.LanguageData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
