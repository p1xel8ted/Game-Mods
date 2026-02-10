// Decompiled with JetBrains decompiler
// Type: CharacterSelectInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class CharacterSelectInfo : BaseMonoBehaviour
{
  public static CharacterSelectInfo Instance;
  public RectTransform Info;
  public Canvas canvas;
  public float TargetX = 600f;
  public TextMeshProUGUI text;
  public TextMeshProUGUI SubText;
  public Image HeadGraphic;
  public float Timer;

  public void Start() => CharacterSelectInfo.Instance = this;

  public void OnEnable()
  {
    this.TargetX = 600f;
    this.Info.localPosition = new Vector3(this.TargetX, 0.0f);
  }

  public void Show(bool show)
  {
    this.TargetX = show ? 0.0f : 600f;
    this.Timer = 0.5f;
  }

  public void SetInfo(string Name, string Subtitle, bool ShowGraphic)
  {
    if (this.text.text != Name.ToUpper())
    {
      this.text.text = Name.ToUpper();
      this.SubText.text = Subtitle;
      this.Info.localPosition = new Vector3(600f, 0.0f);
    }
    if (this.HeadGraphic.gameObject.activeSelf == ShowGraphic)
      return;
    this.HeadGraphic.gameObject.SetActive(ShowGraphic);
  }

  public void Update()
  {
    this.Info.localPosition = Vector3.Lerp(this.Info.localPosition, new Vector3(this.TargetX, 0.0f), 20f * Time.deltaTime);
    if ((double) this.TargetX != 0.0 || (double) (this.Timer -= Time.deltaTime) >= 0.0)
      return;
    this.TargetX = 600f;
  }
}
