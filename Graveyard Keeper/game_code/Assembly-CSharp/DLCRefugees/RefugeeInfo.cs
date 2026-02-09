// Decompiled with JetBrains decompiler
// Type: DLCRefugees.RefugeeInfo
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;

#nullable disable
namespace DLCRefugees;

[Serializable]
public class RefugeeInfo
{
  public string obj_id;
  public string custom_tag;

  public RefugeeInfo()
  {
  }

  public RefugeeInfo(string obj_id, string custom_tag)
  {
    this.obj_id = obj_id;
    this.custom_tag = custom_tag;
  }
}
