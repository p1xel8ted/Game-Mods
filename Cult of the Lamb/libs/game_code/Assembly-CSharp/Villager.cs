// Decompiled with JetBrains decompiler
// Type: Villager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MMTools;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Villager : BaseMonoBehaviour
{
  public Health health;
  public FormationFighter formationFighter;
  public static List<Villager> villagers = new List<Villager>();
  public Rigidbody2D rigidbody2D;
  public StateMachine state;
  public List<BaseMonoBehaviour> BaseMonoBehavioursToDisable = new List<BaseMonoBehaviour>();
  public SkeletonAnimation Spine;
  [TermsPopup("")]
  public string WarningSpeech1;
  [TermsPopup("")]
  public string WarningSpeech2;
  public GameObject Attacker;
  public int AttackCounter;
  public Health EnemyHealth;

  public void Start()
  {
    this.state = this.GetComponent<StateMachine>();
    this.state.CURRENT_STATE = StateMachine.State.CustomAction0;
  }

  public void OnEnable()
  {
    this.health = this.GetComponent<Health>();
    Villager.villagers.Add(this);
    this.health.OnHit += new Health.HitAction(this.OnHit);
    this.health.OnDie += new Health.DieAction(this.OnDie);
  }

  public void OnDisable()
  {
    Villager.villagers.Remove(this);
    this.health.OnHit -= new Health.HitAction(this.OnHit);
    this.health.OnDie -= new Health.DieAction(this.OnDie);
  }

  public void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    this.Attacker = Attacker;
    ConversationObject ConversationObject = (ConversationObject) null;
    Villager component = Conversation_Speaker.Speaker1.GetComponent<Villager>();
    ++component.AttackCounter;
    switch (component.AttackCounter)
    {
      case 1:
        Debug.Log((object) ("WarningSpeech1 " + this.WarningSpeech1));
        ConversationObject = new ConversationObject(new List<ConversationEntry>()
        {
          new ConversationEntry(component.gameObject, component.WarningSpeech1)
        }, (List<MMTools.Response>) null, (System.Action) null);
        break;
      case 2:
        ConversationObject = new ConversationObject(new List<ConversationEntry>()
        {
          new ConversationEntry(component.gameObject, component.WarningSpeech2)
        }, (List<MMTools.Response>) null, new System.Action(this.SoundAlarm));
        break;
    }
    MMConversation.Play(ConversationObject);
    this.formationFighter.knockBackVX = 0.0f;
    this.formationFighter.knockBackVY = 0.0f;
  }

  public void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    if (Health.team2.Count > 1)
      return;
    AmbientMusicController.StopCombat();
    AudioManager.Instance.SetMusicCombatState(false);
  }

  public void SoundAlarm()
  {
    AmbientMusicController.PlayCombat();
    AudioManager.Instance.SetMusicCombatState();
    this.VillagersAttack(this.Attacker);
    foreach (Villager villager in Villager.villagers)
    {
      if ((UnityEngine.Object) villager != (UnityEngine.Object) this)
        villager.VillagersAttack(this.Attacker);
    }
  }

  public void VillagersAttack(GameObject Attacker)
  {
    this.Spine.skeleton.SetSkin("Evil");
    foreach (Behaviour behaviour in this.BaseMonoBehavioursToDisable)
      behaviour.enabled = false;
    this.health.OnHit -= new Health.HitAction(this.OnHit);
    this.EnemyHealth = PlayerFarming.Instance.GetComponent<Health>();
    this.StartCoroutine((IEnumerator) this.DoVillagersAttack());
  }

  public IEnumerator DoVillagersAttack()
  {
    yield return (object) new WaitForEndOfFrame();
    this.formationFighter.enabled = true;
    this.formationFighter.state.CURRENT_STATE = StateMachine.State.Idle;
    this.formationFighter.TargetEnemy = this.EnemyHealth;
  }
}
