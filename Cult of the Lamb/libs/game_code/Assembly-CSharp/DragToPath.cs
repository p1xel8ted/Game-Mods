// Decompiled with JetBrains decompiler
// Type: DragToPath
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (FollowPath))]
[RequireComponent(typeof (CircleCollider2D))]
public class DragToPath : BaseMonoBehaviour
{
  public FollowPath followpath;

  public void Start() => this.followpath = this.GetComponent<FollowPath>();

  public void OnMouseDown() => MouseManager.ShowLine(this.transform);

  public void OnMouseUp()
  {
    MouseManager.HideLine();
    this.followpath.givePath(Camera.main.ScreenToWorldPoint(Input.mousePosition with
    {
      z = -Camera.main.transform.position.z
    }));
  }
}
