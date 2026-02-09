// Decompiled with JetBrains decompiler
// Type: ESCFolderSplitterByNamePrefix
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ESCFolderSplitterByNamePrefix : ESCFolderSplitter
{
  [SerializeField]
  public List<ESCFolderSplitterByNamePrefix.ESCGroupDescription> groups = new List<ESCFolderSplitterByNamePrefix.ESCGroupDescription>();

  public override int GetCollectionID(string filename)
  {
    int collectionId = 0;
    foreach (ESCFolderSplitterByNamePrefix.ESCGroupDescription group in this.groups)
    {
      foreach (string str in group.items)
      {
        if (filename.IndexOf(str, StringComparison.Ordinal) == 0)
          return collectionId;
      }
      ++collectionId;
    }
    return -1;
  }

  [Serializable]
  public class ESCGroupDescription
  {
    [SerializeField]
    public List<string> items = new List<string>();
  }
}
