// Decompiled with JetBrains decompiler
// Type: SimpleSetCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

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
  private static List<SimpleSetCamera> Cameras = new List<SimpleSetCamera>();
  private bool OnCam;
  private GameObject Player;
  public bool Active = true;

  private void OnEnable()
  {
    SimpleSetCamera.Cameras.Add(this);
    this.camera.enabled = false;
  }

  private void Update()
  {
    if (!this.AutomaticallyActivate || !this.Active || (Object) (this.Player = GameObject.FindWithTag("Player")) == (Object) null)
      return;
    if (!this.OnCam && (double) Vector3.Distance(this.Player.transform.position, this.transform.position + this.ActivateOffset) <= (double) this.ActivateDistance)
    {
      this.OnCam = true;
      this.Play();
    }
    if (!this.OnCam || (double) Vector3.Distance(this.Player.transform.position, this.transform.position + this.ActivateOffset) <= (double) this.ActivateDistance)
      return;
    this.OnCam = false;
    if (!((Object) GameManager.GetInstance().CamFollowTarget.TargetCamera == (Object) this.camera))
      return;
    this.Reset();
  }

  public static void DisableAll()
  {
    Debug.Log((object) nameof (DisableAll));
    foreach (SimpleSetCamera camera in SimpleSetCamera.Cameras)
    {
      if (camera.OnCam)
      {
        camera.OnCam = false;
        camera.Reset();
      }
      camera.Active = false;
    }
  }

  public static void EnableAll()
  {
    Debug.Log((object) nameof (EnableAll));
    foreach (SimpleSetCamera camera in SimpleSetCamera.Cameras)
      camera.Active = true;
  }

  private void OnDisable()
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
  }

  public void Reset()
  {
    this.OnCam = false;
    GameManager.GetInstance().CamFollowTarget.ResetTargetCamera(this.DectivateDistance);
  }

  private void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position + this.ActivateOffset, this.ActivateDistance, Color.green);
  }
}
