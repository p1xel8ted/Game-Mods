// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.CastTo`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using System;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Utilities/Converters")]
[Obsolete]
public class CastTo<T> : PureFunctionNode<T, object>
{
  public override T Invoke(object obj)
  {
    try
    {
      return (T) obj;
    }
    catch
    {
      return default (T);
    }
  }
}
