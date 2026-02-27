// Decompiled with JetBrains decompiler
// Type: ResurrectOnHud
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ResurrectOnHud : BaseMonoBehaviour
{
  public AnimationCurve curve = AnimationCurve.Linear(0.0f, 0.0f, 1f, 1f);
  public Image image;
  private static ResurrectOnHud Instance;

  public static bool HasRessurection => ResurrectOnHud.ResurrectionType != 0;

  public static ResurrectionType ResurrectionType
  {
    get => DataManager.Instance.ResurrectionType;
    set
    {
      bool hasRessurection = ResurrectOnHud.HasRessurection;
      DataManager.Instance.ResurrectionType = value;
      if (!((Object) ResurrectOnHud.Instance != (Object) null))
        return;
      if (hasRessurection && !ResurrectOnHud.HasRessurection)
      {
        ResurrectOnHud.Instance.Hide();
      }
      else
      {
        if (hasRessurection || !ResurrectOnHud.HasRessurection)
          return;
        ResurrectOnHud.Instance.Reveal();
      }
    }
  }

  private void OnEnable() => ResurrectOnHud.Instance = this;

  private void OnDisable()
  {
    if (!((Object) ResurrectOnHud.Instance == (Object) this))
      return;
    ResurrectOnHud.Instance = (ResurrectOnHud) null;
  }

  private void Start() => this.image.enabled = ResurrectOnHud.HasRessurection;

  private void Hide()
  {
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.HideRoutine());
  }

  private IEnumerator HideRoutine()
  {
    yield return (object) new WaitForSeconds(1f);
    float Progress = 0.0f;
    float Duration = 0.5f;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      float num = Mathf.SmoothStep(1f, 0.0f, Progress / Duration);
      this.image.rectTransform.localScale = new Vector3(num, num);
      yield return (object) null;
    }
    this.image.enabled = false;
  }

  private void Reveal() => this.StartCoroutine((IEnumerator) this.RevealRoutine());

  private IEnumerator RevealRoutine()
  {
    yield return (object) new WaitForSeconds(0.5f);
    this.image.enabled = true;
    float Progress = 0.0f;
    float Duration = 0.5f;
    float ScaleStart = 3f;
    float ScaleEnd = 1f;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      float num = Mathf.Lerp(ScaleStart, ScaleEnd, this.curve.Evaluate(Progress / Duration));
      this.image.rectTransform.localScale = new Vector3(num, num);
      yield return (object) null;
    }
  }
}
