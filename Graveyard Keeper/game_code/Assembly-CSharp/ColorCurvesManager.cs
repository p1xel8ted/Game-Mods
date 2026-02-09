// Decompiled with JetBrains decompiler
// Type: ColorCurvesManager
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

#nullable disable
[AddComponentMenu("Image Effects/Color Adjustments/Dynamic Color Correction (Curves, Saturation)")]
[RequireComponent(typeof (ColorCorrectionCurves))]
[ExecuteInEditMode]
public class ColorCurvesManager : MonoBehaviour
{
  public float Factor;
  public float SaturationA = 1f;
  public AnimationCurve RedA = new AnimationCurve(new Keyframe[2]
  {
    new Keyframe(0.0f, 0.0f),
    new Keyframe(1f, 1f)
  });
  public AnimationCurve GreenA = new AnimationCurve(new Keyframe[2]
  {
    new Keyframe(0.0f, 0.0f),
    new Keyframe(1f, 1f)
  });
  public AnimationCurve BlueA = new AnimationCurve(new Keyframe[2]
  {
    new Keyframe(0.0f, 0.0f),
    new Keyframe(1f, 1f)
  });
  public AnimationCurve RedADepth = new AnimationCurve(new Keyframe[2]
  {
    new Keyframe(0.0f, 0.0f),
    new Keyframe(1f, 1f)
  });
  public AnimationCurve GreenADepth = new AnimationCurve(new Keyframe[2]
  {
    new Keyframe(0.0f, 0.0f),
    new Keyframe(1f, 1f)
  });
  public AnimationCurve BlueADepth = new AnimationCurve(new Keyframe[2]
  {
    new Keyframe(0.0f, 0.0f),
    new Keyframe(1f, 1f)
  });
  public AnimationCurve ZCurveA = new AnimationCurve(new Keyframe[2]
  {
    new Keyframe(0.0f, 0.0f),
    new Keyframe(1f, 1f)
  });
  public Color SelectiveFromColorA = Color.white;
  public Color SelectiveToColorA = Color.white;
  public float SaturationB = 1f;
  public AnimationCurve RedB = new AnimationCurve(new Keyframe[2]
  {
    new Keyframe(0.0f, 0.0f),
    new Keyframe(1f, 1f)
  });
  public AnimationCurve GreenB = new AnimationCurve(new Keyframe[2]
  {
    new Keyframe(0.0f, 0.0f),
    new Keyframe(1f, 1f)
  });
  public AnimationCurve BlueB = new AnimationCurve(new Keyframe[2]
  {
    new Keyframe(0.0f, 0.0f),
    new Keyframe(1f, 1f)
  });
  public AnimationCurve RedBDepth = new AnimationCurve(new Keyframe[2]
  {
    new Keyframe(0.0f, 0.0f),
    new Keyframe(1f, 1f)
  });
  public AnimationCurve GreenBDepth = new AnimationCurve(new Keyframe[2]
  {
    new Keyframe(0.0f, 0.0f),
    new Keyframe(1f, 1f)
  });
  public AnimationCurve BlueBDepth = new AnimationCurve(new Keyframe[2]
  {
    new Keyframe(0.0f, 0.0f),
    new Keyframe(1f, 1f)
  });
  public AnimationCurve ZCurveB = new AnimationCurve(new Keyframe[2]
  {
    new Keyframe(0.0f, 0.0f),
    new Keyframe(1f, 1f)
  });
  public Color SelectiveFromColorB = Color.white;
  public Color SelectiveToColorB = Color.white;
  public List<Keyframe[]> RedPairedKeyframes;
  public List<Keyframe[]> GreenPairedKeyframes;
  public List<Keyframe[]> BluePairedKeyframes;
  public List<Keyframe[]> DepthRedPairedKeyframes;
  public List<Keyframe[]> DepthGreenPairedKeyframes;
  public List<Keyframe[]> DepthBluePairedKeyframes;
  public List<Keyframe[]> ZCurvePairedKeyframes;
  public ColorCorrectionCurves CurvesScript;
  public const float PAIRING_DISTANCE = 0.01f;
  public const float TANGENT_DISTANCE = 0.0012f;
  public bool ChangesInEditor = true;
  public float LastFactor;
  public float LastSaturationA;
  public float LastSaturationB;

  public void SetFactor(float factor) => this.Factor = factor;

  public void SetSaturationA(float saturationA) => this.SaturationA = saturationA;

  public void SetSaturationB(float saturationB) => this.SaturationB = saturationB;

  public void Start()
  {
    this.LastFactor = this.Factor;
    this.LastSaturationA = this.SaturationA;
    this.LastSaturationB = this.SaturationB;
    this.CurvesScript = this.GetComponent<ColorCorrectionCurves>();
    this.PairCurvesKeyframes();
  }

