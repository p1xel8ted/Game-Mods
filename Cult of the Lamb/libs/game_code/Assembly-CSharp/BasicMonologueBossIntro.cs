// Decompiled with JetBrains decompiler
// Type: BasicMonologueBossIntro
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Spine.Unity;
using System;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class BasicMonologueBossIntro : BossIntro
{
  [SpineAnimation("", "", true, false, dataField = "BossSpine")]
  public string IntroAnimation;
  [SpineAnimation("", "", true, false, dataField = "BossSpine")]
  public string IdleAnimation;
  public Health BossHealth;
  public Interaction_SimpleConversation IntroConversation;
  public string IntroSFX = "event:/dlc/dungeon05/enemy/miniboss_lightning/intro";
  public UnitObject BossUnitObject;
  [TermsPopup("")]
  public string BossName = "NAMES/MiniBoss/Dungeon5/MiniBoss3";
  [SerializeField]
  public string skipIntroVariableName;

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
    BasicMonologueBossIntro monologueBossIntro = this;
    monologueBossIntro.LookAtPosition(PlayerFarming.players[0].transform.position);
    if (!monologueBossIntro.ShouldSkipIntro())
    {
      GameManager.GetInstance().OnConversationNew();
      yield return (object) monologueBossIntro.PlayIntroConversation();
    }
    else
    {
      foreach (Behaviour componentsInChild in monologueBossIntro.GetComponentsInChildren<Interaction_SimpleConversation>(true))
        componentsInChild.enabled = false;
    }
    GameManager.GetInstance().OnConversationEnd();
    yield return (object) new WaitForSeconds(0.5f);
    monologueBossIntro.BossSpine.AnimationState.SetAnimation(0, monologueBossIntro.IntroAnimation, false);
    monologueBossIntro.BossSpine.AnimationState.AddAnimation(0, monologueBossIntro.IdleAnimation, true, 0.0f);
    AudioManager.Instance.PlayOneShot(monologueBossIntro.IntroSFX);
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
    MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
    UIBossHUD.Play(monologueBossIntro.BossHealth, LocalizationManager.GetTranslation(monologueBossIntro.BossName));
    monologueBossIntro.Callback.Invoke();
    yield return (object) null;
  }

  public IEnumerator PlayIntroConversation()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    BasicMonologueBossIntro monologueBossIntro = this;
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
    monologueBossIntro.IntroConversation.gameObject.SetActive(true);
    monologueBossIntro.IntroConversation.Play();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitUntil((Func<bool>) new Func<bool>(monologueBossIntro.\u003CPlayIntroConversation\u003Eb__10_0));
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void LookAtPosition(Vector3 position)
  {
    float angle = Utils.GetAngle(this.transform.position, position);
    this.BossUnitObject.state.facingAngle = angle;
    this.BossUnitObject.state.LookAngle = angle;
  }

  [CompilerGenerated]
  public bool \u003CPlayIntroConversation\u003Eb__10_0() => this.IntroConversation.Finished;
}
