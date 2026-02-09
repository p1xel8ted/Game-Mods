// Decompiled with JetBrains decompiler
// Type: RuntimeValueAttribute
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class RuntimeValueAttribute : PropertyAttribute
{
  public string header;
  public bool read_only;

  public RuntimeValueAttribute()
  {
    this.header = "";
    this.read_only = false;
  }

  public RuntimeValueAttribute(bool read_only)
  {
    this.header = "";
    this.read_only = read_only;
  }

  public RuntimeValueAttribute(string header, bool read_only = false)
  {
    this.header = header;
    this.read_only = read_only;
  }
}
