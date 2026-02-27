// Decompiled with JetBrains decompiler
// Type: CameraWobble
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CameraWobble : MonoBehaviour
{
  public float wobbleAmount = 0.025f;
  public float speedMulti = 0.25f;
  private Vector3 initPos;
  public bool updateInitPos;

  private void Start() => this.initPos = this.transform.position;

  private void OnEnable() => this.initPos = this.transform.position;

  private void Update()
  {
    if (this.updateInitPos)
    {
      this.initPos = this.transform.position;
      this.updateInitPos = false;
    }
    this.transform.position = this.initPos + (Mathf.Sin(Time.time * 2f * this.speedMulti) * Vector3.right * this.wobbleAmount + Mathf.Sin(Time.time * 1.5f * this.speedMulti) * Vector3.up * this.wobbleAmount + Mathf.Sin(Time.time * 1.8f * this.speedMulti) * Vector3.forward * this.wobbleAmount);
  }
}
