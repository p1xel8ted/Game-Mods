// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Flow
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using System;
using System.Collections.Generic;

#nullable disable
namespace FlowCanvas;

[SpoofAOT]
public struct Flow
{
  public int ticks;
  public Dictionary<string, object> parameters;
  public FlowBreak Break;
  public FlowReturn Return;
  public Type ReturnType;

  public static Flow New => new Flow();

  public T ReadParameter<T>(string name)
  {
    object obj1 = (object) default (T);
    if (this.parameters != null)
      this.parameters.TryGetValue(name, out obj1);
    return !(obj1 is T obj2) ? default (T) : obj2;
  }

  public void WriteParameter<T>(string name, T value)
  {
    if (this.parameters == null)
      this.parameters = new Dictionary<string, object>();
    this.parameters[name] = (object) value;
  }
}
