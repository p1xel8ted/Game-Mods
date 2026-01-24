// Decompiled with JetBrains decompiler
// Type: BaseTimer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine.UI;

#nullable disable
public class BaseTimer : BaseMonoBehaviour
{
  public float BaseTime = 5f;
  public float Timer;
  public float Progress;
  public static BaseTimer Instance;
  public Image image;

  public event BaseTimer.BaseTimeComplete OnBaseTimeComplete;

  public void OnEnable()
  {
    BaseTimer.Instance = this;
    this.Progress = 0.5f;
    this.Timer = this.BaseTime * this.Progress;
  }

  public void Update()
  {
  }

  public delegate void BaseTimeComplete();
}
