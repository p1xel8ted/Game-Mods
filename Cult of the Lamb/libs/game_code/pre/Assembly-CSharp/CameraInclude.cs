// Decompiled with JetBrains decompiler
// Type: CameraInclude
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private GameObject _Player;

  private GameObject Player
  {
    set => this._Player = value;
    get
    {
      if ((Object) this._Player == (Object) null)
        this._Player = GameObject.FindWithTag(nameof (Player));
      return this._Player;
    }
  }

  private bool OnCam => GameManager.GetInstance().CameraContains(this.gameObject);

  private void Update()
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
      if (!this.OnCam && (double) Vector2.Distance((Vector2) this.Player.transform.position, (Vector2) (this.transform.position + this.ActivateOffset)) <= (double) this.Distance)
      {
        if ((double) this.TargetZoom != -1.0)
          GameManager.GetInstance().CameraSetTargetZoom(this.TargetZoom);
        GameManager.GetInstance().AddToCamera(this.gameObject, this.Weight);
        if (this.MinZoom != -1)
          GameManager.GetInstance().CamFollowTarget.MinZoom = (float) this.MinZoom;
        if (this.MaxZoom != -1)
          GameManager.GetInstance().CamFollowTarget.MaxZoom = (float) this.MaxZoom;
      }
      if (!this.OnCam || (double) Vector2.Distance((Vector2) this.Player.transform.position, (Vector2) (this.transform.position + this.ActivateOffset)) <= (double) this.Distance)
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

  private void OnDisable()
  {
    if (!(bool) (Object) GameManager.GetInstance())
      return;
    if (this.OnCam)
      GameManager.GetInstance().CameraResetTargetZoom();
    GameManager.GetInstance().RemoveFromCamera(this.gameObject);
  }

  private void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position + this.ActivateOffset, this.Distance, Color.green);
  }
}
