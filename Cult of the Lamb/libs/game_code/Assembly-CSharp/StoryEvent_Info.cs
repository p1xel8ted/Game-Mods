// Decompiled with JetBrains decompiler
// Type: StoryEvent_Info
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class StoryEvent_Info
{
  public List<StoryEvent_Callback> Callbacks;

  public void Create(List<StoryEvent_Callback> Callbacks) => this.Callbacks = Callbacks;
}
