// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Serialization.FullSerializer.Internal.fsCyclicReferenceManager
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable disable
namespace ParadoxNotion.Serialization.FullSerializer.Internal;

public class fsCyclicReferenceManager
{
  public Dictionary<object, int> _objectIds = new Dictionary<object, int>(fsCyclicReferenceManager.ObjectReferenceEqualityComparator.Instance);
  public int _nextId;
  public Dictionary<int, object> _marked = new Dictionary<int, object>();
  public int _depth;

  public void Enter() => ++this._depth;

  public bool Exit()
  {
    --this._depth;
    if (this._depth == 0)
    {
      this._objectIds = new Dictionary<object, int>(fsCyclicReferenceManager.ObjectReferenceEqualityComparator.Instance);
      this._nextId = 0;
      this._marked = new Dictionary<int, object>();
    }
    if (this._depth < 0)
    {
      this._depth = 0;
      throw new InvalidOperationException("Internal Error - Mismatched Enter/Exit");
    }
    return this._depth == 0;
  }

  public object GetReferenceObject(int id)
  {
    return this._marked.ContainsKey(id) ? this._marked[id] : throw new InvalidOperationException($"Internal Deserialization Error - Object definition has not been encountered for object with id={id.ToString()}; have you reordered or modified the serialized data? If this is an issue with an unmodified Full Json implementation and unmodified serialization data, please report an issue with an included test case.");
  }

  public void AddReferenceWithId(int id, object reference) => this._marked[id] = reference;

  public int GetReferenceId(object item)
  {
    int referenceId;
    if (!this._objectIds.TryGetValue(item, out referenceId))
    {
      referenceId = this._nextId++;
      this._objectIds[item] = referenceId;
    }
    return referenceId;
  }

  public bool IsReference(object item) => this._marked.ContainsKey(this.GetReferenceId(item));

  public void MarkSerialized(object item)
  {
    int referenceId = this.GetReferenceId(item);
    if (this._marked.ContainsKey(referenceId))
      throw new InvalidOperationException($"Internal Error - {item?.ToString()} has already been marked as serialized");
    this._marked[referenceId] = item;
  }

  public class ObjectReferenceEqualityComparator : IEqualityComparer<object>
  {
    public static IEqualityComparer<object> Instance = (IEqualityComparer<object>) new fsCyclicReferenceManager.ObjectReferenceEqualityComparator();

    public bool System\u002ECollections\u002EGeneric\u002EIEqualityComparer\u003CSystem\u002EObject\u003E\u002EEquals(
      object x,
      object y)
    {
      return x == y;
    }

    public int System\u002ECollections\u002EGeneric\u002EIEqualityComparer\u003CSystem\u002EObject\u003E\u002EGetHashCode(
      object obj)
    {
      return RuntimeHelpers.GetHashCode(obj);
    }
  }
}
