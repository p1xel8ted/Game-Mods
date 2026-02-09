// Decompiled with JetBrains decompiler
// Type: Com.LuisPedroFonseca.ProCamera2D.BoundariesAnimator
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
namespace Com.LuisPedroFonseca.ProCamera2D;

public class BoundariesAnimator
{
  public Action OnTransitionStarted;
  public Action OnTransitionFinished;
  public bool UseTopBoundary;
  public float TopBoundary;
  public bool UseBottomBoundary;
  public float BottomBoundary;
  public bool UseLeftBoundary;
  public float LeftBoundary;
  public bool UseRightBoundary;
  public float RightBoundary;
  public float TransitionDuration = 1f;
  public EaseType TransitionEaseType;
  public Com.LuisPedroFonseca.ProCamera2D.ProCamera2D ProCamera2D;
  public ProCamera2DNumericBoundaries NumericBoundaries;
  public Func<Vector3, float> Vector3H;
  public Func<Vector3, float> Vector3V;

  public BoundariesAnimator(Com.LuisPedroFonseca.ProCamera2D.ProCamera2D proCamera2D, ProCamera2DNumericBoundaries numericBoundaries)
  {
    this.ProCamera2D = proCamera2D;
    this.NumericBoundaries = numericBoundaries;
    switch (this.ProCamera2D.Axis)
    {
      case MovementAxis.XY:
        this.Vector3H = (Func<Vector3, float>) (vector => vector.x);
        this.Vector3V = (Func<Vector3, float>) (vector => vector.y);
        break;
      case MovementAxis.XZ:
        this.Vector3H = (Func<Vector3, float>) (vector => vector.x);
        this.Vector3V = (Func<Vector3, float>) (vector => vector.z);
        break;
      case MovementAxis.YZ:
        this.Vector3H = (Func<Vector3, float>) (vector => vector.z);
        this.Vector3V = (Func<Vector3, float>) (vector => vector.y);
        break;
    }
  }

  public int GetAnimsCount()
  {
    int animsCount = 0;
    if (this.UseLeftBoundary)
      ++animsCount;
    else if (!this.UseLeftBoundary && this.NumericBoundaries.UseLeftBoundary && this.UseRightBoundary && (double) this.RightBoundary < (double) this.NumericBoundaries.TargetLeftBoundary)
      ++animsCount;
    if (this.UseRightBoundary)
      ++animsCount;
    else if (!this.UseRightBoundary && this.NumericBoundaries.UseRightBoundary && this.UseLeftBoundary && (double) this.LeftBoundary > (double) this.NumericBoundaries.TargetRightBoundary)
      ++animsCount;
    if (this.UseTopBoundary)
      ++animsCount;
    else if (!this.UseTopBoundary && this.NumericBoundaries.UseTopBoundary && this.UseBottomBoundary && (double) this.BottomBoundary > (double) this.NumericBoundaries.TargetTopBoundary)
      ++animsCount;
    if (this.UseBottomBoundary)
      ++animsCount;
    else if (!this.UseBottomBoundary && this.NumericBoundaries.UseBottomBoundary && this.UseTopBoundary && (double) this.TopBoundary < (double) this.NumericBoundaries.TargetBottomBoundary)
      ++animsCount;
    return animsCount;
  }

