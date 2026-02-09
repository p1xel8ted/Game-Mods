// Decompiled with JetBrains decompiler
// Type: Com.LuisPedroFonseca.ProCamera2D.ProCamera2DShake
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Com.LuisPedroFonseca.ProCamera2D;

[HelpURL("http://www.procamera2d.com/user-guide/extension-shake/")]
public class ProCamera2DShake : BasePC2D
{
  public static string ExtensionName = "Shake";
  public static ProCamera2DShake _instance;
  public Action OnShakeCompleted;
  public List<ShakePreset> ShakePresets = new List<ShakePreset>();
  public List<ConstantShakePreset> ConstantShakePresets = new List<ConstantShakePreset>();
  public ConstantShakePreset StartConstantShakePreset;
  public ConstantShakePreset CurrentConstantShakePreset;
  public Transform _shakeParent;
  public List<Coroutine> _applyInfluencesCoroutines = new List<Coroutine>();
  public Coroutine _shakeCoroutine;
  public Vector3 _shakeVelocity;
  public List<Vector3> _shakePositions = new List<Vector3>();
  public Quaternion _rotationTarget;
  public Quaternion _originalRotation;
  public float _rotationTime;
  public float _rotationVelocity;
  public List<Vector3> _influences = new List<Vector3>();
  public Vector3 _influencesSum = Vector3.zero;
  public Vector3[] _constantShakePositions;
  public Vector3 _constantShakePosition;
  public bool _isConstantShaking;

  public static ProCamera2DShake Instance
  {
    get
    {
      if (object.Equals((object) ProCamera2DShake._instance, (object) null))
      {
        ProCamera2DShake._instance = UnityEngine.Object.FindObjectOfType(typeof (ProCamera2DShake)) as ProCamera2DShake;
        if (object.Equals((object) ProCamera2DShake._instance, (object) null))
          throw new UnityException("ProCamera2D does not have a Shake extension.");
      }
      return ProCamera2DShake._instance;
    }
  }

  public static bool Exists => (UnityEngine.Object) ProCamera2DShake._instance != (UnityEngine.Object) null;

  public override void Awake()
  {
    base.Awake();
    ProCamera2DShake._instance = this;
    if ((UnityEngine.Object) this.ProCamera2D.transform.parent != (UnityEngine.Object) null)
    {
      this._shakeParent = new GameObject("ProCamera2DShakeContainer").transform;
      this._shakeParent.parent = this.ProCamera2D.transform.parent;
      this._shakeParent.localPosition = Vector3.zero;
      this.ProCamera2D.transform.parent = this._shakeParent;
    }
    else
      this._shakeParent = this.ProCamera2D.transform.parent = new GameObject("ProCamera2DShakeContainer").transform;
    this._originalRotation = this._transform.localRotation;
  }

  public void Start()
  {
    if (!((UnityEngine.Object) this.StartConstantShakePreset != (UnityEngine.Object) null))
      return;
    this.ConstantShake(this.StartConstantShakePreset);
  }

  public void Update()
  {
    this._influencesSum = Vector3.zero;
    if (this._influences.Count <= 0)
      return;
    this._influencesSum = Utils.GetVectorsSum((IList<Vector3>) this._influences);
    this._influences.Clear();
    this._shakeParent.localPosition = this._influencesSum;
  }

  public void Shake(
    float duration,
    Vector2 strength,
    int vibrato = 10,
    float randomness = 0.1f,
    float initialAngle = -1f,
    Vector3 rotation = default (Vector3),
    float smoothness = 0.1f,
    bool ignoreTimeScale = false)
  {
    if (!this.enabled)
      return;
    ++vibrato;
    if (vibrato < 2)
      vibrato = 2;
    float[] durations = new float[vibrato];
    float num1 = 0.0f;
    for (int index = 0; index < vibrato; ++index)
    {
      float num2 = (float) (index + 1) / (float) vibrato;
      float num3 = duration * num2;
      num1 += num3;
      durations[index] = num3;
    }
    float num4 = duration / num1;
    for (int index = 0; index < vibrato; ++index)
      durations[index] = durations[index] * num4;
    float magnitude = strength.magnitude;
    float num5 = magnitude / (float) vibrato;
    float num6 = (double) initialAngle != -1.0 ? initialAngle : UnityEngine.Random.Range(0.0f, 360f);
    Vector2[] shakes = new Vector2[vibrato];
    shakes[vibrato - 1] = Vector2.zero;
    Quaternion[] rotations = new Quaternion[vibrato];
    rotations[vibrato - 1] = this._originalRotation;
    Quaternion a = Quaternion.Euler(rotation);
    for (int index = 0; index < vibrato - 1; ++index)
    {
      if (index > 0)
        num6 = (float) ((double) num6 - 180.0 + (double) UnityEngine.Random.Range(-90, 90) * (double) randomness);
      Quaternion quaternion = Quaternion.AngleAxis((float) UnityEngine.Random.Range(-90, 90) * randomness, Vector3.up);
      float f = num6 * ((float) Math.PI / 180f);
      Vector3 vector3 = new Vector3(magnitude * Mathf.Cos(f), magnitude * Mathf.Sin(f), 0.0f);
      Vector2 vector = (Vector2) (quaternion * vector3);
      vector.x = Vector2.ClampMagnitude(vector, strength.x).x;
      vector.y = Vector2.ClampMagnitude(vector, strength.y).y;
      shakes[index] = vector;
      magnitude -= num5;
      strength = Vector2.ClampMagnitude(strength, magnitude);
      int num7 = index % 2 == 0 ? 1 : -1;
      float t = (float) index / (float) (vibrato - 1);
      rotations[index] = num7 == 1 ? Quaternion.Lerp(a, Quaternion.identity, t) * this._originalRotation : Quaternion.Inverse(Quaternion.Lerp(a, Quaternion.identity, t)) * this._originalRotation;
    }
    this._applyInfluencesCoroutines.Add(this.ApplyShakesTimed(shakes, rotations, durations, smoothness, ignoreTimeScale));
  }

