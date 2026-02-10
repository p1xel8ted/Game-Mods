// Decompiled with JetBrains decompiler
// Type: SimpleJSON.JSONString
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.IO;
using System.Text;

#nullable disable
namespace SimpleJSON;

public class JSONString : JSONNode
{
  public string m_Data;

  public override JSONNodeType Tag => JSONNodeType.String;

  public override bool IsString => true;

  public override string Value
  {
    get => this.m_Data;
    set => this.m_Data = value;
  }

  public JSONString(string aData) => this.m_Data = aData;

  public override void Serialize(BinaryWriter aWriter)
  {
    aWriter.Write((byte) 3);
    aWriter.Write(this.m_Data);
  }

  public override void WriteToStringBuilder(
    StringBuilder aSB,
    int aIndent,
    int aIndentInc,
    JSONTextMode aMode)
  {
    aSB.Append('"').Append(JSONNode.Escape(this.m_Data)).Append('"');
  }

  public override bool Equals(object obj)
  {
    if (base.Equals(obj))
      return true;
    if (obj is string str)
      return this.m_Data == str;
    JSONString jsonString = obj as JSONString;
    return (JSONNode) jsonString != (object) null && this.m_Data == jsonString.m_Data;
  }

  public override int GetHashCode() => this.m_Data.GetHashCode();
}
