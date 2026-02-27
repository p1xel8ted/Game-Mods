// Decompiled with JetBrains decompiler
// Type: SimpleJSON.JSONBool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.IO;
using System.Text;

#nullable disable
namespace SimpleJSON;

public class JSONBool : JSONNode
{
  public bool m_Data;

  public override JSONNodeType Tag => JSONNodeType.Boolean;

  public override bool IsBoolean => true;

  public override string Value
  {
    get => this.m_Data.ToString();
    set
    {
      bool result;
      if (!bool.TryParse(value, out result))
        return;
      this.m_Data = result;
    }
  }

  public override bool AsBool
  {
    get => this.m_Data;
    set => this.m_Data = value;
  }

  public JSONBool(bool aData) => this.m_Data = aData;

  public JSONBool(string aData) => this.Value = aData;

  public override void Serialize(BinaryWriter aWriter)
  {
    aWriter.Write((byte) 6);
    aWriter.Write(this.m_Data);
  }

  public override void WriteToStringBuilder(
    StringBuilder aSB,
    int aIndent,
    int aIndentInc,
    JSONTextMode aMode)
  {
    aSB.Append(this.m_Data ? "true" : "false");
  }

  public override bool Equals(object obj) => obj != null && obj is bool flag && this.m_Data == flag;

  public override int GetHashCode() => this.m_Data.GetHashCode();
}
