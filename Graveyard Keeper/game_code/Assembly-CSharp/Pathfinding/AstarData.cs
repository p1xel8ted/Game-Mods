// Decompiled with JetBrains decompiler
// Type: Pathfinding.AstarData
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Pathfinding.Serialization;
using Pathfinding.WindowsStore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

#nullable disable
namespace Pathfinding;

[Serializable]
public class AstarData
{
  [CompilerGenerated]
  public NavMeshGraph \u003Cnavmesh\u003Ek__BackingField;
  [CompilerGenerated]
  public GridGraph \u003CgridGraph\u003Ek__BackingField;
  [CompilerGenerated]
  public LayerGridGraph \u003ClayerGridGraph\u003Ek__BackingField;
  [CompilerGenerated]
  public PointGraph \u003CpointGraph\u003Ek__BackingField;
  [CompilerGenerated]
  public RecastGraph \u003CrecastGraph\u003Ek__BackingField;
  [CompilerGenerated]
  public System.Type[] \u003CgraphTypes\u003Ek__BackingField;
  [NonSerialized]
  public NavGraph[] graphs = new NavGraph[0];
  [SerializeField]
  public string dataString;
  [FormerlySerializedAs("data")]
  [SerializeField]
  public byte[] upgradeData;
  public byte[] data_backup;
  public TextAsset file_cachedStartup;
  public byte[] data_cachedStartup;
  [SerializeField]
  public bool cacheStartup;

  public static AstarPath active => AstarPath.active;

  public NavMeshGraph navmesh
  {
    get => this.\u003Cnavmesh\u003Ek__BackingField;
    set => this.\u003Cnavmesh\u003Ek__BackingField = value;
  }

  public GridGraph gridGraph
  {
    get => this.\u003CgridGraph\u003Ek__BackingField;
    set => this.\u003CgridGraph\u003Ek__BackingField = value;
  }

  public LayerGridGraph layerGridGraph
  {
    get => this.\u003ClayerGridGraph\u003Ek__BackingField;
    set => this.\u003ClayerGridGraph\u003Ek__BackingField = value;
  }

  public PointGraph pointGraph
  {
    get => this.\u003CpointGraph\u003Ek__BackingField;
    set => this.\u003CpointGraph\u003Ek__BackingField = value;
  }

  public RecastGraph recastGraph
  {
    get => this.\u003CrecastGraph\u003Ek__BackingField;
    set => this.\u003CrecastGraph\u003Ek__BackingField = value;
  }

  public System.Type[] graphTypes
  {
    get => this.\u003CgraphTypes\u003Ek__BackingField;
    set => this.\u003CgraphTypes\u003Ek__BackingField = value;
  }

  public byte[] data
  {
    get
    {
      if (this.upgradeData != null && this.upgradeData.Length != 0)
      {
        this.data = this.upgradeData;
        this.upgradeData = (byte[]) null;
      }
      return this.dataString == null ? (byte[]) null : Convert.FromBase64String(this.dataString);
    }
    set => this.dataString = value != null ? Convert.ToBase64String(value) : (string) null;
  }

  public byte[] GetData() => this.data;

  public void SetData(byte[] data) => this.data = data;

  public void Awake()
  {
    this.graphs = new NavGraph[0];
    if (this.cacheStartup && (UnityEngine.Object) this.file_cachedStartup != (UnityEngine.Object) null)
      this.LoadFromCache();
    else
      this.DeserializeGraphs();
  }

  public void UpdateShortcuts()
  {
    this.navmesh = (NavMeshGraph) this.FindGraphOfType(typeof (NavMeshGraph));
    this.gridGraph = (GridGraph) this.FindGraphOfType(typeof (GridGraph));
    this.layerGridGraph = (LayerGridGraph) this.FindGraphOfType(typeof (LayerGridGraph));
    this.pointGraph = (PointGraph) this.FindGraphOfType(typeof (PointGraph));
    this.recastGraph = (RecastGraph) this.FindGraphOfType(typeof (RecastGraph));
  }

  public void LoadFromCache()
  {
    AstarPath.active.BlockUntilPathQueueBlocked();
    if ((UnityEngine.Object) this.file_cachedStartup != (UnityEngine.Object) null)
    {
      this.DeserializeGraphs(this.file_cachedStartup.bytes);
      GraphModifier.TriggerEvent(GraphModifier.EventType.PostCacheLoad);
    }
    else
      Debug.LogError((object) "Can't load from cache since the cache is empty");
  }

  public byte[] SerializeGraphs() => this.SerializeGraphs(SerializeSettings.Settings);

  public byte[] SerializeGraphs(SerializeSettings settings)
  {
    return this.SerializeGraphs(settings, out uint _);
  }

  public byte[] SerializeGraphs(SerializeSettings settings, out uint checksum)
  {
    AstarPath.active.BlockUntilPathQueueBlocked();
    AstarSerializer sr = new AstarSerializer(this, settings);
    sr.OpenSerialize();
    this.SerializeGraphsPart(sr);
    byte[] numArray = sr.CloseSerialize();
    checksum = sr.GetChecksum();
    return numArray;
  }

