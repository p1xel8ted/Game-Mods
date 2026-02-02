// Decompiled with JetBrains decompiler
// Type: DecoJobBoardManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using Lamb.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DecoJobBoardManager : JobBoardManager
{
  public override IEnumerator GiveJobReward(ObjectivesData objective)
  {
    DecoJobBoardManager decoJobBoardManager = this;
    UIMenuBase s = UIManager.GetActiveMenu<UIMenuBase>();
    while ((UnityEngine.Object) s != (UnityEngine.Object) null)
      yield return (object) null;
    PlayerFarming instance = PlayerFarming.Instance;
    if (!((UnityEngine.Object) instance == (UnityEngine.Object) null))
    {
      StructureBrain.TYPES decoReward = StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_BARREL1;
      switch (objective)
      {
        case Objectives_PlaceStructure objectivesPlaceStructure:
          using (List<Interaction_JobBoard.JobData>.Enumerator enumerator = decoJobBoardManager.JobBoard.Objectives.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              Interaction_JobBoard.JobData current = enumerator.Current;
              if (current.Objective.Quest == CreateObjective.ObjectiveToGive.QuestType.PlaceDecoration && current.Objective.DecorationType == objectivesPlaceStructure.DecoType)
              {
                decoReward = current.RewardDeco;
                break;
              }
            }
            break;
          }
        case Objectives_BuildStructure objectivesBuildStructure:
          using (List<Interaction_JobBoard.JobData>.Enumerator enumerator = decoJobBoardManager.JobBoard.Objectives.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              Interaction_JobBoard.JobData current = enumerator.Current;
              if (current.Objective.Quest == CreateObjective.ObjectiveToGive.QuestType.BuildStructure && current.Objective.Structure == objectivesBuildStructure.StructureType)
              {
                decoReward = current.RewardDeco;
                break;
              }
            }
            break;
          }
      }
      bool finishedMove = false;
      GameObject gameObject = DecorationCustomTarget.Create(decoJobBoardManager.JobBoard.transform.position + Vector3.back, instance.transform.position + Vector3.back, 1f, decoReward, (System.Action) (() => finishedMove = true));
      AudioManager.Instance.PlayOneShot("event:/Stings/Choir_Short");
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(gameObject.gameObject, 5f);
      gameObject.transform.DOPunchScale(Vector3.one * 0.2f, 0.3f);
      yield return (object) new WaitUntil((Func<bool>) (() => finishedMove));
      try
      {
        StructuresData.CompleteResearch(decoReward);
        StructuresData.SetRevealed(decoReward);
      }
      finally
      {
        GameManager.GetInstance().OnConversationEnd();
      }
    }
  }
}
