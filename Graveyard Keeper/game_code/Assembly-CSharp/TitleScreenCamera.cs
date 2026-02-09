// Decompiled with JetBrains decompiler
// Type: TitleScreenCamera
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class TitleScreenCamera : MonoBehaviour
{
  public Camera _cam;

  public void Update()
  {
    float b = (float) ((double) Screen.height / 96.0 / 2.0);
    if ((Object) this._cam == (Object) null)
      this._cam = this.GetComponent<Camera>();
    if (this._cam.orthographicSize.EqualsTo(b))
      return;
    this._cam.orthographicSize = b;
  }
}