  public void Transition()
  {
    if (!this.NumericBoundaries.HasFiredTransitionStarted && this.OnTransitionStarted != null)
    {
      this.NumericBoundaries.HasFiredTransitionStarted = true;
      this.OnTransitionStarted();
    }
    this.NumericBoundaries.HasFiredTransitionFinished = false;
    this.NumericBoundaries.UseNumericBoundaries = true;
    if (this.UseLeftBoundary)
    {
      this.NumericBoundaries.UseLeftBoundary = true;
      if (this.NumericBoundaries.LeftBoundaryAnimRoutine != null)
        this.NumericBoundaries.StopCoroutine(this.NumericBoundaries.LeftBoundaryAnimRoutine);
      this.NumericBoundaries.LeftBoundaryAnimRoutine = this.NumericBoundaries.StartCoroutine(this.LeftTransitionRoutine(this.TransitionDuration));
    }
    else if (!this.UseLeftBoundary && this.NumericBoundaries.UseLeftBoundary && this.UseRightBoundary && (double) this.RightBoundary < (double) this.NumericBoundaries.TargetLeftBoundary)
    {
      this.NumericBoundaries.UseLeftBoundary = true;
      this.UseLeftBoundary = true;
      this.LeftBoundary = this.RightBoundary - this.ProCamera2D.ScreenSizeInWorldCoordinates.x * 100f;
      if (this.NumericBoundaries.LeftBoundaryAnimRoutine != null)
        this.NumericBoundaries.StopCoroutine(this.NumericBoundaries.LeftBoundaryAnimRoutine);
      this.NumericBoundaries.LeftBoundaryAnimRoutine = this.NumericBoundaries.StartCoroutine(this.LeftTransitionRoutine(this.TransitionDuration, true));
    }
    else if (!this.UseLeftBoundary)
      this.NumericBoundaries.UseLeftBoundary = false;
    if (this.UseRightBoundary)
    {
      this.NumericBoundaries.UseRightBoundary = true;
      if (this.NumericBoundaries.RightBoundaryAnimRoutine != null)
        this.NumericBoundaries.StopCoroutine(this.NumericBoundaries.RightBoundaryAnimRoutine);
      this.NumericBoundaries.RightBoundaryAnimRoutine = this.NumericBoundaries.StartCoroutine(this.RightTransitionRoutine(this.TransitionDuration));
    }
    else if (!this.UseRightBoundary && this.NumericBoundaries.UseRightBoundary && this.UseLeftBoundary && (double) this.LeftBoundary > (double) this.NumericBoundaries.TargetRightBoundary)
    {
      this.NumericBoundaries.UseRightBoundary = true;
      this.UseRightBoundary = true;
      this.RightBoundary = this.LeftBoundary + this.ProCamera2D.ScreenSizeInWorldCoordinates.x * 100f;
      if (this.NumericBoundaries.RightBoundaryAnimRoutine != null)
        this.NumericBoundaries.StopCoroutine(this.NumericBoundaries.RightBoundaryAnimRoutine);
      this.NumericBoundaries.RightBoundaryAnimRoutine = this.NumericBoundaries.StartCoroutine(this.RightTransitionRoutine(this.TransitionDuration, true));
    }
    else if (!this.UseRightBoundary)
      this.NumericBoundaries.UseRightBoundary = false;
    if (this.UseTopBoundary)
    {
      this.NumericBoundaries.UseTopBoundary = true;
      if (this.NumericBoundaries.TopBoundaryAnimRoutine != null)
        this.NumericBoundaries.StopCoroutine(this.NumericBoundaries.TopBoundaryAnimRoutine);
      this.NumericBoundaries.TopBoundaryAnimRoutine = this.NumericBoundaries.StartCoroutine(this.TopTransitionRoutine(this.TransitionDuration));
    }
    else if (!this.UseTopBoundary && this.NumericBoundaries.UseTopBoundary && this.UseBottomBoundary && (double) this.BottomBoundary > (double) this.NumericBoundaries.TargetTopBoundary)
    {
      this.NumericBoundaries.UseTopBoundary = true;
      this.UseTopBoundary = true;
      this.TopBoundary = this.BottomBoundary + this.ProCamera2D.ScreenSizeInWorldCoordinates.y * 100f;
      if (this.NumericBoundaries.TopBoundaryAnimRoutine != null)
        this.NumericBoundaries.StopCoroutine(this.NumericBoundaries.TopBoundaryAnimRoutine);
      this.NumericBoundaries.TopBoundaryAnimRoutine = this.NumericBoundaries.StartCoroutine(this.TopTransitionRoutine(this.TransitionDuration, true));
    }
    else if (!this.UseTopBoundary)
      this.NumericBoundaries.UseTopBoundary = false;
    if (this.UseBottomBoundary)
    {
      this.NumericBoundaries.UseBottomBoundary = true;
      if (this.NumericBoundaries.BottomBoundaryAnimRoutine != null)
        this.NumericBoundaries.StopCoroutine(this.NumericBoundaries.BottomBoundaryAnimRoutine);
      this.NumericBoundaries.BottomBoundaryAnimRoutine = this.NumericBoundaries.StartCoroutine(this.BottomTransitionRoutine(this.TransitionDuration));
    }
    else if (!this.UseBottomBoundary && this.NumericBoundaries.UseBottomBoundary && this.UseTopBoundary && (double) this.TopBoundary < (double) this.NumericBoundaries.TargetBottomBoundary)
    {
      this.NumericBoundaries.UseBottomBoundary = true;
      this.UseBottomBoundary = true;
      this.BottomBoundary = this.TopBoundary - this.ProCamera2D.ScreenSizeInWorldCoordinates.y * 100f;
      if (this.NumericBoundaries.BottomBoundaryAnimRoutine != null)
        this.NumericBoundaries.StopCoroutine(this.NumericBoundaries.BottomBoundaryAnimRoutine);
      this.NumericBoundaries.BottomBoundaryAnimRoutine = this.NumericBoundaries.StartCoroutine(this.BottomTransitionRoutine(this.TransitionDuration, true));
    }
    else
    {
      if (this.UseBottomBoundary)
        return;
      this.NumericBoundaries.UseBottomBoundary = false;
    }
  }

