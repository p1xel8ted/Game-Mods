// Decompiled with JetBrains decompiler
// Type: Microsoft.Mixer.InteractiveGroup
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable disable
namespace Microsoft.Mixer;

[Serializable]
public class InteractiveGroup
{
  [CompilerGenerated]
  public string \u003CGroupID\u003Ek__BackingField;
  [CompilerGenerated]
  public string \u003CSceneID\u003Ek__BackingField;
  public string _etag;

  public string GroupID
  {
    get => this.\u003CGroupID\u003Ek__BackingField;
    set => this.\u003CGroupID\u003Ek__BackingField = value;
  }

  public List<InteractiveParticipant> Participants
  {
    get
    {
      List<InteractiveParticipant> participants = new List<InteractiveParticipant>();
      foreach (InteractiveParticipant participant in InteractivityManager.SingletonInstance.Participants as List<InteractiveParticipant>)
      {
        if (participant._groupID == this.GroupID)
          participants.Add(participant);
      }
      return participants;
    }
  }

  public string SceneID
  {
    get => this.\u003CSceneID\u003Ek__BackingField;
    set => this.\u003CSceneID\u003Ek__BackingField = value;
  }

  public void SetScene(string sceneID)
  {
    this.SceneID = sceneID;
    InteractivityManager.SingletonInstance._SetCurrentSceneInternal(this, sceneID);
  }

  public InteractiveGroup(string groupID)
  {
    if (InteractivityManager.SingletonInstance.InteractivityState != InteractivityState.InteractivityEnabled && InteractivityManager.SingletonInstance.InteractivityState != InteractivityState.Initialized)
      throw new Exception("Error: The InteractivityManager must be initialized and connected to the service to create new groups.");
    this.GroupID = groupID;
    InteractivityManager.SingletonInstance._SendCreateGroupsMessage(this.GroupID, "default");
  }

  public InteractiveGroup(string groupID, string sceneID)
  {
    if (InteractivityManager.SingletonInstance.InteractivityState != InteractivityState.InteractivityEnabled && InteractivityManager.SingletonInstance.InteractivityState != InteractivityState.Initialized)
      throw new Exception("Error: The InteractivityManager must be initialized and connected to the service to create new groups.");
    this.GroupID = groupID;
    this.SceneID = sceneID;
    InteractivityManager.SingletonInstance._SendCreateGroupsMessage(this.GroupID, this.SceneID);
  }

  public InteractiveGroup(string newEtag, string sceneID, string groupID)
  {
    this._etag = newEtag;
    this.SceneID = sceneID;
    this.GroupID = groupID;
  }
}
