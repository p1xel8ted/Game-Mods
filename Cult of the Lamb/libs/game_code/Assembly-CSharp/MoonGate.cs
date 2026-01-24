// Decompiled with JetBrains decompiler
// Type: MoonGate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
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
