// Decompiled with JetBrains decompiler
// Type: CameraWobble
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CameraWobble : MonoBehaviour
{
  public float wobbleAmount = 0.025f;
  public float speedMulti = 0.25f;
  public Vector3 initPos;
  public bool updateInitPos;

  public void Start() => this.initPos = this.transform.position;

  public void OnEnable() => this.initPos = this.transform.position;

  public void Update()
  {
    if (this.updateInitPos)
    {
      this.initPos = this.transform.position;
      this.updateInitPos = false;
    }
    this.transform.position = this.initPos + (Mathf.Sin(Time.time * 2f * this.speedMulti) * Vector3.right * this.wobbleAmount + Mathf.Sin(Time.time * 1.5f * this.speedMulti) * Vector3.up * this.wobbleAmount + Mathf.Sin(Time.time * 1.8f * this.speedMulti) * Vector3.forward * this.wobbleAmount);
  }
}
