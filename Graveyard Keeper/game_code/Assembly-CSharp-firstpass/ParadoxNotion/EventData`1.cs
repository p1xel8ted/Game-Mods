// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.EventData`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Runtime.CompilerServices;

#nullable disable
namespace ParadoxNotion;

public class EventData<T> : EventData
{
  [CompilerGenerated]
  public T \u003Cvalue\u003Ek__BackingField;

  public T value
  {
    get => this.\u003Cvalue\u003Ek__BackingField;
    set => this.\u003Cvalue\u003Ek__BackingField = value;
  }

  public override object GetValue() => (object) this.value;

  public EventData(string name, T value)
    : base(name)
  {
    this.value = value;
  }
}
