// Decompiled with JetBrains decompiler
// Type: MoonGate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class MoonGate : BaseMonoBehaviour
{
  private void Start()
  {
    BaseTimer.Instance.OnBaseTimeComplete += new BaseTimer.BaseTimeComplete(this.OnBaseTimeComplete);
  }

  private void OnDisable()
  {
    BaseTimer.Instance.OnBaseTimeComplete -= new BaseTimer.BaseTimeComplete(this.OnBaseTimeComplete);
  }

  private void OnBaseTimeComplete()
  {
    Debug.Log((object) "END OF DAY!");
    Object.Destroy((Object) this.gameObject);
  }
}
