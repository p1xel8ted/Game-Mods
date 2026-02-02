// Decompiled with JetBrains decompiler
// Type: Interaction_Lore_Professor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_Lore_Professor : Interaction
{
  public string sTalk;
  public bool spoken;
  public AudioClip PreCombatMusic;
  public AudioClip CombatMusic;
  public Vector3 ListenPosition;
  public Interaction_SimpleConversation Conversation;
  public SkeletonAnimation Spine;
  public List<List<Interaction_Lore_Professor.EnemyAndPosition>> Rounds = new List<List<Interaction_Lore_Professor.EnemyAndPosition>>();
  public List<Interaction_Lore_Professor.EnemyAndPosition> Round1 = new List<Interaction_Lore_Professor.EnemyAndPosition>();
  public List<Interaction_Lore_Professor.EnemyAndPosition> Round2 = new List<Interaction_Lore_Professor.EnemyAndPosition>();
  public int DeathCount;

  public void Start()
  {
    this.UpdateLocalisation();
    this.Label = this.sTalk;
    this.ActivateDistance = 2f;
    this.Rounds.Add(this.Round1);
    this.Rounds.Add(this.Round2);
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sTalk = ScriptLocalization.Interactions.Talk;
  }

  public override void OnInteract(StateMachine state)
  {
    this.state = state;
    if (this.spoken)
      return;
    this.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    this.Conversation.Play();
    this.spoken = true;
    this.Label = "";
    this.playerFarming.GoToAndStop(new GameObject()
    {
      transform = {
        position = this.transform.position + this.ListenPosition
      }
    }, this.gameObject);
  }

  public void TellMeMore()
  {
  }

  public void EndConversation()
  {
  }

  public void BeginCombat() => this.StartCoroutine((IEnumerator) this.DoBeginCombat());

  public void OnSpawnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    Victim.OnDie -= new Health.DieAction(this.OnSpawnDie);
    ++this.DeathCount;
  }

  public IEnumerator DoBeginCombat()
  {
    Interaction_Lore_Professor interactionLoreProfessor = this;
    yield return (object) new WaitForEndOfFrame();
    RoomManager.Instance.BlockDoors();
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionLoreProfessor.gameObject, 5f);
    interactionLoreProfessor.Spine.AnimationState.SetAnimation(0, "lute-start", false);
    interactionLoreProfessor.Spine.AnimationState.AddAnimation(0, "lute-loop", true, 0.0f);
    yield return (object) new WaitForSeconds(1.5f);
    AmbientMusicController.PlayCombat(interactionLoreProfessor.PreCombatMusic, interactionLoreProfessor.CombatMusic);
    yield return (object) new WaitForSeconds(1f);
    GameManager.GetInstance().OnConversationEnd();
    yield return (object) new WaitForSeconds(0.5f);
    int CurrentRound = -1;
    while (++CurrentRound < interactionLoreProfessor.Rounds.Count)
    {
      interactionLoreProfessor.DeathCount = 0;
      foreach (Interaction_Lore_Professor.EnemyAndPosition enemyAndPosition in interactionLoreProfessor.Rounds[CurrentRound])
      {
        GameObject gameObject1 = UnityEngine.Object.Instantiate<GameObject>(enemyAndPosition.Enemy, interactionLoreProfessor.transform.parent);
        gameObject1.transform.position = interactionLoreProfessor.transform.position + enemyAndPosition.Position;
        gameObject1.GetComponent<Health>().OnDie += new Health.DieAction(interactionLoreProfessor.OnSpawnDie);
        GameObject gameObject2 = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Enemy Spawner/EnemySpawner")) as GameObject;
        gameObject2.transform.position = interactionLoreProfessor.transform.position + enemyAndPosition.Position;
        gameObject2.GetComponent<EnemySpawner>();
        yield return (object) new WaitForSeconds(enemyAndPosition.Delay);
      }
      while (interactionLoreProfessor.DeathCount < interactionLoreProfessor.Rounds[CurrentRound].Count)
        yield return (object) null;
      yield return (object) new WaitForSeconds(0.5f);
    }
    yield return (object) new WaitForSeconds(1f);
    interactionLoreProfessor.Spine.AnimationState.SetAnimation(0, "lute-stop", true);
    AmbientMusicController.StopCombat();
    AudioManager.Instance.SetMusicCombatState(false);
    yield return (object) new WaitForSeconds(1f);
    interactionLoreProfessor.Spine.skeleton.ScaleX = -1f;
    interactionLoreProfessor.Spine.AnimationState.SetAnimation(0, "teleport-out", false);
    yield return (object) new WaitForSeconds(1.15f);
    RoomManager.Instance.UnbockDoors();
    DataManager.Instance.SetVariable(DataManager.Variables.Goat_First_Meeting, true);
    InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLUE_HEART, 2, interactionLoreProfessor.transform.position + Vector3.back);
    UnityEngine.Object.Destroy((UnityEngine.Object) interactionLoreProfessor.gameObject);
  }

  public new void OnDrawGizmos()
  {
    foreach (Interaction_Lore_Professor.EnemyAndPosition enemyAndPosition in this.Round1)
    {
      if (enemyAndPosition != null)
        Utils.DrawCircleXY(this.transform.position + enemyAndPosition.Position, 0.2f, Color.yellow);
    }
    foreach (Interaction_Lore_Professor.EnemyAndPosition enemyAndPosition in this.Round2)
    {
      if (enemyAndPosition != null)
        Utils.DrawCircleXY(this.transform.position + enemyAndPosition.Position, 0.2f, new Color(1f, 0.64f, 0.0f));
    }
    Utils.DrawCircleXY(this.transform.position + this.ListenPosition, 0.4f, Color.blue);
  }

  [Serializable]
  public class EnemyAndPosition
  {
    public GameObject Enemy;
    public Vector3 Position;
    public float Delay;

    public EnemyAndPosition(GameObject Enemy, Vector3 Position, float Delay)
    {
      this.Enemy = Enemy;
      this.Position = Position;
      this.Delay = Delay;
    }
  }
}
