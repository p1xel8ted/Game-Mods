// Decompiled with JetBrains decompiler
// Type: StoryEvent_Info
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
internal class StoryEvent_Info
{
  public List<StoryEvent_Callback> Callbacks;

  public void Create(List<StoryEvent_Callback> Callbacks) => this.Callbacks = Callbacks;
}
