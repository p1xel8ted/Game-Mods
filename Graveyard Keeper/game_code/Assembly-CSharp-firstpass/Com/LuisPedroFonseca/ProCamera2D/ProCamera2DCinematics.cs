// Decompiled with JetBrains decompiler
// Type: Com.LuisPedroFonseca.ProCamera2D.ProCamera2DCinematics
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace Com.LuisPedroFonseca.ProCamera2D;

[HelpURL("http://www.procamera2d.com/user-guide/extension-cinematics/")]
public class ProCamera2DCinematics : BasePC2D, IPositionOverrider, ISizeOverrider
{
  public static string ExtensionName = "Cinematics";
  public UnityEvent OnCinematicStarted;
  public CinematicEvent OnCinematicTargetReached;
  public UnityEvent OnCinematicFinished;
  public bool _isPlaying;
  public List<CinematicTarget> CinematicTargets = new List<CinematicTarget>();
  public float EndDuration = 1f;
  public EaseType EndEaseType = EaseType.EaseOut;
  public bool UseNumericBoundaries;
  public bool UseLetterbox = true;
  [Range(0.0f, 0.5f)]
  public float LetterboxAmount = 0.1f;
  public float LetterboxAnimDuration = 1f;
  public Color LetterboxColor = Color.black;
  public float _initialCameraSize;
  public ProCamera2DNumericBoundaries _numericBoundaries;
  public ProCamera2DLetterbox _letterbox;
  public Coroutine _startCinematicRoutine;
  public Coroutine _goToCinematicRoutine;
  public Coroutine _endCinematicRoutine;
  public bool _skipTarget;
  public Vector3 _newPos;
  public Vector3 _originalPos;
  public Vector3 _startPos;
  public float _newSize;
  public bool _paused;
  public int _poOrder;
  public int _soOrder = 3000;

  public bool IsPlaying => this._isPlaying;

  public override void Awake()
  {
    base.Awake();
    if (this.UseLetterbox)
      this.SetupLetterbox();
    this.ProCamera2D.AddPositionOverrider((IPositionOverrider) this);
    this.ProCamera2D.AddSizeOverrider((ISizeOverrider) this);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    this.ProCamera2D.RemovePositionOverrider((IPositionOverrider) this);
    this.ProCamera2D.RemoveSizeOverrider((ISizeOverrider) this);
  }

  public Vector3 OverridePosition(float deltaTime, Vector3 originalPosition)
  {
    if (!this.enabled)
      return originalPosition;
    this._originalPos = originalPosition;
    return this._isPlaying ? this._newPos : originalPosition;
  }

  public int POOrder
  {
    get => this._poOrder;
    set => this._poOrder = value;
  }

  public float OverrideSize(float deltaTime, float originalSize)
  {
    return !this.enabled || !this._isPlaying ? originalSize : this._newSize;
  }

  public int SOOrder
  {
    get => this._soOrder;
    set => this._soOrder = value;
  }

  public void Play()
  {
    if (this._isPlaying)
      return;
    this._paused = false;
    if (this.CinematicTargets.Count == 0)
    {
      Debug.LogWarning((object) "No cinematic targets added to the list");
    }
    else
    {
      this._initialCameraSize = this.ProCamera2D.ScreenSizeInWorldCoordinates.y * 0.5f;
      if (this.UseNumericBoundaries && (UnityEngine.Object) this._numericBoundaries == (UnityEngine.Object) null)
        this._numericBoundaries = this.ProCamera2D.GetComponentInChildren<ProCamera2DNumericBoundaries>();
      if ((UnityEngine.Object) this._numericBoundaries == (UnityEngine.Object) null)
        this.UseNumericBoundaries = false;
      this._isPlaying = true;
      if (this._endCinematicRoutine != null)
      {
        this.StopCoroutine(this._endCinematicRoutine);
        this._endCinematicRoutine = (Coroutine) null;
      }
      if (this._startCinematicRoutine != null)
        return;
      this._startCinematicRoutine = this.StartCoroutine(this.StartCinematicRoutine());
    }
  }

  public void Stop()
  {
    if (!this._isPlaying)
      return;
    if (this._startCinematicRoutine != null)
    {
      this.StopCoroutine(this._startCinematicRoutine);
      this._startCinematicRoutine = (Coroutine) null;
    }
    if (this._goToCinematicRoutine != null)
    {
      this.StopCoroutine(this._goToCinematicRoutine);
      this._goToCinematicRoutine = (Coroutine) null;
    }
    if (this._endCinematicRoutine != null)
      return;
    this._endCinematicRoutine = this.StartCoroutine(this.EndCinematicRoutine());
  }

