// Decompiled with JetBrains decompiler
// Type: MiniBossManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using I2.Loc;
using MMBiomeGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class MiniBossManager : BaseMonoBehaviour
{
  public static int CurrentIndex;
  public List<MiniBossController> BossEncounters;
  private MiniBossController CurrentMiniBoss;
  public Interaction_Chest Chest;
  public List<SingleChoiceRewardOption> SingleChoiceRewardOptions = new List<SingleChoiceRewardOption>();
  public GameObject FollowerToSpawn;
  private bool revealed;
  private bool introPlayed;
  private InventoryItem.ITEM_TYPE ForceReward;
  private GameObject Follower;
  private int DeathCount;
  private bool Completed;
  private GameObject Boss;
  private static readonly int GlobalDitherIntensity = Shader.PropertyToID("_GlobalDitherIntensity");

  private void Start()
  {
    this.BossEncounters = new List<MiniBossController>((IEnumerable<MiniBossController>) this.GetComponentsInChildren<MiniBossController>());
    this.CurrentMiniBoss = this.BossEncounters[GameManager.CurrentDungeonLayer - 1];
    if (DataManager.Instance.DungeonCompleted(PlayerFarming.Location))
      this.CurrentMiniBoss = DataManager.Instance.CheckKilledBosses(this.BossEncounters[this.BossEncounters.Count - 1].name) || GameManager.SandboxDungeonEnabled || GameManager.DungeonEndlessLevel != 1 ? this.BossEncounters[Random.Range(0, this.BossEncounters.Count)] : this.BossEncounters[this.BossEncounters.Count - 1];
    foreach (MiniBossController bossEncounter in this.BossEncounters)
    {
      bossEncounter.gameObject.GetComponentInChildren<UnitObject>().CanHaveModifier = false;
      bossEncounter.gameObject.SetActive((Object) bossEncounter == (Object) this.CurrentMiniBoss);
    }
  }

  private void OnEnable()
  {
    if (!((Object) this.Follower != (Object) null))
      return;
    this.StartCoroutine((IEnumerator) this.WaitForFollowerToBeRecruited());
  }

  private void GetAndNameBosses(int DungeonNumber)
  {
    this.BossEncounters = new List<MiniBossController>((IEnumerable<MiniBossController>) this.GetComponentsInChildren<MiniBossController>());
    int index = -1;
    while (++index < this.BossEncounters.Count)
      this.BossEncounters[index].DisplayName = $"NAMES/MiniBoss/Dungeon{(object) DungeonNumber}/MiniBoss{(object) (index + 1)}";
  }

  private void SetDither(float value)
  {
    Shader.SetGlobalFloat(MiniBossManager.GlobalDitherIntensity, value);
  }

  public void Play()
  {
    DOTween.Kill((object) this);
    DOTween.To(new DOSetter<float>(this.SetDither), Shader.GetGlobalFloat(MiniBossManager.GlobalDitherIntensity), SettingsManager.Settings.Accessibility.DitherFadeDistance + 1.5f, 1f).SetEase<Tweener>(Ease.OutQuart);
    foreach (Health health in this.CurrentMiniBoss.EnemiesToTrack)
      health.OnDie += new Health.DieAction(this.H_OnDie);
    this.StartCoroutine((IEnumerator) this.IntroRoutine());
  }

  private IEnumerator IntroRoutine()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    MiniBossManager miniBossManager = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      miniBossManager.introPlayed = true;
      miniBossManager.StartCoroutine((IEnumerator) miniBossManager.CurrentMiniBoss.IntroRoutine());
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForEndOfFrame();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
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
    TrapPoison.RemoveAllPoison();
    Projectile.ClearProjectiles();
    if (!this.Completed && (Victim.gameObject.CompareTag("Boss") || Health.team2.Count <= 1))
    {
      DOTween.Kill((object) this);
      DOTween.To(new DOSetter<float>(this.SetDither), Shader.GetGlobalFloat(MiniBossManager.GlobalDitherIntensity), SettingsManager.Settings.Accessibility.DitherFadeDistance, 1f).SetEase<Tweener>(Ease.OutQuart);
      this.Completed = true;
      if (!DataManager.Instance.CheckKilledBosses(this.CurrentMiniBoss.name))
      {
        AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("KILL_FIRST_BOSS"));
        AudioManager.Instance.PlayOneShot("event:/Stings/boss_kill", PlayerFarming.Instance.gameObject);
        this.Boss = Victim.gameObject;
        this.Follower = Object.Instantiate<GameObject>(this.FollowerToSpawn, this.Boss.transform.position, Quaternion.identity, this.Boss.transform.parent);
        this.Follower.GetComponent<Interaction_FollowerSpawn>().Play(this.CurrentMiniBoss.name, LocalizationManager.GetTranslation(this.CurrentMiniBoss.DisplayName));
        DataManager.SetFollowerSkinUnlocked(this.CurrentMiniBoss.name);
      }
    }
    if (GameManager.CurrentDungeonLayer >= 4)
      return;
    DataManager.Instance.SetDungeonLayer(BiomeGenerator.Instance.DungeonLocation, GameManager.CurrentDungeonLayer + 1);
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
    MiniBossManager miniBossManager = this;
    AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.BossEntryAmbience);
    yield return (object) miniBossManager.StartCoroutine((IEnumerator) miniBossManager.CurrentMiniBoss.OutroRoutine());
    yield return (object) new WaitForSecondsRealtime(1.25f);
    miniBossManager.ForceReward = InventoryItem.ITEM_TYPE.NONE;
    if (!DataManager.Instance.CheckKilledBosses(miniBossManager.CurrentMiniBoss.name))
    {
      Debug.Log((object) ("NEW MINIBOSS! " + miniBossManager.CurrentMiniBoss.name));
      string name = miniBossManager.CurrentMiniBoss.name;
      if (name == "Boss Beholder 1" || name == "Boss Beholder 2" || name == "Boss Beholder 3" || name == "Boss Beholder 4")
        miniBossManager.ForceReward = InventoryItem.ITEM_TYPE.BEHOLDER_EYE;
      Debug.Log((object) ("ForceReward " + (object) miniBossManager.ForceReward));
      yield return (object) new WaitForSeconds(2.5f);
      DataManager.Instance.AddKilledBoss(miniBossManager.CurrentMiniBoss.name);
    }
    miniBossManager.StartCoroutine((IEnumerator) miniBossManager.WaitForFollowerToBeRecruited());
  }

  private IEnumerator WaitForFollowerToBeRecruited()
  {
    MiniBossManager miniBossManager = this;
    while ((Object) miniBossManager.Follower != (Object) null)
      yield return (object) null;
    if (miniBossManager.SingleChoiceRewardOptions.Count > 0)
    {
      foreach (SingleChoiceRewardOption choiceRewardOption in miniBossManager.SingleChoiceRewardOptions)
      {
        choiceRewardOption.Reveal();
        // ISSUE: reference to a compiler-generated method
        choiceRewardOption.Callback.AddListener(new UnityAction(miniBossManager.\u003CWaitForFollowerToBeRecruited\u003Eb__23_0));
      }
    }
    else
      miniBossManager.Chest.RevealBossReward(miniBossManager.ForceReward);
  }

  private IEnumerator WaitForUIToFinish(InventoryItem.ITEM_TYPE ForceReward)
  {
    yield return (object) null;
    while (FoundItemPickUp.FoundItemPickUps.Count > 0)
      yield return (object) null;
    this.Chest.RevealBossReward(ForceReward);
  }
}
