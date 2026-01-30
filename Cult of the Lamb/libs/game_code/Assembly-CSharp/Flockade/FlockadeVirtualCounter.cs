// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeVirtualCounter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;

#nullable disable
namespace Flockade;

public class FlockadeVirtualCounter : IFlockadeCounter
{
  [CompilerGenerated]
  public int \u003CCount\u003Ek__BackingField;

  public FlockadeVirtualCounter(int count) => this.Count = count;

  public int Count
  {
    get => this.\u003CCount\u003Ek__BackingField;
    set => this.\u003CCount\u003Ek__BackingField = value;
  }
}
