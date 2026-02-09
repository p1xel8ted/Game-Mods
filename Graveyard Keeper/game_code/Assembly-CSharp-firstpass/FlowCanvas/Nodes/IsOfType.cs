// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.IsOfType
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion;
using ParadoxNotion.Design;
using System;
using System.Runtime.CompilerServices;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Returns whether the input object is of type T as well as the object casted to T if so")]
[Category("Logic Operators")]
public class IsOfType : PureFunctionNode<bool, object, Type>
{
  [CompilerGenerated]
  public object \u003COBJECT\u003Ek__BackingField;

  public object OBJECT
  {
    get => this.\u003COBJECT\u003Ek__BackingField;
    set => this.\u003COBJECT\u003Ek__BackingField = value;
  }

  public override bool Invoke(object OBJECT, Type type)
  {
    this.OBJECT = OBJECT;
    return OBJECT != null && type.RTIsAssignableFrom(OBJECT.GetType());
  }
}
