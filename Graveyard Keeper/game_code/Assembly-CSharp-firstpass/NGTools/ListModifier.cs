// Decompiled with JetBrains decompiler
// Type: NGTools.ListModifier
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections;

#nullable disable
namespace NGTools;

public class ListModifier : ICollectionModifier
{
  public IList list;

  public int Size => this.list.Count;

  public Type Type => Utility.GetArraySubType(this.list.GetType());

  public ListModifier(IList list) => this.list = list;

  public object Get(int index) => this.list[index];

  public void Set(int index, object value) => this.list[index] = value;
}
