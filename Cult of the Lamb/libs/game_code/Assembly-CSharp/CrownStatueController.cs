// Decompiled with JetBrains decompiler
// Type: CrownStatueController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using FMOD.Studio;
using MMTools;
using System.Collections;
using UnityEngine;

#nullable disable
public class CrownStatueController : MonoBehaviour
{
  public static CrownStatueController Instance;
  public Interaction EnterEndlessMode;
  public Interaction CrownStatue;
  public GameObject Container;
  public GameObject CameraPosition;
  public GameObject Portal;
  public static int FillAlpha = Shader.PropertyToID("_FillAlpha");
  [SerializeField]
  public GameObject LightingObject;
  [SerializeField]
  public Shader UberShaderRef;
  public EventInstance LoopedSound;

  public void OnEnable()
  {
    CrownStatueController.Instance = this;
    this.Container.SetActive(true);
    this.EnterEndlessMode.enabled = false;
    int num = 0;
    foreach (string killedBoss in DataManager.Instance.KilledBosses)
    {
      if (killedBoss.Contains("_P2"))
        ++num;
    }
    if (DataManager.Instance.DeathCatBeaten)
    {
      if (!DataManager.Instance.OnboardedEndlessMode)
      {
        this.Container.SetActive(true);
        this.EnterEndlessMode.enabled = false;
      }
      else
      {
        this.Container.SetActive(false);
        this.EnterEndlessMode.enabled = true;
        this.CrownStatue.enabled = false;
      }
    }
    else
    {
      this.Container.SetActive(true);
      this.EnterEndlessMode.enabled = false;
    }
  }

  public void OnDisable() => CrownStatueController.Instance = (CrownStatueController) null;

  public void EndlessModeOnboarded(System.Action callback)
  {
    this.StartCoroutine((IEnumerator) this.EndlessModeOnboardRoutine(callback));
  }

  public IEnumerator EndlessModeOnboardRoutine(System.Action callback)
  {
    CrownStatueController statueController = this;
    statueController.CrownStatue.enabled = false;
    DataManager.Instance.OnboardedEndlessMode = true;
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    while (MMConversation.CURRENT_CONVERSATION != null)
      yield return (object) null;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(statueController.CameraPosition);
    yield return (object) new WaitForSeconds(1.5f);
    AudioManager.Instance.PlayOneShot("event:/boss/frog/transition_intro_zoom");
    statueController.LoopedSound = AudioManager.Instance.CreateLoop("event:/material/earthquake", statueController.gameObject);
    MMVibrate.RumbleContinuous(1f, 5f);
    statueController.StartCoroutine((IEnumerator) statueController.ShakeCameraWithRampUp());
    GameManager.GetInstance().OnConversationNext(statueController.CameraPosition, 5f);
    yield return (object) new WaitForSeconds(1f);
    AudioManager.Instance.StopLoop(statueController.LoopedSound);
    BiomeConstants.Instance.ImpactFrameForDuration();
    MMVibrate.StopRumble();
    SpriteRenderer[] spriteRenderers = statueController.gameObject.GetComponentsInChildren<SpriteRenderer>();
    Material target = new Material(spriteRenderers[0].material);
    Material oldMat = new Material(spriteRenderers[0].material);
    foreach (SpriteRenderer spriteRenderer in spriteRenderers)
    {
      if ((UnityEngine.Object) spriteRenderer.material.shader == (UnityEngine.Object) statueController.UberShaderRef)
        spriteRenderer.material = target;
    }
    target.DOColor(StaticColors.OffWhiteColor, 2f);
    AudioManager.Instance.PlayOneShot("event:/Stings/generic_positive", statueController.transform.position);
    statueController.LightingObject.SetActive(true);
    yield return (object) new WaitForSeconds(1f);
    foreach (SpriteRenderer spriteRenderer in spriteRenderers)
    {
      if ((UnityEngine.Object) spriteRenderer.material.shader == (UnityEngine.Object) statueController.UberShaderRef)
        spriteRenderer.material = oldMat;
    }
    statueController.LightingObject.SetActive(false);
    AudioManager.Instance.PlayOneShot("event:/door/goop_door_unlock", statueController.transform.position);
    AudioManager.Instance.PlayOneShot("event:/door/door_done", statueController.transform.position);
    BiomeConstants.Instance.EmitSmokeInteractionVFX(statueController.transform.position - Vector3.forward, Vector3.one * 5f);
    MMVibrate.Haptic(MMVibrate.HapticTypes.LightImpact);
    statueController.Container.SetActive(false);
    statueController.Portal.gameObject.SetActive(true);
    BiomeConstants.Instance.EmitSmokeInteractionVFX(statueController.transform.position, new Vector3(5f, 5f, 2f));
    CameraManager.instance.ShakeCameraForDuration(0.8f, 1f, 0.3f);
    yield return (object) new WaitForSeconds(1f);
    GameManager.GetInstance().OnConversationEnd();
    statueController.EnterEndlessMode.enabled = true;
    System.Action action = callback;
    if (action != null)
      action();
  }

  public IEnumerator ShakeCameraWithRampUp()
  {
    float t = 0.0f;
    while ((double) (t += Time.deltaTime) < 1.0)
    {
      float t1 = t / 1f;
      CameraManager.instance.ShakeCameraForDuration(Mathf.Lerp(0.0f, 0.5f, t1), Mathf.Lerp(0.0f, 1.5f, t1), 3.9f, false);
      yield return (object) null;
    }
    CameraManager.instance.Stopshake();
  }
}
