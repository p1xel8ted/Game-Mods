// Decompiled with JetBrains decompiler
// Type: PopulateTutorial
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using TMPro;
using UnityEngine;

#nullable disable
public class PopulateTutorial : MonoBehaviour
{
  public TutorialTopic Topic;
  public TextMeshProUGUI Title;
  public TextMeshProUGUI Description;
  public TextMeshProUGUI Info1;
  public TextMeshProUGUI Info2;
  public TextMeshProUGUI Info3;

  private void OnEnable() => this.Populate();

  private void Populate()
  {
    string Term = "Tutorial UI/" + this.Topic.ToString();
    this.Title.text = LocalizationManager.GetTranslation(Term);
    this.Description.text = LocalizationManager.GetTranslation(Term + "/Description");
    this.Info1.text = LocalizationManager.GetTranslation(Term + "/Info1");
    this.Info2.text = LocalizationManager.GetTranslation(Term + "/Info2");
    this.Info3.text = LocalizationManager.GetTranslation(Term + "/Info3");
  }
}
