// Decompiled with JetBrains decompiler
// Type: SmartControllerParameter
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class SmartControllerParameter
{
  public SmartControllerParameter.Action action;
  public SmartControllerParameter.Condition condition;
  public SmartControllerParameter.SubActionParticle action_particle;
  public GameObject go;
  public MonoBehaviour cmp;
  public AbstractControllerComponent abs_cmp;
  public ParticleSystem particle_system;
  public float v1;
  public float v2;
  public float p1;
  public float p2;
  public float x1;
  public float x2;
  public float range_min;
  public float range_max;
  public AnimationCurve p_curve = new AnimationCurve();
  public Gradient gradient = new Gradient();
  public Material mat;
  public string s;
  public System.Action<float> action_delegate;

  public void Evaluate(float value)
  {
    switch (this.action)
    {
      case SmartControllerParameter.Action.None:
        break;
      case SmartControllerParameter.Action.GameObjectActivate:
        if ((UnityEngine.Object) this.go == (UnityEngine.Object) null)
          break;
        this.go.SetActive(this.EvaluateBoolean(value));
        break;
      case SmartControllerParameter.Action.GameObjectDeactivate:
        if ((UnityEngine.Object) this.go == (UnityEngine.Object) null)
          break;
        this.go.SetActive(!this.EvaluateBoolean(value));
        break;
      case SmartControllerParameter.Action.Action:
        this.action_delegate(this.p_curve.Evaluate(value));
        break;
      case SmartControllerParameter.Action.ComponentActivate:
        if ((UnityEngine.Object) this.go == (UnityEngine.Object) null)
          break;
        this.cmp.enabled = this.EvaluateBoolean(value);
        break;
      case SmartControllerParameter.Action.ComponentDeactivate:
        if ((UnityEngine.Object) this.cmp == (UnityEngine.Object) null)
          break;
        this.cmp.enabled = !this.EvaluateBoolean(value);
        break;
      case SmartControllerParameter.Action.AbstractControllerCmp:
        if ((UnityEngine.Object) this.abs_cmp == (UnityEngine.Object) null)
          break;
        this.abs_cmp.Set(this.p_curve.Evaluate(value));
        break;
      case SmartControllerParameter.Action.MaterialFloatParam:
        if ((UnityEngine.Object) this.mat == (UnityEngine.Object) null)
          break;
        this.mat.SetFloat(this.s, this.p_curve.Evaluate(value));
        break;
      case SmartControllerParameter.Action.ParticleSystem:
        if ((UnityEngine.Object) this.particle_system == (UnityEngine.Object) null)
          break;
        ParticleSystem.MainModule main = this.particle_system.main;
        ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime = this.particle_system.velocityOverLifetime;
        ParticleSystem.EmissionModule emission = this.particle_system.emission;
        switch (this.action_particle)
        {
          case SmartControllerParameter.SubActionParticle.None:
            return;
          case SmartControllerParameter.SubActionParticle.MaxParticles:
            main.maxParticles = (int) this.p_curve.Evaluate(value);
            return;
          case SmartControllerParameter.SubActionParticle.VelocityX:
            ParticleSystem.MinMaxCurve x = velocityOverLifetime.x;
            this.EvaluateAnimationCurve(value, ref x);
            velocityOverLifetime.x = x;
            return;
          case SmartControllerParameter.SubActionParticle.VelocityY:
            ParticleSystem.MinMaxCurve y = velocityOverLifetime.y;
            this.EvaluateAnimationCurve(value, ref y);
            velocityOverLifetime.y = y;
            return;
          case SmartControllerParameter.SubActionParticle.VelocityZ:
            ParticleSystem.MinMaxCurve z = velocityOverLifetime.z;
            this.EvaluateAnimationCurve(value, ref z);
            velocityOverLifetime.z = z;
            return;
          case SmartControllerParameter.SubActionParticle.LifetimeRange:
            ParticleSystem.MinMaxCurve startLifetime = main.startLifetime;
            this.EvaluateAnimationCurve(value, ref startLifetime);
            main.startLifetime = startLifetime;
            return;
          case SmartControllerParameter.SubActionParticle.EmissionRate:
            ParticleSystem.MinMaxCurve rateOverTime = emission.rateOverTime;
            this.EvaluateAnimationCurve(value, ref rateOverTime);
            emission.rateOverTime = rateOverTime;
            return;
          default:
            Debug.LogError((object) ("SubActionParticle is not supported: " + this.action_particle.ToString()));
            return;
        }
      default:
        Debug.LogError((object) ("Action is not supported: " + this.action.ToString()));
        break;
    }
  }

  public void EvaluateCurve(float value, ref ParticleSystem.MinMaxCurve curve, bool inverse = false)
  {
    if (inverse)
    {
      curve.constantMin = this.EvaluateFloatRange(value);
      curve.constantMax = this.EvaluateFloatRangeX(value);
    }
    else
    {
      curve.constantMin = this.EvaluateFloatRangeX(value);
      curve.constantMax = this.EvaluateFloatRange(value);
    }
  }

  public void EvaluateAnimationCurve(
    float value,
    ref ParticleSystem.MinMaxCurve curve,
    bool inverse = false)
  {
    float num = this.p_curve.Evaluate(value);
    curve.constant = num;
    float range = this.GetRange(value);
    if (inverse)
    {
      curve.constantMin = num - range;
      curve.constantMax = num + range;
    }
    else
    {
      curve.constantMin = num + range;
      curve.constantMax = num - range;
    }
  }

  public bool IsBooleanEvaluation()
  {
    switch (this.action)
    {
      case SmartControllerParameter.Action.GameObjectActivate:
      case SmartControllerParameter.Action.GameObjectDeactivate:
      case SmartControllerParameter.Action.ComponentActivate:
      case SmartControllerParameter.Action.ComponentDeactivate:
        return true;
      default:
        return false;
    }
  }

  public bool EvaluateBoolean(float value)
  {
    switch (this.condition)
    {
      case SmartControllerParameter.Condition.False:
        return false;
      case SmartControllerParameter.Condition.True:
        return true;
      case SmartControllerParameter.Condition.More:
        return (double) value > (double) this.v1;
      case SmartControllerParameter.Condition.Less:
        return (double) value < (double) this.v1;
      case SmartControllerParameter.Condition.InRange:
        return (double) value > (double) this.v1 && (double) value < (double) this.v2;
      default:
        throw new ArgumentOutOfRangeException();
    }
  }

  public float GetRange(float value)
  {
    return Mathf.Lerp(this.range_min, this.range_max, this.NormalizeValue(value));
  }

  public float NormalizeValue(float value)
  {
    return (float) (((double) value - (double) this.v1) / ((double) this.v2 - (double) this.v1));
  }

  public float EvaluateFloatRange(float value)
  {
    return Mathf.Lerp(this.p1, this.p2, this.NormalizeValue(value));
  }

  public float EvaluateFloatRangeX(float value)
  {
    return Mathf.Lerp(this.x1, this.x2, this.NormalizeValue(value));
  }

  public enum Action
  {
    None = 0,
    GameObjectActivate = 1,
    GameObjectDeactivate = 2,
    Action = 3,
    ComponentActivate = 4,
    ComponentDeactivate = 5,
    AbstractControllerCmp = 6,
    MaterialFloatParam = 90, // 0x0000005A
    ParticleSystem = 100, // 0x00000064
  }

  public enum SubActionParticle
  {
    None,
    MaxParticles,
    VelocityX,
    VelocityY,
    VelocityZ,
    LifetimeRange,
    EmissionRate,
  }

  public enum Condition
  {
    False,
    True,
    More,
    Less,
    InRange,
  }
}
