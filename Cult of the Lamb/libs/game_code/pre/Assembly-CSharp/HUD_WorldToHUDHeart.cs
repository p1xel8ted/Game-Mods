// Decompiled with JetBrains decompiler
// Type: HUD_WorldToHUDHeart
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class HUD_WorldToHUDHeart : BaseMonoBehaviour
{
  public RectTransform rectTransform;
  public float MaxSpeed = 50f;
  public float Acceleration = 5f;
  public float AngleModifier = 0.2f;
  public float v1 = 0.7f;
  public float v2 = 0.4f;
  private System.Action Callback;

  private void OnEnable() => this.StartCoroutine((IEnumerator) this.MoveRoutine());

  private IEnumerator MoveRoutine()
  {
    HUD_WorldToHUDHeart hudWorldToHudHeart = this;
    float Delay = 0.0f;
    hudWorldToHudHeart.rectTransform.localScale = Vector3.zero;
    while ((double) (Delay += Time.deltaTime) < 0.5)
    {
      hudWorldToHudHeart.rectTransform.localScale = Vector3.Lerp(hudWorldToHudHeart.rectTransform.localScale, Vector3.one * 3f, Delay / 0.5f);
      yield return (object) null;
    }
    float Speed = -hudWorldToHudHeart.MaxSpeed;
    bool Looping = true;
    float Angle = 0.0f;
    while (Looping)
    {
      Angle = Utils.GetAngle(hudWorldToHudHeart.rectTransform.localPosition, Vector3.zero) * ((float) Math.PI / 180f);
      Speed += hudWorldToHudHeart.Acceleration;
      RectTransform rectTransform = hudWorldToHudHeart.rectTransform;
      rectTransform.localPosition = rectTransform.localPosition + new Vector3(Speed * Mathf.Cos(Angle), Speed * Mathf.Sin(Angle)) * Time.deltaTime;
      if ((double) Vector2.Distance((Vector2) hudWorldToHudHeart.rectTransform.localPosition, (Vector2) Vector3.zero) > (double) Speed * (double) Time.deltaTime)
        yield return (object) null;
      else
        Looping = false;
    }
    CameraManager.shakeCamera(0.1f, Angle);
    hudWorldToHudHeart.StartCoroutine((IEnumerator) hudWorldToHudHeart.ScaleRoutine());
  }

  private IEnumerator ScaleRoutine()
  {
    HUD_WorldToHUDHeart hudWorldToHudHeart = this;
    float ScaleSpeed = 0.1f;
    float Scale = hudWorldToHudHeart.rectTransform.localScale.x;
    bool Looping = true;
    while (Looping)
    {
      hudWorldToHudHeart.rectTransform.localPosition = Vector3.Lerp(hudWorldToHudHeart.rectTransform.localPosition, Vector3.zero, 15f * Time.deltaTime);
      ScaleSpeed -= 0.02f * GameManager.DeltaTime;
      Scale += ScaleSpeed;
      hudWorldToHudHeart.rectTransform.localScale = Vector3.one * Scale;
      if ((double) Scale < 0.800000011920929)
      {
        CameraManager.shakeCamera(0.2f, (float) UnityEngine.Random.Range(0, 360));
        Looping = false;
      }
      else
        yield return (object) null;
    }
    hudWorldToHudHeart.rectTransform.localPosition = Vector3.zero;
    if (hudWorldToHudHeart.Callback != null)
      hudWorldToHudHeart.Callback();
    UnityEngine.Object.Destroy((UnityEngine.Object) hudWorldToHudHeart.gameObject);
  }

  public static void Create(Vector3 HUDEndPosition, Vector3 WorldStartPosition, System.Action Callback)
  {
    GameObject withTag = GameObject.FindWithTag("Canvas");
    HUD_WorldToHUDHeart component = (UnityEngine.Object.Instantiate(Resources.Load("Prefabs/UI/Hearts/World To Hud Heart"), HUDEndPosition, Quaternion.identity, withTag.transform) as GameObject).GetComponent<HUD_WorldToHUDHeart>();
    component.Callback = Callback;
    component.rectTransform.position = Camera.main.WorldToScreenPoint(WorldStartPosition);
  }
}
