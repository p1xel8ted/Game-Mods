// Decompiled with JetBrains decompiler
// Type: FollowerMinibossIntro
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Spine.Unity;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FollowerMinibossIntro : BossIntro
{
  [SerializeField]
  public Interaction_SimpleConversation afterMathConversation;
  [SerializeField]
  public AnimationCurve jumpCurve = AnimationCurve.EaseInOut(0.0f, 0.0f, 1f, 1f);
  [SerializeField]
  public UnitObject _attackAI;
  [SerializeField]
  public UnitObject _followerAI;
  [SpineAnimation("", "", true, false, dataField = "BossSpine")]
  [SerializeField]
  public string jumpAnimation;
  [SpineAnimation("", "", true, false, dataField = "BossSpine")]
  [SerializeField]
  public string jumpEndAnimation;
  [SpineAnimation("", "", true, false, dataField = "BossSpine")]
  [SerializeField]
  public string idleAnimation;
  [SpineAnimation("", "", true, false, dataField = "BossSpine")]
  [SerializeField]
  public string runAnimation;
  public Vector3 _lastScientistMovePosition;
  public Health _scientistHealth;

  public void Awake()
  {
    this._scientistHealth = this.GetComponent<Health>();
    this._attackAI.enabled = false;
    this._scientistHealth.enabled = false;
  }

  public override IEnumerator PlayRoutine(bool skipped = false)
  {
    yield return (object) new WaitForSeconds(1f);
    yield return (object) this.JumpTo(this._lastScientistMovePosition);
    yield return (object) this.MoveTo(Vector3.zero, 1f, false);
    yield return (object) this.PlayAfterMathConversation();
    yield return (object) this.StartFight();
  }

  public IEnumerator StartFight()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    FollowerMinibossIntro followerMinibossIntro = this;
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
    GameManager.GetInstance().OnConversationEnd();
    HUD_DisplayName.Play(LocalizationManager.GetTranslation("NAMES/MiniBoss/Dungeon5/ScientistJokeName"), 2, HUD_DisplayName.Positions.Centre);
    GameManager.GetInstance().AddToCamera(followerMinibossIntro.gameObject);
    followerMinibossIntro._scientistHealth.enabled = true;
    followerMinibossIntro._attackAI.enabled = true;
    followerMinibossIntro._followerAI.enabled = false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) null;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator PlayAfterMathConversation()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    FollowerMinibossIntro followerMinibossIntro = this;
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
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(followerMinibossIntro.gameObject);
    followerMinibossIntro.afterMathConversation.Play();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitUntil((Func<bool>) new Func<bool>(followerMinibossIntro.\u003CPlayAfterMathConversation\u003Eb__13_0));
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator MoveTo(Vector3 pos, float radius = 0.1f, bool cancelPath = true)
  {
    FollowerMinibossIntro followerMinibossIntro = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(followerMinibossIntro.gameObject);
    followerMinibossIntro._followerAI.givePath(pos);
    followerMinibossIntro.BossSpine.AnimationState.SetAnimation(0, followerMinibossIntro.runAnimation, true);
    followerMinibossIntro._followerAI.state.CURRENT_STATE = StateMachine.State.Moving;
    followerMinibossIntro._followerAI.state.LookAngle = Utils.GetAngle(followerMinibossIntro.transform.position, pos);
    followerMinibossIntro.BossSpine.skeleton.ScaleX = (double) followerMinibossIntro._followerAI.state.LookAngle <= 90.0 || (double) followerMinibossIntro._followerAI.state.LookAngle >= 270.0 ? -1f : 1f;
    while ((double) Vector3.Distance(followerMinibossIntro.transform.position, pos) > (double) radius)
      yield return (object) null;
    if (cancelPath)
      followerMinibossIntro._followerAI.ClearPaths();
    followerMinibossIntro.BossSpine.AnimationState.SetAnimation(0, followerMinibossIntro.idleAnimation, true);
    followerMinibossIntro._lastScientistMovePosition = followerMinibossIntro.transform.position;
  }

  public IEnumerator JumpTo(Vector3 targetPosition)
  {
    FollowerMinibossIntro followerMinibossIntro = this;
    float elapsed = 0.0f;
    float duration = 0.5f;
    Vector3 startPosition = followerMinibossIntro.transform.position;
    float arcHeight = 2f;
    followerMinibossIntro.BossSpine.AnimationState.SetAnimation(0, followerMinibossIntro.jumpAnimation, true);
    while ((double) elapsed < (double) duration)
    {
      elapsed += Time.deltaTime;
      float num1 = Mathf.Clamp01(elapsed / duration);
      Vector3 vector3 = Vector3.Lerp(startPosition, targetPosition, num1);
      float num2 = followerMinibossIntro.jumpCurve.Evaluate(num1) * arcHeight;
      followerMinibossIntro.transform.position = new Vector3(vector3.x, vector3.y, vector3.z - num2);
      yield return (object) null;
    }
    followerMinibossIntro.BossSpine.AnimationState.SetAnimation(0, followerMinibossIntro.jumpEndAnimation, false);
    followerMinibossIntro.BossSpine.AnimationState.AddAnimation(0, followerMinibossIntro.idleAnimation, true, 0.0f);
  }

  [CompilerGenerated]
  public bool \u003CPlayAfterMathConversation\u003Eb__13_0() => this.afterMathConversation.Finished;
}
