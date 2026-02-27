// Decompiled with JetBrains decompiler
// Type: LoreOffering_Interaction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using EasyCurvedLine;
using FMOD.Studio;
using I2.Loc;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

#nullable disable
public class LoreOffering_Interaction : Interaction
{
  [SerializeField]
  public GameObject armStartPos;
  [SerializeField]
  public GameObject armEndPos;
  [SerializeField]
  public GameObject arm;
  [SerializeField]
  public GameObject skinPos;
  [SerializeField]
  public LoreStone loreStone;
  [SerializeField]
  public SimpleSetCamera simpleSetCamera;
  [SerializeField]
  public SimpleSetCamera simpleSetCameraSpooky;
  [SerializeField]
  public SimpleSetCamera holeSetCamera;
  [SerializeField]
  public SpriteRenderer eyes;
  [SerializeField]
  public SpriteRenderer heart;
  [SerializeField]
  public MeshRenderer lighting_0;
  [SerializeField]
  public MeshRenderer lighting_1;
  [SerializeField]
  public SpriteRenderer symbols;
  [SerializeField]
  public CurvedLineRenderer lineRenderer;
  [SerializeField]
  public TextMeshProUGUI text;
  [SerializeField]
  public CanvasGroup canvasGroup;
  [SerializeField]
  public Animator animator;
  public bool Activated;
  public EventInstance loopedSound;

  public void IncreaseCount() => ++DataManager.Instance.LoreStonesRoomUpTo;

  public void Start()
  {
    this.canvasGroup.DOFade(0.0f, 0.1f);
    this.text.gameObject.SetActive(false);
    this.heart.gameObject.SetActive(false);
    this.simpleSetCameraSpooky.gameObject.SetActive(false);
    this.simpleSetCamera.gameObject.SetActive(true);
    this.eyes.gameObject.SetActive(false);
    this.arm.SetActive(false);
    this.arm.transform.position = this.armStartPos.transform.position;
  }

  public override void GetLabel()
  {
    base.GetLabel();
    if (!this.Activated)
    {
      if ((double) this.playerFarming.health.HP >= 3.0 || (double) this.playerFarming.health.HP >= 2.0 && ((double) this.playerFarming.health.BlueHearts > 0.0 || (double) this.playerFarming.health.BlackHearts > 0.0 || (double) this.playerFarming.health.FireHearts > 0.0 || (double) this.playerFarming.health.IceHearts > 0.0))
        this.Label = string.Join(" ", ScriptLocalization.Interactions.MakeOffering, "<sprite name=\"icon_UIHeart\">");
      else
        this.Label = string.Join(" ", ScriptLocalization.Interactions.NotEnough, "<sprite name=\"icon_UIHeart\">");
    }
    else
      this.Label = "";
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (this.Activated)
      return;
    if ((double) this.playerFarming.health.HP >= 3.0)
    {
      this.Activated = true;
      this.StartCoroutine(this.offeringSequence());
    }
    else
      this.playerFarming.indicator.PlayShake();
  }

