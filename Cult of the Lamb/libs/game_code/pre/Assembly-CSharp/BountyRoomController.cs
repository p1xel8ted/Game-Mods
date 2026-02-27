// Decompiled with JetBrains decompiler
// Type: BountyRoomController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using MMBiomeGeneration;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class BountyRoomController : BaseMonoBehaviour
{
  [SerializeField]
  private int difficulty;
  [SerializeField]
  private EnemyRoundsBase enemyRounds;
  [SerializeField]
  private BarricadeLine barricadeLine;
  [SerializeField]
  private MissionRewardChest rewardChest;
  [SerializeField]
  private Canvas canvas;
  [SerializeField]
  private TMP_Text bountyText;
  [Space]
  [SerializeField]
  private UnityEvent onBountyBeaten;
  private bool triggered;

  private void Awake() => this.rewardChest.Play(DataManager.Instance.ActiveMissions[0]);

  private void BeginBountyRoom()
  {
    this.enemyRounds.BeginCombat(true, new System.Action(this.BountyRoomComplete));
    BiomeGenerator.Instance.CurrentRoom.Active = true;
    BiomeGenerator.Instance.RoomBecameActive();
    PlayerController.CanRespawn = false;
    PlayerFarming.Instance.health.OnDie += new Health.DieAction(this.OnPlayerDied);
    BlockingDoor.CloseAll();
    RoomLockController.CloseAll();
  }

  private void OnPlayerDied(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    this.StartCoroutine((IEnumerator) this.BountyFinishedIE(false));
  }

  private void BountyRoomComplete()
  {
    this.barricadeLine.Open();
    this.StartCoroutine((IEnumerator) this.BountyCompleteIE());
  }

  private IEnumerator BountyCompleteIE()
  {
    this.RemoveBounty();
    yield return (object) new WaitForSeconds(1f);
    this.canvas.gameObject.SetActive(true);
    this.bountyText.text = "Bounty Completed";
    yield return (object) new WaitForSeconds(2f);
    this.canvas.gameObject.SetActive(false);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(this.rewardChest.gameObject, 5f);
    yield return (object) new WaitForSeconds(0.5f);
    this.rewardChest.ShowReward();
    this.onBountyBeaten?.Invoke();
    yield return (object) new WaitForSeconds(1f);
    GameManager.GetInstance().OnConversationEnd();
  }

  private IEnumerator BountyFinishedIE(bool success)
  {
    yield return (object) new WaitForSeconds(1f);
    this.canvas.gameObject.SetActive(true);
    this.bountyText.text = success ? "Bounty Completed" : "Bounty Failed";
    yield return (object) new WaitForSeconds(2f);
    this.RemoveBounty();
    GameManager.ToShip();
    PlayerController.CanRespawn = true;
  }

  private void RemoveBounty()
  {
    foreach (MissionManager.Mission activeMission in DataManager.Instance.ActiveMissions)
    {
      if (activeMission.MissionType == MissionManager.MissionType.Bounty && activeMission.Difficulty == this.difficulty)
      {
        MissionManager.RemoveMission(activeMission);
        break;
      }
    }
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (this.triggered || !(other.gameObject.tag == "Player"))
      return;
    this.BeginBountyRoom();
    this.triggered = true;
  }
}
