// Decompiled with JetBrains decompiler
// Type: IntroManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using MMBiomeGeneration;
using MMTools;
using Spine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class IntroManager : BaseMonoBehaviour
{
  public List<GameObject> GameScene;
  public GameObject DeathScene;
  public DungeonLocationManager DungeonLocationManager;
  public GameObject PlayerPrefab;
  public BiomeGenerator BiomeGenerator;
  public GameObject distortionObject;

  public void Start()
  {
    this.distortionObject.gameObject.SetActive(false);
    GameManager.NewRun("", false);
  }

  public void OnEnable()
  {
    this.StartCoroutine((IEnumerator) this.StopMusic());
    if (!((Object) WeatherSystemController.Instance != (Object) null))
      return;
    WeatherSystemController.Instance.EnteredBuilding();
  }

  public void OnDisable()
  {
  }

  public IEnumerator StopMusic()
  {
    yield return (object) new WaitForSeconds(0.5f);
    AudioManager.Instance.StopCurrentMusic();
    AudioManager.Instance.StopCurrentAtmos();
  }

  public void ToggleGameScene()
  {
    DeviceLightingManager.StopAll();
    DeviceLightingManager.UpdateLocation();
    this.DungeonLocationManager.PlayerPrefab = this.PlayerPrefab;
    this.BiomeGenerator.DoFirstArrivalRoutine = true;
    DataManager.Instance.dungeonRunDuration = Time.time;
    foreach (GameObject gameObject in this.GameScene)
      gameObject.SetActive(true);
    this.DeathScene.SetActive(false);
    Object.FindObjectOfType<IntroRoomPlayerReturns>().Play();
    GameManager.setDefaultGlobalShaders();
    RoomLockController.RoomCompleted();
    WeatherSystemController.Instance.ExitedBuilding();
    PlayerFarming.Instance.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.AnimationState_Event);
  }

  public void AnimationState_Event(TrackEntry trackEntry, Spine.Event e)
  {
    if (!(e.Data.Name == "change-skin"))
      return;
    this.PulseDisplacementObject();
  }

  public void ToggleDeathScene()
  {
    DeviceLightingManager.TransitionLighting(Color.black, new Color(0.7f, 0.65f, 0.1f, 1f), 0.0f);
    AudioManager.Instance.StopCurrentMusic();
    AudioManager.Instance.StopCurrentAtmos(false);
    foreach (GameObject gameObject in this.GameScene)
      gameObject.SetActive(false);
    this.DeathScene.SetActive(true);
    MMTransition.ResumePlay();
    WeatherSystemController.Instance.EnteredBuilding();
  }

  public void DisableBoth()
  {
    AudioManager.Instance.StopCurrentAtmos(false);
    foreach (GameObject gameObject in this.GameScene)
      gameObject.SetActive(false);
    this.DeathScene.SetActive(false);
  }

  public void PulseDisplacementObject()
  {
    if (this.distortionObject.gameObject.activeSelf)
    {
      this.distortionObject.transform.localScale = Vector3.zero;
      this.distortionObject.transform.DORestart();
      this.distortionObject.transform.DOPlay();
    }
    else
    {
      this.distortionObject.SetActive(true);
      this.distortionObject.transform.localScale = Vector3.zero;
      this.distortionObject.transform.DOScale(9f, 0.9f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.distortionObject.SetActive(false)));
    }
  }

  [CompilerGenerated]
  public void \u003CPulseDisplacementObject\u003Eb__14_0()
  {
    this.distortionObject.SetActive(false);
  }
}
