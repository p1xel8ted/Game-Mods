// Decompiled with JetBrains decompiler
// Type: I2.Loc.SimpleJSON.JSONClass
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#nullable disable
namespace I2.Loc.SimpleJSON;

public class JSONClass : JSONNode, IEnumerable
{
  public Dictionary<string, JSONNode> m_Dict = new Dictionary<string, JSONNode>((IEqualityComparer<string>) StringComparer.Ordinal);

  public override JSONNode this[string aKey]
  {
    get
    {
      return this.m_Dict.ContainsKey(aKey) ? this.m_Dict[aKey] : (JSONNode) new JSONLazyCreator((JSONNode) this, aKey);
    }
    set
    {
      if (this.m_Dict.ContainsKey(aKey))
        this.m_Dict[aKey] = value;
      else
        this.m_Dict.Add(aKey, value);
    }
  }

  public override JSONNode this[int aIndex]
  {
    get
    {
      return aIndex < 0 || aIndex >= this.m_Dict.Count ? (JSONNode) null : this.m_Dict.ElementAt<KeyValuePair<string, JSONNode>>(aIndex).Value;
    }
    set
    {
      if (aIndex < 0 || aIndex >= this.m_Dict.Count)
        return;
      this.m_Dict[this.m_Dict.ElementAt<KeyValuePair<string, JSONNode>>(aIndex).Key] = value;
    }
  }

  public override int Count => this.m_Dict.Count;

  public override void Add(string aKey, JSONNode aItem)
  {
    if (!string.IsNullOrEmpty(aKey))
    {
      if (this.m_Dict.ContainsKey(aKey))
        this.m_Dict[aKey] = aItem;
      else
        this.m_Dict.Add(aKey, aItem);
    }
    else
      this.m_Dict.Add(Guid.NewGuid().ToString(), aItem);
  }

  public override JSONNode Remove(string aKey)
  {
    if (!this.m_Dict.ContainsKey(aKey))
      return (JSONNode) null;
    JSONNode jsonNode = this.m_Dict[aKey];
    this.m_Dict.Remove(aKey);
    return jsonNode;
  }

  public override JSONNode Remove(int aIndex)
  {
    if (aIndex < 0 || aIndex >= this.m_Dict.Count)
      return (JSONNode) null;
    KeyValuePair<string, JSONNode> keyValuePair = this.m_Dict.ElementAt<KeyValuePair<string, JSONNode>>(aIndex);
    this.m_Dict.Remove(keyValuePair.Key);
    return keyValuePair.Value;
  }

  public override JSONNode Remove(JSONNode aNode)
  {
    try
    {
      this.m_Dict.Remove(this.m_Dict.Where<KeyValuePair<string, JSONNode>>((Func<KeyValuePair<string, JSONNode>, bool>) (k => k.Value == (object) aNode)).First<KeyValuePair<string, JSONNode>>().Key);
      return aNode;
    }
    catch
    {
      return (JSONNode) null;
    }
  }

  public override IEnumerable<JSONNode> Childs
  {
    get
    {
      foreach (KeyValuePair<string, JSONNode> keyValuePair in this.m_Dict)
        yield return keyValuePair.Value;
    }
  }

  public IEnumerator GetEnumerator()
  {
    foreach (KeyValuePair<string, JSONNode> keyValuePair in this.m_Dict)
      yield return (object) keyValuePair;
  }

  public override string ToString()
  {
    string str = "{";
    foreach (KeyValuePair<string, JSONNode> keyValuePair in this.m_Dict)
    {
      if (str.Length > 2)
        str += ", ";
      str = $"{str}\"{JSONNode.Escape(keyValuePair.Key)}\":{(string) keyValuePair.Value}";
    }
    return str + "}";
  }

  public override string ToString(string aPrefix)
  {
    string str = "{ ";
    foreach (KeyValuePair<string, JSONNode> keyValuePair in this.m_Dict)
    {
      if (str.Length > 3)
        str += ", ";
      str = $"{str}\n{aPrefix}   ";
      str = $"{str}\"{JSONNode.Escape(keyValuePair.Key)}\" : {keyValuePair.Value.ToString(aPrefix + "   ")}";
    }
    return $"{str}\n{aPrefix}}}";
  }

  public override void Serialize(BinaryWriter aWriter)
  {
    aWriter.Write((byte) 2);
    aWriter.Write(this.m_Dict.Count);
    foreach (string key in this.m_Dict.Keys)
    {
      aWriter.Write(key);
      this.m_Dict[key].Serialize(aWriter);
    }
  }
}
