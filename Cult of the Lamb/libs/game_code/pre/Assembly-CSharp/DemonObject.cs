// Decompiled with JetBrains decompiler
// Type: DemonObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class DemonObject
{
  private List<string> Paths = new List<string>()
  {
    "Prefabs/Units/Demons/Demon_Shooty",
    "Prefabs/Units/Demons/Demon_Arrows",
    "Prefabs/Units/Demons/Demon_Chomp"
  };
  public string FilePath;
  private int RandomDemon;

  public void SetRandomDemon()
  {
    this.RandomDemon = UnityEngine.Random.Range(0, this.Paths.Count);
    this.FilePath = this.Paths[this.RandomDemon];
  }
}
