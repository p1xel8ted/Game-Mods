// Decompiled with JetBrains decompiler
// Type: StoryEvent_Info
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class StoryEvent_Info
{
  public List<StoryEvent_Callback> Callbacks;

  public void Create(List<StoryEvent_Callback> Callbacks) => this.Callbacks = Callbacks;
}
