// Decompiled with JetBrains decompiler
// Type: SwayInWind
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SwayInWind : MonoBehaviour
{
  public static float GlobalWindSpeed = 1f;
  [Header("Base Sway")]
  [SerializeField]
  public Transform swayContainer;
  [SerializeField]
  public bool swayOnly;
  [SerializeField]
  public float baseAmplitude = 10f;
  [SerializeField]
  public float baseSpeed = 1f;
  [SerializeField]
  public float triggerImpact = 40f;
  [SerializeField]
  public float impactFalloffTime = 1.2f;
  [SerializeField]
  public float maxRotation = 45f;
  [SerializeField]
  public Vector2 startOffsetRange = new Vector2(0.0f, 6.28318548f);
  [SerializeField]
  public Vector3 swayAxis = new Vector3(0.0f, 0.0f, 1f);
  [SerializeField]
  public bool offsetOriginalRotation;
  [SerializeField]
  public bool playImpactSFX;
  [SerializeField]
  public string impactSFX;
  [SerializeField]
  public Transform rattle;
  [Header("Collision cadence (LongGrass-style)")]
  public int UpdateInterval = 2;
  [HideInInspector]
  public int FrameIntervalOffset;
  public Vector3 Position;
  public List<GameObject> CurrentCollisions = new List<GameObject>();
  public List<GameObject> PreviousCollisions = new List<GameObject>();
  [Range(0.1f, 2f)]
  [SerializeField]
  public float fullImpactRadius = 1f;
  [Tooltip("Optional: filter by tag (leave empty for all).")]
  [SerializeField]
  public string collideWithTag = "";
  [SerializeField]
  public Collider2D c;
  public float impactPower;
  public Coroutine decayRoutine;
  public float baseOffset;
  public Vector3 originalRotation;
  public float progress;
  public float currentSwayValue;
  [Header("Optimisation")]
  public bool TURN_OFF_ON_LOW_QUALITY;

  public void OnEnable()
  {
    if ((Object) this.swayContainer == (Object) null)
      this.swayContainer = this.transform;
    if (this.TURN_OFF_ON_LOW_QUALITY && SettingsManager.Settings != null && SettingsManager.Settings.Graphics.EnvironmentDetail == 0)
      this.TurnOffEverything();
    this.StartCoroutine(this.OnEnableIE());
  }

  public void TurnOffEverything()
  {
    if ((Object) this.c != (Object) null)
      this.c.enabled = false;
    this.enabled = false;
  }

  public IEnumerator OnEnableIE()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    SwayInWind w = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      w.Position = w.transform.position;
      TimeManager.AddToRegion(w);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    w.CurrentCollisions.Clear();
    w.PreviousCollisions.Clear();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) null;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void OnDisable() => TimeManager.RemoveWheat(this);

  public void Start()
  {
    this.baseOffset = Random.Range(this.startOffsetRange.x, this.startOffsetRange.y);
    if (this.offsetOriginalRotation)
      this.originalRotation = this.swayContainer.rotation.eulerAngles;
    this.FrameIntervalOffset = Random.Range(0, Mathf.Max(1, this.UpdateInterval));
  }

  public void Update()
  {
    float deltaTime = Time.deltaTime;
    this.progress += deltaTime * this.baseSpeed * SwayInWind.GlobalWindSpeed;
    this.currentSwayValue = Mathf.Lerp(this.currentSwayValue, Mathf.Sin(this.progress + this.baseOffset) * this.baseAmplitude + this.impactPower, deltaTime * 10f);
    this.swayContainer.localRotation = Quaternion.Euler((this.offsetOriginalRotation ? this.originalRotation : Vector3.zero) + this.swayAxis * Mathf.Clamp(this.currentSwayValue, -this.maxRotation, this.maxRotation));
  }

  public void Colliding(GameObject g)
  {
    if (this.swayOnly || SettingsManager.Settings.Game.PerformanceMode || (Object) g == (Object) null || !string.IsNullOrEmpty(this.collideWithTag) && !g.CompareTag(this.collideWithTag) || this.CurrentCollisions.Contains(g))
      return;
    this.CurrentCollisions.Add(g);
  }

  public void OnTriggerStay2D(Collider2D c)
  {
    if (this.swayOnly)
      return;
    this.Colliding(c.gameObject);
  }

  public void LateUpdate()
  {
    if (this.swayOnly || (Time.frameCount + this.FrameIntervalOffset) % Mathf.Max(1, this.UpdateInterval) != 0)
      return;
    for (int index = 0; index < this.CurrentCollisions.Count; ++index)
    {
      GameObject currentCollision = this.CurrentCollisions[index];
      if ((Object) currentCollision != (Object) null && !this.PreviousCollisions.Contains(currentCollision))
        this.CollisionEnter(currentCollision);
    }
    for (int index = 0; index < this.PreviousCollisions.Count; ++index)
    {
      GameObject previousCollision = this.PreviousCollisions[index];
      if ((Object) previousCollision != (Object) null && !this.CurrentCollisions.Contains(previousCollision))
        this.CollisionExit(previousCollision);
    }
    this.PreviousCollisions.Clear();
    this.PreviousCollisions.AddRange((IEnumerable<GameObject>) this.CurrentCollisions);
    this.CurrentCollisions.Clear();
  }

  public void CollisionEnter(GameObject other)
  {
    if ((Object) other == (Object) null)
      return;
    float num1 = Mathf.Sign(this.transform.position.x - other.transform.position.x);
    float num2 = Mathf.Clamp01((float) (1.0 - (double) Vector3.Distance(this.transform.position, other.transform.position) / (double) Mathf.Max(0.0001f, this.fullImpactRadius)));
    this.AddImpact((this.triggerImpact + Random.Range(-2f, 2f)) * num1 * num2);
  }

  public void CollisionExit(GameObject other)
  {
    if ((Object) other == (Object) null || (Object) this.rattle != (Object) null)
      return;
    this.AddImpact(this.triggerImpact * 0.35f * Mathf.Sign(other.transform.position.x - this.transform.position.x));
  }

  public void AddImpact(float amount)
  {
    this.impactPower += amount;
    if (this.decayRoutine != null)
      this.StopCoroutine(this.decayRoutine);
    this.decayRoutine = this.StartCoroutine(this.DecayImpactRoutine());
    if (string.IsNullOrEmpty(this.impactSFX))
      return;
    AudioManager.Instance.PlayOneShot(this.impactSFX, this.transform.position);
    if (!((Object) this.rattle != (Object) null))
      return;
    this.rattle.DOPunchRotation(new Vector3(0.0f, 0.0f, 20f), 0.5f, 8, 0.5f);
  }

  public IEnumerator DecayImpactRoutine()
  {
    float elapsed = 0.0f;
    float start = this.impactPower;
    while ((double) elapsed < (double) this.impactFalloffTime)
    {
      elapsed += Time.deltaTime;
      float num = Mathf.Clamp01(elapsed / this.impactFalloffTime);
      this.impactPower = Mathf.Lerp(start, 0.0f, 1f - Mathf.Pow(1f - num, 2f));
      yield return (object) null;
    }
    this.impactPower = 0.0f;
  }
}
