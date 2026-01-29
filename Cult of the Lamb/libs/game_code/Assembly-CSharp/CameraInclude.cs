// Decompiled with JetBrains decompiler
// Type: CameraInclude
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CameraInclude : BaseMonoBehaviour
{
  public Vector3 ActivateOffset = Vector3.zero;
  public float Distance = 4f;
  public float TargetZoom = -1f;
  public float Weight = 1f;
  public int MinZoom = -1;
  public int MaxZoom = -1;
  public bool DisableInCoop;
  public GameObject _Player;

  public GameObject Player
  {
    set => this._Player = value;
    get
    {
      if ((Object) this._Player == (Object) null)
        this._Player = GameObject.FindWithTag(nameof (Player));
      return this._Player;
    }
  }

  public bool OnCam
  {
    get
    {
      return !((Object) this.gameObject == (Object) null) && GameManager.GetInstance().CameraContains(this.gameObject);
    }
  }

  public void Update()
  {
    if (!((Object) this.Player != (Object) null))
      return;
    if (LetterBox.IsPlaying)
    {
      if (!this.OnCam || GameManager.GetInstance().CamFollowTarget.IsMoving)
        return;
      if ((double) this.TargetZoom != -1.0)
        GameManager.GetInstance().CameraResetTargetZoom();
      GameManager.GetInstance().RemoveFromCamera(this.gameObject);
    }
    else
    {
      float num = this.Distance;
      if (this.DisableInCoop && PlayerFarming.playersCount > 1)
        num = 0.0f;
      if (!this.OnCam && (double) Vector2.Distance((Vector2) this.Player.transform.position, (Vector2) (this.transform.position + this.ActivateOffset)) <= (double) num)
      {
        if ((double) this.TargetZoom != -1.0)
          GameManager.GetInstance().CameraSetTargetZoom(this.TargetZoom);
        GameManager.GetInstance().AddToCamera(this.gameObject, this.Weight);
        if (this.MinZoom != -1)
          GameManager.GetInstance().CamFollowTarget.MinZoom = (float) this.MinZoom;
        if (this.MaxZoom != -1)
          GameManager.GetInstance().CamFollowTarget.MaxZoom = (float) this.MaxZoom;
      }
      if (!this.OnCam || (double) Vector2.Distance((Vector2) this.Player.transform.position, (Vector2) (this.transform.position + this.ActivateOffset)) <= (double) num)
        return;
      if ((double) this.TargetZoom != -1.0)
        GameManager.GetInstance().CameraResetTargetZoom();
      GameManager.GetInstance().RemoveFromCamera(this.gameObject);
      if (this.MinZoom != -1)
        GameManager.GetInstance().CamFollowTarget.MinZoom = 11f;
      if (this.MaxZoom == -1)
        return;
      GameManager.GetInstance().CamFollowTarget.MaxZoom = 13f;
    }
  }

  public void OnDisable()
  {
    if (!(bool) (Object) GameManager.GetInstance())
      return;
    if (this.OnCam)
      GameManager.GetInstance().CameraResetTargetZoom();
    if (!(bool) (Object) this.gameObject)
      return;
    GameManager.GetInstance().RemoveFromCamera(this.gameObject);
  }

  public void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position + this.ActivateOffset, this.Distance, Color.green);
  }
}
