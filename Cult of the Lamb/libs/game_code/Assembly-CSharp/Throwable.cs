// Decompiled with JetBrains decompiler
// Type: Throwable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using Spine.Unity;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class Throwable : BaseMonoBehaviour
{
  [Header("Trajectory Settings")]
  [SerializeField]
  [Tooltip("Duration of the launch in seconds.")]
  public float moveDuration = 1f;
  [SerializeField]
  [Tooltip("Maximum height of the arc (depth offset).")]
  public float arcHeight = 2f;
  [SerializeField]
  [Tooltip("Curve that controls the arc offset over time (evaluated from 0 to 1).")]
  public AnimationCurve arcCurve = AnimationCurve.EaseInOut(0.0f, 0.0f, 1f, 1f);
  [Header("Target Warning Prefabs")]
  [SerializeField]
  [Tooltip("Prefab for the target warning visual (e.g. a ground circle).")]
  public GameObject targetVisualPrefab;
  [SerializeField]
  [Tooltip("Prefab for an additional target warning visual (e.g. flashing indicator).")]
  public GameObject targetWarningPrefab;
  [SerializeField]
  [Tooltip("A CircleCollider2D used to determine the final scale for the target warning visuals.")]
  public CircleCollider2D targetCollider;
  [SerializeField]
  [Tooltip("If true, the target warning will flash between two colors.")]
  public bool enableTargetFlashing;
  [SerializeField]
  [Tooltip("Time (in seconds) between flash color toggles.")]
  public float flashTickDuration = 0.15f;
  [SerializeField]
  [Tooltip("Only set this if you want the trajectory of the throwable to be affected by time freezes")]
  public SkeletonAnimation spine;
  [Header("Launch Events")]
  [SerializeField]
  public UnityEvent OnLaunchComplete;
  [SerializeField]
  [Tooltip("If true, the thrown object will be destroyed after launch completes.")]
  public bool destroyOnComplete;
  [SerializeField]
  [Tooltip("If true, the thrown object will disable its collider mid-flight.")]
  public bool disableColliderWhileLaunching = true;
  public GameObject targetVisualInstance;
  public GameObject targetWarningInstance;
  public Collider2D selfCollider;
  public Coroutine flashingRoutine;
  public Vector3 targetWorldPosition;
  [Header("SFX")]
  public string LaunchSFX = string.Empty;
  public EventInstance launchInstanceSFX;

  public void Awake() => this.selfCollider = this.GetComponent<Collider2D>();

  public void Launch(Vector3 targetPosition, float duration = -1f)
  {
    this.targetWorldPosition = targetPosition;
    if ((Object) this.targetVisualPrefab != (Object) null)
      this.targetVisualInstance = Object.Instantiate<GameObject>(this.targetVisualPrefab, targetPosition, Quaternion.identity);
    if ((Object) this.targetWarningPrefab != (Object) null)
      this.targetWarningInstance = Object.Instantiate<GameObject>(this.targetWarningPrefab, targetPosition, Quaternion.identity);
    if ((Object) this.targetVisualInstance != (Object) null && (Object) this.targetWarningInstance != (Object) null && (Object) this.targetCollider != (Object) null)
    {
      this.StartCoroutine((IEnumerator) this.ScaleTargetWarning());
      if (this.enableTargetFlashing)
        this.flashingRoutine = this.StartCoroutine((IEnumerator) this.FlashTargetWarning());
    }
    float duration1 = (double) duration > 0.0 ? duration : this.moveDuration;
    this.StartCoroutine((IEnumerator) this.LaunchRoutine(targetPosition, duration1));
    if (string.IsNullOrEmpty(this.LaunchSFX))
      return;
    this.launchInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(this.LaunchSFX, this.transform);
  }

  public IEnumerator LaunchRoutine(Vector3 targetPosition, float duration)
  {
    Throwable throwable = this;
    if (throwable.disableColliderWhileLaunching)
      throwable.selfCollider.enabled = false;
    Vector3 startPosition = throwable.transform.position;
    float elapsed = 0.0f;
    while ((double) elapsed < (double) duration)
    {
      double num1 = (double) elapsed;
      double deltaTime = (double) Time.deltaTime;
      SkeletonAnimation spine = throwable.spine;
      double num2 = spine != null ? (double) spine.timeScale : 1.0;
      double num3 = deltaTime * num2;
      elapsed = (float) (num1 + num3);
      float num4 = Mathf.Clamp01(elapsed / duration);
      Vector3 vector3 = Vector3.Lerp(startPosition, targetPosition, num4);
      float num5 = throwable.arcCurve.Evaluate(num4) * throwable.arcHeight;
      throwable.transform.position = new Vector3(vector3.x, vector3.y, vector3.z - num5);
      yield return (object) null;
    }
    throwable.transform.position = targetPosition;
    if ((Object) throwable.targetVisualInstance != (Object) null)
      Object.Destroy((Object) throwable.targetVisualInstance);
    if ((Object) throwable.targetWarningInstance != (Object) null)
      Object.Destroy((Object) throwable.targetWarningInstance);
    if (throwable.flashingRoutine != null)
      throwable.StopCoroutine(throwable.flashingRoutine);
    if (throwable.destroyOnComplete)
      Object.Destroy((Object) throwable.gameObject);
    if (throwable.disableColliderWhileLaunching)
      throwable.selfCollider.enabled = true;
    throwable.OnLaunchComplete?.Invoke();
  }

  public IEnumerator ScaleTargetWarning()
  {
    float scaleProgress = 0.0f;
    while ((double) scaleProgress < 1.0)
    {
      scaleProgress += Time.deltaTime * 8f;
      scaleProgress = Mathf.Min(scaleProgress, 1f);
      Vector3 vector3 = Vector3.one * this.targetCollider.radius * scaleProgress;
      if ((Object) this.targetVisualInstance != (Object) null)
        this.targetVisualInstance.transform.localScale = vector3;
      if ((Object) this.targetWarningInstance != (Object) null)
        this.targetWarningInstance.transform.localScale = vector3;
      yield return (object) null;
    }
  }

  public IEnumerator FlashTargetWarning()
  {
    Color white = Color.white;
    Color currentColor = white;
    float timer = 0.0f;
    while (true)
    {
      timer += Time.deltaTime;
      if ((double) timer >= (double) this.flashTickDuration)
      {
        currentColor = currentColor == white ? Color.red : white;
        if ((Object) this.targetVisualInstance != (Object) null)
        {
          SpriteRenderer component = this.targetVisualInstance.GetComponent<SpriteRenderer>();
          if ((Object) component != (Object) null && (Object) component.material != (Object) null)
            component.material.SetColor("_Color", currentColor);
        }
        if ((Object) this.targetWarningInstance != (Object) null)
        {
          SpriteRenderer component = this.targetWarningInstance.GetComponent<SpriteRenderer>();
          if ((Object) component != (Object) null && (Object) component.material != (Object) null)
            component.material.SetColor("_Color", currentColor);
        }
        timer = 0.0f;
      }
      yield return (object) null;
    }
  }

  public void OnDisable()
  {
    if (string.IsNullOrEmpty(this.LaunchSFX))
      return;
    AudioManager.Instance.StopOneShotInstanceEarly(this.launchInstanceSFX, STOP_MODE.IMMEDIATE);
  }
}
