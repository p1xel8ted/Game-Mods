// Decompiled with JetBrains decompiler
// Type: MoonGate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
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
