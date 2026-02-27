// Decompiled with JetBrains decompiler
// Type: StreakyLine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class StreakyLine : MonoBehaviour
{
  public Transform scaler;
  public Transform shifter;
  private float shiftLegnth = 2.5f;
  private float shiftSpeed = 25f;
  private float shiftPos;
  private float scaleLength = 0.5f;
  private float scaleSpeed = 80f;
  private float scalePos;

  private void ScalerAndShifter()
  {
    this.scaler = this.gameObject.transform.GetChild(0).transform;
    this.shifter = this.gameObject.transform.GetChild(0).GetChild(0).transform;
  }

  private void Start()
  {
    if ((Object) this.scaler == (Object) null)
      this.shiftPos = (float) Random.Range(0, 360);
    this.scalePos = (float) Random.Range(0, 360);
    this.shiftSpeed = (float) Random.Range(20, 50);
    this.scaleSpeed = (float) Random.Range(50, 80 /*0x50*/);
  }

  private void Update()
  {
    if ((double) this.shiftPos < 360.0)
      this.shiftPos += this.shiftSpeed * Time.deltaTime;
    else
      this.shiftPos -= 360f;
    if ((double) this.scalePos < 360.0)
      this.scalePos += this.scaleSpeed * Time.deltaTime;
    else
      this.scalePos -= 360f;
    this.shifter.localPosition = new Vector3((float) (1.0 + (double) Mathf.Cos(this.shiftPos) * (double) this.shiftLegnth), 0.0f, 0.0f);
    this.scaler.localScale = new Vector3(1f, (float) (1.0 + (double) Mathf.Sin(this.scalePos) * ((double) this.scaleLength * 2.0)), 1f);
  }
}
