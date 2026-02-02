// Decompiled with JetBrains decompiler
// Type: LightningCrossBossIntro
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Spine.Unity;
using System;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class LightningCrossBossIntro : BossIntro
{
  [SpineAnimation("", "", true, false, dataField = "BossSpine")]
  public string IntroAnimation;
  [SpineAnimation("", "", true, false, dataField = "BossSpine")]
  public string IdleAnimation;
  public Health BossHealth;
  public Interaction_SimpleConversation IntroConversation;
  public string IntroSFX = "event:/dlc/dungeon05/enemy/miniboss_lightning/intro";
  public UnitObject bossUnitObject;
  public string bossName = "NAMES/MiniBoss/Dungeon5/MiniBoss3";
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
    LightningCrossBossIntro lightningCrossBossIntro = this;
    lightningCrossBossIntro.LookAtPosition(PlayerFarming.players[0].transform.position);
    yield return (object) lightningCrossBossIntro.PositionPlayers();
    if (lightningCrossBossIntro.ShouldSkipIntro())
    {
      foreach (Behaviour componentsInChild in lightningCrossBossIntro.GetComponentsInChildren<Interaction_SimpleConversation>(true))
        componentsInChild.enabled = false;
    }
    else
    {
      GameManager.GetInstance().OnConversationNew();
      yield return (object) lightningCrossBossIntro.PlayIntroConversation();
    }
    GameManager.GetInstance().OnConversationEnd();
    yield return (object) new WaitForSeconds(0.5f);
    lightningCrossBossIntro.BossSpine.AnimationState.SetAnimation(0, lightningCrossBossIntro.IntroAnimation, false);
    lightningCrossBossIntro.BossSpine.AnimationState.AddAnimation(0, lightningCrossBossIntro.IdleAnimation, true, 0.0f);
    AudioManager.Instance.PlayOneShot(lightningCrossBossIntro.IntroSFX);
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
    MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
    UIBossHUD.Play(lightningCrossBossIntro.BossHealth, LocalizationManager.GetTranslation(lightningCrossBossIntro.bossName));
    lightningCrossBossIntro.Callback.Invoke();
    yield return (object) null;
  }

  public IEnumerator PositionPlayers()
  {
    LightningCrossBossIntro lightningCrossBossIntro = this;
    Vector3 endPosition = new Vector3(0.0f, -2f, 0.0f);
    PlayerFarming.Instance.GoToAndStop(endPosition, lightningCrossBossIntro.gameObject, groupAction: true);
    while (PlayerFarming.Instance.GoToAndStopping && (double) Vector3.Distance(PlayerFarming.Instance.transform.position, endPosition) > 1.0)
      yield return (object) null;
  }

  public IEnumerator PlayIntroConversation()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    LightningCrossBossIntro lightningCrossBossIntro = this;
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
    lightningCrossBossIntro.IntroConversation.Play();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitUntil((Func<bool>) new Func<bool>(lightningCrossBossIntro.\u003CPlayIntroConversation\u003Eb__11_0));
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void LookAtPosition(Vector3 position)
  {
    float angle = Utils.GetAngle(this.transform.position, position);
    this.bossUnitObject.state.facingAngle = angle;
    this.bossUnitObject.state.LookAngle = angle;
  }

  [CompilerGenerated]
  public bool \u003CPlayIntroConversation\u003Eb__11_0() => this.IntroConversation.Finished;
}
