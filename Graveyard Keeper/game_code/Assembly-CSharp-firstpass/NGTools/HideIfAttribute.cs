// Decompiled with JetBrains decompiler
// Type: NGTools.HideIfAttribute
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace NGTools;

public class HideIfAttribute : PropertyAttribute
{
  public string fieldName;
  public Ops @operator;
  public MultiOps multiOperator;
  public object[] values;

  public HideIfAttribute(string fieldName, Ops @operator, object value)
  {
    this.fieldName = fieldName;
    this.@operator = @operator;
    this.multiOperator = MultiOps.None;
    this.values = new object[1]{ value };
  }

  public HideIfAttribute(string fieldName, MultiOps multiOperator, params object[] values)
  {
    this.fieldName = fieldName;
    this.@operator = Ops.None;
    this.multiOperator = multiOperator;
    this.values = values;
  }
}
