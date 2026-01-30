// Decompiled with JetBrains decompiler
// Type: SimpleJSON.JSONObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

#nullable disable
namespace SimpleJSON;

public class JSONObject : JSONNode, IEnumerable
{
  public Dictionary<string, JSONNode> m_Dict = new Dictionary<string, JSONNode>();
  public bool inline;

  public override JSONNodeType Tag => JSONNodeType.Object;

  public override bool IsObject => true;

  public override JSONNode this[string aKey]
  {
    get
    {
      return this.m_Dict.ContainsKey(aKey) ? this.m_Dict[aKey] : (JSONNode) new JSONLazyCreator((JSONNode) this, aKey);
    }
    set
    {
      if (value == (object) null)
        value = (JSONNode) new JSONNull();
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
      if (value == (object) null)
        value = (JSONNode) new JSONNull();
      if (aIndex < 0 || aIndex >= this.m_Dict.Count)
        return;
      this.m_Dict[this.m_Dict.ElementAt<KeyValuePair<string, JSONNode>>(aIndex).Key] = value;
    }
  }

  public override int Count => this.m_Dict.Count;

  public override void Add(string aKey, JSONNode aItem)
  {
    if (aItem == (object) null)
      aItem = (JSONNode) new JSONNull();
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

  public override IEnumerable<JSONNode> Children
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

  public override void WriteToStringBuilder(
    StringBuilder aSB,
    int aIndent,
    int aIndentInc,
    JSONTextMode aMode)
  {
    aSB.Append('{');
    bool flag = true;
    if (this.inline)
      aMode = JSONTextMode.Compact;
    foreach (KeyValuePair<string, JSONNode> keyValuePair in this.m_Dict)
    {
      if (!flag)
        aSB.Append(',');
      flag = false;
      if (aMode == JSONTextMode.Indent)
        aSB.AppendLine();
      if (aMode == JSONTextMode.Indent)
        aSB.Append(' ', aIndent + aIndentInc);
      aSB.Append('"').Append(JSONNode.Escape(keyValuePair.Key)).Append('"');
      if (aMode == JSONTextMode.Compact)
        aSB.Append(':');
      else
        aSB.Append(" : ");
      keyValuePair.Value.WriteToStringBuilder(aSB, aIndent + aIndentInc, aIndentInc, aMode);
    }
    if (aMode == JSONTextMode.Indent)
      aSB.AppendLine().Append(' ', aIndent);
    aSB.Append('}');
  }
}