  public void Update() => this.UpdateScript();

  public void UpdateScript()
  {
    if (!this.PairedListsInitiated())
      this.PairCurvesKeyframes();
    if (this.ChangesInEditor)
    {
      this.PairCurvesKeyframes();
      this.UpdateScriptParameters();
      this.CurvesScript.UpdateParameters();
      this.ChangesInEditor = false;
    }
    else
    {
      if ((double) this.Factor == (double) this.LastFactor && (double) this.SaturationA == (double) this.LastSaturationA && (double) this.SaturationB == (double) this.LastSaturationB)
        return;
      this.UpdateScriptParameters();
      this.CurvesScript.UpdateParameters();
      this.LastFactor = this.Factor;
      this.LastSaturationA = this.SaturationA;
      this.LastSaturationB = this.SaturationB;
    }
  }

  public void EditorHasChanged()
  {
    this.ChangesInEditor = true;
    this.UpdateScript();
  }

  public static List<Keyframe[]> PairKeyframes(AnimationCurve curveA, AnimationCurve curveB)
  {
    if (curveA.length == curveB.length)
      return ColorCurvesManager.SimplePairKeyframes(curveA, curveB);
    List<Keyframe[]> keyframeArrayList = new List<Keyframe[]>();
    List<Keyframe> keyframeList1 = new List<Keyframe>();
    List<Keyframe> keyframeList2 = new List<Keyframe>();
    keyframeList1.AddRange((IEnumerable<Keyframe>) curveA.keys);
    keyframeList2.AddRange((IEnumerable<Keyframe>) curveB.keys);
    int index1 = 0;
    while (index1 < keyframeList1.Count)
    {
      Keyframe aKeyframe = keyframeList1[index1];
      int index2 = keyframeList2.FindIndex((Predicate<Keyframe>) (bKeyframe => (double) Mathf.Abs(aKeyframe.time - bKeyframe.time) < 0.0099999997764825821));
      if (index2 >= 0)
      {
        Keyframe[] keyframeArray = new Keyframe[2]
        {
          keyframeList1[index1],
          keyframeList2[index2]
        };
        keyframeArrayList.Add(keyframeArray);
        keyframeList1.RemoveAt(index1);
        keyframeList2.RemoveAt(index2);
      }
      else
        ++index1;
    }
    foreach (Keyframe kf in keyframeList1)
    {
      Keyframe pair = ColorCurvesManager.CreatePair(kf, curveB);
      keyframeArrayList.Add(new Keyframe[2]{ kf, pair });
    }
    foreach (Keyframe kf in keyframeList2)
    {
      Keyframe pair = ColorCurvesManager.CreatePair(kf, curveA);
      keyframeArrayList.Add(new Keyframe[2]{ pair, kf });
    }
    return keyframeArrayList;
  }

  public static List<Keyframe[]> SimplePairKeyframes(AnimationCurve curveA, AnimationCurve curveB)
  {
    List<Keyframe[]> keyframeArrayList = new List<Keyframe[]>();
    if (curveA.length != curveB.length)
      throw new UnityException("Simple Pair cannot work with curves with different number of Keyframes.");
    for (int index = 0; index < curveA.length; ++index)
      keyframeArrayList.Add(new Keyframe[2]
      {
        curveA.keys[index],
        curveB.keys[index]
      });
    return keyframeArrayList;
  }

  public static Keyframe CreatePair(Keyframe kf, AnimationCurve curve)
  {
    Keyframe pair = new Keyframe();
    pair.time = kf.time;
    pair.value = curve.Evaluate(kf.time);
    if ((double) kf.time >= 0.0012000000569969416)
    {
      float time = kf.time - 0.0012f;
      pair.inTangent = (float) (((double) curve.Evaluate(time) - (double) curve.Evaluate(kf.time)) / ((double) time - (double) kf.time));
    }
    if ((double) kf.time + 0.0012000000569969416 <= 1.0)
    {
      float time = kf.time + 0.0012f;
      pair.outTangent = (float) (((double) curve.Evaluate(time) - (double) curve.Evaluate(kf.time)) / ((double) time - (double) kf.time));
    }
    return pair;
  }

  public static AnimationCurve CreateCurveFromKeyframes(
    IList<Keyframe[]> keyframePairs,
    float factor)
  {
    Keyframe[] keyframeArray = new Keyframe[keyframePairs.Count];
    for (int index = 0; index < keyframePairs.Count; ++index)
    {
      Keyframe[] keyframePair = keyframePairs[index];
      keyframeArray[index] = ColorCurvesManager.AverageKeyframe(keyframePair[0], keyframePair[1], factor);
    }
    return new AnimationCurve(keyframeArray);
  }

