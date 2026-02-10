// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.CanvasScalerFitter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Rewired.Utils;
using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Rewired.UI.ControlMapper;

[RequireComponent(typeof (CanvasScalerExt))]
public class CanvasScalerFitter : MonoBehaviour
{
  [SerializeField]
  public CanvasScalerFitter.BreakPoint[] breakPoints;
  public CanvasScalerExt canvasScaler;
  public int screenWidth;
  public int screenHeight;
  public Action ScreenSizeChanged;

  public void OnEnable()
  {
    this.canvasScaler = this.GetComponent<CanvasScalerExt>();
    this.Update();
    this.canvasScaler.ForceRefresh();
  }

  public void Update()
  {
    if (Screen.width == this.screenWidth && Screen.height == this.screenHeight)
      return;
    this.screenWidth = Screen.width;
    this.screenHeight = Screen.height;
    this.UpdateSize();
  }

  public void UpdateSize()
  {
    if (this.canvasScaler.uiScaleMode != CanvasScaler.ScaleMode.ScaleWithScreenSize || this.breakPoints == null)
      return;
    float num1 = (float) Screen.width / (float) Screen.height;
    float num2 = float.PositiveInfinity;
    int index1 = 0;
    for (int index2 = 0; index2 < this.breakPoints.Length; ++index2)
    {
      float num3 = Mathf.Abs(num1 - this.breakPoints[index2].screenAspectRatio);
      if (((double) num3 <= (double) this.breakPoints[index2].screenAspectRatio || MathTools.IsNear(this.breakPoints[index2].screenAspectRatio, 0.01f)) && (double) num3 < (double) num2)
      {
        num2 = num3;
        index1 = index2;
      }
    }
    this.canvasScaler.referenceResolution = this.breakPoints[index1].referenceResolution;
  }

  [Serializable]
  public class BreakPoint
  {
    [SerializeField]
    public string name;
    [SerializeField]
    public float screenAspectRatio;
    [SerializeField]
    public Vector2 referenceResolution;
  }
}