  public void SerializeGraphsPart(AstarSerializer sr)
  {
    sr.SerializeGraphs(this.graphs);
    sr.SerializeExtraInfo();
  }

  public void DeserializeGraphs()
  {
    if (this.data == null)
      return;
    this.DeserializeGraphs(this.data);
  }

  public void ClearGraphs()
  {
    if (this.graphs == null)
      return;
    for (int index = 0; index < this.graphs.Length; ++index)
    {
      if (this.graphs[index] != null)
        this.graphs[index].OnDestroy();
    }
    this.graphs = (NavGraph[]) null;
    this.UpdateShortcuts();
  }

  public void OnDestroy() => this.ClearGraphs();

  public void DeserializeGraphs(byte[] bytes)
  {
    AstarPath.active.BlockUntilPathQueueBlocked();
    this.ClearGraphs();
    this.DeserializeGraphsAdditive(bytes);
  }

  public void DeserializeGraphsAdditive(byte[] bytes)
  {
    AstarPath.active.BlockUntilPathQueueBlocked();
    try
    {
      if (bytes == null)
        throw new ArgumentNullException(nameof (bytes));
      AstarSerializer sr = new AstarSerializer(this);
      if (sr.OpenDeserialize(bytes))
      {
        this.DeserializeGraphsPartAdditive(sr);
        sr.CloseDeserialize();
        this.UpdateShortcuts();
      }
      else
        Debug.Log((object) "Invalid data file (cannot read zip).\nThe data is either corrupt or it was saved using a 3.0.x or earlier version of the system");
      AstarData.active.VerifyIntegrity();
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ("Caught exception while deserializing data.\n" + ex?.ToString()));
      this.graphs = new NavGraph[0];
      this.data_backup = bytes;
    }
  }

  public void DeserializeGraphsPart(AstarSerializer sr)
  {
    this.ClearGraphs();
    this.DeserializeGraphsPartAdditive(sr);
  }

  public void DeserializeGraphsPartAdditive(AstarSerializer sr)
  {
    if (this.graphs == null)
      this.graphs = new NavGraph[0];
    List<NavGraph> navGraphList = new List<NavGraph>((IEnumerable<NavGraph>) this.graphs);
    sr.SetGraphIndexOffset(navGraphList.Count);
    navGraphList.AddRange((IEnumerable<NavGraph>) sr.DeserializeGraphs());
    this.graphs = navGraphList.ToArray();
    sr.DeserializeExtraInfo();
    for (int i = 0; i < this.graphs.Length; i++)
    {
      if (this.graphs[i] != null)
        this.graphs[i].GetNodes((GraphNodeDelegateCancelable) (node =>
        {
          node.GraphIndex = (uint) i;
          return true;
        }));
    }
    for (int index1 = 0; index1 < this.graphs.Length; ++index1)
    {
      for (int index2 = index1 + 1; index2 < this.graphs.Length; ++index2)
      {
        if (this.graphs[index1] != null && this.graphs[index2] != null && this.graphs[index1].guid == this.graphs[index2].guid)
        {
          Debug.LogWarning((object) "Guid Conflict when importing graphs additively. Imported graph will get a new Guid.\nThis message is (relatively) harmless.");
          this.graphs[index1].guid = Pathfinding.Util.Guid.NewGuid();
          break;
        }
      }
    }
    sr.PostDeserialization();
  }

  public void FindGraphTypes()
  {
    System.Type[] types = WindowsStoreCompatibility.GetTypeInfo(typeof (AstarPath)).Assembly.GetTypes();
    List<System.Type> typeList = new List<System.Type>();
    foreach (System.Type type in types)
    {
      for (System.Type baseType = type.BaseType; System.Type.op_Inequality(baseType, (System.Type) null); baseType = baseType.BaseType)
      {
        if (object.Equals((object) baseType, (object) typeof (NavGraph)))
        {
          typeList.Add(type);
          break;
        }
      }
    }
    this.graphTypes = typeList.ToArray();
  }

  [Obsolete("If really necessary. Use System.Type.GetType instead.")]
  public System.Type GetGraphType(string type)
  {
    for (int index = 0; index < this.graphTypes.Length; ++index)
    {
      if (this.graphTypes[index].Name == type)
        return this.graphTypes[index];
    }
    return (System.Type) null;
  }

  [Obsolete("Use CreateGraph(System.Type) instead")]
  public NavGraph CreateGraph(string type)
  {
    Debug.Log((object) $"Creating Graph of type '{type}'");
    for (int index = 0; index < this.graphTypes.Length; ++index)
    {
      if (this.graphTypes[index].Name == type)
        return this.CreateGraph(this.graphTypes[index]);
    }
    Debug.LogError((object) $"Graph type ({type}) wasn't found");
    return (NavGraph) null;
  }

  public NavGraph CreateGraph(System.Type type)
  {
    NavGraph instance = Activator.CreateInstance(type) as NavGraph;
    instance.active = AstarData.active;
    return instance;
  }

  [Obsolete("Use AddGraph(System.Type) instead")]
  public NavGraph AddGraph(string type)
  {
    NavGraph graph = (NavGraph) null;
    for (int index = 0; index < this.graphTypes.Length; ++index)
    {
      if (this.graphTypes[index].Name == type)
        graph = this.CreateGraph(this.graphTypes[index]);
    }
    if (graph == null)
    {
      Debug.LogError((object) $"No NavGraph of type '{type}' could be found");
      return (NavGraph) null;
    }
    this.AddGraph(graph);
    return graph;
  }

  public NavGraph AddGraph(System.Type type)
  {
    NavGraph graph = (NavGraph) null;
    for (int index = 0; index < this.graphTypes.Length; ++index)
    {
      if (object.Equals((object) this.graphTypes[index], (object) type))
        graph = this.CreateGraph(this.graphTypes[index]);
    }
    if (graph == null)
    {
      Debug.LogError((object) $"No NavGraph of type '{type?.ToString()}' could be found, {this.graphTypes.Length.ToString()} graph types are avaliable");
      return (NavGraph) null;
    }
    this.AddGraph(graph);
    return graph;
  }

  public void AddGraph(NavGraph graph)
  {
    AstarPath.active.BlockUntilPathQueueBlocked();
    bool flag = false;
    for (int index = 0; index < this.graphs.Length; ++index)
    {
      if (this.graphs[index] == null)
      {
        this.graphs[index] = graph;
        graph.graphIndex = (uint) index;
        flag = true;
      }
    }
    if (!flag)
    {
      this.graphs = this.graphs == null || this.graphs.Length < (int) byte.MaxValue ? new List<NavGraph>((IEnumerable<NavGraph>) (this.graphs ?? new NavGraph[0]))
      {
        graph
      }.ToArray() : throw new Exception($"Graph Count Limit Reached. You cannot have more than {((uint) byte.MaxValue).ToString()} graphs. Some compiler directives can change this limit, e.g ASTAR_MORE_AREAS, look under the 'Optimizations' tab in the A* Inspector");
      graph.graphIndex = (uint) (this.graphs.Length - 1);
    }
    this.UpdateShortcuts();
    graph.active = AstarData.active;
    graph.Awake();
  }

  public bool RemoveGraph(NavGraph graph)
  {
    AstarData.active.FlushWorkItemsInternal(false);
    AstarData.active.BlockUntilPathQueueBlocked();
    graph.OnDestroy();
    int index = Array.IndexOf<NavGraph>(this.graphs, graph);
    if (index == -1)
      return false;
    this.graphs[index] = (NavGraph) null;
    this.UpdateShortcuts();
    return true;
  }

  public static NavGraph GetGraph(GraphNode node)
  {
    if (node == null)
      return (NavGraph) null;
    AstarPath active = AstarPath.active;
    if ((UnityEngine.Object) active == (UnityEngine.Object) null)
      return (NavGraph) null;
    AstarData astarData = active.astarData;
    if (astarData == null)
      return (NavGraph) null;
    if (astarData.graphs == null)
      return (NavGraph) null;
    uint graphIndex = node.GraphIndex;
    return (long) graphIndex >= (long) astarData.graphs.Length ? (NavGraph) null : astarData.graphs[(int) graphIndex];
  }

  public NavGraph FindGraphOfType(System.Type type)
  {
    if (this.graphs != null)
    {
      for (int index = 0; index < this.graphs.Length; ++index)
      {
        if (this.graphs[index] != null && object.Equals((object) this.graphs[index].GetType(), (object) type))
          return this.graphs[index];
      }
    }
    return (NavGraph) null;
  }

  public IEnumerable FindGraphsOfType(System.Type type)
  {
    if (this.graphs != null)
    {
      for (int i = 0; i < this.graphs.Length; ++i)
      {
        if (this.graphs[i] != null && object.Equals((object) this.graphs[i].GetType(), (object) type))
          yield return (object) this.graphs[i];
      }
    }
  }

  public IEnumerable GetUpdateableGraphs()
  {
    if (this.graphs != null)
    {
      for (int i = 0; i < this.graphs.Length; ++i)
      {
        if (this.graphs[i] is IUpdatableGraph)
          yield return (object) this.graphs[i];
      }
    }
  }

  public IEnumerable GetRaycastableGraphs()
  {
    if (this.graphs != null)
    {
      for (int i = 0; i < this.graphs.Length; ++i)
      {
        if (this.graphs[i] is IRaycastableGraph)
          yield return (object) this.graphs[i];
      }
    }
  }

  public int GetGraphIndex(NavGraph graph)
  {
    if (graph == null)
      throw new ArgumentNullException(nameof (graph));
    if (this.graphs != null)
    {
      for (int graphIndex = 0; graphIndex < this.graphs.Length; ++graphIndex)
      {
        if (graph == this.graphs[graphIndex])
          return graphIndex;
      }
    }
    Debug.LogError((object) "Graph doesn't exist");
    return -1;
  }
}
