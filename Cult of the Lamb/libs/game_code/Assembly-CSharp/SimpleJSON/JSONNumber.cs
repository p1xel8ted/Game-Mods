// Decompiled with JetBrains decompiler
// Type: SimpleJSON.JSONNumber
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.IO;
using System.Text;

#nullable disable
namespace SimpleJSON;

public class JSONNumber : JSONNode
{
  public double m_Data;

  public override JSONNodeType Tag => JSONNodeType.Number;

  public override bool IsNumber => true;

  public override string Value
  {
    get => this.m_Data.ToString();
    set
    {
      double result;
      if (!double.TryParse(value, out result))
        return;
      this.m_Data = result;
    }
  }

  public override double AsDouble
  {
    get => this.m_Data;
    set => this.m_Data = value;
  }

  public JSONNumber(double aData) => this.m_Data = aData;

  public JSONNumber(string aData) => this.Value = aData;

  public override void Serialize(BinaryWriter aWriter)
  {
    aWriter.Write((byte) 4);
    aWriter.Write(this.m_Data);
  }

  public override void WriteToStringBuilder(
    StringBuilder aSB,
    int aIndent,
    int aIndentInc,
    JSONTextMode aMode)
  {
    aSB.Append(this.m_Data);
  }

  public static bool IsNumeric(object value)
  {
    switch (value)
    {
      case int _:
      case uint _:
      case float _:
      case double _:
      case Decimal _:
      case long _:
      case ulong _:
      case short _:
      case ushort _:
      case sbyte _:
        return true;
      default:
        return value is byte;
    }
  }

  public override bool Equals(object obj)
  {
    if (obj == null)
      return false;
    if (base.Equals(obj))
      return true;
    JSONNumber jsonNumber = obj as JSONNumber;
    if ((JSONNode) jsonNumber != (object) null)
      return this.m_Data == jsonNumber.m_Data;
    return JSONNumber.IsNumeric(obj) && Convert.ToDouble(obj) == this.m_Data;
  }

  public override int GetHashCode() => this.m_Data.GetHashCode();
}
