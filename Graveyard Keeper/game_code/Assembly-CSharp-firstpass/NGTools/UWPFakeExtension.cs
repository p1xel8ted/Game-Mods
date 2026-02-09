// Decompiled with JetBrains decompiler
// Type: NGTools.UWPFakeExtension
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace NGTools;

public static class UWPFakeExtension
{
  public static System.Reflection.Assembly Assembly(this Type t) => t.Assembly;

  public static Type BaseType(this Type t) => t.BaseType;

  public static bool IsInterface(this Type t) => t.IsInterface;

  public static bool IsPrimitive(this Type t) => t.IsPrimitive;

  public static bool IsGenericType(this Type t) => t.IsGenericType;

  public static bool IsEnum(this Type t) => t.IsEnum;

  public static bool IsClass(this Type t) => t.IsClass;

  public static bool IsValueType(this Type t) => t.IsValueType;
}
