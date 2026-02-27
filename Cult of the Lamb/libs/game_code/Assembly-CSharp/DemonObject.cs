// Decompiled with JetBrains decompiler
// Type: DemonObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
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
