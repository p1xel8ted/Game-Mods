// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Serialization.JSONSerializer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Serialization.FullSerializer;
using ParadoxNotion.Serialization.FullSerializer.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

#nullable disable
namespace ParadoxNotion.Serialization;

public static class JSONSerializer
{
  public static Dictionary<string, fsData> cache = new Dictionary<string, fsData>();
  public static object serializerLock = new object();
  public static fsSerializer serializer = new fsSerializer();
  public static bool init = false;
  public static bool applicationPlaying = true;

  public static string Serialize(
    System.Type type,
    object value,
    bool pretyJson = false,
    List<UnityEngine.Object> objectReferences = null)
  {
    lock (JSONSerializer.serializerLock)
    {
      if (!JSONSerializer.init)
      {
        JSONSerializer.serializer.AddConverter((fsBaseConverter) new fsUnityObjectConverter());
        JSONSerializer.init = true;
      }
      if (objectReferences != null)
      {
        objectReferences.Clear();
        JSONSerializer.serializer.Context.Set<List<UnityEngine.Object>>(objectReferences);
      }
      System.Type overrideConverterType = typeof (UnityEngine.Object).RTIsAssignableFrom(type) ? typeof (fsReflectedConverter) : (System.Type) null;
      fsData data;
      JSONSerializer.serializer.TrySerialize(type, overrideConverterType, value, out data).AssertSuccess();
      return pretyJson ? fsJsonPrinter.PrettyJson(data) : fsJsonPrinter.CompressedJson(data);
    }
  }

  public static T Deserialize<T>(
    string serializedState,
    List<UnityEngine.Object> objectReferences = null,
    T deserialized = null)
  {
    return (T) JSONSerializer.Deserialize(typeof (T), serializedState, objectReferences, (object) deserialized);
  }

  public static object Deserialize(
    System.Type type,
    string serializedState,
    List<UnityEngine.Object> objectReferences = null,
    object deserialized = null)
  {
    lock (JSONSerializer.serializerLock)
    {
      if (!JSONSerializer.init)
      {
        JSONSerializer.serializer.AddConverter((fsBaseConverter) new fsUnityObjectConverter());
        JSONSerializer.init = true;
      }
      if (objectReferences != null)
        JSONSerializer.serializer.Context.Set<List<UnityEngine.Object>>(objectReferences);
      fsData data = (fsData) null;
      JSONSerializer.cache.TryGetValue(serializedState, out data);
      if (data == (fsData) null)
      {
        data = fsJsonParser.Parse(serializedState);
        JSONSerializer.cache[serializedState] = data;
      }
      System.Type overrideConverterType = typeof (UnityEngine.Object).RTIsAssignableFrom(type) ? typeof (fsReflectedConverter) : (System.Type) null;
      JSONSerializer.serializer.TryDeserialize(data, type, overrideConverterType, ref deserialized).AssertSuccess();
      return deserialized;
    }
  }

  public static T Clone<T>(T original, List<UnityEngine.Object> objectReferences = null)
  {
    return (T) JSONSerializer.Clone((object) original, objectReferences);
  }

  public static object Clone(object original, List<UnityEngine.Object> objectReferences = null)
  {
    System.Type type = original.GetType();
    return JSONSerializer.Deserialize(type, JSONSerializer.Serialize(type, original, objectReferences: objectReferences), objectReferences);
  }

  public static void ShowData(string json, string name = "")
  {
    string contents = fsJsonPrinter.PrettyJson(fsJsonParser.Parse(json));
    string str = $"{Path.GetTempPath()}{(string.IsNullOrEmpty(name) ? Guid.NewGuid().ToString() : name)}.json";
    File.WriteAllText(str, contents);
    Process.Start(str);
  }
}
