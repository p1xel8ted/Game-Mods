// Decompiled with JetBrains decompiler
// Type: OtherMinibossManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class OtherMinibossManager : BaseMonoBehaviour
{
  public static int CurrentIndex;
  public List<MiniBossController> BossEncounters;
  private MiniBossController CurrentMiniBoss;
  public Interaction_Chest Chest;
  private bool revealed;
  private bool introPlayed;
  private GameObject Follower;
  private int DeathCount;
  private bool Completed;
  private GameObject Boss;

  private void Start()
  {
    this.BossEncounters = new List<MiniBossController>((IEnumerable<MiniBossController>) this.GetComponentsInChildren<MiniBossController>());
    this.CurrentMiniBoss = this.BossEncounters[Random.Range(0, GameManager.CurrentDungeonLayer - 1)];
    foreach (MiniBossController bossEncounter in this.BossEncounters)
    {
      bossEncounter.gameObject.GetComponentInChildren<UnitObject>().CanHaveModifier = false;
      bossEncounter.gameObject.SetActive((Object) bossEncounter == (Object) this.CurrentMiniBoss);
    }
  }

  private void GetAndNameBosses(int DungeonNumber)
  {
    this.BossEncounters = new List<MiniBossController>((IEnumerable<MiniBossController>) this.GetComponentsInChildren<MiniBossController>());
    int index = -1;
    while (++index < this.BossEncounters.Count)
      this.BossEncounters[index].DisplayName = $"NAMES/MiniBoss/Dungeon{(object) DungeonNumber}/MiniBoss{(object) (index + 1)}";
  }

  public void Play()
  {
    AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.MainBossA);
    foreach (Health health in this.CurrentMiniBoss.EnemiesToTrack)
      health.OnDie += new Health.DieAction(this.H_OnDie);
    this.StartCoroutine((IEnumerator) this.IntroRoutine());
  }

  private void OnEnable()
  {
    if (this.introPlayed)
      return;
    this.StartCoroutine((IEnumerator) this.IntroRoutine());
  }

  private IEnumerator IntroRoutine()
  {
    OtherMinibossManager otherMinibossManager = this;
    otherMinibossManager.introPlayed = true;
    yield return (object) new WaitForEndOfFrame();
    while ((Object) PlayerFarming.Instance == (Object) null || PlayerFarming.Instance.GoToAndStopping)
      yield return (object) null;
    otherMinibossManager.StartCoroutine((IEnumerator) otherMinibossManager.CurrentMiniBoss.IntroRoutine());
    RoomLockController.CloseAll();
  }

  private void H_OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    ++this.DeathCount;
    Victim.OnDie -= new Health.DieAction(this.H_OnDie);
    if (this.Completed || !Victim.gameObject.CompareTag("Boss") && Health.team2.Count > 1)
      return;
    this.Completed = true;
  }

  private void Update()
  {
    if (!this.introPlayed || this.revealed || this.DeathCount < this.CurrentMiniBoss.EnemiesToTrack.Count)
      return;
    this.revealed = true;
    this.StartCoroutine((IEnumerator) this.RevealRoutine());
  }

  private IEnumerator RevealRoutine()
  {
    OtherMinibossManager otherMinibossManager = this;
    AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.BossEntryAmbience);
    yield return (object) otherMinibossManager.StartCoroutine((IEnumerator) otherMinibossManager.CurrentMiniBoss.OutroRoutine());
    yield return (object) new WaitForSeconds(0.5f);
    otherMinibossManager.Chest.RevealBossReward(InventoryItem.ITEM_TYPE.NONE);
  }
}
