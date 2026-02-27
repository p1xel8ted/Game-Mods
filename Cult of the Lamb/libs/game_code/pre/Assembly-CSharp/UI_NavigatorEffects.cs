// Decompiled with JetBrains decompiler
// Type: UI_NavigatorEffects
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private Vector3 targetOffset = Vector3.zero;
  private Vector3 newOffset = Vector3.zero;
  private Vector3 parOffset = Vector3.zero;
  private Vector3 curOffset = Vector3.zero;
  private float smoothTime = 0.3f;
  private Vector3 velocityOffset = Vector3.zero;
  private float curveDeltaTime;

  private void OnEnable()
  {
    MonoSingleton<UINavigatorNew>.Instance.OnSelectionChanged += new Action<Selectable, Selectable>(this.OnChangeSelection);
  }

  private void OnDisable()
  {
    if (!((UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance != (UnityEngine.Object) null))
      return;
    MonoSingleton<UINavigatorNew>.Instance.OnSelectionChanged -= new Action<Selectable, Selectable>(this.OnChangeSelection);
  }

  private void OnChangeSelection(Selectable newSelectable, Selectable previousSelectable)
  {
    this.newOffset = newSelectable.transform.position - previousSelectable.transform.position;
    this.targetOffset += this.newOffset / 10000f;
    this.parOffset = -(newSelectable.transform.position - previousSelectable.transform.position) / 100f;
  }

  private void Update()
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