  public void Shake(int presetIndex)
  {
    if (presetIndex <= this.ShakePresets.Count - 1)
      this.Shake(this.ShakePresets[presetIndex].Duration, (Vector2) this.ShakePresets[presetIndex].Strength, this.ShakePresets[presetIndex].Vibrato, this.ShakePresets[presetIndex].Randomness, this.ShakePresets[presetIndex].UseRandomInitialAngle ? -1f : this.ShakePresets[presetIndex].InitialAngle, this.ShakePresets[presetIndex].Rotation, this.ShakePresets[presetIndex].Smoothness, this.ShakePresets[presetIndex].IgnoreTimeScale);
    else
      Debug.LogWarning((object) ("Could not find a shake preset with the index: " + presetIndex.ToString()));
  }

  public void Shake(string presetName)
  {
    for (int index = 0; index < this.ShakePresets.Count; ++index)
    {
      if (this.ShakePresets[index].name == presetName)
      {
        this.Shake(index);
        return;
      }
    }
    Debug.LogWarning((object) ("Could not find a shake preset with the name: " + presetName));
  }

  public void Shake(ShakePreset preset)
  {
    this.Shake(preset.Duration, (Vector2) preset.Strength, preset.Vibrato, preset.Randomness, preset.UseRandomInitialAngle ? -1f : preset.InitialAngle, preset.Rotation, preset.Smoothness, preset.IgnoreTimeScale);
  }

  public void StopShaking()
  {
    for (int index = 0; index < this._applyInfluencesCoroutines.Count; ++index)
      this.StopCoroutine(this._applyInfluencesCoroutines[index]);
    this._shakePositions.Clear();
    if (this._shakeCoroutine != null)
    {
      this.StopCoroutine(this._shakeCoroutine);
      this._shakeCoroutine = (Coroutine) null;
    }
    this.ShakeCompleted();
  }

  public void ConstantShake(ConstantShakePreset preset)
  {
    if ((UnityEngine.Object) this.CurrentConstantShakePreset != (UnityEngine.Object) null)
      this.StopConstantShaking(0.0f);
    this.CurrentConstantShakePreset = preset;
    this._isConstantShaking = true;
    this._constantShakePositions = new Vector3[preset.Layers.Count];
    for (int index = 0; index < preset.Layers.Count; ++index)
      this.StartCoroutine(this.CalculateConstantShakePosition(index, preset.Layers[index].Frequency.x, preset.Layers[index].Frequency.y, preset.Layers[index].AmplitudeHorizontal, preset.Layers[index].AmplitudeVertical, preset.Layers[index].AmplitudeDepth));
    this.StartCoroutine(this.ConstantShakeRoutine(preset.Intensity));
  }

  public void ConstantShake(string presetName)
  {
    for (int index = 0; index < this.ConstantShakePresets.Count; ++index)
    {
      if (this.ConstantShakePresets[index].name == presetName)
      {
        this.ConstantShake(this.ConstantShakePresets[index]);
        return;
      }
    }
    Debug.LogWarning((object) $"Could not find a ConstantShakePreset with the name: {presetName}. Remember you need to add it to the ConstantShakePresets list first.");
  }

  public void ConstantShake(int presetIndex)
  {
    if (presetIndex <= this.ConstantShakePresets.Count - 1)
      this.ConstantShake(this.ConstantShakePresets[presetIndex]);
    else
      Debug.LogWarning((object) $"Could not find a ConstantShakePreset with the index: {presetIndex.ToString()}. Remember you need to add it to the ConstantShakePresets list first.");
  }

