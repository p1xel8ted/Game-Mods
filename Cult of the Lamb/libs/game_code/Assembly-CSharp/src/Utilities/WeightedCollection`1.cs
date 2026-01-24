// Decompiled with JetBrains decompiler
// Type: src.Utilities.WeightedCollection`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace src.Utilities;

public class WeightedCollection<T> : IEnumerable, IEnumerator<T>, IEnumerator, IDisposable
{
  public List<WeightedCollection<T>.WeightedElement> _contents = new List<WeightedCollection<T>.WeightedElement>();
  public int _position = -1;

  public int Count => this._contents.Count;

  public T this[int index]
  {
    get => this._contents[index].Element;
    set => this._contents[index].Element = value;
  }

  public void Add(T element, float weight)
  {
    this.Add(new WeightedCollection<T>.WeightedElement()
    {
      Element = element,
      Weight = weight
    });
  }

  public void Add(
    WeightedCollection<T>.WeightedElement weightedElement)
  {
    if (this.Contains(weightedElement))
      this._contents.Find((Predicate<WeightedCollection<T>.WeightedElement>) (e => e.Element.Equals((object) weightedElement.Element))).Weight += weightedElement.Weight;
    else
      this._contents.Add(weightedElement);
  }

  public void Remove(T element)
  {
    if (!this.Contains(element))
      return;
    this._contents.Remove(this._contents.Find((Predicate<WeightedCollection<T>.WeightedElement>) (e => e.Element.Equals((object) element))));
  }

  public bool Contains(
    WeightedCollection<T>.WeightedElement weightedElement)
  {
    return this.Contains(weightedElement.Element);
  }

  public bool Contains(T element)
  {
    return this._contents.Any<WeightedCollection<T>.WeightedElement>((Func<WeightedCollection<T>.WeightedElement, bool>) (e => e.Element.Equals((object) element)));
  }

  public T GetRandomItem() => this._contents[this.GetRandomIndex()].Element;

  public int GetRandomIndex()
  {
    List<float> distributedWeights = this.GetDistributedWeights();
    float rand = UnityEngine.Random.Range(0.0f, distributedWeights.Last<float>());
    return distributedWeights.IndexOf(distributedWeights.First<float>((Func<float, bool>) (weight => (double) rand < (double) weight)));
  }

  public List<float> GetDistributedWeights()
  {
    List<float> distributedWeights = new List<float>()
    {
      this._contents[0].Weight
    };
    for (int index = 1; index < this.Count; ++index)
      distributedWeights.Add(distributedWeights[index - 1] + this._contents[index].Weight);
    return distributedWeights;
  }

  public void Clear() => this._contents.Clear();

  public IEnumerator GetEnumerator() => (IEnumerator) this;

  public bool MoveNext()
  {
    ++this._position;
    return this._position < this._contents.Count;
  }

  public void Reset() => this._position = -1;

  public T Current => this._contents[this._position].Element;

  object IEnumerator.Current => (object) this.Current;

  public void Dispose()
  {
  }

  public class WeightedElement
  {
    public T Element;
    public float Weight;
  }
}
