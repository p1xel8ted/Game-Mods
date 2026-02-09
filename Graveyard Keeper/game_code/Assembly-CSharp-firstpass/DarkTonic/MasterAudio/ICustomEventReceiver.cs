// Decompiled with JetBrains decompiler
// Type: DarkTonic.MasterAudio.ICustomEventReceiver
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace DarkTonic.MasterAudio;

public interface ICustomEventReceiver
{
  void CheckForIllegalCustomEvents();

  void ReceiveEvent(string customEventName, Vector3 originPoint);

  bool SubscribesToEvent(string customEventName);

  void RegisterReceiver();

  void UnregisterReceiver();

  IList<AudioEventGroup> GetAllEvents();
}
