// Decompiled with JetBrains decompiler
// Type: SimpleCompleteCustomQuest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SimpleCompleteCustomQuest : MonoBehaviour
{
  public Objectives.CustomQuestTypes CustomQuestType;

  public void CompleteQuest() => ObjectiveManager.CompleteCustomObjective(this.CustomQuestType);
}
