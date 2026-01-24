// Decompiled with JetBrains decompiler
// Type: ArrayExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;

#nullable disable
public static class ArrayExtensions
{
  public static int IndexOf<T>(this T[] array, T item) => Array.IndexOf<T>(array, item);

  public static T LastElement<T>(this T[] array)
  {
    return array.Length != 0 ? array[array.Length - 1] : default (T);
  }

  public static bool Contains<T>(this T[] array, T item) => array.IndexOf<T>(item) != -1;

  public static T RandomElement<T>(this T[] array) => array[UnityEngine.Random.Range(0, array.Length)];
}
