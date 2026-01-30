// Decompiled with JetBrains decompiler
// Type: Fire
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Fire : BaseMonoBehaviour
{
  public static List<Fire> Fires = new List<Fire>();
  public float Progress;
  public float WorkRequired = 3f;
  public float Scale = 1f;
  public float Delay;

  public void OnEnable() => Fire.Fires.Add(this);

  public void OnDisable() => Fire.Fires.Remove(this);

  public void Update()
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
