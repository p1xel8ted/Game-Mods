// Decompiled with JetBrains decompiler
// Type: MA_SampleICustomEventReceiver
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using DarkTonic.MasterAudio;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MA_SampleICustomEventReceiver : MonoBehaviour, ICustomEventReceiver
{
  public List<string> _eventsSubscribedTo = new List<string>()
  {
    "PlayerMoved",
    "PlayerStoppedMoving"
  };

  public void Awake()
  {
  }

  public void Start() => this.CheckForIllegalCustomEvents();

  public void OnEnable() => this.RegisterReceiver();

  public void OnDisable()
  {
    if ((Object) DarkTonic.MasterAudio.MasterAudio.SafeInstance == (Object) null || DarkTonic.MasterAudio.MasterAudio.AppIsShuttingDown)
      return;
    this.UnregisterReceiver();
  }

  public void CheckForIllegalCustomEvents()
  {
    for (int index = 0; index < this._eventsSubscribedTo.Count; ++index)
    {
      if (!DarkTonic.MasterAudio.MasterAudio.CustomEventExists(this._eventsSubscribedTo[index]))
        Debug.LogError((object) $"Custom Event, listened to by '{this.name}', could not be found in MasterAudio.");
    }
  }

  public void ReceiveEvent(string customEventName, Vector3 originPoint)
  {
    switch (customEventName)
    {
      case "PlayerMoved":
        Debug.Log((object) $"PlayerMoved event recieved by '{this.name}'.");
        break;
      case "PlayerStoppedMoving":
        Debug.Log((object) $"PlayerStoppedMoving event recieved by '{this.name}'.");
        break;
    }
  }

  public bool SubscribesToEvent(string customEventName)
  {
    return !string.IsNullOrEmpty(customEventName) && this._eventsSubscribedTo.Contains(customEventName);
  }

  public void RegisterReceiver()
  {
    DarkTonic.MasterAudio.MasterAudio.AddCustomEventReceiver((ICustomEventReceiver) this, this.transform);
  }

  public void UnregisterReceiver()
  {
    DarkTonic.MasterAudio.MasterAudio.RemoveCustomEventReceiver((ICustomEventReceiver) this);
  }

  public IList<AudioEventGroup> GetAllEvents()
  {
    List<AudioEventGroup> allEvents = new List<AudioEventGroup>();
    for (int index = 0; index < this._eventsSubscribedTo.Count; ++index)
      allEvents.Add(new AudioEventGroup()
      {
        customEventName = this._eventsSubscribedTo[index]
      });
    return (IList<AudioEventGroup>) allEvents;
  }
}
