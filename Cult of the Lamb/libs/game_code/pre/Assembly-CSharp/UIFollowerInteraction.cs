// Decompiled with JetBrains decompiler
// Type: UIFollowerInteraction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private Canvas canvas;
  public System.Action CallbackClose;

  public void Show(FollowerBrain Brain, System.Action CallbackClose)
  {
    Time.timeScale = 0.0f;
    float num = (float) (((double) Brain.Stats.Satiation + (75.0 - (double) Brain.Stats.Starvation)) / 175.0);
    Debug.Log((object) ("SatiationTotal " + (object) num));
    this.FollowerName.text = $"<sprite name=\"img_SwirleyLeft\">{Brain.Info.Name}{(Brain.Info.XPLevel > 1 ? $"{ScriptLocalization.Interactions.Level} {Brain.Info.XPLevel.ToNumeral()}" : "")}<sprite name=\"img_SwirleyRight\">";
    this.FollowerStats.text = $"{((double) Brain.Stats.Reeducation > 0.0 ? (object) $"Disseter <b>{(object) Brain.Stats.Reeducation}%</b> \n\n" : (object) "")}Faith {((double) Brain.Stats.Happiness > 25.0 ? (object) "<color=green><b>" : (object) "<color=red><b>")}{(object) Brain.Stats.Happiness}%</color> - Hunger {((double) Brain.Stats.Satiation > 0.0 ? (object) "<color=green><b>" : (object) "<color=red><b>")}{(object) (float) ((double) Mathf.Floor(num * 100f) / 1.0)}%</color> - Tierdness {((double) Brain.Stats.Rest > 20.0 ? (object) "<color=green><b>" : (object) "<color=red><b>")}{(object) (float) ((double) Mathf.Floor(Brain.Stats.Rest * 1f) / 1.0)}%</color>";
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
    this.StartCoroutine((IEnumerator) this.FadeIn());
  }

  private IEnumerator FadeIn()
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
    this.StartCoroutine((IEnumerator) this.FadeOut());
  }

  private IEnumerator FadeOut()
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
