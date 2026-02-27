// Decompiled with JetBrains decompiler
// Type: I2.Loc.SimpleJSON.JSONArray
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace I2.Loc.SimpleJSON;

public class JSONArray : JSONNode, IEnumerable
{
  private List<JSONNode> m_List = new List<JSONNode>();

  public override JSONNode this[int aIndex]
  {
    get
    {
      return aIndex < 0 || aIndex >= this.m_List.Count ? (JSONNode) new JSONLazyCreator((JSONNode) this) : this.m_List[aIndex];
    }
    set
    {
      if (aIndex < 0 || aIndex >= this.m_List.Count)
        this.m_List.Add(value);
      else
        this.m_List[aIndex] = value;
    }
  }

  public override JSONNode this[string aKey]
  {
    get => (JSONNode) new JSONLazyCreator((JSONNode) this);
    set => this.m_List.Add(value);
  }

  public override int Count => this.m_List.Count;

  public override void Add(string aKey, JSONNode aItem) => this.m_List.Add(aItem);

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

  public override IEnumerable<JSONNode> Childs
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

  public override string ToString()
  {
    string str = "[ ";
    foreach (JSONNode jsonNode in this.m_List)
    {
      if (str.Length > 2)
        str += ", ";
      str += jsonNode.ToString();
    }
    return str + " ]";
  }

  public override string ToString(string aPrefix)
  {
    string str = "[ ";
    foreach (JSONNode jsonNode in this.m_List)
    {
      if (str.Length > 3)
        str += ", ";
      str = $"{str}\n{aPrefix}   ";
      str += jsonNode.ToString(aPrefix + "   ");
    }
    return $"{str}\n{aPrefix}]";
  }

  public override void Serialize(BinaryWriter aWriter)
  {
    aWriter.Write((byte) 1);
    aWriter.Write(this.m_List.Count);
    for (int index = 0; index < this.m_List.Count; ++index)
      this.m_List[index].Serialize(aWriter);
  }
}
