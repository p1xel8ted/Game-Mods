// Decompiled with JetBrains decompiler
// Type: BountyRoomController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMBiomeGeneration;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class BountyRoomController : BaseMonoBehaviour
{
  [SerializeField]
  public int difficulty;
  [SerializeField]
  public EnemyRoundsBase enemyRounds;
  [SerializeField]
  public BarricadeLine barricadeLine;
  [SerializeField]
  public MissionRewardChest rewardChest;
  [SerializeField]
  public Canvas canvas;
  [SerializeField]
  public TMP_Text bountyText;
  [Space]
  [SerializeField]
  public UnityEvent onBountyBeaten;
  public bool triggered;

  public void Awake() => this.rewardChest.Play(DataManager.Instance.ActiveMissions[0]);

  public void BeginBountyRoom()
  {
    this.enemyRounds.BeginCombat(true, new System.Action(this.BountyRoomComplete));
    BiomeGenerator.Instance.CurrentRoom.Active = true;
    BiomeGenerator.Instance.RoomBecameActive();
    PlayerController.CanRespawn = false;
    PlayerFarming.Instance.health.OnDie += new Health.DieAction(this.OnPlayerDied);
    BlockingDoor.CloseAll();
    RoomLockController.CloseAll();
  }

  public void OnPlayerDied(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    this.StartCoroutine(this.BountyFinishedIE(false));
  }

  public void BountyRoomComplete()
  {
    this.barricadeLine.Open();
    this.StartCoroutine(this.BountyCompleteIE());
  }

  public IEnumerator BountyCompleteIE()
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

  public IEnumerator BountyFinishedIE(bool success)
  {
    yield return (object) new WaitForSeconds(1f);
    this.canvas.gameObject.SetActive(true);
    this.bountyText.text = success ? "Bounty Completed" : "Bounty Failed";
    yield return (object) new WaitForSeconds(2f);
    this.RemoveBounty();
    GameManager.ToShip();
    PlayerController.CanRespawn = true;
  }

  public void RemoveBounty()
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

  public void OnTriggerEnter2D(Collider2D other)
  {
    if (this.triggered || !other.gameObject.CompareTag("Player"))
      return;
    this.BeginBountyRoom();
    this.triggered = true;
  }
}
