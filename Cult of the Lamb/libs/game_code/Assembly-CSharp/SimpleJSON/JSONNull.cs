// Decompiled with JetBrains decompiler
// Type: SimpleJSON.JSONNull
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.IO;
using System.Text;

#nullable disable
namespace SimpleJSON;

public class JSONNull : JSONNode
{
  public override JSONNodeType Tag => JSONNodeType.NullValue;

  public override bool IsNull => true;

  public override string Value
  {
    get => "null";
    set
    {
    }
  }

  public override bool AsBool
  {
    get => false;
    set
    {
    }
  }

  public override bool Equals(object obj) => this == obj || obj is JSONNull;

  public override int GetHashCode() => 0;

  public override void Serialize(BinaryWriter aWriter) => aWriter.Write((byte) 5);

  public override void WriteToStringBuilder(
    StringBuilder aSB,
    int aIndent,
    int aIndentInc,
    JSONTextMode aMode)
  {
    aSB.Append("null");
  }
}
