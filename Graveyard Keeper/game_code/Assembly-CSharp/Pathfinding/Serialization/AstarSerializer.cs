// Decompiled with JetBrains decompiler
// Type: Pathfinding.Serialization.AstarSerializer
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Pathfinding.Ionic.Zip;
using Pathfinding.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

#nullable disable
namespace Pathfinding.Serialization;

public class AstarSerializer
{
  public AstarData data;
  public ZipFile zip;
  public MemoryStream zipStream;
  public GraphMeta meta;
  public SerializeSettings settings;
  public NavGraph[] graphs;
  public Dictionary<NavGraph, int> graphIndexInZip;
  public int graphIndexOffset;
  public const string binaryExt = ".binary";
  public const string jsonExt = ".json";
  public uint checksum = uint.MaxValue;
  public UTF8Encoding encoding = new UTF8Encoding();
  public static StringBuilder _stringBuilder = new StringBuilder();

  public static StringBuilder GetStringBuilder()
  {
    AstarSerializer._stringBuilder.Length = 0;
    return AstarSerializer._stringBuilder;
  }

  public AstarSerializer(AstarData data)
  {
    this.data = data;
    this.settings = SerializeSettings.Settings;
  }

  public AstarSerializer(AstarData data, SerializeSettings settings)
  {
    this.data = data;
    this.settings = settings;
  }

  public void SetGraphIndexOffset(int offset) => this.graphIndexOffset = offset;

  public void AddChecksum(byte[] bytes)
  {
    this.checksum = Checksum.GetChecksum(bytes, this.checksum);
  }

  public void AddEntry(string name, byte[] bytes) => this.zip.AddEntry(name, bytes);

  public uint GetChecksum() => this.checksum;

  public void OpenSerialize()
  {
    this.zipStream = new MemoryStream();
    this.zip = new ZipFile();
    this.zip.AlternateEncoding = Encoding.UTF8;
    this.zip.AlternateEncodingUsage = ZipOption.Always;
    this.meta = new GraphMeta();
  }

  public byte[] CloseSerialize()
  {
    byte[] bytes = this.SerializeMeta();
    this.AddChecksum(bytes);
    this.AddEntry("meta.json", bytes);
    DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    foreach (ZipEntry entry in (IEnumerable<ZipEntry>) this.zip.Entries)
    {
      entry.AccessedTime = dateTime;
      entry.CreationTime = dateTime;
      entry.LastModified = dateTime;
      entry.ModifiedTime = dateTime;
    }
    this.zip.Save((Stream) this.zipStream);
    this.zip.Dispose();
    byte[] array = this.zipStream.ToArray();
    this.zip = (ZipFile) null;
    this.zipStream = (MemoryStream) null;
    return array;
  }

  public void SerializeGraphs(NavGraph[] _graphs)
  {
    this.graphs = this.graphs == null ? _graphs : throw new InvalidOperationException("Cannot serialize graphs multiple times.");
    if (this.zip == null)
      throw new NullReferenceException("You must not call CloseSerialize before a call to this function");
    if (this.graphs == null)
      this.graphs = new NavGraph[0];
    for (int index = 0; index < this.graphs.Length; ++index)
    {
      if (this.graphs[index] != null)
      {
        byte[] bytes = this.Serialize(this.graphs[index]);
        this.AddChecksum(bytes);
        this.AddEntry($"graph{index.ToString()}.json", bytes);
      }
    }
  }

  public byte[] SerializeMeta()
  {
    if (this.graphs == null)
      throw new Exception("No call to SerializeGraphs has been done");
    this.meta.version = AstarPath.Version;
    this.meta.graphs = this.graphs.Length;
    this.meta.guids = new List<string>();
    this.meta.typeNames = new List<string>();
    for (int index = 0; index < this.graphs.Length; ++index)
    {
      if (this.graphs[index] != null)
      {
        this.meta.guids.Add(this.graphs[index].guid.ToString());
        this.meta.typeNames.Add(this.graphs[index].GetType().FullName);
      }
      else
      {
        this.meta.guids.Add((string) null);
        this.meta.typeNames.Add((string) null);
      }
    }
    StringBuilder stringBuilder = AstarSerializer.GetStringBuilder();
    TinyJsonSerializer.Serialize((object) this.meta, stringBuilder);
    return this.encoding.GetBytes(stringBuilder.ToString());
  }

