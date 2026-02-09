// Decompiled with JetBrains decompiler
// Type: QuestState
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;

#nullable disable
[Serializable]
public class QuestState
{
  public QuestDefinition definition = new QuestDefinition();
  public long start_time;
  public QuestState.State state;

  public QuestState.State CheckQuestProgress()
  {
    if (this.definition.IsFailed())
      return QuestState.State.Failed;
    return this.definition.IsSucceed() ? QuestState.State.Succeeded : QuestState.State.InProgress;
  }

  public void FromGJCode(string gjcode)
  {
  }

  public string ToGJCode() => (string) null;

  public void OnDeserialize()
  {
  }

  [Serializable]
  public enum State
  {
    Failed = -1, // 0xFFFFFFFF
    InProgress = 0,
    Succeeded = 1,
  }
}
