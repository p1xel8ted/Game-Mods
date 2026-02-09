// Decompiled with JetBrains decompiler
// Type: DarkTonic.MasterAudio.EventSoundsDropHandler
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;
using UnityEngine.EventSystems;

#nullable disable
namespace DarkTonic.MasterAudio;

public class EventSoundsDropHandler : EventSoundsUGUIHandler, IDropHandler, IEventSystemHandler
{
  public void OnDrop(PointerEventData data)
  {
    if (!((Object) this.eventSounds != (Object) null))
      return;
    this.eventSounds.OnDrop(data);
  }
}
