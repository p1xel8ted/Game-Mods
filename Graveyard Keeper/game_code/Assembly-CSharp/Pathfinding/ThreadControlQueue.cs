// Decompiled with JetBrains decompiler
// Type: Pathfinding.ThreadControlQueue
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Threading;

#nullable disable
namespace Pathfinding;

public class ThreadControlQueue
{
  public Path head;
  public Path tail;
  public object lockObj = new object();
  public int numReceivers;
  public bool blocked;
  public int blockedReceivers;
  public bool starving;
  public bool terminate;
  public ManualResetEvent block = new ManualResetEvent(true);

  public ThreadControlQueue(int numReceivers) => this.numReceivers = numReceivers;

  public bool IsEmpty => this.head == null;

  public bool IsTerminating => this.terminate;

  public void Block()
  {
    lock (this.lockObj)
    {
      this.blocked = true;
      this.block.Reset();
    }
  }

  public void Unblock()
  {
    lock (this.lockObj)
    {
      this.blocked = false;
      this.block.Set();
    }
  }

  public void Lock() => Monitor.Enter(this.lockObj);

  public void Unlock() => Monitor.Exit(this.lockObj);

  public bool AllReceiversBlocked
  {
    get
    {
      lock (this.lockObj)
        return this.blocked && this.blockedReceivers == this.numReceivers;
    }
  }

  public void PushFront(Path p)
  {
    lock (this.lockObj)
    {
      if (this.terminate)
        return;
      if (this.tail == null)
      {
        this.head = p;
        this.tail = p;
        if (this.starving && !this.blocked)
        {
          this.starving = false;
          this.block.Set();
        }
        else
          this.starving = false;
      }
      else
      {
        p.next = this.head;
        this.head = p;
      }
    }
  }

  public void Push(Path p)
  {
    lock (this.lockObj)
    {
      if (this.terminate)
        return;
      if (this.tail == null)
      {
        this.head = p;
        this.tail = p;
        if (this.starving && !this.blocked)
        {
          this.starving = false;
          this.block.Set();
        }
        else
          this.starving = false;
      }
      else
      {
        this.tail.next = p;
        this.tail = p;
      }
    }
  }

  public void Starving()
  {
    this.starving = true;
    this.block.Reset();
  }

  public void TerminateReceivers()
  {
    lock (this.lockObj)
    {
      this.terminate = true;
      this.block.Set();
    }
  }

  public Path Pop()
  {
    Monitor.Enter(this.lockObj);
    try
    {
      if (this.terminate)
      {
        ++this.blockedReceivers;
        throw new ThreadControlQueue.QueueTerminationException();
      }
      if (this.head == null)
        this.Starving();
      while (this.blocked || this.starving)
      {
        ++this.blockedReceivers;
        if (this.blockedReceivers != this.numReceivers && this.blockedReceivers > this.numReceivers)
          throw new InvalidOperationException($"More receivers are blocked than specified in constructor ({this.blockedReceivers.ToString()} > {this.numReceivers.ToString()})");
        Monitor.Exit(this.lockObj);
        this.block.WaitOne();
        Monitor.Enter(this.lockObj);
        if (this.terminate)
          throw new ThreadControlQueue.QueueTerminationException();
        --this.blockedReceivers;
        if (this.head == null)
          this.Starving();
      }
      Path head = this.head;
      if (this.head.next == null)
        this.tail = (Path) null;
      this.head = this.head.next;
      return head;
    }
    finally
    {
      Monitor.Exit(this.lockObj);
    }
  }

  public void ReceiverTerminated()
  {
    Monitor.Enter(this.lockObj);
    ++this.blockedReceivers;
    Monitor.Exit(this.lockObj);
  }

  public Path PopNoBlock(bool blockedBefore)
  {
    Monitor.Enter(this.lockObj);
    try
    {
      if (this.terminate)
      {
        ++this.blockedReceivers;
        throw new ThreadControlQueue.QueueTerminationException();
      }
      if (this.head == null)
        this.Starving();
      if (this.blocked || this.starving)
      {
        if (!blockedBefore)
        {
          ++this.blockedReceivers;
          if (this.terminate)
            throw new ThreadControlQueue.QueueTerminationException();
          if (this.blockedReceivers != this.numReceivers && this.blockedReceivers > this.numReceivers)
            throw new InvalidOperationException($"More receivers are blocked than specified in constructor ({this.blockedReceivers.ToString()} > {this.numReceivers.ToString()})");
        }
        return (Path) null;
      }
      if (blockedBefore)
        --this.blockedReceivers;
      Path head = this.head;
      if (this.head.next == null)
        this.tail = (Path) null;
      this.head = this.head.next;
      return head;
    }
    finally
    {
      Monitor.Exit(this.lockObj);
    }
  }

  public class QueueTerminationException : Exception
  {
  }
}
