// Decompiled with JetBrains decompiler
// Type: Villager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private static List<Villager> villagers = new List<Villager>();
  public Rigidbody2D rigidbody2D;
  private StateMachine state;
  public List<BaseMonoBehaviour> BaseMonoBehavioursToDisable = new List<BaseMonoBehaviour>();
  public SkeletonAnimation Spine;
  [TermsPopup("")]
  public string WarningSpeech1;
  [TermsPopup("")]
  public string WarningSpeech2;
  private GameObject Attacker;
  private int AttackCounter;
  private Health EnemyHealth;

  private void Start()
  {
    this.state = this.GetComponent<StateMachine>();
    this.state.CURRENT_STATE = StateMachine.State.CustomAction0;
  }

  private void OnEnable()
  {
    this.health = this.GetComponent<Health>();
    Villager.villagers.Add(this);
    this.health.OnHit += new Health.HitAction(this.OnHit);
    this.health.OnDie += new Health.DieAction(this.OnDie);
  }

  private void OnDisable()
  {
    Villager.villagers.Remove(this);
    this.health.OnHit -= new Health.HitAction(this.OnHit);
    this.health.OnDie -= new Health.DieAction(this.OnDie);
  }

  private void OnHit(
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

  private void OnDie(
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

  private void SoundAlarm()
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

  private IEnumerator DoVillagersAttack()
  {
    yield return (object) new WaitForEndOfFrame();
    this.formationFighter.enabled = true;
    this.formationFighter.state.CURRENT_STATE = StateMachine.State.Idle;
    this.formationFighter.TargetEnemy = this.EnemyHealth;
  }
}
