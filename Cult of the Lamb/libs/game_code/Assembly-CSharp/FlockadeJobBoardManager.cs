// Decompiled with JetBrains decompiler
// Type: FlockadeJobBoardManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Flockade;
using Lamb.UI;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FlockadeJobBoardManager : JobBoardManager
{
  [SerializeField]
  public GameObject flockadePiecesPrefab;
  [SerializeField]
  public List<FlockadePieceType> _flockadeReward = new List<FlockadePieceType>();
  [SerializeField]
  public List<FlockadePieceType> _rancherReward = new List<FlockadePieceType>();
  [SerializeField]
  public List<FlockadePieceType> _tarotReward = new List<FlockadePieceType>();
  [SerializeField]
  public List<FlockadePieceType> _blacksmithReward = new List<FlockadePieceType>();
  [SerializeField]
  public List<FlockadePieceType> _decorationsReward = new List<FlockadePieceType>();
  [SerializeField]
  public List<FlockadePieceType> _graveyardReward = new List<FlockadePieceType>();

  public override IEnumerator GiveJobReward(ObjectivesData objective)
  {
    FlockadeJobBoardManager flockadeJobBoardManager = this;
    UIMenuBase s = UIManager.GetActiveMenu<UIMenuBase>();
    while ((Object) s != (Object) null)
      yield return (object) null;
    PlayerFarming playerFarming = PlayerFarming.Instance;
    if (!((Object) playerFarming == (Object) null))
    {
      playerFarming.state.facingAngle = Utils.GetAngle(playerFarming.state.transform.position, flockadeJobBoardManager.transform.position);
      PlayerFarming.SetStateForAllPlayers(StateMachine.State.CustomAnimation);
      yield return (object) flockadeJobBoardManager.\u003C\u003En__0(objective);
      if (objective is Objectives_WinFlockadeBet objectivesWinFlockadeBet)
      {
        IReadOnlyList<FlockadePieceType> rewardPieces = (IReadOnlyList<FlockadePieceType>) null;
        switch (objectivesWinFlockadeBet.OpponentTermId)
        {
          case "NAMES/FlockadeNPC":
            rewardPieces = (IReadOnlyList<FlockadePieceType>) flockadeJobBoardManager._flockadeReward;
            break;
          case "NAMES/Rancher":
            rewardPieces = (IReadOnlyList<FlockadePieceType>) flockadeJobBoardManager._rancherReward;
            break;
          case "NAMES/TarotNPC":
            rewardPieces = (IReadOnlyList<FlockadePieceType>) flockadeJobBoardManager._tarotReward;
            break;
          case "NAMES/BlacksmithNPC":
            rewardPieces = (IReadOnlyList<FlockadePieceType>) flockadeJobBoardManager._blacksmithReward;
            break;
          case "NAMES/DecoNPC":
            rewardPieces = (IReadOnlyList<FlockadePieceType>) flockadeJobBoardManager._decorationsReward;
            break;
          case "NAMES/GraveyardNPC":
            rewardPieces = (IReadOnlyList<FlockadePieceType>) flockadeJobBoardManager._graveyardReward;
            break;
        }
        if (rewardPieces != null && rewardPieces.Count != 0)
        {
          Vector3 position = flockadeJobBoardManager.JobBoard.transform.position + Vector3.back;
          flockadePiecesBag flockadePieces = Object.Instantiate<GameObject>(flockadeJobBoardManager.flockadePiecesPrefab, position, Quaternion.identity).GetComponent<flockadePiecesBag>();
          flockadePieces.Configure(FlockadePieceManager.GetFlockadeGamePieceConfiguration(rewardPieces[0]));
          flockadePieces.transform.DOPunchScale(Vector3.one * 0.2f, 0.3f);
          AudioManager.Instance.PlayOneShot("event:/Stings/Choir_Short");
          GameManager.GetInstance().OnConversationNew();
          GameManager.GetInstance().OnConversationNext(flockadePieces.gameObject, 5f);
          yield return (object) new WaitForSeconds(1f);
          flockadePieces.transform.DOMove(playerFarming.transform.position + Vector3.back, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack);
          yield return (object) new WaitForSeconds(1f);
          AudioManager.Instance.PlayOneShot("event:/Stings/Choir_mid", flockadeJobBoardManager.transform.position);
          playerFarming.state.CURRENT_STATE = StateMachine.State.FoundItem;
          GameManager.GetInstance().OnConversationNext(playerFarming.gameObject, 5f);
          yield return (object) new WaitForSeconds(1f);
          Object.Destroy((Object) flockadePieces.gameObject);
          yield return (object) FlockadePieceManager.AwardPieces(rewardPieces);
          PlayerFarming.SetStateForAllPlayers();
          GameManager.GetInstance().OnConversationEnd();
        }
      }
    }
  }

  public void Reveal()
  {
    if (DataManager.Instance.GetVariable(this.JobBoard.ActiveVariable))
      return;
    this.JobBoard.Reveal();
  }

  [CompilerGenerated]
  [DebuggerHidden]
  public IEnumerator \u003C\u003En__0(ObjectivesData objective) => base.GiveJobReward(objective);
}
