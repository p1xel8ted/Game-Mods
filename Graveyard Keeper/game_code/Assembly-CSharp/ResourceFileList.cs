// Decompiled with JetBrains decompiler
// Type: ResourceFileList
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
[CreateAssetMenu]
public class ResourceFileList : ScriptableObject
{
  public string file_type = "prefab";
  [HideInInspector]
  public bool can_be_rescanned = true;
  public List<string> folders = new List<string>();
  public List<string> files = new List<string>();
}
