// Decompiled with JetBrains decompiler
// Type: PixelPerfect
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class PixelPerfect : MonoBehaviour
{
  public int pixel_k = 2;
  public bool only_in_editor = true;

  public void OnBecameVisible()
  {
    if (!Application.isPlaying || !this.only_in_editor)
      return;
    this.enabled = false;
  }

  public void OnDrawGizmosSelected()
  {
    if (Application.isPlaying)
      return;
    this.DoPixelPerfect();
  }

  public void Update()
  {
    if (Application.isPlaying && this.only_in_editor)
      return;
    this.DoPixelPerfect();
  }

  public void DoPixelPerfect(bool force = false)
  {
    Transform transform = this.transform;
    Vector3 localPosition = transform.localPosition;
    Vector3 vector3 = localPosition;
    int num = 96 /*0x60*/ / this.pixel_k;
    localPosition.x = Mathf.Round(localPosition.x * (float) num) / (float) num;
    localPosition.y = Mathf.Round(localPosition.y * (float) num) / (float) num;
    if ((double) Mathf.Abs(localPosition.x) < 1E-05)
      localPosition.x = 0.0f;
    if ((double) Mathf.Abs(localPosition.y) < 1E-05)
      localPosition.y = 0.0f;
    if (!force && (double) (vector3 - localPosition).sqrMagnitude < 0.0001)
      return;
    transform.localPosition = localPosition;
  }

  public void OnValidate() => this.DoPixelPerfect(true);
}
