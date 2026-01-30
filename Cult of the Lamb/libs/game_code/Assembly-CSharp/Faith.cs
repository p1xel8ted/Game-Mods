// Decompiled with JetBrains decompiler
// Type: Faith
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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

  public void Update()
  {
    this.image.fillAmount = this.worshipper.wim.v_i.Faith / 100f;
    this.image.color = this.gradient.Evaluate(this.image.fillAmount);
    this.NameText.text = this.worshipper.wim.v_i.Name;
  }
}
