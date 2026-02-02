// Decompiled with JetBrains decompiler
// Type: FinalBossIntroManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  public Color backgroundCameraColor;
  [SerializeField]
  public Interaction_WeaponSelectionPodium[] weaponPodiums;
  [SerializeField]
  public Interaction_WeaponSelectionPodium[] cursePodiums;
  [SerializeField]
  public RoomLockController blockingDoor;
  public Camera camera;
  public bool weaponSelected;
  public bool curseSelected;

  public void Start()
  {
    if (!DungeonSandboxManager.Active)
      return;
    foreach (Component weaponPodium in this.weaponPodiums)
      weaponPodium.gameObject.SetActive(false);
    foreach (Component cursePodium in this.cursePodiums)
      cursePodium.gameObject.SetActive(false);
    RoomLockController.OpenAll();
  }

  public void OnEnable() => this.StartCoroutine((IEnumerator) this.Play());

  public void Update()
  {
    if (this.blockingDoor.Completed)
      return;
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if (player.currentWeapon == EquipmentType.None || player.currentCurse == EquipmentType.None)
        return;
    }
    RoomLockController.OpenAll();
    this.blockingDoor.Completed = true;
    PlayerFarming.ReloadAllFaith();
  }

  public IEnumerator Play()
  {
    FinalBossIntroManager bossIntroManager = this;
    yield return (object) new WaitForEndOfFrame();
    if (bossIntroManager.gameObject.activeInHierarchy)
    {
      DataManager.Instance.CameFromDeathCatFight = true;
      WeatherSystemController.Instance.EnteredBuilding();
      bossIntroManager.camera = Camera.main;
      BiomeGenerator.Instance.SpawnDemons = true;
      BiomeGenerator.Instance.DoSpawnDemons();
      yield return (object) new WaitForEndOfFrame();
      while ((Object) PlayerFarming.Instance == (Object) null)
        yield return (object) null;
      Vector3 vector3 = new Vector3(0.0f, -43f, 0.0f);
      if (PlayerFarming.playersCount > 1)
        vector3.x -= 0.75f;
      foreach (PlayerFarming player in PlayerFarming.players)
      {
        player.transform.position = vector3;
        player.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
        player.simpleSpineAnimator.Animate("rituals/final-ritual-land", 0, false);
        player.simpleSpineAnimator.AddAnimate("idle", 0, false, 0.0f);
        vector3.x += 1.75f;
      }
      AudioManager.Instance.PlayOneShot("event:/pentagram/pentagram_teleport_segment", bossIntroManager.gameObject);
      yield return (object) new WaitForSeconds(1f);
      bossIntroManager.camera.backgroundColor = bossIntroManager.backgroundCameraColor;
      yield return (object) new WaitForSeconds(2f);
      PlayerFarming.SetStateForAllPlayers();
      foreach (PlayerFarming player in PlayerFarming.players)
      {
        if (player.GoToAndStopping)
          player.EndGoToAndStop();
        player.circleCollider2D.enabled = true;
      }
      GameManager.GetInstance().OnConversationEnd();
      GameManager.GetInstance().AddPlayerToCamera();
      RoomLockController.CloseAll(false);
      GameManager.InitialDungeonEnter = true;
      bossIntroManager.StartCoroutine((IEnumerator) BiomeGenerator.Instance.DelayActivateRoom(!DungeonSandboxManager.Active));
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