  public void StopConstantShaking(float duration = 0.3f)
  {
    this.CurrentConstantShakePreset = (ConstantShakePreset) null;
    this._isConstantShaking = false;
    if ((double) duration > 0.0)
    {
      this.StartCoroutine(this.StopConstantShakeRoutine(duration));
    }
    else
    {
      this.StopAllCoroutines();
      this._constantShakePosition = Vector3.zero;
      this._influences.Clear();
      this._influences.Add(this._constantShakePosition);
    }
  }

  public Coroutine ApplyShakesTimed(
    Vector2[] shakes,
    Vector3[] rotations,
    float[] durations,
    float smoothness = 0.1f,
    bool ignoreTimeScale = false)
  {
    if (!this.enabled)
      return (Coroutine) null;
    Quaternion[] rotations1 = new Quaternion[rotations.Length];
    for (int index = 0; index < rotations.Length; ++index)
      rotations1[index] = Quaternion.Euler(rotations[index]) * this._originalRotation;
    return this.ApplyShakesTimed(shakes, rotations1, durations);
  }

  public void ApplyInfluenceIgnoringBoundaries(Vector2 influence)
  {
    if ((double) Time.deltaTime < 9.9999997473787516E-05 || float.IsNaN(influence.x) || float.IsNaN(influence.y))
      return;
    this._influences.Add(this.VectorHV(influence.x, influence.y));
  }

  public Coroutine ApplyShakesTimed(
    Vector2[] shakes,
    Quaternion[] rotations,
    float[] durations,
    float smoothness = 0.1f,
    bool ignoreTimeScale = false)
  {
    Coroutine coroutine = this.StartCoroutine(this.ApplyShakesTimedRoutine((IList<Vector2>) shakes, (IList<Quaternion>) rotations, durations, ignoreTimeScale));
    if (this._shakeCoroutine != null)
      return coroutine;
    this._shakeCoroutine = this.StartCoroutine(this.ShakeRoutine(smoothness, ignoreTimeScale));
    return coroutine;
  }

  public IEnumerator ShakeRoutine(float smoothness, bool ignoreTimeScale = false)
  {
    ProCamera2DShake proCamera2Dshake = this;
    while (proCamera2Dshake._shakePositions.Count > 0 || (double) Vector3.Distance(proCamera2Dshake._shakeParent.localPosition, proCamera2Dshake._influencesSum) > 0.0099999997764825821 || (double) Quaternion.Angle(proCamera2Dshake._transform.localRotation, proCamera2Dshake._originalRotation) > 0.0099999997764825821)
    {
      Vector3 target = Utils.GetVectorsSum((IList<Vector3>) proCamera2Dshake._shakePositions) + proCamera2Dshake._influencesSum;
      Vector3 vector3 = Vector3.zero;
      if (ignoreTimeScale)
        vector3 = Vector3.SmoothDamp(proCamera2Dshake._shakeParent.localPosition, target, ref proCamera2Dshake._shakeVelocity, smoothness, float.MaxValue, Time.unscaledDeltaTime);
      else if ((double) proCamera2Dshake.ProCamera2D.DeltaTime > 0.0)
        vector3 = Vector3.SmoothDamp(proCamera2Dshake._shakeParent.localPosition, target, ref proCamera2Dshake._shakeVelocity, smoothness);
      proCamera2Dshake._shakeParent.localPosition = vector3;
      proCamera2Dshake._shakePositions.Clear();
      if (ignoreTimeScale)
        proCamera2Dshake._rotationTime = Mathf.SmoothDamp(proCamera2Dshake._rotationTime, 1f, ref proCamera2Dshake._rotationVelocity, smoothness, float.MaxValue, Time.unscaledDeltaTime);
      else if ((double) proCamera2Dshake.ProCamera2D.DeltaTime > 0.0)
        proCamera2Dshake._rotationTime = Mathf.SmoothDamp(proCamera2Dshake._rotationTime, 1f, ref proCamera2Dshake._rotationVelocity, smoothness, float.MaxValue, proCamera2Dshake.ProCamera2D.DeltaTime);
      Quaternion quaternion = Quaternion.Slerp(proCamera2Dshake._transform.localRotation, proCamera2Dshake._rotationTarget, proCamera2Dshake._rotationTime);
      proCamera2Dshake._transform.localRotation = quaternion;
      proCamera2Dshake._rotationTarget = proCamera2Dshake._originalRotation;
      yield return (object) proCamera2Dshake.ProCamera2D.GetYield();
    }
    proCamera2Dshake.ShakeCompleted();
  }