  public byte[] Serialize(NavGraph graph)
  {
    StringBuilder stringBuilder = AstarSerializer.GetStringBuilder();
    TinyJsonSerializer.Serialize((object) graph, stringBuilder);
    return this.encoding.GetBytes(stringBuilder.ToString());
  }

  [Obsolete("Not used anymore. You can safely remove the call to this function.")]
  public void SerializeNodes()
  {
  }

  public static int GetMaxNodeIndexInAllGraphs(NavGraph[] graphs)
  {
    int maxIndex = 0;
    for (int index = 0; index < graphs.Length; ++index)
    {
      if (graphs[index] != null)
        graphs[index].GetNodes((GraphNodeDelegateCancelable) (node =>
        {
          maxIndex = Math.Max(node.NodeIndex, maxIndex);
          if (node.NodeIndex == -1)
            Debug.LogError((object) "Graph contains destroyed nodes. This is a bug.");
          return true;
        }));
    }
    return maxIndex;
  }

  public static byte[] SerializeNodeIndices(NavGraph[] graphs)
  {
    MemoryStream output = new MemoryStream();
    BinaryWriter wr = new BinaryWriter((Stream) output);
    int indexInAllGraphs = AstarSerializer.GetMaxNodeIndexInAllGraphs(graphs);
    wr.Write(indexInAllGraphs);
    int maxNodeIndex2 = 0;
    for (int index = 0; index < graphs.Length; ++index)
    {
      if (graphs[index] != null)
        graphs[index].GetNodes((GraphNodeDelegateCancelable) (node =>
        {
          maxNodeIndex2 = Math.Max(node.NodeIndex, maxNodeIndex2);
          wr.Write(node.NodeIndex);
          return true;
        }));
    }
    if (maxNodeIndex2 != indexInAllGraphs)
      throw new Exception("Some graphs are not consistent in their GetNodes calls, sequential calls give different results.");
    byte[] array = output.ToArray();
    wr.Close();
    return array;
  }

  public static byte[] SerializeGraphExtraInfo(NavGraph graph)
  {
    MemoryStream output = new MemoryStream();
    BinaryWriter writer = new BinaryWriter((Stream) output);
    GraphSerializationContext ctx = new GraphSerializationContext(writer);
    graph.SerializeExtraInfo(ctx);
    byte[] array = output.ToArray();
    writer.Close();
    return array;
  }

  public static byte[] SerializeGraphNodeReferences(NavGraph graph)
  {
    MemoryStream output = new MemoryStream();
    BinaryWriter writer = new BinaryWriter((Stream) output);
    GraphSerializationContext ctx = new GraphSerializationContext(writer);
    graph.GetNodes((GraphNodeDelegateCancelable) (node =>
    {
      node.SerializeReferences(ctx);
      return true;
    }));
    writer.Close();
    return output.ToArray();
  }

  public void SerializeExtraInfo()
  {
    if (!this.settings.nodes)
      return;
    byte[] bytes1 = this.graphs != null ? AstarSerializer.SerializeNodeIndices(this.graphs) : throw new InvalidOperationException("Cannot serialize extra info with no serialized graphs (call SerializeGraphs first)");
    this.AddChecksum(bytes1);
    this.AddEntry("graph_references.binary", bytes1);
    for (int index = 0; index < this.graphs.Length; ++index)
    {
      if (this.graphs[index] != null)
      {
        byte[] bytes2 = AstarSerializer.SerializeGraphExtraInfo(this.graphs[index]);
        this.AddChecksum(bytes2);
        this.AddEntry($"graph{index.ToString()}_extra.binary", bytes2);
        byte[] bytes3 = AstarSerializer.SerializeGraphNodeReferences(this.graphs[index]);
        this.AddChecksum(bytes3);
        this.AddEntry($"graph{index.ToString()}_references.binary", bytes3);
      }
    }
    byte[] bytes4 = this.SerializeNodeLinks();
    this.AddChecksum(bytes4);
    this.AddEntry("node_link2.binary", bytes4);
  }

  public byte[] SerializeNodeLinks()
  {
    MemoryStream output = new MemoryStream();
    NodeLink2.SerializeReferences(new GraphSerializationContext(new BinaryWriter((Stream) output)));
    return output.ToArray();
  }

