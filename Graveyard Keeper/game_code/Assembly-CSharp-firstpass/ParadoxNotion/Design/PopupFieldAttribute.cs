// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Design.PopupFieldAttribute
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace ParadoxNotion.Design;

[AttributeUsage(AttributeTargets.Field)]
public class PopupFieldAttribute : Attribute
{
  public object[] values;
  public string staticPath;

  public PopupFieldAttribute(params object[] values) => this.values = values;

  public PopupFieldAttribute(string staticPath) => this.staticPath = staticPath;
}