  public IEnumerator offeringSequence()
  {
    LoreOffering_Interaction offeringInteraction = this;
    GameManager.InMenu = true;
    offeringInteraction.simpleSetCamera.gameObject.SetActive(false);
    GameManager.GetInstance().OnConversationNew(true, true, true, offeringInteraction.playerFarming);
    GameManager.GetInstance().OnConversationNext(offeringInteraction.playerFarming.gameObject);
    offeringInteraction.playerFarming._state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    offeringInteraction.playerFarming.health.OnHitCallback.Invoke();
    MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
    AudioManager.Instance.PlayOneShot("event:/Stings/church_bell", offeringInteraction.playerFarming.gameObject);
    float duration = offeringInteraction.playerFarming.simpleSpineAnimator.Animate("rip-heart", 0, false).Animation.Duration;
    offeringInteraction.playerFarming.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(duration - 1f);
    offeringInteraction.symbols.material.DOFloat(1f, "_Reveal", 3f);
    offeringInteraction.playerFarming.health.HP -= 2f;
    offeringInteraction.heart.gameObject.SetActive(true);
    offeringInteraction.heart.transform.position = offeringInteraction.playerFarming.transform.position - new Vector3(0.0f, 0.0f, 1f);
    GameManager.GetInstance().OnConversationNext(offeringInteraction.playerFarming.gameObject, 10f);
    BiomeConstants.Instance.EmitSmokeExplosionVFX(offeringInteraction.heart.transform.position);
    offeringInteraction.heart.gameObject.transform.DOPunchScale(Vector3.one * 0.1f, 1f);
    offeringInteraction.heart.transform.DOMoveY(offeringInteraction.armStartPos.transform.position.y, 2f).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InQuad);
    offeringInteraction.heart.transform.DOMoveZ(offeringInteraction.heart.transform.position.z - 0.5f, 2f).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InQuad);
    offeringInteraction.heart.transform.DOMoveX(offeringInteraction.armStartPos.transform.position.x, 2f).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBounce).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(new TweenCallback(offeringInteraction.\u003CofferingSequence\u003Eb__22_0));
    AudioManager.Instance.PlayOneShot("event:/ui/map_location_pan", offeringInteraction.heart.gameObject);
    yield return (object) new WaitForSeconds(1.5f);
    offeringInteraction.holeSetCamera.Play();
  }

  public override void OnDisable()
  {
    base.OnDisable();
    AudioManager.Instance.StopLoop(this.loopedSound);
    this.canvasGroup.gameObject.SetActive(false);
  }

  public IEnumerator offeringSequenceSecond()
  {
    LoreOffering_Interaction offeringInteraction = this;
    CameraManager.instance.ShakeCameraForDuration(0.1f, 1f, 2f);
    AudioManager.Instance.PlayOneShot("event:/locations/light_house/fireplace_shake", offeringInteraction.playerFarming.gameObject);
    offeringInteraction.eyes.gameObject.SetActive(true);
    offeringInteraction.eyes.color = new Color(1f, 1f, 1f, 0.0f);
    offeringInteraction.eyes.DOFade(1f, 1f);
    yield return (object) new WaitForSeconds(2f);
    BiomeConstants.Instance.DepthOfFieldTween(2f, 8.7f, 26f, 1f, 200f);
    BiomeConstants.Instance.ChromaticAbberationTween(2f, BiomeConstants.Instance.ChromaticAberrationDefaultValue, 10f);
    offeringInteraction.symbols.material.DOFloat(0.0f, "_Reveal", 3f);
    offeringInteraction.lighting_0.material.DOFade(0.5f, 1f);
    offeringInteraction.lighting_1.material.DOFade(0.5f, 1f);
    offeringInteraction.simpleSetCameraSpooky.gameObject.SetActive(false);
    CameraManager.instance.ShakeCameraForDuration(0.1f, 1f, 2f);
    offeringInteraction.arm.transform.position = offeringInteraction.armStartPos.transform.position;
    offeringInteraction.arm.SetActive(true);
    offeringInteraction.lineRenderer.GetLines();
    offeringInteraction.holeSetCamera.Reset();
    GameManager.GetInstance().OnConversationNext(offeringInteraction.arm, 10f);
    AudioManager.Instance.PlayOneShot("event:/ui/map_location_pan", offeringInteraction.arm.gameObject);
    offeringInteraction.arm.transform.DOMove(offeringInteraction.armEndPos.transform.position, 2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuad);
    yield return (object) new WaitForSeconds(1f);
    BiomeConstants.Instance.DepthOfFieldTween(2f, 8.7f, 26f, 1f, 0.0f);
    BiomeConstants.Instance.ChromaticAbberationTween(2f, 10f, BiomeConstants.Instance.ChromaticAberrationDefaultValue);
    offeringInteraction.canvasGroup.gameObject.SetActive(false);
    GameManager.InMenu = false;
    yield return (object) new WaitForSeconds(2f);
    offeringInteraction.loreStone.forceLore = true;
    int loreID = DataManager.Instance.LoreStonesRoomUpTo;
    offeringInteraction.loreStone.forcedLore = loreID;
    offeringInteraction.loreStone.useLightingVolume = false;
    offeringInteraction.loreStone.OnInteract(offeringInteraction.playerFarming._state);
    ++DataManager.Instance.LoreStonesRoomUpTo;
    offeringInteraction.arm.transform.DOMove(offeringInteraction.armEndPos.transform.position, 1.5f).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(new TweenCallback(offeringInteraction.\u003CofferingSequenceSecond\u003Eb__25_0));
    if (loreID == 0 || DataManager.Instance.LoreStonesRoomUpTo > LoreSystem.LoreTotalLoreRoom)
    {
      LoreOffering_Interaction coroutineSupport = offeringInteraction;
      while (LetterBox.IsPlaying)
        yield return (object) null;
      offeringInteraction.simpleSetCameraSpooky.gameObject.SetActive(true);
      GameManager.GetInstance().OnConversationNew(true, true, true, offeringInteraction.playerFarming);
      offeringInteraction.simpleSetCamera.gameObject.SetActive(false);
      offeringInteraction.simpleSetCameraSpooky.gameObject.SetActive(true);
      yield return (object) new WaitForSeconds(1f);
      offeringInteraction.text.gameObject.SetActive(true);
      DeviceLightingManager.PulseAllLighting(Color.white, Color.red, 0.35f, new KeyCode[0]);
      offeringInteraction.loopedSound = AudioManager.Instance.CreateLoop("event:/atmos/forest/temple_door_hum", offeringInteraction.playerFarming.gameObject, true);
      if (loreID == 0)
        offeringInteraction.text.text = LocalizationManager.GetTranslation("QUOTE/Lore1");
      else
        offeringInteraction.text.text = LocalizationManager.GetTranslation("QUOTE/Lore2");
      offeringInteraction.canvasGroup.gameObject.SetActive(true);
      offeringInteraction.canvasGroup.DOFade(1f, 1f);
      Color TargetColor = offeringInteraction.text.color;
      AudioManager.Instance.PlayOneShot("event:/dialogue/msk/standard_msk");
      ShortcutExtensionsTMPText.DOColor(offeringInteraction.text, Color.black, 2.5f).OnComplete<TweenerCore<Color, Color, ColorOptions>>((TweenCallback) (() =>
      {
        MMVibrate.Rumble(0.025f, 0.05f, 3f, (MonoBehaviour) coroutineSupport, coroutineSupport.playerFarming);
        ShortcutExtensionsTMPText.DOColor(coroutineSupport.text, TargetColor, 2.5f).SetDelay<TweenerCore<Color, Color, ColorOptions>>(2.5f);
      }));
      yield return (object) new WaitForSeconds(6f);
      offeringInteraction.animator.StartPlayback();
      offeringInteraction.canvasGroup.DOFade(0.0f, 1f).OnComplete<TweenerCore<float, float, FloatOptions>>(new TweenCallback(offeringInteraction.\u003CofferingSequenceSecond\u003Eb__25_2));
      if (DataManager.Instance.LoreStonesRoomUpTo > LoreSystem.LoreTotalLoreRoom && !DataManager.Instance.FollowerSkinsUnlocked.Contains("Snake"))
      {
        yield return (object) new WaitForSeconds(1f);
        offeringInteraction.simpleSetCameraSpooky.gameObject.SetActive(true);
        GameManager.GetInstance().OnConversationNew(offeringInteraction.playerFarming);
        yield return (object) new WaitForSeconds(1f);
        FollowerSkinCustomTarget.Create(offeringInteraction.skinPos.transform.position, offeringInteraction.playerFarming.transform.position, (Transform) null, 2.5f, "Snake", (System.Action) null);
        yield return (object) new WaitForSeconds(1.5f);
        offeringInteraction.simpleSetCameraSpooky.gameObject.SetActive(false);
        GameManager.GetInstance().OnConversationNext(offeringInteraction.playerFarming.gameObject);
      }
      else
      {
        offeringInteraction.simpleSetCameraSpooky.gameObject.SetActive(false);
        GameManager.GetInstance().OnConversationEnd();
        GameManager.GetInstance().CameraResetTargetZoom();
      }
    }
    AudioManager.Instance.StopLoop(offeringInteraction.loopedSound);
  }

  [CompilerGenerated]
  public void \u003CofferingSequence\u003Eb__22_0()
  {
    this.StartCoroutine(this.offeringSequenceSecond());
  }

  [CompilerGenerated]
  public void \u003CofferingSequenceSecond\u003Eb__25_0()
  {
    GameManager.GetInstance()._CamFollowTarget.ClearAllTargets();
    this.arm.transform.DOMove(this.armStartPos.transform.position, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InQuad);
    this.simpleSetCamera.gameObject.SetActive(false);
  }

  [CompilerGenerated]
  public void \u003CofferingSequenceSecond\u003Eb__25_2()
  {
    this.canvasGroup.gameObject.SetActive(false);
  }
}