  public IEnumerator LeftTransitionRoutine(float duration, bool turnOffBoundaryAfterwards = false)
  {
    float initialLeftBoundary = this.Vector3H(this.ProCamera2D.LocalPosition) - this.ProCamera2D.ScreenSizeInWorldCoordinates.x / 2f;
    this.NumericBoundaries.TargetLeftBoundary = this.LeftBoundary;
    float t = 0.0f;
    while ((double) t <= 1.0)
    {
      t += this.ProCamera2D.DeltaTime / duration;
      if (this.UseLeftBoundary && this.UseRightBoundary && (double) this.LeftBoundary < (double) initialLeftBoundary)
        this.NumericBoundaries.LeftBoundary = this.LeftBoundary;
      else if (this.UseLeftBoundary)
      {
        this.NumericBoundaries.LeftBoundary = Utils.EaseFromTo(initialLeftBoundary, this.LeftBoundary, t, this.TransitionEaseType);
        float num = this.Vector3H(this.ProCamera2D.LocalPosition) - this.ProCamera2D.ScreenSizeInWorldCoordinates.x / 2f;
        if ((double) num < (double) this.NumericBoundaries.TargetLeftBoundary && (double) this.NumericBoundaries.LeftBoundary < (double) num)
          this.NumericBoundaries.LeftBoundary = num;
      }
      yield return (object) this.ProCamera2D.GetYield();
    }
    if (turnOffBoundaryAfterwards)
    {
      this.NumericBoundaries.UseLeftBoundary = false;
      this.UseLeftBoundary = false;
    }
    if (!this.NumericBoundaries.HasFiredTransitionFinished && this.OnTransitionFinished != null)
    {
      this.NumericBoundaries.HasFiredTransitionStarted = false;
      this.NumericBoundaries.HasFiredTransitionFinished = true;
      this.OnTransitionFinished();
    }
  }

  public IEnumerator RightTransitionRoutine(float duration, bool turnOffBoundaryAfterwards = false)
  {
    float initialRightBoundary = this.Vector3H(this.ProCamera2D.LocalPosition) + this.ProCamera2D.ScreenSizeInWorldCoordinates.x / 2f;
    this.NumericBoundaries.TargetRightBoundary = this.RightBoundary;
    float t = 0.0f;
    while ((double) t <= 1.0)
    {
      t += this.ProCamera2D.DeltaTime / duration;
      if (this.UseRightBoundary && this.UseLeftBoundary && (double) this.RightBoundary > (double) initialRightBoundary)
        this.NumericBoundaries.RightBoundary = this.RightBoundary;
      else if (this.UseRightBoundary)
      {
        this.NumericBoundaries.RightBoundary = Utils.EaseFromTo(initialRightBoundary, this.RightBoundary, t, this.TransitionEaseType);
        float num = this.Vector3H(this.ProCamera2D.LocalPosition) + this.ProCamera2D.ScreenSizeInWorldCoordinates.x / 2f;
        if ((double) num > (double) this.NumericBoundaries.TargetRightBoundary && (double) this.NumericBoundaries.RightBoundary > (double) num)
          this.NumericBoundaries.RightBoundary = num;
      }
      yield return (object) this.ProCamera2D.GetYield();
    }
    if (turnOffBoundaryAfterwards)
    {
      this.NumericBoundaries.UseRightBoundary = false;
      this.UseRightBoundary = false;
    }
    if (!this.NumericBoundaries.HasFiredTransitionFinished && this.OnTransitionFinished != null)
    {
      this.NumericBoundaries.HasFiredTransitionStarted = false;
      this.NumericBoundaries.HasFiredTransitionFinished = true;
      this.OnTransitionFinished();
    }
  }

