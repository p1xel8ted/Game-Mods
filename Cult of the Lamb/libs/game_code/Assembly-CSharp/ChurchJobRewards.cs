// Decompiled with JetBrains decompiler
// Type: ChurchJobRewards
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ChurchJobRewards : BaseMonoBehaviour
{
  public Interaction_JobBoard jobBoard;
  [SerializeField]
  public GhostNPC graveyardNPC;
  [SerializeField]
  public ChurchJobRewardDictionary fleeceToLoreReward = new ChurchJobRewardDictionary();

  public void Awake() => this.jobBoard = this.GetComponent<Interaction_JobBoard>();

  public void OnEnable()
  {
    this.jobBoard.OnJobCompleted += new Interaction_JobBoard.JobEvent(this.OnJobCompleted);
  }

  public void OnDisable()
  {
    this.jobBoard.OnJobCompleted -= new Interaction_JobBoard.JobEvent(this.OnJobCompleted);
  }

  public void OnJobCompleted(ObjectivesData objective)
  {
    if (!(objective is Objectives_ShowFleece fleeceObjective))
      return;
    this.StartCoroutine((IEnumerator) this.PostRewardIE(fleeceObjective));
  }

  public IEnumerator PostRewardIE(Objectives_ShowFleece fleeceObjective)
  {
    int valueOrDefault = CollectionExtensions.GetValueOrDefault<PlayerFleeceManager.FleeceType, int>((IReadOnlyDictionary<PlayerFleeceManager.FleeceType, int>) this.fleeceToLoreReward, fleeceObjective.FleeceType);
    LoreStone loreStone = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.LORE_STONE, 1, PlayerFarming.Instance.transform.position).GetComponent<LoreStone>();
    loreStone.SetLore(valueOrDefault, isLambLore: true);
    loreStone.OnInteract(PlayerFarming.Instance.state);
    yield return (object) null;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(loreStone.gameObject, 5f);
    yield return (object) new WaitUntil((Func<bool>) (() =>
    {
      if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null && PlayerFarming.Instance._state.CURRENT_STATE != StateMachine.State.InActive)
      {
        foreach (PlayerFarming player in PlayerFarming.players)
          player.SetInactive();
      }
      return !loreStone.IsRunning;
    }));
    while (LetterBox.IsPlaying)
      yield return (object) null;
    if (!((UnityEngine.Object) this.jobBoard == (UnityEngine.Object) null))
    {
      if (this.jobBoard.BoardCompleted)
      {
        this.jobBoard.AddHideLock();
        yield return (object) this.jobBoard.HideIE();
        if ((UnityEngine.Object) this.graveyardNPC?.JobBoardCompleteConvo != (UnityEngine.Object) null)
        {
          this.graveyardNPC.JobBoardCompleteConvo.ResetConvo();
          yield return (object) this.graveyardNPC.JobBoardCompleteConvo.PlayIE();
        }
        this.jobBoard.RemoveHideLock();
      }
      else
      {
        GhostNPC graveyardNpc = this.graveyardNPC;
        if ((graveyardNpc != null ? (graveyardNpc.HasJobCompleteConvos ? 1 : 0) : 0) != 0)
        {
          Interaction_SimpleConversation jobCompleteConvo = this.graveyardNPC.NextJobCompleteConvo;
          jobCompleteConvo.ResetConvo();
          yield return (object) jobCompleteConvo.PlayIE();
        }
      }
      GameManager.GetInstance().OnConversationEnd();
    }
  }
}
