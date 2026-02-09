// Decompiled with JetBrains decompiler
// Type: PixelPerfectGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class PixelPerfectGUI : MonoBehaviour
{
  public bool work_only_in_editor = true;
  public int pixel_size = 2;

  public void DoRound()
  {
    Vector3 localPosition = this.transform.localPosition;
    Vector3 vector3 = new Vector3(Mathf.Round(localPosition.x / (float) this.pixel_size) * (float) this.pixel_size, Mathf.Round(localPosition.y / (float) this.pixel_size) * (float) this.pixel_size, localPosition.z);
    if ((double) (vector3 - this.transform.localPosition).magnitude < 0.001)
      return;
    this.transform.localPosition = vector3;
    this.transform.localScale = Vector3.one;
  }

  public void Update()
  {
    if (Application.isPlaying)
      return;
    this.DoRound();
  }

  public void LateUpdate()
  {
    if (Application.isPlaying && this.work_only_in_editor)
      return;
    this.DoRound();
  }
}
