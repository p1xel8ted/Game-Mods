// Decompiled with JetBrains decompiler
// Type: DeathCatBossIntro
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class DeathCatBossIntro : BossIntro
{
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "BossSpine")]
  public string introAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "BossSpine")]
  public string idleAnimation;

  public override IEnumerator PlayRoutine(bool skipped = false)
  {
    DeathCatBossIntro deathCatBossIntro = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(deathCatBossIntro.CameraTarget, 10f);
    yield return (object) new WaitForSeconds(0.5f);
    deathCatBossIntro.BossSpine.AnimationState.SetAnimation(0, deathCatBossIntro.introAnimation, false);
    yield return (object) new WaitForSeconds(1.8f);
    CameraManager.instance.ShakeCameraForDuration(0.2f, 0.3f, 1.5f);
    float ShakeDuration = 2.3f;
    while ((double) (ShakeDuration -= Time.deltaTime) > 0.0)
    {
      CameraManager.shakeCamera((float) ((1.0 - (double) ShakeDuration / 2.2999999523162842) * 0.20000000298023224), (float) Random.Range(0, 360));
      yield return (object) null;
    }
    deathCatBossIntro.BossSpine.AnimationState.SetAnimation(0, deathCatBossIntro.idleAnimation, true);
    GameManager.GetInstance().OnConversationEnd();
    yield return (object) new WaitForSeconds(0.5f);
    deathCatBossIntro.Callback?.Invoke();
  }
}