  public void Toggle()
  {
    if (this._isPlaying)
      this.Stop();
    else
      this.Play();
  }

  public void GoToNextTarget() => this._skipTarget = true;

  public void Pause() => this._paused = true;

  public void Unpause() => this._paused = false;

  public void AddCinematicTarget(
    Transform targetTransform,
    float easeInDuration = 1f,
    float holdDuration = 1f,
    float zoom = 1f,
    EaseType easeType = EaseType.EaseOut,
    string sendMessageName = "",
    string sendMessageParam = "",
    int index = -1)
  {
    CinematicTarget cinematicTarget = new CinematicTarget()
    {
      TargetTransform = targetTransform,
      EaseInDuration = easeInDuration,
      HoldDuration = holdDuration,
      Zoom = zoom,
      EaseType = easeType,
      SendMessageName = sendMessageName,
      SendMessageParam = sendMessageParam
    };
    if (index == -1 || index > this.CinematicTargets.Count)
      this.CinematicTargets.Add(cinematicTarget);
    else
      this.CinematicTargets.Insert(index, cinematicTarget);
  }

  public void RemoveCinematicTarget(Transform targetTransform)
  {
    for (int index = 0; index < this.CinematicTargets.Count; ++index)
    {
      if (this.CinematicTargets[index].TargetTransform.GetInstanceID() == targetTransform.GetInstanceID())
        this.CinematicTargets.Remove(this.CinematicTargets[index]);
    }
  }

  public IEnumerator StartCinematicRoutine()
  {
    ProCamera2DCinematics camera2Dcinematics = this;
    if (camera2Dcinematics.OnCinematicStarted != null)
      camera2Dcinematics.OnCinematicStarted.Invoke();
    camera2Dcinematics._startPos = camera2Dcinematics.ProCamera2D.LocalPosition;
    camera2Dcinematics._newPos = camera2Dcinematics.ProCamera2D.LocalPosition;
    camera2Dcinematics._newSize = camera2Dcinematics.ProCamera2D.ScreenSizeInWorldCoordinates.y * 0.5f;
    if (camera2Dcinematics.UseLetterbox)
      camera2Dcinematics.LetterboxEffect(true);
    int count = -1;
    while (count < camera2Dcinematics.CinematicTargets.Count - 1)
    {
      ++count;
      camera2Dcinematics._skipTarget = false;
      camera2Dcinematics._goToCinematicRoutine = camera2Dcinematics.StartCoroutine(camera2Dcinematics.GoToCinematicTargetRoutine(camera2Dcinematics.CinematicTargets[count], count));
      yield return (object) camera2Dcinematics._goToCinematicRoutine;
    }
    camera2Dcinematics.Stop();
  }

  public void LetterboxEffect(bool show)
  {
    if ((UnityEngine.Object) this._letterbox == (UnityEngine.Object) null)
      this.SetupLetterbox();
    if (show)
    {
      this._letterbox.Color = this.LetterboxColor;
      this._letterbox.TweenTo(this.LetterboxAmount, this.LetterboxAnimDuration);
    }
    else
    {
      if (!((UnityEngine.Object) this._letterbox != (UnityEngine.Object) null) || (double) this.LetterboxAmount <= 0.0)
        return;
      this._letterbox.TweenTo(0.0f, this.LetterboxAnimDuration);
    }
  }

