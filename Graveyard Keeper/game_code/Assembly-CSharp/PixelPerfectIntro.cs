// Decompiled with JetBrains decompiler
// Type: PixelPerfectIntro
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class PixelPerfectIntro : MonoBehaviour
{
  public bool work_only_in_editor = true;
  public int pixel_size = 3;

  public void DoRound()
  {
    Vector3 localPosition = this.transform.localPosition;
    float num = (float) ((double) this.pixel_size / 48.0 / 3.0);
    Vector3 vector3 = new Vector3(Mathf.Round(localPosition.x / num) * num, Mathf.Round(localPosition.y / num) * num, localPosition.z);
    if ((double) (vector3 - this.transform.localPosition).magnitude < 0.001)
      return;
    this.transform.localPosition = vector3;
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
