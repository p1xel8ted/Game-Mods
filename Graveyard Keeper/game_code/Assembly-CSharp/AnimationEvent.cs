// Decompiled with JetBrains decompiler
// Type: AnimationEvent
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class AnimationEvent : MonoBehaviour
{
  public List<EventDelegate> on_event = new List<EventDelegate>();
  public List<EventDelegate> on_event2 = new List<EventDelegate>();
  public List<EventDelegate> on_event3 = new List<EventDelegate>();
  public List<EventDelegate> on_event4 = new List<EventDelegate>();
  public List<EventDelegate> on_event5 = new List<EventDelegate>();

  public void OnEvent() => EventDelegate.Execute(this.on_event);

  public void OnEvent2() => EventDelegate.Execute(this.on_event2);

  public void OnEvent3() => EventDelegate.Execute(this.on_event3);

  public void OnEvent4() => EventDelegate.Execute(this.on_event4);

  public void OnEvent5() => EventDelegate.Execute(this.on_event5);
}
