// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Serialization.FullSerializer.Internal.fsReflectionUtility
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace ParadoxNotion.Serialization.FullSerializer.Internal;

public static class fsReflectionUtility
{
  public static Type GetInterface(Type type, Type interfaceType)
  {
    if (interfaceType.Resolve().IsGenericType && !interfaceType.Resolve().IsGenericTypeDefinition)
      throw new ArgumentException("GetInterface requires that if the interface type is generic, then it must be the generic type definition, not a specific generic type instantiation");
    for (; Type.op_Inequality(type, (Type) null); type = type.Resolve().BaseType)
    {
      foreach (Type type1 in type.GetInterfaces())
      {
        if (type1.Resolve().IsGenericType)
        {
          if (Type.op_Equality(interfaceType, type1.GetGenericTypeDefinition()))
            return type1;
        }
        else if (Type.op_Equality(interfaceType, type1))
          return type1;
      }
    }
    return (Type) null;
  }
}
