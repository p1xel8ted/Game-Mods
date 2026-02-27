// Decompiled with JetBrains decompiler
// Type: FinalBossIntroManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using MMBiomeGeneration;
using System.Collections;
using UnityEngine;

#nullable disable
public class FinalBossIntroManager : MonoBehaviour
{
  [SerializeField]
  private Color backgroundCameraColor;
  [SerializeField]
  private Interaction_WeaponSelectionPodium[] weaponPodiums;
  [SerializeField]
  private Interaction_WeaponSelectionPodium[] cursePodiums;
  [SerializeField]
  private RoomLockController blockingDoor;
  private Camera camera;
  private bool weaponSelected;
  private bool curseSelected;

  private void Start()
  {
    foreach (Interaction weaponPodium in this.weaponPodiums)
      weaponPodium.OnInteraction += new Interaction.InteractionEvent(this.WeaponSelected);
    foreach (Interaction cursePodium in this.cursePodiums)
      cursePodium.OnInteraction += new Interaction.InteractionEvent(this.CurseSelected);
  }

  private void OnEnable() => this.StartCoroutine((IEnumerator) this.Play());

  private void WeaponSelected(StateMachine state)
  {
    this.weaponSelected = true;
    if (!this.weaponSelected || !this.curseSelected)
      return;
    RoomLockController.OpenAll();
    this.blockingDoor.Completed = true;
    FaithAmmo.Reload();
  }

  private void CurseSelected(StateMachine state)
  {
    this.curseSelected = true;
    if (!this.weaponSelected || !this.curseSelected)
      return;
    RoomLockController.OpenAll();
    this.blockingDoor.Completed = true;
  }

  private IEnumerator Play()
  {
    FinalBossIntroManager bossIntroManager = this;
    yield return (object) new WaitForEndOfFrame();
    if (bossIntroManager.gameObject.activeInHierarchy)
    {
      DataManager.Instance.CameFromDeathCatFight = true;
      WeatherController.InsideBuilding = true;
      bossIntroManager.camera = Camera.main;
      BiomeGenerator.Instance.SpawnDemons();
      yield return (object) new WaitForEndOfFrame();
      PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/final-ritual-land", 0, false);
      PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, false, 0.0f);
      AudioManager.Instance.PlayOneShot("event:/pentagram/pentagram_teleport_segment", bossIntroManager.gameObject);
      yield return (object) new WaitForSeconds(1f);
      bossIntroManager.camera.backgroundColor = bossIntroManager.backgroundCameraColor;
      yield return (object) new WaitForSeconds(2f);
      PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.Idle;
      PlayerFarming.Instance.EndGoToAndStop();
      RoomLockController.CloseAll();
      GameManager.InitialDungeonEnter = true;
      bossIntroManager.StartCoroutine((IEnumerator) BiomeGenerator.Instance.DelayActivateRoom(true));
    }
  }

  public void CameraFocusOnDeathCat()
  {
    GameManager.GetInstance().CamFollowTarget.transform.DORotate(new Vector3(-60f, 0.0f, 0.0f), 3f).SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(Ease.InOutSine);
  }

  public void ResetCameraFocus()
  {
    GameManager.GetInstance().CamFollowTarget.transform.DORotate(new Vector3(-45f, 0.0f, 0.0f), 1f).SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(Ease.InOutSine);
  }
}
