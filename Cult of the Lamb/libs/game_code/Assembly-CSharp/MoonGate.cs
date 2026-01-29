// Decompiled with JetBrains decompiler
// Type: MoonGate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
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