  public void ShakeCompleted()
  {
    this._shakeParent.localPosition = this._influencesSum;
    this._transform.localRotation = this._originalRotation;
    this._shakeCoroutine = (Coroutine) null;
    if (this.OnShakeCompleted == null)
      return;
    this.OnShakeCompleted();
  }

  public IEnumerator ApplyShakesTimedRoutine(
    IList<Vector2> shakes,
    IList<Quaternion> rotations,
    float[] durations,
    bool ignoreTimeScale = false)
  {
    ProCamera2DShake proCamera2Dshake = this;
    int count = -1;
    while (count < durations.Length - 1)
    {
      ++count;
      float duration = durations[count];
      yield return (object) proCamera2Dshake.StartCoroutine(proCamera2Dshake.ApplyShakeTimedRoutine(shakes[count], rotations[count], duration, ignoreTimeScale));
    }
  }

  public IEnumerator ApplyShakeTimedRoutine(
    Vector2 shake,
    Quaternion rotation,
    float duration,
    bool ignoreTimeScale = false)
  {
    ProCamera2DShake proCamera2Dshake = this;
    proCamera2Dshake._rotationTime = 0.0f;
    proCamera2Dshake._rotationVelocity = 0.0f;
    while ((double) duration > 0.0)
    {
      if (ignoreTimeScale)
        duration -= Time.unscaledDeltaTime;
      else
        duration -= proCamera2Dshake.ProCamera2D.DeltaTime;
      proCamera2Dshake._shakePositions.Add(proCamera2Dshake.VectorHV(shake.x, shake.y));
      proCamera2Dshake._rotationTarget = rotation;
      yield return (object) proCamera2Dshake.ProCamera2D.GetYield();
    }
  }

  public IEnumerator StopConstantShakeRoutine(float duration)
  {
    ProCamera2DShake proCamera2Dshake = this;
    Vector3 velocity = Vector3.zero;
    proCamera2Dshake._influences.Clear();
    while ((double) duration >= 0.0)
    {
      duration -= proCamera2Dshake.ProCamera2D.DeltaTime;
      proCamera2Dshake._constantShakePosition = Vector3.SmoothDamp(proCamera2Dshake._constantShakePosition, Vector3.zero, ref velocity, duration, float.MaxValue);
      proCamera2Dshake._influences.Add(proCamera2Dshake._constantShakePosition);
      yield return (object) proCamera2Dshake.ProCamera2D.GetYield();
    }
  }

  public IEnumerator CalculateConstantShakePosition(
    int index,
    float frequencyMin,
    float frequencyMax,
    float amplitudeX,
    float amplitudeY,
    float amplitudeZ)
  {
    ProCamera2DShake proCamera2Dshake = this;
    while (proCamera2Dshake._isConstantShaking)
    {
      float seconds = UnityEngine.Random.Range(frequencyMin, frequencyMax);
      Vector3 insideUnitSphere = UnityEngine.Random.insideUnitSphere;
      float num1 = insideUnitSphere.x * amplitudeX;
      float num2 = insideUnitSphere.y * amplitudeY;
      float num3 = insideUnitSphere.z * amplitudeZ;
      if (index < proCamera2Dshake._constantShakePositions.Length)
        proCamera2Dshake._constantShakePositions[index] = proCamera2Dshake.VectorHVD(num1, num2, num3);
      yield return (object) new WaitForSeconds(seconds);
    }
  }

  public IEnumerator ConstantShakeRoutine(float intensity)
  {
    ProCamera2DShake proCamera2Dshake = this;
    while (proCamera2Dshake._isConstantShaking)
    {
      if ((double) proCamera2Dshake.ProCamera2D.DeltaTime > 0.0)
      {
        Vector3 vector3 = Utils.GetVectorsSum((IList<Vector3>) proCamera2Dshake._constantShakePositions) / (float) proCamera2Dshake._constantShakePositions.Length;
        proCamera2Dshake._constantShakePosition.Set(Utils.SmoothApproach(proCamera2Dshake._constantShakePosition.x, vector3.x, vector3.x, intensity, proCamera2Dshake.ProCamera2D.DeltaTime), Utils.SmoothApproach(proCamera2Dshake._constantShakePosition.y, vector3.y, vector3.y, intensity, proCamera2Dshake.ProCamera2D.DeltaTime), Utils.SmoothApproach(proCamera2Dshake._constantShakePosition.z, vector3.z, vector3.z, intensity, proCamera2Dshake.ProCamera2D.DeltaTime));
        proCamera2Dshake._influences.Add(proCamera2Dshake._constantShakePosition);
      }
      yield return (object) proCamera2Dshake.ProCamera2D.GetYield();
    }
  }
}
