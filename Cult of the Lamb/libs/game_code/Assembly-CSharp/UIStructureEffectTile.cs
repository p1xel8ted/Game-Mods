// Decompiled with JetBrains decompiler
// Type: UIStructureEffectTile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class UIStructureEffectTile : 
  BaseMonoBehaviour,
  ISelectHandler,
  IEventSystemHandler,
  IDeselectHandler
{
  public StructureEffectManager.EffectType Type;
  public TextMeshProUGUI Text;
  public TextMeshProUGUI AvailabilityText;
  public Selectable Selectable;
  public RectTransform Container;
  public StructureEffectManager.State State;
  public Image CoolDownProgressIcon;
  public Canvas canvas;
  public Coroutine cShakeRoutine;

  public void Init(StructureEffectManager.EffectType Type, int ID)
  {
    this.canvas = this.GetComponentInParent<Canvas>();
    this.Type = Type;
    this.Text.text = Type.ToString();
    this.CoolDownProgressIcon.gameObject.SetActive(false);
    switch (this.State = StructureEffectManager.GetEffectAvailability(ID, Type))
    {
      case StructureEffectManager.State.DoesntExist:
        this.AvailabilityText.text = "Available";
        break;
      case StructureEffectManager.State.Active:
        this.AvailabilityText.text = "Currently Active";
        break;
      case StructureEffectManager.State.Cooldown:
        this.AvailabilityText.text = "Cooling Down...";
        this.CoolDownProgressIcon.gameObject.SetActive(true);
        this.CoolDownProgressIcon.fillAmount = 1f - StructureEffectManager.GetEffectCoolDownProgress(ID, Type);
        break;
    }
  }

  public void Shake()
  {
    if (this.cShakeRoutine != null)
      this.StopCoroutine(this.cShakeRoutine);
    this.cShakeRoutine = this.StartCoroutine((IEnumerator) this.ShakeRoutine());
  }

  public IEnumerator ShakeRoutine()
  {
    float Progress = 0.0f;
    float Duration = 2f;
    float Speed = 100f * this.canvas.scaleFactor;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      this.Container.localPosition = Vector3.right * Utils.BounceLerpUnscaledDeltaTime(0.0f, this.Container.localPosition.x, ref Speed);
      yield return (object) null;
    }
    this.Container.localPosition = Vector3.zero;
  }

  public void OnSelect(BaseEventData eventData)
  {
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.Selected(this.Container.localScale.x, 1.3f));
  }

  public void OnDeselect(BaseEventData eventData)
  {
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.DeSelected());
  }

  public IEnumerator Selected(float Starting, float Target)
  {
    float Progress = 0.0f;
    float Duration = 0.2f;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      this.Container.localScale = Vector3.one * Mathf.SmoothStep(Starting, Target, Progress / Duration);
      yield return (object) null;
    }
    this.Container.localScale = Vector3.one * Target;
  }

  public IEnumerator DeSelected()
  {
    float Progress = 0.0f;
    float Duration = 0.3f;
    float StartingScale = this.Container.localScale.x;
    float TargetScale = 1f;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      this.Container.localScale = Vector3.one * Mathf.SmoothStep(StartingScale, TargetScale, Progress / Duration);
      yield return (object) null;
    }
    this.Container.localScale = Vector3.one * TargetScale;
  }
}
