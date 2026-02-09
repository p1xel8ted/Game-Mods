// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.ExtractorNode
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using ParadoxNotion;
using ParadoxNotion.Design;
using System;
using System.Collections.Generic;

#nullable disable
namespace FlowCanvas.Nodes;

[DoNotList]
public abstract class ExtractorNode : SimplexNode
{
  public static Dictionary<Type, Type> _extractors;

  public static Type GetExtractorType(Type type)
  {
    if (ExtractorNode._extractors == null)
    {
      ExtractorNode._extractors = new Dictionary<Type, Type>();
      foreach (Type type1 in ((IEnumerable<Type>) ReflectionTools.GetAllTypes()).Where<Type>((Func<Type, bool>) (t => !t.IsGenericTypeDefinition && !t.IsAbstract && t.RTIsSubclassOf(typeof (ExtractorNode)))))
      {
        Type parameterType = type1.RTGetMethod("Invoke").GetParameters()[0].ParameterType;
        ExtractorNode._extractors[parameterType] = type1;
      }
    }
    Type extractorType = (Type) null;
    ExtractorNode._extractors.TryGetValue(type, out extractorType);
    return extractorType;
  }
}
