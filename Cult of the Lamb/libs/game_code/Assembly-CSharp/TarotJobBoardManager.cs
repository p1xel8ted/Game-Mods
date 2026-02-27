// Decompiled with JetBrains decompiler
// Type: TarotJobBoardManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using Lamb.UI;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class TarotJobBoardManager : MonoBehaviour
{
  [SerializeField]
  public GhostNPC _tarotNPC;
  [SerializeField]
  public Interaction_JobBoard _tarotJobBoard;
  [SerializeField]
  public Interaction_SimpleConversation _jobBoardCompletedConversation;

  public void Start()
  {
    this._tarotJobBoard.OnJobCompleted += new Interaction_JobBoard.JobEvent(this.OnJobCompleted);
  }

  public void OnBoardCompleted() => this._tarotJobBoard.Hide();

  public void OnJobCompleted(ObjectivesData objective)
  {
    this._tarotJobBoard.AddHideLock();
    this.StartCoroutine(this.JobCompleteRoutine(objective));
  }

  public IEnumerator JobCompleteRoutine(ObjectivesData objective)
  {
    try
    {
      yield return (object) this.GiveJobReward(objective);
      Debug.Log((object) $"Job Board Complete? {this._tarotJobBoard.BoardCompleted}");
      if (this._tarotJobBoard.BoardCompleted)
      {
        yield return (object) this.GiveBoardReward();
        Debug.Log((object) "Job Board Complete: Hiding board");
        yield return (object) this.HideBoard();
        Debug.Log((object) "Resetting Job Board Complete Convo");
        this._jobBoardCompletedConversation.ResetConvo();
        Debug.Log((object) "Running Job Board Complete Convo");
        yield return (object) this._jobBoardCompletedConversation.PlayIE();
      }
      else
      {
        GhostNPC tarotNpc = this._tarotNPC;
        if ((tarotNpc != null ? (tarotNpc.HasJobCompleteConvos ? 1 : 0) : 0) != 0)
        {
          Interaction_SimpleConversation jobCompleteConvo = this._tarotNPC.NextJobCompleteConvo;
          Debug.Log((object) "Running Job Complete Convo");
          jobCompleteConvo.ResetConvo();
          yield return (object) jobCompleteConvo?.PlayIE();
        }
      }
    }
    finally
    {
      this._tarotJobBoard.RemoveHideLock();
    }
  }

  public IEnumerator GiveJobReward(ObjectivesData objective)
  {
    TarotJobBoardManager tarotJobBoardManager = this;
    UIMenuBase s = UIManager.GetActiveMenu<UIMenuBase>();
    while ((UnityEngine.Object) s != (UnityEngine.Object) null)
      yield return (object) null;
    PlayerFarming instance = PlayerFarming.Instance;
    if (!((UnityEngine.Object) instance == (UnityEngine.Object) null))
    {
      TarotCards.Card CardType = TarotCards.Card.Hearts1;
      if (objective is Objectives_FindChildren objectivesFindChildren)
      {
        foreach (Interaction_JobBoard.JobData objective1 in tarotJobBoardManager._tarotJobBoard.Objectives)
        {
          if (objective1.Objective.Quest == CreateObjective.ObjectiveToGive.QuestType.FindChildren && objective1.Objective.ChildLocation == objectivesFindChildren.Location)
          {
            CardType = objective1.RewardTarot;
            break;
          }
        }
      }
      bool finishedMove = false;
      TarotCustomTarget tarotCustomTarget = TarotCustomTarget.Create(tarotJobBoardManager._tarotJobBoard.transform.position + Vector3.back, instance.transform.position + Vector3.back, 1f, CardType, (System.Action) (() => finishedMove = true));
      AudioManager.Instance.PlayOneShot("event:/Stings/Choir_Short", tarotJobBoardManager.transform.position);
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(tarotCustomTarget.gameObject, 5f);
      tarotCustomTarget.transform.DOPunchScale(Vector3.one * 0.2f, 0.3f);
      yield return (object) new WaitUntil((Func<bool>) (() => finishedMove));
      GameManager.GetInstance().OnConversationEnd();
    }
  }

  public IEnumerator HideBoard()
  {
    if (!((UnityEngine.Object) this._tarotJobBoard == (UnityEngine.Object) null))
      yield return (object) this._tarotJobBoard.HideIE();
  }

  public IEnumerator GiveBoardReward()
  {
    yield break;
  }
}
