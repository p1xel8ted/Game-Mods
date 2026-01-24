// Decompiled with JetBrains decompiler
// Type: PuzzleController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using MMBiomeGeneration;
using MMRoomGeneration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class PuzzleController : BaseMonoBehaviour
{
  public static PuzzleController Instance;
  [SerializeField]
  public GameObject[] puzzles;
  [SerializeField]
  public GameObject cameraTarget;
  [SerializeField]
  public Interaction_SpecialWool specialWool;
  [SerializeField]
  public Interaction_TeleportHome teleporter;
  [SerializeField]
  public GenerateRoom puzzleRoom;
  [SerializeField]
  public UnityEvent onComplete;
  [SerializeField]
  public GameObject[] ghostGraves;
  [SerializeField]
  public List<SingleChoiceRewardOption> singleChoiceRewardOptions;
  [SerializeField]
  public GameObject[] normalObjs;
  [SerializeField]
  public GameObject[] completedObjs;
  [SerializeField]
  public SpriteRenderer[] rotSpriteRenderers;
  public EventInstance rotLoopInstance;
  public string insideAtmosParam = "inside";
  public string rotLoopSFX = "event:/dlc/atmos/yngya_belly_interior";
  public bool completed;
  public GameObject Player;
  public bool configured;

  public void OnEnable()
  {
    this.StartCoroutine((IEnumerator) this.WaitForPlayer());
    AudioManager.Instance.AdjustAtmosParameter(this.insideAtmosParam, 1f);
  }

  public void OnDisable()
  {
    PlayerReturnToBase.Disabled = false;
    Interaction_TeleportHome.PlayerActivatingEnd -= new Action<Interaction_TeleportHome>(this.OnTeleportOut);
    AudioManager.Instance.AdjustAtmosParameter(this.insideAtmosParam, 0.0f);
    AudioManager.Instance.StopLoop(this.rotLoopInstance);
  }

  public void OnDestroy() => PuzzleController.Instance = (PuzzleController) null;

  public void Awake()
  {
    PuzzleController.Instance = this;
    foreach (GameObject normalObj in this.normalObjs)
      normalObj.gameObject.SetActive(true);
    foreach (GameObject completedObj in this.completedObjs)
      completedObj.gameObject.SetActive(false);
    foreach (SpriteRenderer rotSpriteRenderer in this.rotSpriteRenderers)
    {
      if ((UnityEngine.Object) rotSpriteRenderer != (UnityEngine.Object) null)
        rotSpriteRenderer.gameObject.SetActive(false);
    }
    this.specialWool.gameObject.SetActive(false);
  }

  public IEnumerator WaitForPlayer()
  {
    PuzzleController puzzleController = this;
    while ((UnityEngine.Object) (puzzleController.Player = GameObject.FindGameObjectWithTag("Player")) == (UnityEngine.Object) null)
      yield return (object) null;
    if (puzzleController.completed)
    {
      AudioManager.Instance.StopLoop(puzzleController.rotLoopInstance);
      puzzleController.rotLoopInstance = AudioManager.Instance.CreateLoop(puzzleController.rotLoopSFX, true);
    }
    if (!puzzleController.configured && !((UnityEngine.Object) BiomeGenerator.Instance.CurrentRoom.generateRoom == (UnityEngine.Object) puzzleController.puzzleRoom))
    {
      puzzleController.configured = true;
      for (int index = 0; index < puzzleController.puzzles.Length; ++index)
        puzzleController.puzzles[index].gameObject.SetActive(DataManager.Instance.PuzzleRoomsCompleted == index);
      BiomeGenerator.Instance.CurrentRoom.Active = true;
      BlockingDoor.CloseAll();
      RoomLockController.CloseAll();
      PlayerReturnToBase.Disabled = true;
      foreach (PlayerFarming player in PlayerFarming.players)
        player.health.invincible = true;
      HUD_Manager.Instance.ReturnToBaseTransitions.MoveBackOutFunction();
      Interaction_TeleportHome.PlayerActivatingEnd += new Action<Interaction_TeleportHome>(puzzleController.OnTeleportOut);
      puzzleController.teleporter.DisableTeleporter();
      if (PlayerFarming.Location == FollowerLocation.Dungeon1_5 && DataManager.Instance.CultLeader5_StoryPosition == -1)
        DataManager.Instance.CultLeader5_StoryPosition = 0;
    }
  }

  public void Complete()
  {
    if (this.completed)
      return;
    foreach (Interaction componentsInChild in this.puzzles[DataManager.Instance.PuzzleRoomsCompleted].GetComponentsInChildren<Interaction>())
      componentsInChild.Interactable = false;
    this.completed = true;
    AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.StandardAmbience);
    ++DataManager.Instance.PuzzleRoomsCompleted;
    this.onComplete?.Invoke();
    this.StartCoroutine((IEnumerator) this.CompletedSequenceIE());
  }

  public void RevealRewardPodiums()
  {
    if (DataManager.Instance.TotalShrineGhostJuice <= 4)
    {
      this.singleChoiceRewardOptions.RemoveAt(1);
      this.singleChoiceRewardOptions[0].transform.position = new Vector3(0.0f, this.singleChoiceRewardOptions[0].transform.position.y, this.singleChoiceRewardOptions[0].transform.position.z);
    }
    foreach (SingleChoiceRewardOption choiceRewardOption in this.singleChoiceRewardOptions)
    {
      choiceRewardOption.Reveal();
      choiceRewardOption.Callback.AddListener((UnityAction) (() => this.StartCoroutine((IEnumerator) this.WaitForUIToFinish())));
    }
  }

  public IEnumerator WaitForUIToFinish()
  {
    PuzzleController puzzleController = this;
    yield return (object) null;
    while (FoundItemPickUp.FoundItemPickUps.Count > 0)
      yield return (object) null;
    puzzleController.StartCoroutine((IEnumerator) puzzleController.RevealTeleportSequence());
  }

  public void Update()
  {
  }

  public IEnumerator CompletedSequenceIE()
  {
    PuzzleController puzzleController = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(puzzleController.cameraTarget, 12f);
    AudioManager.Instance.PlayOneShot("event:/dlc/env/puzzle_room/complete", puzzleController.transform.position);
    AudioManager.Instance.PlayOneShot("event:/dlc/music/puzzle_room/stinger_complete");
    puzzleController.StartCoroutine((IEnumerator) puzzleController.ShakeCameraWithRampUp(1.5f));
    yield return (object) new WaitForSeconds(1.5f);
    GameManager.GetInstance().OnConversationNext(puzzleController.cameraTarget, 16f);
    BiomeConstants.Instance.ImpactFrameForDuration(0.1f);
    foreach (GameObject normalObj in puzzleController.normalObjs)
      normalObj.gameObject.SetActive(false);
    foreach (GameObject completedObj in puzzleController.completedObjs)
      completedObj.gameObject.SetActive(true);
    yield return (object) new WaitForSeconds(1f);
    Shader.SetGlobalVector("_GlobalFirstLocationPos", new Vector4(0.0f, 9.5f, -3f, 0.0f));
    foreach (SpriteRenderer rotSpriteRenderer in puzzleController.rotSpriteRenderers)
    {
      if ((UnityEngine.Object) rotSpriteRenderer != (UnityEngine.Object) null)
      {
        rotSpriteRenderer.material.SetFloat("_RotReveal", 0.0f);
        rotSpriteRenderer.material.DOFloat(1f, "_RotReveal", 3.2f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.Linear);
        rotSpriteRenderer.gameObject.SetActive(true);
      }
    }
    puzzleController.rotLoopInstance = AudioManager.Instance.CreateLoop(puzzleController.rotLoopSFX, true);
    yield return (object) new WaitForSeconds(3f);
    GameManager.GetInstance().OnConversationNext(puzzleController.specialWool.gameObject, 4f);
    yield return (object) new WaitForSeconds(1f);
    AudioManager.Instance.PlayOneShot("event:/dlc/env/puzzle_room/item_appear", puzzleController.transform.position);
    puzzleController.specialWool.gameObject.SetActive(true);
    BiomeConstants.Instance.EmitSmokeExplosionVFX(puzzleController.specialWool.transform.position);
    yield return (object) new WaitForSeconds(1f);
    GameManager.GetInstance().OnConversationEnd();
  }

  public IEnumerator RevealTeleportSequence()
  {
    yield return (object) new WaitForSeconds(2f);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(this.teleporter.gameObject);
    yield return (object) new WaitForSeconds(1f);
    this.teleporter.EnableTeleporter(Vector3.one * 1.42f);
    yield return (object) new WaitForSeconds(1.5f);
    GameManager.GetInstance().OnConversationEnd();
  }

  public IEnumerator ShakeCameraWithRampUp(float duration)
  {
    float t = 0.0f;
    while ((double) (t += Time.deltaTime) < (double) duration)
    {
      float t1 = t / duration;
      CameraManager.instance.ShakeCameraForDuration(Mathf.Lerp(0.0f, 0.5f, t1), Mathf.Lerp(0.0f, 1.5f, t1), duration, false);
      yield return (object) null;
    }
    CameraManager.instance.Stopshake();
  }

  public void OnTeleportOut(Interaction_TeleportHome teleporter)
  {
    if (!((UnityEngine.Object) this.teleporter == (UnityEngine.Object) teleporter))
      return;
    Interaction_TeleportHome.PlayerActivatingEnd -= new Action<Interaction_TeleportHome>(this.OnTeleportOut);
    PlayerReturnToBase.Disabled = false;
  }

  [CompilerGenerated]
  public void \u003CRevealRewardPodiums\u003Eb__24_0()
  {
    this.StartCoroutine((IEnumerator) this.WaitForUIToFinish());
  }
}
