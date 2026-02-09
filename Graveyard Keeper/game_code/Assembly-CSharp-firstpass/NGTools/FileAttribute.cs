// Decompiled with JetBrains decompiler
// Type: NGTools.FileAttribute
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace NGTools;

public class FileAttribute : PropertyAttribute
{
  public FileAttribute.Mode mode;
  public string extension;

  public FileAttribute(FileAttribute.Mode mode, string extension)
  {
    this.mode = mode;
    this.extension = extension;
  }

  public enum Mode
  {
    Open,
    Save,
  }
}
