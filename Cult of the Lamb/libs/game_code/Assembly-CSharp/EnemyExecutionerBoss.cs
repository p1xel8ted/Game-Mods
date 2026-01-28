// Decompiled with JetBrains decompiler
// Type: EnemyExecutionerBoss
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EnemyExecutionerBoss : BaseMonoBehaviour
{
  public List<FormationFighter> GruntsToEnable = new List<FormationFighter>();
  public List<GameObject> SummonList = new List<GameObject>();
  public AudioClip PreCombatMusic;
  public AudioClip CombatMusic;
  public TriggerCanvasGroup TriggerCanvasGroup;
  public SkeletonAnimation Spine;
  public Vector3 ActivateOffset;
  public float ActivateDistance = 5f;
  public bool Activated;
  public GameObject Player;
  public Health health;
  public EnemyBrute enemyBrute;
  public Interaction_MonsterHeart Interaction_MonsterHeart;
  public bool NotDead = true;
  public float Timer;
  public GameObject EnemySpawnerGO;
  public int SummonedCount;

  public void OnEnable() => this.health.OnDie += new Health.DieAction(this.OnDie);

  public void OnDisable()
  {
    this.health.OnDie += new Health.DieAction(this.OnDie);
    this.Interaction_MonsterHeart.OnHeartTaken -= new Interaction_MonsterHeart.HeartTaken(this.OnHeartTaken);
  }

  public void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    int index = -1;
    while (++index < Health.team2.Count)
    {
      if ((UnityEngine.Object) Health.team2[index] != (UnityEngine.Object) null && (UnityEngine.Object) Health.team2[index] != (UnityEngine.Object) this.enemyBrute.health)
        Health.team2[index].DestroyNextFrame();
    }
    this.StopAllCoroutines();
    this.NotDead = false;
    this.GetComponent<Collider2D>().enabled = false;
    AmbientMusicController.StopCombat();
    AudioManager.Instance.SetMusicCombatState(false);
    AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.BossEntryAmbience);
    this.Interaction_MonsterHeart.Play();
    this.enemyBrute.enabled = false;
    this.Interaction_MonsterHeart.OnHeartTaken += new Interaction_MonsterHeart.HeartTaken(this.OnHeartTaken);
    this.Spine.AnimationState.SetAnimation(0, "die", false);
    this.Spine.AnimationState.AddAnimation(0, "dead", true, 0.0f);
    foreach (FormationFighter formationFighter in this.GruntsToEnable)
    {
      if ((UnityEngine.Object) formationFighter != (UnityEngine.Object) null)
        formationFighter.health.DealDamage(float.MaxValue, this.gameObject, this.transform.position);
    }
  }

  public void OnHeartTaken() => DataManager.Instance.DefeatedExecutioner = true;

  public void Update()
  {
    if (this.Activated)
    {
      if (!this.NotDead || this.SummonedCount > 1 || (double) (this.Timer += Time.deltaTime) <= 7.0 || !this.enemyBrute.enabled || this.enemyBrute.state.CURRENT_STATE != StateMachine.State.Moving && this.enemyBrute.state.CURRENT_STATE != StateMachine.State.Idle)
        return;
      this.enemyBrute.StopAllCoroutines();
      this.StartCoroutine((IEnumerator) this.Summon());
    }
    else
    {
      if ((UnityEngine.Object) (this.Player = PlayerFarming.FindClosestPlayerGameObject(this.transform.position)) == (UnityEngine.Object) null || (double) Vector3.Distance(this.transform.position + this.ActivateOffset, this.Player.transform.position) >= (double) this.ActivateDistance)
        return;
      this.Play();
    }
  }

  public IEnumerator Summon()
  {
    EnemyExecutionerBoss enemyExecutionerBoss = this;
    enemyExecutionerBoss.enemyBrute.enabled = false;
    enemyExecutionerBoss.enemyBrute.state.CURRENT_STATE = StateMachine.State.CustomAction0;
    yield return (object) new WaitForEndOfFrame();
    enemyExecutionerBoss.Spine.AnimationState.SetAnimation(0, "summon", false);
    enemyExecutionerBoss.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    yield return (object) new WaitForSeconds(0.733333349f);
    int num = -1;
    while (++num < 3)
    {
      float f = (float) (120 * num) * ((float) Math.PI / 180f);
      Vector3 position = enemyExecutionerBoss.transform.position + new Vector3(3f * Mathf.Cos(f), 3f * Mathf.Sin(f));
      enemyExecutionerBoss.EnemySpawnerGO = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Enemy Spawner/EnemySpawner"), position, Quaternion.identity, enemyExecutionerBoss.transform.parent) as GameObject;
      enemyExecutionerBoss.EnemySpawnerGO.GetComponent<EnemySpawner>().InitAndInstantiate(enemyExecutionerBoss.SummonList[UnityEngine.Random.Range(0, enemyExecutionerBoss.SummonList.Count)]).GetComponent<Health>().OnDie += new Health.DieAction(enemyExecutionerBoss.RemoveSpawned);
      ++enemyExecutionerBoss.SummonedCount;
    }
    yield return (object) new WaitForSeconds(1.4666667f);
    enemyExecutionerBoss.Timer = 0.0f;
    enemyExecutionerBoss.enemyBrute.enabled = true;
    enemyExecutionerBoss.enemyBrute.state.CURRENT_STATE = StateMachine.State.Idle;
    yield return (object) new WaitForEndOfFrame();
    enemyExecutionerBoss.enemyBrute.StartCoroutine((IEnumerator) enemyExecutionerBoss.enemyBrute.ChasePlayer());
  }

  public void RemoveSpawned(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    --this.SummonedCount;
    Victim.OnDie -= new Health.DieAction(this.RemoveSpawned);
  }

  public void Play() => this.StartCoroutine((IEnumerator) this.DoPlay());

  public IEnumerator DoPlay()
  {
    EnemyExecutionerBoss enemyExecutionerBoss = this;
    enemyExecutionerBoss.Activated = true;
    yield return (object) new WaitForEndOfFrame();
    GameManager.GetInstance().OnConversationNew((PlayerFarming) null);
    GameManager.GetInstance().OnConversationNext(enemyExecutionerBoss.gameObject, 5f);
    BlockingDoor.CloseAll();
    yield return (object) new WaitForSeconds(1f);
    AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.MainBossA);
    enemyExecutionerBoss.Spine.AnimationState.SetAnimation(0, "execute", false);
    enemyExecutionerBoss.Spine.AnimationState.AddAnimation(0, "wake-up", false, 0.0f);
    enemyExecutionerBoss.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    yield return (object) new WaitForSeconds(5f);
    foreach (Behaviour behaviour in enemyExecutionerBoss.GruntsToEnable)
      behaviour.enabled = true;
    GameManager.GetInstance().OnConversationEnd();
    enemyExecutionerBoss.enemyBrute.enabled = true;
  }

  public void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position + this.ActivateOffset, this.ActivateDistance, Color.green);
  }
}
