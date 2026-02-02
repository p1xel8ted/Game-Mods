// Decompiled with JetBrains decompiler
// Type: LoadingIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class LoadingIcon : MonoBehaviour
{
  public float Scale;
  public float ScaleSpeed;
  public float Rotation;
  public float Rotation2;
  public Image Icon;
  public Image Icon2;
  public RectTransform rectTransform;
  public bool ForceFifty;
  public float Timer;
  public float Duration = 0.3f;

  public void UpdateProgress(float Progress)
  {
    this.Icon.fillAmount = Progress;
    this.Icon2.fillAmount = Progress;
    if (!this.ForceFifty)
      return;
    this.Icon.fillAmount = 0.5f;
    this.Icon2.fillAmount = 0.5f;
  }

  public void OnEnable()
  {
    this.Timer = 0.0f;
    this.Icon.fillAmount = this.Icon2.fillAmount = 0.0f;
  }

  public void Update()
  {
    this.Timer += Time.unscaledDeltaTime;
    this.Rotation -= 2f;
    this.Icon.rectTransform.eulerAngles = new Vector3(0.0f, 0.0f, this.Rotation);
    this.Rotation2 += 4f;
    this.Icon2.rectTransform.eulerAngles = new Vector3(0.0f, 0.0f, this.Rotation2);
  }
}