  public IEnumerator GoToCinematicTargetRoutine(CinematicTarget cinematicTarget, int targetIndex)
  {
    ProCamera2DCinematics camera2Dcinematics = this;
    if (!((UnityEngine.Object) cinematicTarget.TargetTransform == (UnityEngine.Object) null))
    {
      float initialPosH = camera2Dcinematics.Vector3H(camera2Dcinematics.ProCamera2D.LocalPosition);
      float initialPosV = camera2Dcinematics.Vector3V(camera2Dcinematics.ProCamera2D.LocalPosition);
      float currentCameraSize = camera2Dcinematics.ProCamera2D.ScreenSizeInWorldCoordinates.y * 0.5f;
      float t = 0.0f;
      if ((double) cinematicTarget.EaseInDuration > 0.0)
      {
        while ((double) t <= 1.0)
        {
          if (!camera2Dcinematics._paused)
          {
            t += camera2Dcinematics.ProCamera2D.DeltaTime / cinematicTarget.EaseInDuration;
            float horizontalPos = Utils.EaseFromTo(initialPosH, camera2Dcinematics.Vector3H(cinematicTarget.TargetTransform.position) - camera2Dcinematics.Vector3H(camera2Dcinematics.ProCamera2D.ParentPosition), t, cinematicTarget.EaseType);
            float verticalPos = Utils.EaseFromTo(initialPosV, camera2Dcinematics.Vector3V(cinematicTarget.TargetTransform.position) - camera2Dcinematics.Vector3V(camera2Dcinematics.ProCamera2D.ParentPosition), t, cinematicTarget.EaseType);
            if (camera2Dcinematics.UseNumericBoundaries)
              camera2Dcinematics.LimitToNumericBoundaries(ref horizontalPos, ref verticalPos);
            camera2Dcinematics._newPos = camera2Dcinematics.VectorHVD(horizontalPos, verticalPos, 0.0f);
            camera2Dcinematics._newSize = Utils.EaseFromTo(currentCameraSize, camera2Dcinematics._initialCameraSize / cinematicTarget.Zoom, t, cinematicTarget.EaseType);
            if (camera2Dcinematics._skipTarget)
              yield break;
          }
          yield return (object) camera2Dcinematics.ProCamera2D.GetYield();
        }
      }
      else
      {
        float num1 = camera2Dcinematics.Vector3H(cinematicTarget.TargetTransform.position) - camera2Dcinematics.Vector3H(camera2Dcinematics.ProCamera2D.ParentPosition);
        float num2 = camera2Dcinematics.Vector3V(cinematicTarget.TargetTransform.position) - camera2Dcinematics.Vector3V(camera2Dcinematics.ProCamera2D.ParentPosition);
        camera2Dcinematics._newPos = camera2Dcinematics.VectorHVD(num1, num2, 0.0f);
        camera2Dcinematics._newSize = camera2Dcinematics._initialCameraSize / cinematicTarget.Zoom;
      }
      if (camera2Dcinematics.OnCinematicTargetReached != null)
        camera2Dcinematics.OnCinematicTargetReached.Invoke(targetIndex);
      if (!string.IsNullOrEmpty(cinematicTarget.SendMessageName))
        cinematicTarget.TargetTransform.SendMessage(cinematicTarget.SendMessageName, (object) cinematicTarget.SendMessageParam, SendMessageOptions.DontRequireReceiver);
      t = 0.0f;
      while ((double) cinematicTarget.HoldDuration < 0.0 || (double) t <= (double) cinematicTarget.HoldDuration)
      {
        if (!camera2Dcinematics._paused)
        {
          t += camera2Dcinematics.ProCamera2D.DeltaTime;
          float horizontalPos = camera2Dcinematics.Vector3H(cinematicTarget.TargetTransform.position) - camera2Dcinematics.Vector3H(camera2Dcinematics.ProCamera2D.ParentPosition);
          float verticalPos = camera2Dcinematics.Vector3V(cinematicTarget.TargetTransform.position) - camera2Dcinematics.Vector3V(camera2Dcinematics.ProCamera2D.ParentPosition);
          if (camera2Dcinematics.UseNumericBoundaries)
            camera2Dcinematics.LimitToNumericBoundaries(ref horizontalPos, ref verticalPos);
          camera2Dcinematics._newPos = camera2Dcinematics.VectorHVD(horizontalPos, verticalPos, 0.0f);
          if (camera2Dcinematics._skipTarget)
            break;
        }
        yield return (object) camera2Dcinematics.ProCamera2D.GetYield();
      }
    }
  }

