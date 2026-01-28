// Decompiled with JetBrains decompiler
// Type: SimpleCompleteCustomQuest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SimpleCompleteCustomQuest : MonoBehaviour
{
  public Objectives.CustomQuestTypes CustomQuestType;

  public void CompleteQuest() => ObjectiveManager.CompleteCustomObjective(this.CustomQuestType);
}
