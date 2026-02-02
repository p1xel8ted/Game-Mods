// Decompiled with JetBrains decompiler
// Type: MMTools.Conversation_Speaker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace MMTools;

public class Conversation_Speaker : MonoBehaviour
{
  public Conversation_Speaker.ID id;
  public static List<Conversation_Speaker> speakers = new List<Conversation_Speaker>();

  public void OnEnable() => Conversation_Speaker.speakers.Add(this);

  public void OnDisable() => Conversation_Speaker.speakers.Remove(this);

  public static GameObject Speaker1
  {
    get
    {
      foreach (Conversation_Speaker speaker in Conversation_Speaker.speakers)
      {
        if (speaker.id == Conversation_Speaker.ID.Speaker_1)
          return speaker.gameObject;
      }
      return (GameObject) null;
    }
  }

  public static GameObject Speaker2
  {
    get
    {
      foreach (Conversation_Speaker speaker in Conversation_Speaker.speakers)
      {
        if (speaker.id == Conversation_Speaker.ID.Speaker_2)
          return speaker.gameObject;
      }
      return (GameObject) null;
    }
  }

  public static GameObject GetSpeakerByID(Conversation_Speaker.ID id)
  {
    foreach (Conversation_Speaker speaker in Conversation_Speaker.speakers)
    {
      if (speaker.id == id)
        return speaker.gameObject;
    }
    return (GameObject) null;
  }

  public enum ID
  {
    Speaker_1,
    Speaker_2,
    Speaker_3,
    Speaker_4,
  }
}
