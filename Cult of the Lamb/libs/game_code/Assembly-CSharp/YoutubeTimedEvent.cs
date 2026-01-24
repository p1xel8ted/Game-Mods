// Decompiled with JetBrains decompiler
// Type: YoutubeTimedEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine.Events;

#nullable disable
[Serializable]
public class YoutubeTimedEvent
{
  public int time;
  public bool pauseVideo;
  public UnityEvent timeEvent;
  public bool called;

  public bool Called
  {
    get => this.called;
    set => this.called = value;
  }
}
