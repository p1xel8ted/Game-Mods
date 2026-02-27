// Decompiled with JetBrains decompiler
// Type: LoadingIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class LoadingIcon : MonoBehaviour
{
  private float Scale;
  private float ScaleSpeed;
  private float Rotation;
  private float Rotation2;
  public Image Icon;
  public Image Icon2;
  public RectTransform rectTransform;
  public bool ForceFifty;
  private float Timer;
  private float Duration = 0.3f;

  public void UpdateProgress(float Progress)
  {
    this.Icon.fillAmount = Progress;
    this.Icon2.fillAmount = Progress;
    if (!this.ForceFifty)
      return;
    this.Icon.fillAmount = 0.5f;
    this.Icon2.fillAmount = 0.5f;
  }

  private void OnEnable()
  {
    this.Timer = 0.0f;
    this.Icon.fillAmount = this.Icon2.fillAmount = 0.0f;
  }

  private void Update()
  {
    this.Timer += Time.unscaledDeltaTime;
    this.Rotation -= 2f;
    this.Icon.rectTransform.eulerAngles = new Vector3(0.0f, 0.0f, this.Rotation);
    this.Rotation2 += 4f;
    this.Icon2.rectTransform.eulerAngles = new Vector3(0.0f, 0.0f, this.Rotation2);
  }
}
