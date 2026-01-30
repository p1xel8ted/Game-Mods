// Decompiled with JetBrains decompiler
// Type: SimpleSetCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class SimpleSetCamera : BaseMonoBehaviour
{
  public bool AutomaticallyActivate = true;
  public Camera camera;
  public AnimationCurve animationCurve;
  public float Duration = 2f;
  public float ActivateDistance = 2f;
  public Vector3 ActivateOffset = Vector3.zero;
  public float DectivateDistance = 2f;
  public bool DisableInCoop;
  public static List<SimpleSetCamera> Cameras = new List<SimpleSetCamera>();
  public bool OnCam;
  [SerializeField]
  public string sfx;
  public GameObject Player;
  public UnityEvent OnComplete;
  public bool Active = true;

  public void OnEnable()
  {
    SimpleSetCamera.Cameras.Add(this);
    this.camera.enabled = false;
  }

  public void Update()
  {
    if (!this.AutomaticallyActivate || !this.Active || PlayerFarming.playersCount == 0)
      return;
    bool flag = true;
    float num = this.ActivateDistance;
    if (this.DisableInCoop)
    {
      Debug.Log((object) this.gameObject.transform.parent.name);
      if (PlayerFarming.playersCount > 1)
      {
        num = 0.0f;
        if (this.OnCam)
          this.Reset();
      }
    }
    for (int index = 0; index < PlayerFarming.playersCount; ++index)
    {
      if ((double) Vector3.Distance(PlayerFarming.players[index].transform.position, this.transform.position + this.ActivateOffset) > (double) num)
      {
        flag = false;
        break;
      }
    }
    if (!this.OnCam & flag)
    {
      this.OnCam = true;
      this.Play();
    }
    else
    {
      if (!this.OnCam || flag)
        return;
      this.OnCam = false;
      if (!((Object) GameManager.GetInstance().CamFollowTarget.TargetCamera == (Object) this.camera))
        return;
      this.Reset();
    }
  }

  public static void DisableAll(float cameraResetSpeedMultiplier = 1f)
  {
    Debug.Log((object) nameof (DisableAll));
    foreach (SimpleSetCamera camera in SimpleSetCamera.Cameras)
    {
      if (camera.OnCam)
      {
        camera.OnCam = false;
        camera.Reset(cameraResetSpeedMultiplier);
      }
      camera.Active = false;
    }
  }

  public void Disable()
  {
    this.Active = false;
    this.Reset();
  }

  public static void EnableAll()
  {
    Debug.Log((object) nameof (EnableAll));
    foreach (SimpleSetCamera camera in SimpleSetCamera.Cameras)
      camera.Active = true;
  }

  public void OnDisable()
  {
    SimpleSetCamera.Cameras.Remove(this);
    if (!this.OnCam)
      return;
    this.OnCam = false;
    if (!((Object) GameManager.GetInstance() != (Object) null) || !((Object) GameManager.GetInstance().CamFollowTarget.TargetCamera == (Object) this.camera))
      return;
    this.Reset();
  }

  public void Play()
  {
    this.OnCam = true;
    GameManager.GetInstance().CamFollowTarget.SetTargetCamera(this.camera, this.Duration, this.animationCurve);
    AudioManager.Instance.PlayOneShot(this.sfx, this.transform.position);
    if (this.OnComplete == null)
      return;
    this.StartCoroutine((IEnumerator) this.TriggerEndEvent());
  }

  public IEnumerator TriggerEndEvent()
  {
    yield return (object) new WaitForSeconds(this.Duration);
    this.OnComplete.Invoke();
  }

  public void Reset(float cameraResetSpeedMultiplier = 1f)
  {
    if ((double) cameraResetSpeedMultiplier <= 0.0)
      cameraResetSpeedMultiplier = 1f;
    this.OnCam = false;
    GameManager.GetInstance().CamFollowTarget.ResetTargetCamera(this.DectivateDistance / cameraResetSpeedMultiplier);
  }

  public void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position + this.ActivateOffset, this.ActivateDistance, Color.green);
  }
}
