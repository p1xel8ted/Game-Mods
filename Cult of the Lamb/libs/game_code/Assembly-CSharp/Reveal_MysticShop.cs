// Decompiled with JetBrains decompiler
// Type: Reveal_MysticShop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using MMTools;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class Reveal_MysticShop : MonoBehaviour
{
  [SerializeField]
  public List<GameObject> ObjectsToMove = new List<GameObject>();
  public List<float> ObjectsToMoveZ = new List<float>();
  [SerializeField]
  public List<GameObject> ObjectsToDisable = new List<GameObject>();
  [SerializeField]
  public GameObject ritualFX;
  [SerializeField]
  public SimpleSetCamera simpleCamera;
  [SerializeField]
  public SimpleSetCamera doorCamera;
  [SerializeField]
  public SkeletonAnimation mysticSeller;
  [SerializeField]
  public SkeletonRendererCustomMaterials mysticSellerMaterialOverride;
  [SerializeField]
  public Transform playerPosition;
  [SerializeField]
  public Canvas canvas;
  [SerializeField]
  public Image image;
  [SerializeField]
  public GameObject rift;
  [SerializeField]
  public GameObject cameraPos;
  [SerializeField]
  public GameObject revealDoorsCameraPos;
  [SerializeField]
  public Interaction_SimpleConversation introConversation;
  [SerializeField]
  public Interaction_BaseDungeonDoor[] doors;
  [SerializeField]
  public Interaction_BaseDungeonDoor[] newDoors;
  [SerializeField]
  public GameObject crownPosition;
  public ParticleSystem doorParticles1;
  public ParticleSystem doorParticles2;
  public ParticleSystem doorParticles3;
  public ParticleSystem doorParticles4;
  public ParticleSystem rockParticles1;
  public ParticleSystem rockParticles2;
  public Vector3 mysticSellerStartingPos;
  public EventInstance LoopedSound;
  public static int FillAlpha = Shader.PropertyToID("_FillAlpha");
  public Interaction_MysticShop mysticShop;

  public void OnDisable() => AudioManager.Instance.StopLoop(this.LoopedSound);

  public void Start()
  {
    this.mysticShop = this.GetComponent<Interaction_MysticShop>();
    this.ritualFX.SetActive(false);
    this.canvas.gameObject.SetActive(false);
    this.mysticSeller.gameObject.SetActive(DataManager.Instance.OnboardedMysticShop);
    this.rift.gameObject.SetActive(DataManager.Instance.OnboardedMysticShop);
    this.introConversation.enabled = false;
    this.simpleCamera.gameObject.SetActive(!DataManager.Instance.OnboardedMysticShop);
    this.doorCamera.gameObject.SetActive(!DataManager.Instance.OnboardedMysticShop);
    foreach (Component door in this.doors)
      door.gameObject.SetActive(!DataManager.Instance.OnboardedMysticShop);
    foreach (Component newDoor in this.newDoors)
      newDoor.gameObject.SetActive(DataManager.Instance.OnboardedMysticShop);
    if (!DataManager.Instance.OnboardedMysticShop)
      return;
    foreach (GameObject gameObject in this.ObjectsToDisable)
      gameObject.SetActive(false);
  }

  public void Reveal() => this.StartCoroutine(this.RevealRoutine());

  public IEnumerator RevealRoutine()
  {
    Reveal_MysticShop revealMysticShop = this;
    revealMysticShop.ritualFX.SetActive(false);
    Vector3 position = revealMysticShop.mysticSeller.transform.position;
    revealMysticShop.mysticSellerStartingPos = position;
    Vector3 vector3 = position - new Vector3(0.0f, -2f, 0.0f);
    revealMysticShop.mysticSeller.transform.position = vector3;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject, 15f);
    bool Waiting = true;
    PlayerFarming.Instance.GoToAndStop(revealMysticShop.playerPosition.position + new Vector3(0.0f, -2f), revealMysticShop.playerPosition.gameObject, GoToCallback: (System.Action) (() =>
    {
      PlayerFarming.Instance.transform.position = this.playerPosition.transform.position + new Vector3(0.0f, -2f);
      Waiting = false;
    }), groupAction: true);
    while (Waiting)
      yield return (object) null;
    GameManager.GetInstance().OnConversationNext(revealMysticShop.mysticSeller.gameObject, 20f);
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForSeconds(0.5f);
    AudioManager.Instance.PlayOneShot("event:/material/earthquake", revealMysticShop.transform.position);
    AudioManager.Instance.PlayOneShot("event:/boss/frog/transition_intro_zoom");
    MMVibrate.RumbleContinuous(1f, 5f);
    revealMysticShop.StartCoroutine(revealMysticShop.ShakeCameraWithRampUp());
    foreach (GameObject gameObject in revealMysticShop.ObjectsToMove)
    {
      if (gameObject.activeSelf)
      {
        gameObject.transform.DOShakeRotation(4f, 20f).SetEase<Tweener>(Ease.InBack);
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
      }
    }
    yield return (object) new WaitForSeconds(4f);
    CameraManager.instance.ShakeCameraForDuration(2f, 5f, 0.5f);
    BiomeConstants.Instance.ImpactFrameForDuration();
    revealMysticShop.LoopedSound = AudioManager.Instance.CreateLoop("event:/door/eye_beam_door_open", true);
    AudioManager.Instance.PlayOneShot("event:/boss/frog/transition_zoom_back");
    BiomeConstants.Instance.ChromaticAbberationTween(1f, BiomeConstants.Instance.ChromaticAberrationDefaultValue, 0.75f);
    AudioManager.Instance.SetMusicPsychedelic(1f);
    MMVibrate.StopRumble();
    revealMysticShop.mysticSeller.gameObject.SetActive(true);
    revealMysticShop.ritualFX.SetActive(true);
    revealMysticShop.mysticSeller.GetComponent<MeshRenderer>().materials[0].color = Color.black;
    revealMysticShop.mysticSeller.GetComponent<MeshRenderer>().materials[0].DOColor(Color.white, 2f).SetDelay<TweenerCore<Color, Color, ColorOptions>>(1f);
    revealMysticShop.mysticSeller.transform.DOMove(revealMysticShop.mysticSellerStartingPos, 2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    revealMysticShop.ObjectsToMoveZ.Clear();
    foreach (GameObject gameObject in revealMysticShop.ObjectsToMove)
    {
      if (gameObject.activeSelf)
      {
        revealMysticShop.ObjectsToMoveZ.Add(gameObject.transform.position.z);
        gameObject.transform.DOMoveZ((float) UnityEngine.Random.Range(-5, -10), 2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutCirc);
        gameObject.transform.DOShakeRotation(6f, 20f).SetEase<Tweener>(Ease.OutBack);
      }
    }
    GameManager.GetInstance().OnConversationNext(revealMysticShop.cameraPos, 15f);
    yield return (object) new WaitForSeconds(4f);
    GameManager.GetInstance().OnConversationNext(revealMysticShop.cameraPos, 12f);
    revealMysticShop.canvas.gameObject.SetActive(true);
    revealMysticShop.image.color = new Color(1f, 1f, 1f, 0.0f);
    DOTweenModuleUI.DOColor(revealMysticShop.image, StaticColors.RedColor, 2f);
    AudioManager.Instance.PlayOneShot("event:/boss/frog/transition_intro_zoom", revealMysticShop.transform.gameObject);
    yield return (object) new WaitForSeconds(1.5f);
    revealMysticShop.rockParticles1.Play();
    revealMysticShop.rockParticles2.Play();
    yield return (object) new WaitForSeconds(0.5f);
    foreach (GameObject gameObject in revealMysticShop.ObjectsToDisable)
      gameObject.SetActive(false);
    revealMysticShop.canvas.gameObject.SetActive(false);
    revealMysticShop.rift.SetActive(true);
    BiomeConstants.Instance.ChromaticAbberationTween(1f, 0.75f, BiomeConstants.Instance.ChromaticAberrationDefaultValue);
    AudioManager.Instance.StopLoop(revealMysticShop.LoopedSound);
    revealMysticShop.ritualFX.SetActive(false);
    GameManager.GetInstance().OnConversationNext(revealMysticShop.cameraPos, 15f);
    int index = 0;
    int num = 0;
    foreach (GameObject gameObject in revealMysticShop.ObjectsToMove)
    {
      if (gameObject.activeSelf)
      {
        float duration = UnityEngine.Random.Range(0.5f, 1f);
        gameObject.transform.DOMoveZ(revealMysticShop.ObjectsToMoveZ[index], duration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBounce);
        if (num == 3)
        {
          AudioManager.Instance.PlayOneShotDelayed("event:/material/stone_break", duration - duration / 5f, gameObject.transform);
          num = 0;
        }
        ++num;
        ++index;
      }
    }
    AudioManager.Instance.PlayOneShot("event:/boss/frog/transition_zoom_back", revealMysticShop.transform.gameObject);
    yield return (object) new WaitForSeconds(2f);
    revealMysticShop.introConversation.Play();
    yield return (object) new WaitForEndOfFrame();
    while (MMConversation.CURRENT_CONVERSATION != null)
      yield return (object) null;
    revealMysticShop.introConversation.enabled = false;
    yield return (object) new WaitForEndOfFrame();
    GameManager.GetInstance().OnConversationNext(revealMysticShop.revealDoorsCameraPos, 20f);
    yield return (object) new WaitForSeconds(2f);
    DataManager.Instance.UnlockedDungeonDoor.Clear();
    DataManager.Instance.UnlockedDungeonDoor.Add(FollowerLocation.Dungeon1_1);
    Interaction_BaseDungeonDoor component1 = revealMysticShop.doors[0].GetComponent<Interaction_BaseDungeonDoor>();
    component1.doorInnerBlack.DOColor(Color.white, 0.1f);
    component1.doorLightSource.SetActive(true);
    SpriteRenderer[] componentsInChildren = component1.gameObject.GetComponentsInChildren<SpriteRenderer>();
    Material newFillMat = new Material(componentsInChildren[0].material);
    newFillMat.SetFloat(Reveal_MysticShop.FillAlpha, 1f);
    foreach (Renderer renderer in componentsInChildren)
      renderer.material = newFillMat;
    AudioManager.Instance.PlayOneShot("event:/Stings/generic_positive", revealMysticShop.transform.position);
    yield return (object) new WaitForSeconds(0.33f);
    revealMysticShop.doorParticles1.Play();
    revealMysticShop.doors[0].gameObject.SetActive(false);
    BiomeConstants.Instance.EmitSmokeInteractionVFX(revealMysticShop.newDoors[0].transform.position - Vector3.forward, Vector3.one * 5f);
    revealMysticShop.newDoors[0].gameObject.SetActive(true);
    MMVibrate.Haptic(MMVibrate.HapticTypes.LightImpact);
    CameraManager.shakeCamera(0.1f);
    AudioManager.Instance.PlayOneShot("event:/door/goop_door_unlock", revealMysticShop.transform.position);
    AudioManager.Instance.PlayOneShot("event:/door/door_done", revealMysticShop.newDoors[0].transform.position);
    yield return (object) new WaitForSeconds(0.5f);
    Interaction_BaseDungeonDoor component2 = revealMysticShop.doors[1].GetComponent<Interaction_BaseDungeonDoor>();
    component2.doorInnerBlack.DOColor(Color.white, 0.1f);
    component2.doorLightSource.SetActive(true);
    foreach (Renderer componentsInChild in component2.gameObject.GetComponentsInChildren<SpriteRenderer>())
      componentsInChild.material = newFillMat;
    AudioManager.Instance.PlayOneShot("event:/Stings/generic_positive", revealMysticShop.transform.position);
    yield return (object) new WaitForSeconds(0.33f);
    revealMysticShop.doorParticles2.Play();
    revealMysticShop.doors[1].gameObject.SetActive(false);
    BiomeConstants.Instance.EmitSmokeInteractionVFX(revealMysticShop.newDoors[1].transform.position - Vector3.forward, Vector3.one * 5f);
    revealMysticShop.newDoors[1].gameObject.SetActive(true);
    MMVibrate.Haptic(MMVibrate.HapticTypes.LightImpact);
    CameraManager.shakeCamera(0.2f);
    AudioManager.Instance.PlayOneShot("event:/door/goop_door_unlock", revealMysticShop.transform.position);
    AudioManager.Instance.PlayOneShot("event:/door/door_done", revealMysticShop.newDoors[1].transform.position);
    yield return (object) new WaitForSeconds(0.5f);
    Interaction_BaseDungeonDoor component3 = revealMysticShop.doors[2].GetComponent<Interaction_BaseDungeonDoor>();
    component3.doorInnerBlack.DOColor(Color.white, 0.1f);
    component3.doorLightSource.SetActive(true);
    foreach (Renderer componentsInChild in component3.gameObject.GetComponentsInChildren<SpriteRenderer>())
      componentsInChild.material = newFillMat;
    AudioManager.Instance.PlayOneShot("event:/Stings/generic_positive", revealMysticShop.transform.position);
    yield return (object) new WaitForSeconds(0.33f);
    revealMysticShop.doorParticles3.Play();
    revealMysticShop.doors[2].gameObject.SetActive(false);
    BiomeConstants.Instance.EmitSmokeInteractionVFX(revealMysticShop.newDoors[2].transform.position - Vector3.forward, Vector3.one * 5f);
    revealMysticShop.newDoors[2].gameObject.SetActive(true);
    MMVibrate.Haptic(MMVibrate.HapticTypes.LightImpact);
    CameraManager.shakeCamera(0.3f);
    AudioManager.Instance.PlayOneShot("event:/door/goop_door_unlock", revealMysticShop.transform.position);
    AudioManager.Instance.PlayOneShot("event:/door/door_done", revealMysticShop.newDoors[2].transform.position);
    yield return (object) new WaitForSeconds(0.5f);
    Interaction_BaseDungeonDoor component4 = revealMysticShop.doors[3].GetComponent<Interaction_BaseDungeonDoor>();
    component4.doorInnerBlack.DOColor(Color.white, 0.1f);
    component4.doorLightSource.SetActive(true);
    foreach (Renderer componentsInChild in component4.gameObject.GetComponentsInChildren<SpriteRenderer>())
      componentsInChild.material = newFillMat;
    AudioManager.Instance.PlayOneShot("event:/Stings/generic_positive", revealMysticShop.transform.position);
    yield return (object) new WaitForSeconds(0.33f);
    revealMysticShop.doorParticles4.Play();
    revealMysticShop.doors[3].gameObject.SetActive(false);
    BiomeConstants.Instance.EmitSmokeInteractionVFX(revealMysticShop.newDoors[3].transform.position - Vector3.forward, Vector3.one * 5f);
    revealMysticShop.newDoors[3].gameObject.SetActive(true);
    MMVibrate.Haptic(MMVibrate.HapticTypes.LightImpact);
    CameraManager.shakeCamera(0.4f);
    AudioManager.Instance.PlayOneShot("event:/door/goop_door_unlock", revealMysticShop.transform.position);
    AudioManager.Instance.PlayOneShot("event:/door/door_done", revealMysticShop.newDoors[3].transform.position);
    yield return (object) new WaitForSeconds(2f);
    MMVibrate.Haptic(MMVibrate.HapticTypes.Success);
    AudioManager.Instance.PlayOneShot("event:/Stings/boss_door_complete", revealMysticShop.transform.gameObject);
    AudioManager.Instance.SetMusicPsychedelic(0.0f);
    GameManager.GetInstance().OnConversationEnd();
    ObjectiveManager.Add((ObjectivesData) new Objectives_CollectItem("Objectives/GroupTitles/MysticShop", InventoryItem.ITEM_TYPE.GOD_TEAR, 1)
    {
      CustomTerm = "Objectives/CollectItem/DivineCrystals"
    }, true);
    Interaction_MysticShop component5 = revealMysticShop.GetComponent<Interaction_MysticShop>();
    component5.StopMusic();
    component5.Interactable = true;
  }

  public void DLCReveal() => this.StartCoroutine(this.DLCRevealRoutine());

  public IEnumerator DLCRevealRoutine()
  {
    Reveal_MysticShop revealMysticShop = this;
    bool Waiting = true;
    PlayerFarming.Instance.GoToAndStop(revealMysticShop.playerPosition.position + new Vector3(0.0f, -2f), revealMysticShop.playerPosition.gameObject, GoToCallback: (System.Action) (() =>
    {
      PlayerFarming.Instance.transform.position = this.playerPosition.transform.position + new Vector3(0.0f, -2f);
      Waiting = false;
    }));
    while (Waiting)
      yield return (object) null;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(revealMysticShop.revealDoorsCameraPos, 20f);
    yield return (object) new WaitForSeconds(2f);
    Interaction_BaseDungeonDoor component1 = revealMysticShop.newDoors[0].GetComponent<Interaction_BaseDungeonDoor>();
    component1.Crown.gameObject.SetActive(true);
    DOTween.Sequence().Append((Tween) component1.Crown.transform.DOMove(component1.Crown.transform.position + Vector3.down * 4f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack));
    yield return (object) new WaitForSeconds(0.5f);
    Interaction_BaseDungeonDoor component2 = revealMysticShop.newDoors[1].GetComponent<Interaction_BaseDungeonDoor>();
    component2.Crown.gameObject.SetActive(true);
    DOTween.Sequence().Append((Tween) component2.Crown.transform.DOMove(component2.Crown.transform.position + Vector3.down * 4f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack));
    yield return (object) new WaitForSeconds(0.5f);
    Interaction_BaseDungeonDoor component3 = revealMysticShop.newDoors[2].GetComponent<Interaction_BaseDungeonDoor>();
    component3.Crown.gameObject.SetActive(true);
    DOTween.Sequence().Append((Tween) component3.Crown.transform.DOMove(component3.Crown.transform.position + Vector3.down * 4f + Vector3.back * 2f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack));
    yield return (object) new WaitForSeconds(0.5f);
    Interaction_BaseDungeonDoor component4 = revealMysticShop.newDoors[3].GetComponent<Interaction_BaseDungeonDoor>();
    component4.Crown.gameObject.SetActive(true);
    DOTween.Sequence().Append((Tween) component4.Crown.transform.DOMove(component4.Crown.transform.position + Vector3.down * 4f + Vector3.back * 2f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack));
    yield return (object) new WaitForSeconds(1.33f);
    DG.Tweening.Sequence s1 = DOTween.Sequence();
    Vector3 endValue1 = revealMysticShop.crownPosition.transform.position + Vector3.left * 3f + Vector3.forward * 0.5f;
    s1.Append((Tween) revealMysticShop.newDoors[0].Crown.transform.DOMove(endValue1, 2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack));
    s1.AppendInterval(1f);
    s1.Append((Tween) revealMysticShop.newDoors[0].Crown.transform.DOMove(endValue1 + new Vector3(-1f, 0.0f, -1f) * 1.5f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack));
    s1.Append((Tween) revealMysticShop.newDoors[0].Crown.transform.DOMove(revealMysticShop.mysticSeller.transform.position + Vector3.back * 3f + Vector3.up * 3f + Vector3.left * 2f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack));
    DG.Tweening.Sequence s2 = DOTween.Sequence();
    Vector3 endValue2 = revealMysticShop.crownPosition.transform.position + Vector3.right * 3f + Vector3.forward * 0.5f;
    s2.Append((Tween) revealMysticShop.newDoors[1].Crown.transform.DOMove(endValue2, 2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack));
    s2.AppendInterval(1f);
    s2.Append((Tween) revealMysticShop.newDoors[1].Crown.transform.DOMove(endValue2 + new Vector3(1f, 0.0f, -1f) * 1.5f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack));
    s2.Append((Tween) revealMysticShop.newDoors[1].Crown.transform.DOMove(revealMysticShop.mysticSeller.transform.position + Vector3.back * 3f + Vector3.up * 3f + Vector3.right * 2f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack));
    DG.Tweening.Sequence s3 = DOTween.Sequence();
    Vector3 endValue3 = revealMysticShop.crownPosition.transform.position + Vector3.left * 1f - Vector3.forward * 0.5f;
    s3.Append((Tween) revealMysticShop.newDoors[2].Crown.transform.DOMove(endValue3, 2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack));
    s3.AppendInterval(1f);
    s3.Append((Tween) revealMysticShop.newDoors[2].Crown.transform.DOMove(endValue3 + new Vector3(-1f, 0.0f, 1f) * 1.5f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack));
    s3.Append((Tween) revealMysticShop.newDoors[2].Crown.transform.DOMove(revealMysticShop.mysticSeller.transform.position + Vector3.back * 3f + Vector3.up * 3f + Vector3.left * 2f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack));
    DG.Tweening.Sequence s4 = DOTween.Sequence();
    Vector3 endValue4 = revealMysticShop.crownPosition.transform.position + Vector3.right * 1f - Vector3.forward * 0.5f;
    s4.Append((Tween) revealMysticShop.newDoors[3].Crown.transform.DOMove(endValue4, 2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack));
    s4.AppendInterval(1f);
    s4.Append((Tween) revealMysticShop.newDoors[3].Crown.transform.DOMove(endValue4 + new Vector3(1f, 0.0f, 1f) * 1.5f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack));
    s4.Append((Tween) revealMysticShop.newDoors[3].Crown.transform.DOMove(revealMysticShop.mysticSeller.transform.position + Vector3.back * 3f + Vector3.up * 3f + Vector3.right * 2f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack));
    GameManager.GetInstance().CamFollowTarget.MoveSpeed /= 5f;
    GameManager.GetInstance().OnConversationNext(revealMysticShop.cameraPos, 14f);
    yield return (object) new WaitForSeconds(4.5f);
    revealMysticShop.mysticShop.LightingOverrideAngry.gameObject.SetActive(true);
    CameraManager.instance.ShakeCameraForDuration(0.5f, 0.75f, 2f);
    GameManager.GetInstance().CamFollowTarget.MoveSpeed *= 5f;
    GameManager.GetInstance().OnConversationNext(revealMysticShop.cameraPos, 10f);
    yield return (object) new WaitForSeconds(4f);
    revealMysticShop.mysticShop.LightingOverrideAngry.gameObject.SetActive(false);
    MMVibrate.Haptic(MMVibrate.HapticTypes.Success);
    AudioManager.Instance.PlayOneShot("event:/Stings/boss_door_complete", revealMysticShop.transform.gameObject);
    AudioManager.Instance.SetMusicPsychedelic(0.0f);
    GameManager.GetInstance().OnConversationEnd();
    revealMysticShop.GetComponent<Interaction_MysticShop>().StopMusic();
  }

  public IEnumerator ShakeCameraWithRampUp()
  {
    float t = 0.0f;
    while ((double) (t += Time.deltaTime) < 3.9000000953674316)
    {
      float t1 = t / 3.9f;
      CameraManager.instance.ShakeCameraForDuration(Mathf.Lerp(0.0f, 0.5f, t1), Mathf.Lerp(0.0f, 1.5f, t1), 3.9f, false);
      yield return (object) null;
    }
    CameraManager.instance.Stopshake();
  }
}
