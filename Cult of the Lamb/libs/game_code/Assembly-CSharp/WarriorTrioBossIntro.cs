// Decompiled with JetBrains decompiler
// Type: WarriorTrioBossIntro
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMODUnity;
using System;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class WarriorTrioBossIntro : BossIntro
{
  [SerializeField]
  public EnemyWolfGuardian[] warriors;
  [SerializeField]
  public Interaction_SimpleConversation introConversation;
  [SerializeField]
  public Health health;
  [EventRef]
  public string IntroSwoopInSFX = "event:/dlc/dungeon05/enemy/miniboss_wolf_guardian/intro_swoop_in";
  public bool DEBUG_START;
  public Vector3 startPosLeft = new Vector3(-9.7f, 0.0f, 0.0f);
  public Vector3 startPosCentre = new Vector3(0.0f, 5.2f, 0.0f);
  public Vector3 startPosRight = new Vector3(9.7f, 0.0f, 0.0f);
  public Vector3 endPosLeft = new Vector3(-3.4f, 0.0f, 0.0f);
  public Vector3 endPosCentre = new Vector3(0.0f, 0.41f, 0.0f);
  public Vector3 endPosRight = new Vector3(3.4f, 0.0f, 0.0f);
  public string bossName = "NAMES/MiniBoss/Dungeon5/MiniBoss4";
  public Vector2 playerGoToPosition = new Vector2(0.0f, -2.3f);
  [SerializeField]
  public float playerGoToPositionDistanceCheck = 1.5f;
  [SerializeField]
  public string skipIntroVariableName;

  public void Awake()
  {
    this.SetupWarriorStateMachines();
    this.PositionWarriorTrioStart();
  }

  public bool ShouldSkipIntro()
  {
    if (this.skipIntroVariableName == null || this.skipIntroVariableName == "")
      return false;
    FieldInfo field = typeof (DataManager).GetField(this.skipIntroVariableName);
    if (field != (FieldInfo) null && field.FieldType == typeof (bool))
    {
      bool flag = (bool) field.GetValue((object) DataManager.Instance);
      Debug.Log((object) $" variable to skip found {this.skipIntroVariableName} / {flag.ToString()}");
      return flag;
    }
    Debug.Log((object) ("checking variable to skip not found " + this.skipIntroVariableName));
    return false;
  }

  public override IEnumerator PlayRoutine(bool skipped = false)
  {
    WarriorTrioBossIntro warriorTrioBossIntro = this;
    yield return (object) warriorTrioBossIntro.PositionPlayers();
    yield return (object) warriorTrioBossIntro.MoveTrioToCentre();
    if (!warriorTrioBossIntro.ShouldSkipIntro())
    {
      GameManager.GetInstance().OnConversationNew();
      yield return (object) warriorTrioBossIntro.PlayIntroConversation();
    }
    else
    {
      foreach (Behaviour componentsInChild in warriorTrioBossIntro.GetComponentsInChildren<Interaction_SimpleConversation>(true))
        componentsInChild.enabled = false;
    }
    GameManager.GetInstance().OnConversationEnd();
    warriorTrioBossIntro.Callback.Invoke();
    yield return (object) null;
  }

  public void SetupWarriorStateMachines()
  {
    foreach (EnemyWolfGuardian warrior in this.warriors)
      warrior.SetupStateMachine(false);
  }

  public void PositionWarriorTrioStart()
  {
    if (this.warriors.Length == 0)
      this.warriors = this.GetComponentsInChildren<EnemyWolfGuardian>(true);
    this.warriors[0].GoToIntroPrepState(this.startPosLeft);
    this.warriors[1].GoToIntroPrepState(this.startPosCentre);
    this.warriors[2].GoToIntroPrepState(this.startPosRight);
  }

  public IEnumerator MoveTrioToCentre()
  {
    float duration = 1.5f;
    float time = 0.0f;
    this.warriors[0].GoToFlyInState(this.endPosLeft, duration, true, true);
    this.warriors[1].GoToFlyInState(this.endPosCentre, duration, true, true);
    this.warriors[2].GoToFlyInState(this.endPosRight, duration, true, true);
    if (!string.IsNullOrEmpty(this.IntroSwoopInSFX))
      AudioManager.Instance.PlayOneShot(this.IntroSwoopInSFX);
    while ((double) time < (double) duration)
    {
      time += Time.deltaTime;
      foreach (EnemyWolfGuardian warrior in this.warriors)
        warrior.ForceHeadLookAtTarget(new Vector3(0.0f, -4f, 0.0f));
      yield return (object) null;
    }
  }

  public IEnumerator PlayIntroConversation()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    WarriorTrioBossIntro warriorTrioBossIntro = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    warriorTrioBossIntro.introConversation.Play();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitUntil(new Func<bool>(warriorTrioBossIntro.\u003CPlayIntroConversation\u003Eb__21_0));
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator PositionPlayers()
  {
    PlayerFarming.Instance.GoToAndStop((Vector3) this.playerGoToPosition, this.warriors[1].gameObject, groupAction: true);
    while ((double) Vector3.Distance(PlayerFarming.Instance.transform.position, (Vector3) this.playerGoToPosition) > (double) this.playerGoToPositionDistanceCheck)
      yield return (object) null;
    yield return (object) new WaitForSeconds(0.3f);
  }

  public void OnDrawGizmos()
  {
    Utils.DrawCircleXY((Vector3) this.playerGoToPosition, this.playerGoToPositionDistanceCheck, Color.red);
  }

  [CompilerGenerated]
  public bool \u003CPlayIntroConversation\u003Eb__21_0() => this.introConversation.Finished;
}
