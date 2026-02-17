// Decompiled with JetBrains decompiler
// Type: OtherMinibossManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class OtherMinibossManager : BaseMonoBehaviour
{
  public static int CurrentIndex;
  public List<MiniBossController> BossEncounters;
  public MiniBossController CurrentMiniBoss;
  public Interaction_Chest Chest;
  public bool revealed;
  public bool introPlayed;
  public GameObject Follower;
  public int DeathCount;
  public bool Completed;
  public GameObject Boss;

  public void Start()
  {
    this.BossEncounters = new List<MiniBossController>((IEnumerable<MiniBossController>) this.GetComponentsInChildren<MiniBossController>());
    this.CurrentMiniBoss = this.BossEncounters[Random.Range(0, GameManager.CurrentDungeonLayer - 1)];
    foreach (MiniBossController bossEncounter in this.BossEncounters)
    {
      bossEncounter.gameObject.GetComponentInChildren<UnitObject>().CanHaveModifier = false;
      bossEncounter.gameObject.SetActive((Object) bossEncounter == (Object) this.CurrentMiniBoss);
    }
  }

  public void GetAndNameBosses(int DungeonNumber)
  {
    this.BossEncounters = new List<MiniBossController>((IEnumerable<MiniBossController>) this.GetComponentsInChildren<MiniBossController>());
    int index = -1;
    while (++index < this.BossEncounters.Count)
      this.BossEncounters[index].DisplayName = $"NAMES/MiniBoss/Dungeon{DungeonNumber.ToString()}/MiniBoss{(index + 1).ToString()}";
  }

  public void Play()
  {
    AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.MainBossA);
    foreach (Health health in this.CurrentMiniBoss.EnemiesToTrack)
      health.OnDie += new Health.DieAction(this.H_OnDie);
    this.StartCoroutine((IEnumerator) this.IntroRoutine());
  }

  public void OnEnable()
  {
    if (this.introPlayed)
      return;
    this.StartCoroutine((IEnumerator) this.IntroRoutine());
  }

  public IEnumerator IntroRoutine()
  {
    OtherMinibossManager otherMinibossManager = this;
    otherMinibossManager.introPlayed = true;
    yield return (object) new WaitForEndOfFrame();
    while ((Object) PlayerFarming.Instance == (Object) null || PlayerFarming.Instance.GoToAndStopping)
      yield return (object) null;
    otherMinibossManager.StartCoroutine((IEnumerator) otherMinibossManager.CurrentMiniBoss.IntroRoutine());
    RoomLockController.CloseAll();
  }

  public void H_OnDie(
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

  public void Update()
  {
    if (!this.introPlayed || this.revealed || this.DeathCount < this.CurrentMiniBoss.EnemiesToTrack.Count)
      return;
    this.revealed = true;
    this.StartCoroutine((IEnumerator) this.RevealRoutine());
  }

  public IEnumerator RevealRoutine()
  {
    OtherMinibossManager otherMinibossManager = this;
    AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.BossEntryAmbience);
    yield return (object) otherMinibossManager.StartCoroutine((IEnumerator) otherMinibossManager.CurrentMiniBoss.OutroRoutine());
    yield return (object) new WaitForSeconds(0.5f);
    otherMinibossManager.Chest.RevealBossReward(InventoryItem.ITEM_TYPE.NONE);
  }
}
