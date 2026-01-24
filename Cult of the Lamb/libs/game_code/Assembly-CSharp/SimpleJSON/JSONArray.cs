// Decompiled with JetBrains decompiler
// Type: SimpleJSON.JSONArray
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable disable
namespace SimpleJSON;

public class JSONArray : JSONNode, IEnumerable
{
  public List<JSONNode> m_List = new List<JSONNode>();
  public bool inline;

  public override JSONNodeType Tag => JSONNodeType.Array;

  public override bool IsArray => true;

  public override JSONNode this[int aIndex]
  {
    get
    {
      return aIndex < 0 || aIndex >= this.m_List.Count ? (JSONNode) new JSONLazyCreator((JSONNode) this) : this.m_List[aIndex];
    }
    set
    {
      if (value == (object) null)
        value = (JSONNode) new JSONNull();
      if (aIndex < 0 || aIndex >= this.m_List.Count)
        this.m_List.Add(value);
      else
        this.m_List[aIndex] = value;
    }
  }

  public override JSONNode this[string aKey]
  {
    get => (JSONNode) new JSONLazyCreator((JSONNode) this);
    set
    {
      if (value == (object) null)
        value = (JSONNode) new JSONNull();
      this.m_List.Add(value);
    }
  }

  public override int Count => this.m_List.Count;

  public override void Add(string aKey, JSONNode aItem)
  {
    if (aItem == (object) null)
      aItem = (JSONNode) new JSONNull();
    this.m_List.Add(aItem);
  }

  public override JSONNode Remove(int aIndex)
  {
    if (aIndex < 0 || aIndex >= this.m_List.Count)
      return (JSONNode) null;
    JSONNode jsonNode = this.m_List[aIndex];
    this.m_List.RemoveAt(aIndex);
    return jsonNode;
  }

  public override JSONNode Remove(JSONNode aNode)
  {
    this.m_List.Remove(aNode);
    return aNode;
  }

  public override IEnumerable<JSONNode> Children
  {
    get
    {
      foreach (JSONNode child in this.m_List)
        yield return child;
    }
  }

  public IEnumerator GetEnumerator()
  {
    foreach (object obj in this.m_List)
      yield return obj;
  }

  public override void Serialize(BinaryWriter aWriter)
  {
    aWriter.Write((byte) 1);
    aWriter.Write(this.m_List.Count);
    for (int index = 0; index < this.m_List.Count; ++index)
      this.m_List[index].Serialize(aWriter);
  }

  public override void WriteToStringBuilder(
    StringBuilder aSB,
    int aIndent,
    int aIndentInc,
    JSONTextMode aMode)
  {
    aSB.Append('[');
    int count = this.m_List.Count;
    if (this.inline)
      aMode = JSONTextMode.Compact;
    for (int index = 0; index < count; ++index)
    {
      if (index > 0)
        aSB.Append(',');
      if (aMode == JSONTextMode.Indent)
        aSB.AppendLine();
      if (aMode == JSONTextMode.Indent)
        aSB.Append(' ', aIndent + aIndentInc);
      this.m_List[index].WriteToStringBuilder(aSB, aIndent + aIndentInc, aIndentInc, aMode);
    }
    if (aMode == JSONTextMode.Indent)
      aSB.AppendLine().Append(' ', aIndent);
    aSB.Append(']');
  }
}
