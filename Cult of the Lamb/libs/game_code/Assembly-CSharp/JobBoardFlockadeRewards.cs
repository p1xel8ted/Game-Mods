// Decompiled with JetBrains decompiler
// Type: JobBoardFlockadeRewards
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Flockade;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class JobBoardFlockadeRewards : MonoBehaviour
{
  [SerializeField]
  public List<FlockadePieceType> _rewards = new List<FlockadePieceType>();
  [SerializeField]
  public GameObject _ghostNPC;
  [SerializeField]
  public Interaction_JobBoard _jobBoard;

  public void OnBoardCompleted()
  {
    Debug.Log((object) "Starting the reward sequence");
    this.StartCoroutine((IEnumerator) this.GiveRewardRoutine());
  }

  public IEnumerator GiveRewardRoutine()
  {
    JobBoardFlockadeRewards boardFlockadeRewards = this;
    if (boardFlockadeRewards._rewards.Count != 0)
    {
      yield return (object) boardFlockadeRewards._jobBoard.HideIE();
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(boardFlockadeRewards._ghostNPC, 5f);
      yield return (object) new WaitForSeconds(1f);
      AudioManager.Instance.PlayOneShot("event:/Stings/Choir_mid", boardFlockadeRewards.transform.position);
      if ((Object) PlayerFarming.Instance != (Object) null)
      {
        PlayerFarming instance = PlayerFarming.Instance;
        instance.state.CURRENT_STATE = StateMachine.State.FoundItem;
        GameManager.GetInstance().OnConversationNext(instance.gameObject, 5f);
      }
      yield return (object) new WaitForSeconds(1f);
      yield return (object) FlockadePieceManager.AwardPieces((IReadOnlyList<FlockadePieceType>) boardFlockadeRewards._rewards);
      GameManager.GetInstance().OnConversationEnd();
    }
  }
}