  public void SerializeEditorSettings(GraphEditorBase[] editors)
  {
    if (editors == null || !this.settings.editorSettings)
      return;
    for (int index = 0; index < editors.Length && editors[index] != null; ++index)
    {
      StringBuilder stringBuilder = AstarSerializer.GetStringBuilder();
      TinyJsonSerializer.Serialize((object) editors[index], stringBuilder);
      byte[] bytes = this.encoding.GetBytes(stringBuilder.ToString());
      if (bytes.Length > 2)
      {
        this.AddChecksum(bytes);
        this.AddEntry($"graph{index.ToString()}_editor.json", bytes);
      }
    }
  }

  public ZipEntry GetEntry(string name) => this.zip[name];

  public bool ContainsEntry(string name) => this.GetEntry(name) != null;

  public bool OpenDeserialize(byte[] bytes)
  {
    this.zipStream = new MemoryStream();
    this.zipStream.Write(bytes, 0, bytes.Length);
    this.zipStream.Position = 0L;
    try
    {
      this.zip = ZipFile.Read((Stream) this.zipStream);
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ("Caught exception when loading from zip\n" + ex?.ToString()));
      this.zipStream.Dispose();
      return false;
    }
    if (this.ContainsEntry("meta.json"))
    {
      this.meta = this.DeserializeMeta(this.GetEntry("meta.json"));
    }
    else
    {
      if (!this.ContainsEntry("meta.binary"))
        throw new Exception("No metadata found in serialized data.");
      this.meta = this.DeserializeBinaryMeta(this.GetEntry("meta.binary"));
    }
    if (AstarSerializer.FullyDefinedVersion(this.meta.version) > AstarSerializer.FullyDefinedVersion(AstarPath.Version))
      Debug.LogWarning((object) $"Trying to load data from a newer version of the A* Pathfinding Project\nCurrent version: {AstarPath.Version?.ToString()} Data version: {this.meta.version?.ToString()}\nThis is usually fine as the stored data is usually backwards and forwards compatible.\nHowever node data (not settings) can get corrupted between versions (even though I try my best to keep compatibility), so it is recommended to recalculate any caches (those for faster startup) and resave any files. Even if it seems to load fine, it might cause subtle bugs.\n");
    else if (AstarSerializer.FullyDefinedVersion(this.meta.version) < AstarSerializer.FullyDefinedVersion(AstarPath.Version))
      Debug.LogWarning((object) $"Trying to load data from an older version of the A* Pathfinding Project\nCurrent version: {AstarPath.Version?.ToString()} Data version: {this.meta.version?.ToString()}\nThis is usually fine, it just means you have upgraded to a new version.\nHowever node data (not settings) can get corrupted between versions (even though I try my best to keep compatibility), so it is recommended to recalculate any caches (those for faster startup) and resave any files. Even if it seems to load fine, it might cause subtle bugs.\n");
    return true;
  }

  public static Version FullyDefinedVersion(Version v)
  {
    return new Version(Mathf.Max(v.Major, 0), Mathf.Max(v.Minor, 0), Mathf.Max(v.Build, 0), Mathf.Max(v.Revision, 0));
  }

  public void CloseDeserialize()
  {
    this.zipStream.Dispose();
    this.zip.Dispose();
    this.zip = (ZipFile) null;
    this.zipStream = (MemoryStream) null;
  }

  public NavGraph DeserializeGraph(int zipIndex, int graphIndex)
  {
    System.Type graphType = this.meta.GetGraphType(zipIndex);
    if (object.Equals((object) graphType, (object) null))
      return (NavGraph) null;
    NavGraph graph = this.data.CreateGraph(graphType);
    graph.graphIndex = (uint) graphIndex;
    string name1 = $"graph{zipIndex.ToString()}.json";
    string name2 = $"graph{zipIndex.ToString()}.binary";
    if (this.ContainsEntry(name1))
      TinyJsonDeserializer.Deserialize(AstarSerializer.GetString(this.GetEntry(name1)), graphType, (object) graph);
    else if (this.ContainsEntry(name2))
    {
      GraphSerializationContext ctx = new GraphSerializationContext(AstarSerializer.GetBinaryReader(this.GetEntry(name2)), (GraphNode[]) null, graph.graphIndex, this.meta);
      graph.DeserializeSettingsCompatibility(ctx);
    }
    else
      throw new FileNotFoundException($"Could not find data for graph {zipIndex.ToString()} in zip. Entry 'graph{zipIndex.ToString()}.json' does not exist");
    if (graph.guid.ToString() != this.meta.guids[zipIndex])
      throw new Exception($"Guid in graph file not equal to guid defined in meta file. Have you edited the data manually?\n{graph.guid.ToString()} != {this.meta.guids[zipIndex]}");
    return graph;
  }

  public NavGraph[] DeserializeGraphs()
  {
    List<NavGraph> navGraphList = new List<NavGraph>();
    this.graphIndexInZip = new Dictionary<NavGraph, int>();
    for (int zipIndex = 0; zipIndex < this.meta.graphs; ++zipIndex)
    {
      int graphIndex = navGraphList.Count + this.graphIndexOffset;
      NavGraph key = this.DeserializeGraph(zipIndex, graphIndex);
      if (key != null)
      {
        navGraphList.Add(key);
        this.graphIndexInZip[key] = zipIndex;
      }
    }
    this.graphs = navGraphList.ToArray();
    return this.graphs;
  }

  public bool DeserializeExtraInfo(NavGraph graph)
  {
    ZipEntry entry = this.GetEntry($"graph{this.graphIndexInZip[graph].ToString()}_extra.binary");
    if (entry == null)
      return false;
    GraphSerializationContext ctx = new GraphSerializationContext(AstarSerializer.GetBinaryReader(entry), (GraphNode[]) null, graph.graphIndex, this.meta);
    graph.DeserializeExtraInfo(ctx);
    return true;
  }

  public bool AnyDestroyedNodesInGraphs()
  {
    bool result = false;
    for (int index = 0; index < this.graphs.Length; ++index)
      this.graphs[index].GetNodes((GraphNodeDelegateCancelable) (node =>
      {
        if (node.Destroyed)
          result = true;
        return true;
      }));
    return result;
  }

  public GraphNode[] DeserializeNodeReferenceMap()
  {
    BinaryReader reader = AstarSerializer.GetBinaryReader(this.GetEntry("graph_references.binary") ?? throw new Exception("Node references not found in the data. Was this loaded from an older version of the A* Pathfinding Project?"));
    GraphNode[] int2Node = new GraphNode[reader.ReadInt32() + 1];
    try
    {
      for (int index = 0; index < this.graphs.Length; ++index)
        this.graphs[index].GetNodes((GraphNodeDelegateCancelable) (node =>
        {
          int2Node[reader.ReadInt32()] = node;
          return true;
        }));
    }
    catch (Exception ex)
    {
      throw new Exception("Some graph(s) has thrown an exception during GetNodes, or some graph(s) have deserialized more or fewer nodes than were serialized", ex);
    }
    if (reader.BaseStream.Position != reader.BaseStream.Length)
      throw new Exception($"{(reader.BaseStream.Length / 4L).ToString()} nodes were serialized, but only data for {(reader.BaseStream.Position / 4L).ToString()} nodes was found. The data looks corrupt.");
    reader.Close();
    return int2Node;
  }

  public void DeserializeNodeReferences(NavGraph graph, GraphNode[] int2Node)
  {
    int num = this.graphIndexInZip[graph];
    GraphSerializationContext ctx = new GraphSerializationContext(AstarSerializer.GetBinaryReader(this.GetEntry($"graph{num.ToString()}_references.binary") ?? throw new Exception($"Node references for graph {num.ToString()} not found in the data. Was this loaded from an older version of the A* Pathfinding Project?")), int2Node, graph.graphIndex, this.meta);
    graph.GetNodes((GraphNodeDelegateCancelable) (node =>
    {
      node.DeserializeReferences(ctx);
      return true;
    }));
  }

  public void DeserializeExtraInfo()
  {
    bool flag = false;
    for (int index = 0; index < this.graphs.Length; ++index)
      flag |= this.DeserializeExtraInfo(this.graphs[index]);
    if (!flag)
      return;
    if (this.AnyDestroyedNodesInGraphs())
      Debug.LogError((object) "Graph contains destroyed nodes. This is a bug.");
    GraphNode[] int2Node = this.DeserializeNodeReferenceMap();
    for (int index = 0; index < this.graphs.Length; ++index)
      this.DeserializeNodeReferences(this.graphs[index], int2Node);
    this.DeserializeNodeLinks(int2Node);
  }

  public void DeserializeNodeLinks(GraphNode[] int2Node)
  {
    ZipEntry entry = this.GetEntry("node_link2.binary");
    if (entry == null)
      return;
    NodeLink2.DeserializeReferences(new GraphSerializationContext(AstarSerializer.GetBinaryReader(entry), int2Node, 0U, this.meta));
  }

  public void PostDeserialization()
  {
    for (int index = 0; index < this.graphs.Length; ++index)
      this.graphs[index].PostDeserialization();
  }

  public void DeserializeEditorSettings(GraphEditorBase[] graphEditors)
  {
    if (graphEditors == null)
      return;
    for (int index1 = 0; index1 < graphEditors.Length; ++index1)
    {
      if (graphEditors[index1] != null)
      {
        for (int index2 = 0; index2 < this.graphs.Length; ++index2)
        {
          if (graphEditors[index1].target == this.graphs[index2])
          {
            ZipEntry entry = this.GetEntry($"graph{this.graphIndexInZip[this.graphs[index2]].ToString()}_editor.json");
            if (entry != null)
            {
              TinyJsonDeserializer.Deserialize(AstarSerializer.GetString(entry), graphEditors[index1].GetType(), (object) graphEditors[index1]);
              break;
            }
          }
        }
      }
    }
  }

  public static BinaryReader GetBinaryReader(ZipEntry entry)
  {
    MemoryStream input = new MemoryStream();
    entry.Extract((Stream) input);
    input.Position = 0L;
    return new BinaryReader((Stream) input);
  }

  public static string GetString(ZipEntry entry)
  {
    MemoryStream memoryStream = new MemoryStream();
    entry.Extract((Stream) memoryStream);
    memoryStream.Position = 0L;
    StreamReader streamReader = new StreamReader((Stream) memoryStream);
    string end = streamReader.ReadToEnd();
    streamReader.Dispose();
    return end;
  }

  public GraphMeta DeserializeMeta(ZipEntry entry)
  {
    return TinyJsonDeserializer.Deserialize(AstarSerializer.GetString(entry), typeof (GraphMeta)) as GraphMeta;
  }

  public GraphMeta DeserializeBinaryMeta(ZipEntry entry)
  {
    GraphMeta graphMeta = new GraphMeta();
    BinaryReader binaryReader = AstarSerializer.GetBinaryReader(entry);
    int major = !(binaryReader.ReadString() != "A*") ? binaryReader.ReadInt32() : throw new Exception("Invalid magic number in saved data");
    int minor = binaryReader.ReadInt32();
    int build = binaryReader.ReadInt32();
    int revision = binaryReader.ReadInt32();
    graphMeta.version = major >= 0 ? (minor >= 0 ? (build >= 0 ? (revision >= 0 ? new Version(major, minor, build, revision) : new Version(major, minor, build)) : new Version(major, minor)) : new Version(major, 0)) : new Version(0, 0);
    graphMeta.graphs = binaryReader.ReadInt32();
    graphMeta.guids = new List<string>();
    int num1 = binaryReader.ReadInt32();
    for (int index = 0; index < num1; ++index)
      graphMeta.guids.Add(binaryReader.ReadString());
    graphMeta.typeNames = new List<string>();
    int num2 = binaryReader.ReadInt32();
    for (int index = 0; index < num2; ++index)
      graphMeta.typeNames.Add(binaryReader.ReadString());
    return graphMeta;
  }

  public static void SaveToFile(string path, byte[] data)
  {
    using (FileStream fileStream = new FileStream(path, FileMode.Create))
      fileStream.Write(data, 0, data.Length);
  }

  public static byte[] LoadFromFile(string path)
  {
    using (FileStream fileStream = new FileStream(path, FileMode.Open))
    {
      byte[] buffer = new byte[(int) fileStream.Length];
      fileStream.Read(buffer, 0, (int) fileStream.Length);
      return buffer;
    }
  }
}
