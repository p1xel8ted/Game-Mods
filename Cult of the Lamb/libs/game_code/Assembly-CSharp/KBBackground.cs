// Decompiled with JetBrains decompiler
// Type: KBBackground
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class KBBackground : MonoBehaviour
{
  public const string kRectBRotate = "_RectBRotate";
  public const float kRectBRotateStartValue = 0.0f;
  public const float kRectBRotateEndValue = 0.5f;
  public const string kRectMaskCutoff = "_RectMaskCutoff";
  public const float kRectMaskCutoffStartValue = 0.541f;
  public const float kRectMaskCutoffEndValue = 0.7f;
  public const string kNoiseAInf = "_NoiseAInf";
  public const float kNoiseAInfStartValue = 0.893f;
  public const float kNoiseAInfEndValue = 0.2f;
  [SerializeField]
  public Image _background;
  public Material _backgroundMaterialInstance;

  public void Awake()
  {
    this._backgroundMaterialInstance = new Material(this._background.material);
    this._background.material = this._backgroundMaterialInstance;
  }

  public void OnDestroy()
  {
    if (!((Object) this._backgroundMaterialInstance != (Object) null))
      return;
    Object.Destroy((Object) this._backgroundMaterialInstance);
    this._backgroundMaterialInstance = (Material) null;
  }

  public IEnumerator TransitionToEndValues()
  {
    float progress = 0.0f;
    float duration = 1f;
    while ((double) progress < (double) duration)
    {
      progress = Mathf.Clamp(progress + Time.unscaledDeltaTime, 0.0f, duration);
      float t = progress / duration;
      this._backgroundMaterialInstance.SetFloat("_NoiseAInf", Mathf.SmoothStep(0.893f, 0.2f, t));
      this._backgroundMaterialInstance.SetFloat("_RectMaskCutoff", Mathf.SmoothStep(0.541f, 0.7f, t));
      this._backgroundMaterialInstance.SetFloat("_RectBRotate", Mathf.SmoothStep(0.0f, 0.5f, t));
      yield return (object) null;
    }
  }

  public IEnumerator TransitionToStartValues()
  {
    float progress = 0.0f;
    float duration = 1f;
    while ((double) progress < (double) duration)
    {
      progress = Mathf.Clamp(progress + Time.unscaledDeltaTime, 0.0f, duration);
      float t = progress / duration;
      this._backgroundMaterialInstance.SetFloat("_NoiseAInf", Mathf.SmoothStep(0.2f, 0.893f, t));
      this._backgroundMaterialInstance.SetFloat("_RectMaskCutoff", Mathf.SmoothStep(0.7f, 0.541f, t));
      this._backgroundMaterialInstance.SetFloat("_RectBRotate", Mathf.SmoothStep(0.5f, 0.0f, t));
      yield return (object) null;
    }
  }
}
