// Decompiled with JetBrains decompiler
// Type: Unity.Cloud.CyclicalList`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Unity.Cloud;

public class CyclicalList<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
{
  public int count;
  public T[] items;
  public int nextPointer;

  public CyclicalList(int capacity) => this.items = new T[capacity];

  public int Capacity => this.items.Length;

  public int Count => this.count;

  public bool IsReadOnly => false;

  public T this[int index]
  {
    get
    {
      return index >= 0 && index < this.count ? this.items[this.GetPointer(index)] : throw new IndexOutOfRangeException();
    }
    set
    {
      if (index < 0 || index >= this.count)
        throw new IndexOutOfRangeException();
      this.items[this.GetPointer(index)] = value;
    }
  }

  public void Add(T item)
  {
    this.items[this.nextPointer] = item;
    ++this.count;
    if (this.count > this.items.Length)
      this.count = this.items.Length;
    ++this.nextPointer;
    if (this.nextPointer < this.items.Length)
      return;
    this.nextPointer = 0;
  }

  public void Clear()
  {
    this.count = 0;
    this.nextPointer = 0;
  }

  public bool Contains(T item)
  {
    foreach (T obj in this)
    {
      if (obj.Equals((object) item))
        return true;
    }
    return false;
  }

  public void CopyTo(T[] array, int arrayIndex)
  {
    int num = 0;
    foreach (T obj in this)
    {
      int index = arrayIndex + num;
      if (index >= array.Length)
        break;
      array[index] = obj;
      ++num;
    }
  }

  public IEnumerator<T> GetEnumerator() => (IEnumerator<T>) new CyclicalList<T>.Enumerator(this);

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

  public T GetNextEviction() => this.items[this.nextPointer];

  public int GetPointer(int index)
  {
    if (index < 0 || index >= this.count)
      throw new IndexOutOfRangeException();
    return this.count < this.items.Length ? index : (this.nextPointer + index) % this.count;
  }

  public int IndexOf(T item)
  {
    int num = 0;
    foreach (T obj in this)
    {
      if (obj.Equals((object) item))
        return num;
      ++num;
    }
    return -1;
  }

  public void Insert(int index, T item)
  {
    if (index < 0 || index >= this.count)
      throw new IndexOutOfRangeException();
  }

  public bool Remove(T item) => false;

  public void RemoveAt(int index)
  {
    if (index < 0 || index >= this.count)
      throw new IndexOutOfRangeException();
  }

  public struct Enumerator(CyclicalList<T> list) : IEnumerator<T>, IEnumerator, IDisposable
  {
    public int currentIndex = -1;
    public CyclicalList<T> list = list;

    public T Current
    {
      get
      {
        return this.currentIndex < 0 || this.currentIndex >= this.list.Count ? default (T) : this.list[this.currentIndex];
      }
    }

    object IEnumerator.Current => (object) this.Current;

    public void Dispose()
    {
    }

    public bool MoveNext()
    {
      ++this.currentIndex;
      return this.currentIndex < this.list.count;
    }

    public void Reset() => this.currentIndex = 0;
  }
}
