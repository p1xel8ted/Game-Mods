// Decompiled with JetBrains decompiler
// Type: NGTools.ArrayModifier
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace NGTools;

public class ArrayModifier : ICollectionModifier
{
  public Array array;

  public int Size => this.array.Length;

  public Type Type => Utility.GetArraySubType(this.array.GetType());

  public ArrayModifier(Array array) => this.array = array;

  public object Get(int index) => this.array.GetValue(index);

  public void Set(int index, object value) => this.array.SetValue(value, index);
}
