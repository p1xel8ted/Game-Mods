// Decompiled with JetBrains decompiler
// Type: MMTools.BuyEntry
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using Spine.Unity;
using System;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace MMTools;

[Serializable]
public class BuyEntry
{
  [TermsPopup("")]
  public string CharacterName = "-";
  [TermsPopup("")]
  public string TermToSpeak = "-";
  [TextArea(5, 10)]
  public string DisplayText = "";
  public string TermName;
  public string TermCategory = "";
  public string TermDescription;
  [TextArea(5, 10)]
  public string Text;
  public Vector3 Offset = (Vector3) Vector2.zero;
  public Color color;
  public GameObject Speaker;
  public SkeletonAnimation SkeletonData;
  [SpineAnimation("", "SkeletonData", true, false)]
  public string Animation;
  public AudioClip audioClip;
  public UnityEvent Callback;

  public BuyEntry(
    GameObject Speaker,
    string TermToSpeak = "-",
    string Animation = "talk",
    AudioClip audioClip = null,
    UnityEvent Callback = null)
  {
    this.Speaker = Speaker;
    this.TermToSpeak = TermToSpeak;
    this.Animation = Animation;
    this.audioClip = audioClip;
    this.Callback = Callback;
  }

  private string TermString
  {
    get
    {
      this.DisplayText = LocalizationManager.Sources.Count <= 0 || !(this.TermToSpeak != "-") || !(this.TermToSpeak != "") ? "" : LocalizationManager.Sources[0].GetTranslation(this.TermToSpeak);
      return "";
    }
  }

  private void AddTerm()
  {
    LanguageSourceData source = LocalizationManager.Sources[0];
    TermData termData = source.AddTerm($"Conversation_{this.TermCategory}/ {this.TermName}", eTermType.Text);
    termData.SetTranslation(0, this.Text);
    termData.Description = this.TermDescription;
    source.UpdateDictionary();
    this.TermToSpeak = termData.Term;
  }

  private void Shake() => this.Text += "<shake></shake>";

  private void Wiggle() => this.Text += "<wiggle></wiggle>";

  private void Bounce() => this.Text += "<bounce></bounce>";

  private void Rotation() => this.Text += "<rot></rot>";

  private void Swing() => this.Text += "<swing></swing>";

  private void Rainbow() => this.Text += "<rainb></rainb>";

  private void Speed() => this.Text += "<speed=0.2><speed=1>";

  private void Bold() => this.Text += "<b></b>";

  private void Italic() => this.Text += "<i></i>";

  private void Spirits() => this.Text += "<sprite name=\"icon_spirits\">";

  private void Meat() => this.Text += "<sprite name=\"icon_meat\">";

  private void Wood() => this.Text += "<sprite name=\"icon_wood\">";

  private void Stone() => this.Text += "<sprite name=\"icon_stone\">";

  private void Heart() => this.Text += "<sprite name=\"icon_heart\">";

  private void HalfHeart() => this.Text += "<sprite name=\"icon_heart_half\">";

  private void BlueHeart() => this.Text += "<sprite name=\"icon_blueheart\">";

  private void BlueHeartHalf() => this.Text += "<sprite name=\"icon_blueheart_half\">";

  private void TimeToken() => this.Text += "<sprite name=\"icon_timetoken\">";

  private void Flowers() => this.Text += "<sprite name=\"icon_flowers\">";

  private void StainedGlass() => this.Text += "<sprite name=\"icon_stainedglass\">";

  private void Bones() => this.Text += "<sprite name=\"icon_bones\">";

  private void BlackGold() => this.Text += "<sprite name=\"icon_blackgold\">";

  private void Brambles() => this.Text += "<sprite name=\"icon_brambles\">";

  private void SetColor()
  {
    Debug.Log((object) "COLOR!");
    this.Text = $"{this.Text}<#{ColorUtility.ToHtmlStringRGB(this.color)}></color>";
  }
}
