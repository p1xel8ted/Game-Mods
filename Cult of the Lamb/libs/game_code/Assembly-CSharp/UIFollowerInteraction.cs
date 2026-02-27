// Decompiled with JetBrains decompiler
// Type: UIFollowerInteraction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System.Collections;
using TMPro;
using UnityEngine;

#nullable disable
public class UIFollowerInteraction : BaseMonoBehaviour
{
  public TextMeshProUGUI FollowerName;
  public TextMeshProUGUI FollowerRole;
  public TextMeshProUGUI FollowerStats;
  public TextMeshProUGUI FollowerTraits;
  public TextMeshProUGUI FollowerThoughts;
  public RectTransform Container;
  public Canvas canvas;
  public System.Action CallbackClose;

  public void Show(FollowerBrain Brain, System.Action CallbackClose)
  {
    Time.timeScale = 0.0f;
    float num1 = (float) (((double) Brain.Stats.Satiation + (75.0 - (double) Brain.Stats.Starvation)) / 175.0);
    Debug.Log((object) ("SatiationTotal " + num1.ToString()));
    string str1 = Brain.Info.XPLevel > 1 ? $"{ScriptLocalization.Interactions.Level} {Brain.Info.XPLevel.ToNumeral()}" : "";
    if (Brain.Info.IsDisciple)
      str1 = " <sprite name=\"icon_Disciple\">";
    this.FollowerName.text = $"<sprite name=\"img_SwirleyLeft\">{Brain.Info.Name}{str1}<sprite name=\"img_SwirleyRight\">";
    TextMeshProUGUI followerStats = this.FollowerStats;
    string[] strArray = new string[11];
    float num2;
    string str2;
    if ((double) Brain.Stats.Reeducation <= 0.0)
    {
      str2 = "";
    }
    else
    {
      num2 = Brain.Stats.Reeducation;
      str2 = $"Disseter <b>{num2.ToString()}%</b> \n\n";
    }
    strArray[0] = str2;
    strArray[1] = "Faith ";
    strArray[2] = (double) Brain.Stats.Happiness > 25.0 ? "<color=green><b>" : "<color=red><b>";
    num2 = Brain.Stats.Happiness;
    strArray[3] = num2.ToString();
    strArray[4] = "%</color> - Hunger ";
    strArray[5] = (double) Brain.Stats.Satiation > 0.0 ? "<color=green><b>" : "<color=red><b>";
    num2 = Mathf.Floor(num1 * 100f) / 1f;
    strArray[6] = num2.ToString();
    strArray[7] = "%</color> - Tierdness ";
    strArray[8] = (double) Brain.Stats.Rest > 20.0 ? "<color=green><b>" : "<color=red><b>";
    num2 = Mathf.Floor(Brain.Stats.Rest * 1f) / 1f;
    strArray[9] = num2.ToString();
    strArray[10] = "%</color>";
    string str3 = string.Concat(strArray);
    followerStats.text = str3;
    this.FollowerTraits.text = "";
    foreach (FollowerTrait.TraitType trait in Brain.Info.Traits)
    {
      TextMeshProUGUI followerTraits = this.FollowerTraits;
      followerTraits.text = $"{followerTraits.text}<b>{FollowerTrait.GetLocalizedTitle(trait)}</b>- <i>{FollowerTrait.GetLocalizedDescription(trait)}</i> \n";
    }
    this.FollowerRole.text = $"<b><color=yellow>{FollowerRoleInfo.GetLocalizedName(Brain.Info.FollowerRole)}</color></b>";
    this.FollowerThoughts.text = Brain.GetThoughtString(18f);
    this.CallbackClose = CallbackClose;
    this.canvas = this.GetComponentInParent<Canvas>();
    this.StopAllCoroutines();
    GameManager.GetInstance().CameraSetOffset(new Vector3(-1.4f, 0.0f, 0.0f));
    this.StartCoroutine(this.FadeIn());
  }

  public IEnumerator FadeIn()
  {
    float Progress = 0.0f;
    float Duration = 0.3f;
    Vector3 StartingPosition = new Vector3(-700f * this.canvas.scaleFactor, 0.0f);
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      this.Container.localPosition = Vector3.Lerp(StartingPosition, Vector3.zero, Mathf.SmoothStep(0.0f, 1f, Progress / Duration));
      yield return (object) null;
    }
    this.Container.localPosition = Vector3.zero;
    if (this.CallbackClose != null)
    {
      while (InputManager.UI.GetCancelButtonDown() || InputManager.UI.GetAcceptButtonDown())
        yield return (object) null;
      while (!InputManager.UI.GetCancelButtonDown() && !InputManager.UI.GetAcceptButtonDown())
        yield return (object) null;
      System.Action callbackClose = this.CallbackClose;
      if (callbackClose != null)
        callbackClose();
      this.Close();
    }
  }

  public void Close()
  {
    Time.timeScale = 1f;
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    this.StopAllCoroutines();
    this.StartCoroutine(this.FadeOut());
  }

  public IEnumerator FadeOut()
  {
    UIFollowerInteraction followerInteraction = this;
    float Progress = 0.0f;
    float Duration = 0.5f;
    Vector3 StartingPosition = followerInteraction.Container.localPosition;
    Vector3 TargetPosition = new Vector3(-700f * followerInteraction.canvas.scaleFactor, 0.0f);
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      followerInteraction.Container.localPosition = Vector3.Lerp(StartingPosition, TargetPosition, Mathf.SmoothStep(0.0f, 1f, Progress / Duration));
      yield return (object) null;
    }
    followerInteraction.Container.localPosition = Vector3.zero;
    UnityEngine.Object.Destroy((UnityEngine.Object) followerInteraction.gameObject);
  }
}
