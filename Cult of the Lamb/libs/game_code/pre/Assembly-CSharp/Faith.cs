// Decompiled with JetBrains decompiler
// Type: Faith
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class Faith : BaseMonoBehaviour
{
  public Image image;
  public Worshipper worshipper;
  public Gradient gradient;
  public TextMeshProUGUI NameText;

  private void Update()
  {
    this.image.fillAmount = this.worshipper.wim.v_i.Faith / 100f;
    this.image.color = this.gradient.Evaluate(this.image.fillAmount);
    this.NameText.text = this.worshipper.wim.v_i.Name;
  }
}
