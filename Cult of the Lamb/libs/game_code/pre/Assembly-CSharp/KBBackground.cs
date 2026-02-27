// Decompiled with JetBrains decompiler
// Type: KBBackground
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class KBBackground : MonoBehaviour
{
  private const string kRectBRotate = "_RectBRotate";
  private const float kRectBRotateStartValue = 0.0f;
  private const float kRectBRotateEndValue = 0.5f;
  private const string kRectMaskCutoff = "_RectMaskCutoff";
  private const float kRectMaskCutoffStartValue = 0.541f;
  private const float kRectMaskCutoffEndValue = 0.7f;
  private const string kNoiseAInf = "_NoiseAInf";
  private const float kNoiseAInfStartValue = 0.893f;
  private const float kNoiseAInfEndValue = 0.2f;
  [SerializeField]
  private Image _background;
  private Material _backgroundMaterialInstance;

  private void Awake()
  {
    this._backgroundMaterialInstance = new Material(this._background.material);
    this._background.material = this._backgroundMaterialInstance;
  }

  private void OnDestroy()
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
