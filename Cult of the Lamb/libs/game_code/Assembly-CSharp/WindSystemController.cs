// Decompiled with JetBrains decompiler
// Type: WindSystemController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class WindSystemController : MonoBehaviour
{
  public ParticleSystem particleSystem;
  public ParticleSystem.VelocityOverLifetimeModule windVelocity;
  public AnimationCurve curveMinX = new AnimationCurve();
  public AnimationCurve curveMaxX = new AnimationCurve();
  public AnimationCurve curveMinY = new AnimationCurve();
  public AnimationCurve curveMaxY = new AnimationCurve();
  public float lastWindSpeed;
  public float lastWindDensity;
  public float windSpeed = 3f;
  public float scrollSpeed;
  public float windDensity = 0.1f;
  public float transitionRate = 3f;
  public Vector2 windDirection = new Vector2(1f, 0.2f);
  public Vector2 lastWindDirection = new Vector2(0.0f, 0.0f);
  public Vector2 windTimer = new Vector2(0.0f, 0.0f);

  public void Initialise(ParticleSystem particleSystem) => this.particleSystem = particleSystem;

  public void StartWind(WeatherSystemController.WeatherData weatherData)
  {
    foreach (WeatherSystemController.ShaderVariable shaderVariable in weatherData.ShaderVariables)
    {
      if (shaderVariable.ShaderKey == "_Wind_Density")
        this.windDensity = shaderVariable.TargetValue;
      else if (shaderVariable.ShaderKey == "_Wind_Speed")
      {
        this.windSpeed = shaderVariable.TargetValue;
        this.scrollSpeed = shaderVariable.TargetValue / 10f;
      }
    }
  }

  public void StopWind()
  {
    this.windDensity = 0.1f;
    this.windSpeed = 3f;
    this.scrollSpeed = 0.0f;
    Shader.SetGlobalVector("_WindTimer", (Vector4) this.windTimer);
    Shader.SetGlobalFloat("_WindDensity", this.lastWindDensity);
  }

  public void Update()
  {
    if (!(bool) (Object) this.particleSystem)
      return;
    this.UpdateWindSettings();
  }

  public void UpdateWindSettings()
  {
    ParticleSystem.EmissionModule emission = this.particleSystem.emission;
    ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime = this.particleSystem.velocityOverLifetime;
    this.curveMinX.keys = WindSystemController.Linear(0.0f, 1f, (float) ((double) this.lastWindDirection.x * (double) this.lastWindSpeed * 1.1000000238418579));
    this.curveMaxX.keys = WindSystemController.Linear(0.0f, 1f, (float) ((double) this.lastWindDirection.x * (double) this.lastWindSpeed * 0.89999997615814209));
    this.curveMinY.keys = WindSystemController.Linear(0.0f, 1f, (float) ((double) this.lastWindDirection.y * (double) this.lastWindSpeed * 1.1000000238418579));
    this.curveMaxY.keys = WindSystemController.Linear(0.0f, 1f, (float) ((double) this.lastWindDirection.y * (double) this.lastWindSpeed * 0.89999997615814209));
    ParticleSystem.MinMaxCurve minMaxCurve1 = new ParticleSystem.MinMaxCurve();
    minMaxCurve1.mode = ParticleSystemCurveMode.TwoCurves;
    minMaxCurve1.curveMin = this.curveMinX;
    minMaxCurve1.curveMax = this.curveMaxX;
    minMaxCurve1.curveMultiplier = 1f;
    ParticleSystem.MinMaxCurve minMaxCurve2 = new ParticleSystem.MinMaxCurve();
    minMaxCurve2.mode = ParticleSystemCurveMode.TwoCurves;
    minMaxCurve2.curveMin = this.curveMinY;
    minMaxCurve2.curveMax = this.curveMaxY;
    minMaxCurve2.curveMultiplier = 1f;
    velocityOverLifetime.x = minMaxCurve1;
    velocityOverLifetime.y = minMaxCurve2;
    this.windVelocity = velocityOverLifetime;
    if (!Mathf.Approximately(this.lastWindDirection.x, this.windDirection.x))
      this.lastWindDirection.x = Mathf.MoveTowards(this.lastWindDirection.x, this.windDirection.x, this.transitionRate * Time.deltaTime);
    if (!Mathf.Approximately(this.lastWindDirection.y, this.windDirection.y))
      this.lastWindDirection.y = Mathf.MoveTowards(this.lastWindDirection.y, this.windDirection.y, this.transitionRate * Time.deltaTime);
    if (!Mathf.Approximately(this.lastWindSpeed, this.windSpeed))
      this.lastWindSpeed = Mathf.MoveTowards(this.lastWindSpeed, this.windSpeed, this.transitionRate * Time.deltaTime);
    if (!Mathf.Approximately(this.lastWindDensity, this.windDensity))
      this.lastWindDensity = Mathf.MoveTowards(this.lastWindDensity, this.windDensity, this.transitionRate * Time.deltaTime);
    this.windTimer.x += Time.deltaTime * this.lastWindSpeed * this.lastWindDirection.x;
    this.windTimer.y += Time.deltaTime * this.lastWindSpeed * this.lastWindDirection.y;
    Shader.SetGlobalVector("_WindTimer", (Vector4) this.windTimer);
    Shader.SetGlobalFloat("_WindDensity", this.lastWindDensity);
    if (!((Object) ScreenSpaceOverlay.Instance != (Object) null))
      return;
    ScreenSpaceOverlay.Instance.SetWindSpeed(this.scrollSpeed);
  }

  public static Keyframe[] Linear(float timeStart, float timeEnd, float value)
  {
    return (double) timeStart == (double) timeEnd ? new Keyframe[1]
    {
      new Keyframe(timeStart, value)
    } : new Keyframe[2]
    {
      new Keyframe(timeStart, value, 0.0f, 0.0f),
      new Keyframe(timeEnd, value, 0.0f, 0.0f)
    };
  }
}
