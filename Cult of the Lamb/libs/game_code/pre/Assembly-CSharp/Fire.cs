// Decompiled with JetBrains decompiler
// Type: Fire
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Fire : BaseMonoBehaviour
{
  public static List<Fire> Fires = new List<Fire>();
  private float Progress;
  private float WorkRequired = 3f;
  private float Scale = 1f;
  private float Delay;

  private void OnEnable() => Fire.Fires.Add(this);

  private void OnDisable() => Fire.Fires.Remove(this);

  private void Update()
  {
    if ((double) (this.Delay -= Time.deltaTime) >= 0.0 || (double) this.Progress <= 0.0)
      return;
    this.DoWork(-0.5f * Time.deltaTime);
  }

  public void DoWork(float WorkDone)
  {
    this.Progress += WorkDone;
    this.Scale = (float) (1.0 - (double) this.Progress / (double) this.WorkRequired);
    if ((double) WorkDone > 0.0)
      this.Delay = 1f;
    this.transform.localScale = new Vector3(this.Scale, this.Scale, this.Scale);
    if ((double) this.Progress <= 0.0)
      this.Progress = 0.0f;
    if ((double) this.Progress < (double) this.WorkRequired)
      return;
    this.GetComponent<Structure>().RemoveStructure();
    Object.Destroy((Object) this.gameObject);
  }
}
