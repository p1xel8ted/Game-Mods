// Decompiled with JetBrains decompiler
// Type: Goat_GuardianDoor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class Goat_GuardianDoor : BaseMonoBehaviour
{
  public SkeletonAnimation Spine;
  public EnemyGuardian1 Guardian;
  public EnemyGuardian2 Guardian2;
  public Interaction DoorInteraction;
  public SkeletonAnimation DoorSpine;
  public GameObject DoorCameraFocus;

  public void Play() => this.StartCoroutine((IEnumerator) this.DoPlay());

  public IEnumerator DoPlay()
  {
    Goat_GuardianDoor goatGuardianDoor = this;
    goatGuardianDoor.DoorInteraction.enabled = false;
    yield return (object) new WaitForEndOfFrame();
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(goatGuardianDoor.gameObject, 5f);
    BlockingDoor.CloseAll();
    goatGuardianDoor.Spine.AnimationState.SetAnimation(0, "lute-start", false);
    goatGuardianDoor.Spine.AnimationState.AddAnimation(0, "lute-loop", true, 0.0f);
    yield return (object) new WaitForSeconds(1.5f);
    AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.MainBossA);
    yield return (object) new WaitForSeconds(0.7f);
    GameManager.GetInstance().OnConversationEnd();
    yield return (object) new WaitForSeconds(0.5f);
    goatGuardianDoor.Guardian.Play();
  }

  public IEnumerator EndGuardianFightRoutine()
  {
    Goat_GuardianDoor goatGuardianDoor = this;
    GameManager.GetInstance().OnConversationNext(goatGuardianDoor.gameObject, 6f);
    yield return (object) new WaitForSeconds(1f);
    goatGuardianDoor.Spine.AnimationState.SetAnimation(0, "lute-stop", true);
    AmbientMusicController.StopCombat();
    AudioManager.Instance.SetMusicCombatState(false);
    yield return (object) new WaitForSeconds(1f);
    goatGuardianDoor.Spine.skeleton.ScaleX = -1f;
    goatGuardianDoor.Spine.AnimationState.SetAnimation(0, "teleport-out", false);
    yield return (object) new WaitForSeconds(1.7f);
    goatGuardianDoor.Spine.enabled = false;
    GameManager.GetInstance().OnConversationEnd();
    Object.Destroy((Object) goatGuardianDoor.gameObject);
  }
}
