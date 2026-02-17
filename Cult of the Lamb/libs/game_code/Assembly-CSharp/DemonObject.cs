// Decompiled with JetBrains decompiler
// Type: DemonObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class DemonObject
{
  public List<string> Paths = new List<string>()
  {
    "Prefabs/Units/Demons/Demon_Shooty",
    "Prefabs/Units/Demons/Demon_Arrows",
    "Prefabs/Units/Demons/Demon_Chomp"
  };
  public string FilePath;
  public int RandomDemon;

  public void SetRandomDemon()
  {
    this.RandomDemon = UnityEngine.Random.Range(0, this.Paths.Count);
    this.FilePath = this.Paths[this.RandomDemon];
  }
}
