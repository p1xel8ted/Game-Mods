// Decompiled with JetBrains decompiler
// Type: ZeroAlphaCuller
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[DisallowMultipleComponent]
public class ZeroAlphaCuller : BaseMonoBehaviour
{
  [Range(0.0f, 0.1f)]
  public float threshold = 1f / 1000f;
  [Tooltip("Check less often if many sprites change alpha. 0 = every frame.")]
  public int framesBetweenChecks;
  public SpriteRenderer spriteRenderer;
  public int frameMod;

  public void Awake() => this.spriteRenderer = this.GetComponent<SpriteRenderer>();

  public void LateUpdate()
  {
    if (!(bool) (Object) this.spriteRenderer || this.framesBetweenChecks > 0 && Time.frameCount % (this.framesBetweenChecks + 1) != this.frameMod)
      return;
    bool flag = (double) this.spriteRenderer.color.a > (double) this.threshold;
    if (this.spriteRenderer.enabled == flag)
      return;
    this.spriteRenderer.enabled = flag;
  }
}
