// Decompiled with JetBrains decompiler
// Type: DarkTonic.MasterAudio.CustomEvent
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace DarkTonic.MasterAudio;

[Serializable]
public class CustomEvent
{
  public string EventName;
  public string ProspectiveName;
  public bool IsEditing;
  public bool eventExpanded = true;
  public DarkTonic.MasterAudio.MasterAudio.CustomEventReceiveMode eventReceiveMode;
  public float distanceThreshold = 1f;
  public DarkTonic.MasterAudio.MasterAudio.EventReceiveFilter eventRcvFilterMode;
  public int filterModeQty = 1;
  public bool isTemporary;
  public int frameLastFired = -1;
  public string categoryName = "[Uncategorized]";

  public CustomEvent(string eventName)
  {
    this.EventName = eventName;
    this.ProspectiveName = eventName;
  }
}
