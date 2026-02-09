// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.ConvertTo`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using System;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Utilities/Converters")]
[Obsolete]
public class ConvertTo<T> : PureFunctionNode<T, IConvertible> where T : IConvertible
{
  public override T Invoke(IConvertible obj) => (T) Convert.ChangeType((object) obj, typeof (T));
}