  public static Keyframe AverageKeyframe(Keyframe a, Keyframe b, float factor)
  {
    return new Keyframe()
    {
      time = (float) ((double) a.time * (1.0 - (double) factor) + (double) b.time * (double) factor),
      value = (float) ((double) a.value * (1.0 - (double) factor) + (double) b.value * (double) factor),
      inTangent = (float) ((double) a.inTangent * (1.0 - (double) factor) + (double) b.inTangent * (double) factor),
      outTangent = (float) ((double) a.outTangent * (1.0 - (double) factor) + (double) b.outTangent * (double) factor)
    };
  }

  public void PairCurvesKeyframes()
  {
    this.RedPairedKeyframes = ColorCurvesManager.PairKeyframes(this.RedA, this.RedB);
    this.GreenPairedKeyframes = ColorCurvesManager.PairKeyframes(this.GreenA, this.GreenB);
    this.BluePairedKeyframes = ColorCurvesManager.PairKeyframes(this.BlueA, this.BlueB);
    if (!this.ScriptAdvancedMode() && this.PairedListsInitiated())
      return;
    this.DepthRedPairedKeyframes = ColorCurvesManager.PairKeyframes(this.RedADepth, this.RedBDepth);
    this.DepthGreenPairedKeyframes = ColorCurvesManager.PairKeyframes(this.GreenADepth, this.GreenBDepth);
    this.DepthBluePairedKeyframes = ColorCurvesManager.PairKeyframes(this.BlueADepth, this.BlueBDepth);
    this.ZCurvePairedKeyframes = ColorCurvesManager.PairKeyframes(this.ZCurveA, this.ZCurveB);
  }

  public void UpdateScriptParameters()
  {
    this.Factor = Mathf.Clamp01(this.Factor);
    this.SaturationA = Mathf.Clamp(this.SaturationA, 0.0f, 5f);
    this.SaturationB = Mathf.Clamp(this.SaturationB, 0.0f, 5f);
    this.CurvesScript.saturation = Mathf.Lerp(this.SaturationA, this.SaturationB, this.Factor);
    this.CurvesScript.redChannel = ColorCurvesManager.CreateCurveFromKeyframes((IList<Keyframe[]>) this.RedPairedKeyframes, this.Factor);
    this.CurvesScript.greenChannel = ColorCurvesManager.CreateCurveFromKeyframes((IList<Keyframe[]>) this.GreenPairedKeyframes, this.Factor);
    this.CurvesScript.blueChannel = ColorCurvesManager.CreateCurveFromKeyframes((IList<Keyframe[]>) this.BluePairedKeyframes, this.Factor);
    if (this.ScriptAdvancedMode())
    {
      this.CurvesScript.depthRedChannel = ColorCurvesManager.CreateCurveFromKeyframes((IList<Keyframe[]>) this.DepthRedPairedKeyframes, this.Factor);
      this.CurvesScript.depthGreenChannel = ColorCurvesManager.CreateCurveFromKeyframes((IList<Keyframe[]>) this.DepthGreenPairedKeyframes, this.Factor);
      this.CurvesScript.depthBlueChannel = ColorCurvesManager.CreateCurveFromKeyframes((IList<Keyframe[]>) this.DepthBluePairedKeyframes, this.Factor);
      this.CurvesScript.zCurve = ColorCurvesManager.CreateCurveFromKeyframes((IList<Keyframe[]>) this.ZCurvePairedKeyframes, this.Factor);
    }
    if (!this.ScriptSelective())
      return;
    this.CurvesScript.selectiveFromColor = Color.Lerp(this.SelectiveFromColorA, this.SelectiveFromColorB, this.Factor);
    this.CurvesScript.selectiveToColor = Color.Lerp(this.SelectiveToColorA, this.SelectiveToColorB, this.Factor);
  }

  public bool PairedListsInitiated()
  {
    return this.RedPairedKeyframes != null && this.GreenPairedKeyframes != null && this.BluePairedKeyframes != null && this.DepthRedPairedKeyframes != null && this.DepthGreenPairedKeyframes != null && this.DepthBluePairedKeyframes != null;
  }

  public bool ScriptAdvancedMode()
  {
    return !((UnityEngine.Object) this.CurvesScript == (UnityEngine.Object) null) && this.CurvesScript.mode == ColorCorrectionCurves.ColorCorrectionMode.Advanced;
  }

  public bool ScriptSelective()
  {
    return !((UnityEngine.Object) this.CurvesScript == (UnityEngine.Object) null) && this.CurvesScript.selectiveCc;
  }
}
