// Decompiled with JetBrains decompiler
// Type: NoisyCircleRippleController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
[RequireComponent(typeof (RawImage))]
public class NoisyCircleRippleController : MonoBehaviour
{
  [Header("Defaults")]
  public Vector2 DefaultCenterUV = new Vector2(0.5f, 0.5f);
  public Transform spawnPoint;
  public Material _mat;
  public float _t;
  public bool _active;
  public static int PID_Active = Shader.PropertyToID("_RippleActive");
  public static int PID_Time = Shader.PropertyToID("_RippleT");
  public static int PID_Center = Shader.PropertyToID("_Center");
  public static int PID_Speed = Shader.PropertyToID("_Speed");
  public static int PID_MaxRadius = Shader.PropertyToID("_MaxRadius");
  public static int PID_StartRadius = Shader.PropertyToID("_StartRadius");

  public float Speed => this._mat.GetFloat(NoisyCircleRippleController.PID_Speed);

  public float MaxRadius => this._mat.GetFloat(NoisyCircleRippleController.PID_MaxRadius);

  public float StartRadius => this._mat.GetFloat(NoisyCircleRippleController.PID_StartRadius);

  public void Awake()
  {
    RawImage component = this.GetComponent<RawImage>();
    this._mat = component.material;
    component.material = this._mat;
    this._mat.SetFloat(NoisyCircleRippleController.PID_Active, 0.0f);
    this._mat.SetVector(NoisyCircleRippleController.PID_Center, new Vector4(this.DefaultCenterUV.x, this.DefaultCenterUV.y, 0.0f, 0.0f));
    this._mat.SetFloat(NoisyCircleRippleController.PID_Time, 0.0f);
    this._t = 0.0f;
    this._active = false;
  }

  public void Update()
  {
    if (!this._active)
      return;
    this._t += Time.deltaTime;
    this._mat.SetFloat(NoisyCircleRippleController.PID_Time, this._t);
    if ((double) this.StartRadius + (double) this.Speed * (double) this._t < (double) this.MaxRadius)
      return;
    this._active = false;
    this._mat.SetFloat(NoisyCircleRippleController.PID_Active, 0.0f);
  }

  public void TriggerAtUV()
  {
    AudioManager.Instance.PlayOneShot("event:/dlc/env/yngya_shrine/ghost_fly", this.spawnPoint.transform.position);
    Vector3 screenPoint = Camera.main.WorldToScreenPoint(this.spawnPoint.transform.position);
    Vector2 vector2 = Vector2.ClampMagnitude(new Vector2(screenPoint.x / (float) Screen.width, screenPoint.y / (float) Screen.height), 1f);
    float num = 0.0f;
    this._mat.SetFloat(NoisyCircleRippleController.PID_StartRadius, num);
    this._mat.SetVector(NoisyCircleRippleController.PID_Center, new Vector4(vector2.x, vector2.y, 0.0f, 0.0f));
    this._t = 0.0f;
    this._mat.SetFloat(NoisyCircleRippleController.PID_Time, 0.0f);
    this._active = true;
    this._mat.SetFloat(NoisyCircleRippleController.PID_Active, 1f);
  }

  public void RippleBurst() => this.StartCoroutine((IEnumerator) this.SpawnRipples());

  public IEnumerator SpawnRipples()
  {
    this.TriggerAtUV();
    yield return (object) new WaitForSeconds(0.5f);
    this.TriggerAtUV();
    yield return (object) new WaitForSeconds(0.5f);
    this.TriggerAtUV();
  }
}
