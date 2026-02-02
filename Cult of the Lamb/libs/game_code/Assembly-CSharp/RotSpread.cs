// Decompiled with JetBrains decompiler
// Type: RotSpread
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

#nullable disable
public class RotSpread : MonoBehaviour
{
  [SerializeField]
  public float duration;
  [SerializeField]
  public GameObject targetPosiiton;
  [SerializeField]
  public SpriteRenderer[] targets;

  public void Awake()
  {
    foreach (Renderer target in this.targets)
      target.material.SetFloat("_RotReveal", 0.0f);
  }

  public void Play()
  {
    Shader.SetGlobalVector("_GlobalFirstLocationPos", (Vector4) this.targetPosiiton.transform.position);
    foreach (SpriteRenderer target in this.targets)
    {
      target.material.DOFloat(1f, "_RotReveal", this.duration).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutSine);
      target.material.SetVector("_LocalFirstLocationPos", (Vector4) this.targetPosiiton.transform.position);
    }
  }
}