  public IEnumerator TopTransitionRoutine(float duration, bool turnOffBoundaryAfterwards = false)
  {
    float initialTopBoundary = this.Vector3V(this.ProCamera2D.LocalPosition) + this.ProCamera2D.ScreenSizeInWorldCoordinates.y / 2f;
    this.NumericBoundaries.TargetTopBoundary = this.TopBoundary;
    float t = 0.0f;
    while ((double) t <= 1.0)
    {
      t += this.ProCamera2D.DeltaTime / duration;
      if (this.UseTopBoundary && this.UseBottomBoundary && (double) this.TopBoundary > (double) initialTopBoundary)
        this.NumericBoundaries.TopBoundary = this.TopBoundary;
      else if (this.UseTopBoundary)
      {
        this.NumericBoundaries.TopBoundary = Utils.EaseFromTo(initialTopBoundary, this.TopBoundary, t, this.TransitionEaseType);
        float num = this.Vector3V(this.ProCamera2D.LocalPosition) + this.ProCamera2D.ScreenSizeInWorldCoordinates.y / 2f;
        if ((double) num > (double) this.NumericBoundaries.TargetTopBoundary && (double) this.NumericBoundaries.TopBoundary > (double) num)
          this.NumericBoundaries.TopBoundary = num;
      }
      yield return (object) this.ProCamera2D.GetYield();
    }
    if (turnOffBoundaryAfterwards)
    {
      this.NumericBoundaries.UseTopBoundary = false;
      this.UseTopBoundary = false;
    }
    if (!this.NumericBoundaries.HasFiredTransitionFinished && this.OnTransitionFinished != null)
    {
      this.NumericBoundaries.HasFiredTransitionStarted = false;
      this.NumericBoundaries.HasFiredTransitionFinished = true;
      this.OnTransitionFinished();
    }
  }

  public IEnumerator BottomTransitionRoutine(float duration, bool turnOffBoundaryAfterwards = false)
  {
    float initialBottomBoundary = this.Vector3V(this.ProCamera2D.LocalPosition) - this.ProCamera2D.ScreenSizeInWorldCoordinates.y / 2f;
    this.NumericBoundaries.TargetBottomBoundary = this.BottomBoundary;
    float t = 0.0f;
    while ((double) t <= 1.0)
    {
      t += this.ProCamera2D.DeltaTime / duration;
      if (this.UseBottomBoundary && this.UseTopBoundary && (double) this.BottomBoundary < (double) initialBottomBoundary)
        this.NumericBoundaries.BottomBoundary = this.BottomBoundary;
      else if (this.UseBottomBoundary)
      {
        this.NumericBoundaries.BottomBoundary = Utils.EaseFromTo(initialBottomBoundary, this.BottomBoundary, t, this.TransitionEaseType);
        float num = this.Vector3V(this.ProCamera2D.LocalPosition) - this.ProCamera2D.ScreenSizeInWorldCoordinates.y / 2f;
        if ((double) num < (double) this.NumericBoundaries.TargetBottomBoundary && (double) this.NumericBoundaries.BottomBoundary < (double) num)
          this.NumericBoundaries.BottomBoundary = num;
      }
      yield return (object) this.ProCamera2D.GetYield();
    }
    if (turnOffBoundaryAfterwards)
    {
      this.NumericBoundaries.UseBottomBoundary = false;
      this.UseBottomBoundary = false;
    }
    if (!this.NumericBoundaries.HasFiredTransitionFinished && this.OnTransitionFinished != null)
    {
      this.NumericBoundaries.HasFiredTransitionStarted = false;
      this.NumericBoundaries.HasFiredTransitionFinished = true;
      this.OnTransitionFinished();
    }
  }
}
