// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Serialization.FullSerializer.fsTypeExtensions
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using System;
using System.Collections.Generic;

#nullable disable
namespace ParadoxNotion.Serialization.FullSerializer;

public static class fsTypeExtensions
{
  public static string CSharpName(this Type type) => type.CSharpName(false);

  public static string CSharpName(
    this Type type,
    bool includeNamespace,
    bool ensureSafeDeclarationName)
  {
    string str = type.CSharpName(includeNamespace);
    if (ensureSafeDeclarationName)
      str = str.Replace('>', '_').Replace('<', '_').Replace('.', '_');
    return str;
  }

  public static string CSharpName(this Type type, bool includeNamespace)
  {
    if (Type.op_Equality(type, typeof (void)))
      return "void";
    if (Type.op_Equality(type, typeof (int)))
      return "int";
    if (Type.op_Equality(type, typeof (float)))
      return "float";
    if (Type.op_Equality(type, typeof (bool)))
      return "bool";
    if (Type.op_Equality(type, typeof (double)))
      return "double";
    if (Type.op_Equality(type, typeof (string)))
      return "string";
    if (type.IsGenericParameter)
      return type.ToString();
    string str1 = "";
    IEnumerable<Type> source = (IEnumerable<Type>) type.GetGenericArguments();
    if (type.IsNested)
    {
      str1 = $"{str1}{type.DeclaringType.CSharpName()}.";
      if (type.DeclaringType.GetGenericArguments().Length != 0)
        source = source.Skip<Type>(type.DeclaringType.GetGenericArguments().Length);
    }
    string str2 = source.Any<Type>() ? $"{str1 + type.Name.Substring(0, type.Name.IndexOf('`'))}<{string.Join(",", source.Select<Type, string>((Func<Type, string>) (t => t.CSharpName(includeNamespace))).ToArray<string>())}>" : str1 + type.Name;
    if (includeNamespace && type.Namespace != null)
      str2 = $"{type.Namespace}.{str2}";
    return str2;
  }
}
