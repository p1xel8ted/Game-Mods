// Decompiled with JetBrains decompiler
// Type: Pathfinding.Util.LockFreeStack
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Threading;

#nullable disable
namespace Pathfinding.Util;

public class LockFreeStack
{
  public Path head;

  public void Push(Path p)
  {
    do
    {
      p.next = this.head;
    }
    while (Interlocked.CompareExchange<Path>(ref this.head, p, p.next) != p.next);
  }

  public Path PopAll() => Interlocked.Exchange<Path>(ref this.head, (Path) null);
}
