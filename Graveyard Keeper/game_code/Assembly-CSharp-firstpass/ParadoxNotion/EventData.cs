// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.EventData
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Runtime.CompilerServices;

#nullable disable
namespace ParadoxNotion;

public class EventData
{
  [CompilerGenerated]
  public string \u003Cname\u003Ek__BackingField;

  public string name
  {
    get => this.\u003Cname\u003Ek__BackingField;
    set => this.\u003Cname\u003Ek__BackingField = value;
  }

  public object value => this.GetValue();

  public virtual object GetValue() => (object) null;

  public EventData(string name) => this.name = name;
}
