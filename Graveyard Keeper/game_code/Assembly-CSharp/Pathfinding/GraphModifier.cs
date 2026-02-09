// Decompiled with JetBrains decompiler
// Type: Pathfinding.GraphModifier
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Pathfinding;

[ExecuteInEditMode]
public abstract class GraphModifier : MonoBehaviour
{
  public static GraphModifier root;
  public GraphModifier prev;
  public GraphModifier next;
  [HideInInspector]
  [SerializeField]
  public ulong uniqueID;
  public static Dictionary<ulong, GraphModifier> usedIDs = new Dictionary<ulong, GraphModifier>();

  public static List<T> GetModifiersOfType<T>() where T : GraphModifier
  {
    GraphModifier graphModifier = GraphModifier.root;
    List<T> modifiersOfType = new List<T>();
    for (; (Object) graphModifier != (Object) null; graphModifier = graphModifier.next)
    {
      T obj = graphModifier as T;
      if ((Object) obj != (Object) null)
        modifiersOfType.Add(obj);
    }
    return modifiersOfType;
  }

  public static void FindAllModifiers()
  {
    GraphModifier[] objectsOfType = Object.FindObjectsOfType(typeof (GraphModifier)) as GraphModifier[];
    for (int index = 0; index < objectsOfType.Length; ++index)
    {
      if (objectsOfType[index].enabled)
        objectsOfType[index].OnEnable();
    }
  }

  public static void TriggerEvent(GraphModifier.EventType type)
  {
    if (!Application.isPlaying)
      GraphModifier.FindAllModifiers();
    GraphModifier graphModifier = GraphModifier.root;
    switch (type)
    {
      case GraphModifier.EventType.PostScan:
        for (; (Object) graphModifier != (Object) null; graphModifier = graphModifier.next)
          graphModifier.OnPostScan();
        break;
      case GraphModifier.EventType.PreScan:
        for (; (Object) graphModifier != (Object) null; graphModifier = graphModifier.next)
          graphModifier.OnPreScan();
        break;
      case GraphModifier.EventType.LatePostScan:
        for (; (Object) graphModifier != (Object) null; graphModifier = graphModifier.next)
          graphModifier.OnLatePostScan();
        break;
      case GraphModifier.EventType.PreUpdate:
        for (; (Object) graphModifier != (Object) null; graphModifier = graphModifier.next)
          graphModifier.OnGraphsPreUpdate();
        break;
      case GraphModifier.EventType.PostUpdate:
        for (; (Object) graphModifier != (Object) null; graphModifier = graphModifier.next)
          graphModifier.OnGraphsPostUpdate();
        break;
      case GraphModifier.EventType.PostCacheLoad:
        for (; (Object) graphModifier != (Object) null; graphModifier = graphModifier.next)
          graphModifier.OnPostCacheLoad();
        break;
    }
  }

  public virtual void OnEnable()
  {
    this.RemoveFromLinkedList();
    this.AddToLinkedList();
    this.ConfigureUniqueID();
  }

  public virtual void OnDisable() => this.RemoveFromLinkedList();

  public virtual void Awake() => this.ConfigureUniqueID();

  public void ConfigureUniqueID()
  {
    GraphModifier graphModifier;
    if (GraphModifier.usedIDs.TryGetValue(this.uniqueID, out graphModifier) && (Object) graphModifier != (Object) this)
      this.Reset();
    GraphModifier.usedIDs[this.uniqueID] = this;
  }

  public void AddToLinkedList()
  {
    if ((Object) GraphModifier.root == (Object) null)
    {
      GraphModifier.root = this;
    }
    else
    {
      this.next = GraphModifier.root;
      GraphModifier.root.prev = this;
      GraphModifier.root = this;
    }
  }

  public void RemoveFromLinkedList()
  {
    if ((Object) GraphModifier.root == (Object) this)
    {
      GraphModifier.root = this.next;
      if ((Object) GraphModifier.root != (Object) null)
        GraphModifier.root.prev = (GraphModifier) null;
    }
    else
    {
      if ((Object) this.prev != (Object) null)
        this.prev.next = this.next;
      if ((Object) this.next != (Object) null)
        this.next.prev = this.prev;
    }
    this.prev = (GraphModifier) null;
    this.next = (GraphModifier) null;
  }

  public virtual void OnDestroy() => GraphModifier.usedIDs.Remove(this.uniqueID);

  public virtual void OnPostScan()
  {
  }

  public virtual void OnPreScan()
  {
  }

  public virtual void OnLatePostScan()
  {
  }

  public virtual void OnPostCacheLoad()
  {
  }

  public virtual void OnGraphsPreUpdate()
  {
  }

  public virtual void OnGraphsPostUpdate()
  {
  }

  public void Reset()
  {
    this.uniqueID = (ulong) Random.Range(0, int.MaxValue) | (ulong) Random.Range(0, int.MaxValue) << 32 /*0x20*/;
    GraphModifier.usedIDs[this.uniqueID] = this;
  }

  public enum EventType
  {
    PostScan = 1,
    PreScan = 2,
    LatePostScan = 4,
    PreUpdate = 8,
    PostUpdate = 16, // 0x00000010
    PostCacheLoad = 32, // 0x00000020
  }
}