  public IEnumerator EndCinematicRoutine()
  {
    ProCamera2DCinematics camera2Dcinematics = this;
    camera2Dcinematics.LetterboxEffect(false);
    float initialPosH = camera2Dcinematics.Vector3H(camera2Dcinematics._newPos);
    float initialPosV = camera2Dcinematics.Vector3V(camera2Dcinematics._newPos);
    float currentCameraSize = camera2Dcinematics.ProCamera2D.ScreenSizeInWorldCoordinates.y * 0.5f;
    float t = 0.0f;
    while ((double) t <= 1.0)
    {
      if (!camera2Dcinematics._paused)
      {
        t += camera2Dcinematics.ProCamera2D.DeltaTime / camera2Dcinematics.EndDuration;
        float horizontalPos;
        float verticalPos;
        if (camera2Dcinematics.ProCamera2D.CameraTargets.Count > 0)
        {
          horizontalPos = Utils.EaseFromTo(initialPosH, camera2Dcinematics.Vector3H(camera2Dcinematics._originalPos), t, camera2Dcinematics.EndEaseType);
          verticalPos = Utils.EaseFromTo(initialPosV, camera2Dcinematics.Vector3V(camera2Dcinematics._originalPos), t, camera2Dcinematics.EndEaseType);
        }
        else
        {
          horizontalPos = Utils.EaseFromTo(initialPosH, camera2Dcinematics.Vector3H(camera2Dcinematics._startPos), t, camera2Dcinematics.EndEaseType);
          verticalPos = Utils.EaseFromTo(initialPosV, camera2Dcinematics.Vector3V(camera2Dcinematics._startPos), t, camera2Dcinematics.EndEaseType);
        }
        if (camera2Dcinematics.UseNumericBoundaries)
          camera2Dcinematics.LimitToNumericBoundaries(ref horizontalPos, ref verticalPos);
        camera2Dcinematics._newPos = camera2Dcinematics.VectorHVD(horizontalPos, verticalPos, 0.0f);
        camera2Dcinematics._newSize = Utils.EaseFromTo(currentCameraSize, camera2Dcinematics._initialCameraSize, t, camera2Dcinematics.EndEaseType);
      }
      yield return (object) camera2Dcinematics.ProCamera2D.GetYield();
    }
    camera2Dcinematics._isPlaying = false;
    if (camera2Dcinematics.ProCamera2D.CameraTargets.Count == 0)
      camera2Dcinematics.ProCamera2D.Reset();
    if (camera2Dcinematics.OnCinematicFinished != null)
      camera2Dcinematics.OnCinematicFinished.Invoke();
  }

  public void SetupLetterbox()
  {
    ProCamera2DLetterbox componentInChildren = this.ProCamera2D.gameObject.GetComponentInChildren<ProCamera2DLetterbox>();
    if ((UnityEngine.Object) componentInChildren == (UnityEngine.Object) null)
      ((IEnumerable<Camera>) this.ProCamera2D.gameObject.GetComponentsInChildren<Camera>()).OrderByDescending<Camera, float>((Func<Camera, float>) (c => c.depth)).ToArray<Camera>()[0].gameObject.AddComponent<ProCamera2DLetterbox>();
    this._letterbox = componentInChildren;
  }

  public void LimitToNumericBoundaries(ref float horizontalPos, ref float verticalPos)
  {
    if (this._numericBoundaries.UseLeftBoundary && (double) horizontalPos - (double) this.ProCamera2D.ScreenSizeInWorldCoordinates.x / 2.0 < (double) this._numericBoundaries.LeftBoundary)
      horizontalPos = this._numericBoundaries.LeftBoundary + this.ProCamera2D.ScreenSizeInWorldCoordinates.x / 2f;
    else if (this._numericBoundaries.UseRightBoundary && (double) horizontalPos + (double) this.ProCamera2D.ScreenSizeInWorldCoordinates.x / 2.0 > (double) this._numericBoundaries.RightBoundary)
      horizontalPos = this._numericBoundaries.RightBoundary - this.ProCamera2D.ScreenSizeInWorldCoordinates.x / 2f;
    if (this._numericBoundaries.UseBottomBoundary && (double) verticalPos - (double) this.ProCamera2D.ScreenSizeInWorldCoordinates.y / 2.0 < (double) this._numericBoundaries.BottomBoundary)
    {
      verticalPos = this._numericBoundaries.BottomBoundary + this.ProCamera2D.ScreenSizeInWorldCoordinates.y / 2f;
    }
    else
    {
      if (!this._numericBoundaries.UseTopBoundary || (double) verticalPos + (double) this.ProCamera2D.ScreenSizeInWorldCoordinates.y / 2.0 <= (double) this._numericBoundaries.TopBoundary)
        return;
      verticalPos = this._numericBoundaries.TopBoundary - this.ProCamera2D.ScreenSizeInWorldCoordinates.y / 2f;
    }
  }
}
