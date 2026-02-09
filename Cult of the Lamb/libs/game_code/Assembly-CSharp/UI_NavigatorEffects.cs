// Decompiled with JetBrains decompiler
// Type: UI_NavigatorEffects
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using src.UINavigator;
using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UI_NavigatorEffects : BaseMonoBehaviour
{
  public Image UIScreenGoop;
  public ParticleSystemForceField particleForce;
  public float forceMult = 0.1f;
  public float particleSpeed = 10f;
  public Vector3 targetOffset = Vector3.zero;
  public Vector3 newOffset = Vector3.zero;
  public Vector3 parOffset = Vector3.zero;
  public Vector3 curOffset = Vector3.zero;
  public float smoothTime = 0.3f;
  public Vector3 velocityOffset = Vector3.zero;
  public float curveDeltaTime;

  public void OnEnable()
  {
    MonoSingleton<UINavigatorNew>.Instance.OnSelectionChanged += new Action<Selectable, Selectable>(this.OnChangeSelection);
  }

  public void OnDisable()
  {
    if (!((UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance != (UnityEngine.Object) null))
      return;
    MonoSingleton<UINavigatorNew>.Instance.OnSelectionChanged -= new Action<Selectable, Selectable>(this.OnChangeSelection);
  }

  public void OnChangeSelection(Selectable newSelectable, Selectable previousSelectable)
  {
    this.newOffset = newSelectable.transform.position - previousSelectable.transform.position;
    this.targetOffset += this.newOffset / 10000f;
    this.parOffset = -(newSelectable.transform.position - previousSelectable.transform.position) / 100f;
  }

  public void Update()
  {
    this.curOffset = Vector3.SmoothDamp(this.curOffset, this.targetOffset, ref this.velocityOffset, this.smoothTime, 1f, Time.unscaledDeltaTime);
    this.UIScreenGoop.material.SetVector("_UINavOffset", (Vector4) this.curOffset);
    if (!((UnityEngine.Object) this.particleForce != (UnityEngine.Object) null))
      return;
    this.parOffset = Vector3.MoveTowards(this.parOffset, Vector3.zero, Time.unscaledDeltaTime * this.particleSpeed);
    this.particleForce.directionX = (ParticleSystem.MinMaxCurve) (this.parOffset.x * this.forceMult);
    this.particleForce.directionY = (ParticleSystem.MinMaxCurve) (this.parOffset.y * this.forceMult);
  }
}
