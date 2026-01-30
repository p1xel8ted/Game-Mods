// Decompiled with JetBrains decompiler
// Type: UIWeaponCardSoul
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class UIWeaponCardSoul : BaseMonoBehaviour
{
  public static List<UIWeaponCardSoul> uIWeaponCardSouls = new List<UIWeaponCardSoul>();
  public UIWeaponCardSoul.CardComplete OnCardComplete;
  public Canvas canvas;
  public RectTransform rectTransform;
  public float StartSpeed = 1000f;
  public float MaxSpeed = 2000f;
  public float Speed;
  public float Easing = 5f;
  public Vector3 NewPosition;
  public float Angle;
  public Vector3 TargetPosition;
  public Buttons CurrentButtons;
  public RectTransform Trail;
  public Vector3 PreviousPosition;
  public RectTransform TrailTarget;
  public float TrailScale = 1f;
  public float TrailAngle;
  public float ScaleModifier = 6f;
  public Coroutine cMoveRoutine;
  public float Scale = 1f;

  public void Play(Buttons CurrentButtons, Vector3 StartPosition, Vector3 TargetPosition)
  {
    this.canvas = this.GetComponentInParent<Canvas>();
    this.CurrentButtons = CurrentButtons;
    this.TargetPosition = TargetPosition;
    this.rectTransform = this.GetComponent<RectTransform>();
    this.rectTransform.position = StartPosition;
    this.PreviousPosition = this.rectTransform.position;
    this.OnPlay();
  }

  public void OnEnable() => UIWeaponCardSoul.uIWeaponCardSouls.Add(this);

  public void OnDisable() => UIWeaponCardSoul.uIWeaponCardSouls.Remove(this);

  public void OnPlay()
  {
    this.rectTransform = this.GetComponent<RectTransform>();
    this.Speed = -this.StartSpeed;
    this.Angle = (float) (((double) Utils.GetAngle(this.TargetPosition, this.rectTransform.position) + (double) UnityEngine.Random.Range(-180, 180)) * (Math.PI / 180.0));
    this.cMoveRoutine = this.StartCoroutine((IEnumerator) this.MoveRoutine());
  }

  public void Stop()
  {
    if (this.cMoveRoutine == null)
      return;
    this.StopCoroutine(this.cMoveRoutine);
  }

  public IEnumerator MoveRoutine()
  {
    UIWeaponCardSoul uiWeaponCardSoul = this;
    float TimeLimit = 0.0f;
    while ((double) Vector3.Distance(uiWeaponCardSoul.rectTransform.position, uiWeaponCardSoul.TargetPosition) > (double) uiWeaponCardSoul.MaxSpeed * 0.01666666753590107)
    {
      uiWeaponCardSoul.TrailAngle = Utils.GetAngle(uiWeaponCardSoul.Trail.position, uiWeaponCardSoul.PreviousPosition);
      uiWeaponCardSoul.Trail.eulerAngles = new Vector3(0.0f, 0.0f, uiWeaponCardSoul.TrailAngle);
      uiWeaponCardSoul.TrailScale = Vector3.Distance(uiWeaponCardSoul.Trail.position, uiWeaponCardSoul.PreviousPosition) / 40f;
      uiWeaponCardSoul.Trail.localScale = new Vector3(uiWeaponCardSoul.TrailScale * uiWeaponCardSoul.ScaleModifier, 1f, 1f);
      uiWeaponCardSoul.PreviousPosition = uiWeaponCardSoul.rectTransform.position;
      uiWeaponCardSoul.Scale = (float) ((double) uiWeaponCardSoul.Speed / (double) uiWeaponCardSoul.MaxSpeed * 0.5);
      uiWeaponCardSoul.rectTransform.localScale = new Vector3(uiWeaponCardSoul.Scale + 0.5f, uiWeaponCardSoul.Scale + 0.5f, 1f);
      if ((double) uiWeaponCardSoul.Speed < (double) uiWeaponCardSoul.MaxSpeed)
        uiWeaponCardSoul.Speed += (float) (100.0 * ((double) Time.deltaTime * 60.0));
      uiWeaponCardSoul.Angle = (double) (TimeLimit += Time.deltaTime) >= 0.699999988079071 ? Utils.GetAngle(uiWeaponCardSoul.rectTransform.position, uiWeaponCardSoul.TargetPosition) * ((float) Math.PI / 180f) : Utils.SmoothAngle(uiWeaponCardSoul.Angle, Utils.GetAngle(uiWeaponCardSoul.rectTransform.position, uiWeaponCardSoul.TargetPosition) * ((float) Math.PI / 180f), uiWeaponCardSoul.Easing);
      uiWeaponCardSoul.NewPosition = new Vector3(uiWeaponCardSoul.Speed * Mathf.Cos(uiWeaponCardSoul.Angle), uiWeaponCardSoul.Speed * Mathf.Sin(uiWeaponCardSoul.Angle)) * Time.deltaTime;
      RectTransform rectTransform = uiWeaponCardSoul.rectTransform;
      rectTransform.position = rectTransform.position + uiWeaponCardSoul.NewPosition;
      yield return (object) null;
    }
    UIWeaponCardSoul.CardComplete onCardComplete = uiWeaponCardSoul.OnCardComplete;
    if (onCardComplete != null)
      onCardComplete(uiWeaponCardSoul.CurrentButtons);
    UnityEngine.Object.Destroy((UnityEngine.Object) uiWeaponCardSoul.gameObject);
  }

  public delegate void CardComplete(Buttons CurrentButton);
}
