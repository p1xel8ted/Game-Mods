// Decompiled with JetBrains decompiler
// Type: CharDefinition
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class CharDefinition : BalanceBaseObject
{
  public List<string> jobs_can_do = new List<string>();
  public float time_k = 1f;
  public GameRes perc_vector = new GameRes();
}
