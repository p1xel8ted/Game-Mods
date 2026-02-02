// Decompiled with JetBrains decompiler
// Type: MoonGate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class MoonGate : BaseMonoBehaviour
{
  public void Start()
  {
    BaseTimer.Instance.OnBaseTimeComplete += new BaseTimer.BaseTimeComplete(this.OnBaseTimeComplete);
  }

  public void OnDisable()
  {
    BaseTimer.Instance.OnBaseTimeComplete -= new BaseTimer.BaseTimeComplete(this.OnBaseTimeComplete);
  }

  public void OnBaseTimeComplete()
  {
    Debug.Log((object) "END OF DAY!");
    Object.Destroy((Object) this.gameObject);
  }
}
