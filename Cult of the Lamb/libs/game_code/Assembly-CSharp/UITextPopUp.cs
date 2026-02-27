// Decompiled with JetBrains decompiler
// Type: UITextPopUp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using TMPro;
using UnityEngine;

#nullable disable
public class UITextPopUp : BaseMonoBehaviour
{
  public TextMeshProUGUI Text;
  public RectTransform rectTransform;
  public CanvasGroup canvasGroup;
  public GameObject LockObject;
  public Vector3 Offset;
  public Canvas canvas;
  public bool Moving;
  public float Scale = 3f;
  public float ScaleSpeed;
  public float Speed = 50f;
  public float LifeDuration = 3f;
  public Vector3 DistanceTravelled;

  public static void Create(
    string String,
    Color Color,
    Vector3 Position,
    Vector3 Offset,
    GameObject LockObject)
  {
    Object.Instantiate<UITextPopUp>(UnityEngine.Resources.Load<UITextPopUp>("Prefabs/UI/UI Text PopUp"), GlobalCanvasReference.Instance).Play(String, Color, Position, Offset, LockObject);
  }

  public static void Create(string String, Color Color, GameObject LockObject, Vector3 Offset)
  {
    Object.Instantiate<UITextPopUp>(UnityEngine.Resources.Load<UITextPopUp>("Prefabs/UI/UI Text PopUp"), GlobalCanvasReference.Instance).Play(String, Color, Vector3.zero, Offset, LockObject);
  }

  public void Play(
    string String,
    Color Color,
    Vector3 Position,
    Vector3 Offset,
    GameObject LockObject)
  {
    this.Text.text = String;
    this.Text.color = Color;
    this.Offset = Offset;
    this.LockObject = LockObject;
    this.canvas = this.GetComponentInParent<Canvas>();
    this.rectTransform.position = Camera.main.WorldToScreenPoint(Position);
    this.StartCoroutine(this.Loop());
    this.StartCoroutine(this.Move());
  }

  public IEnumerator Loop()
  {
    UITextPopUp uiTextPopUp = this;
    float Progress = 0.0f;
    while ((double) (Progress += Time.deltaTime) < (double) uiTextPopUp.LifeDuration)
    {
      uiTextPopUp.canvasGroup.alpha = Mathf.Lerp(0.0f, 1f, Progress / 0.25f);
      uiTextPopUp.ScaleSpeed += (float) ((1.0 - (double) uiTextPopUp.Scale) * 0.40000000596046448);
      uiTextPopUp.Scale += (uiTextPopUp.ScaleSpeed *= 0.6f);
      uiTextPopUp.rectTransform.localScale = Vector3.one * uiTextPopUp.Scale;
      yield return (object) null;
    }
    uiTextPopUp.Moving = true;
    yield return (object) new WaitForSeconds(0.5f);
    Progress = 0.0f;
    float Duration = 0.3f;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      uiTextPopUp.canvasGroup.alpha = Mathf.Lerp(1f, 0.0f, Progress / Duration);
      yield return (object) null;
    }
    uiTextPopUp.StopAllCoroutines();
    Object.Destroy((Object) uiTextPopUp.gameObject);
  }

  public IEnumerator Move()
  {
    while (true)
    {
      if ((Object) this.LockObject != (Object) null)
      {
        this.rectTransform.position = Camera.main.WorldToScreenPoint(this.LockObject.transform.position + this.Offset);
        if (this.Moving)
        {
          this.DistanceTravelled += new Vector3(0.0f, this.Speed * Time.deltaTime * this.canvas.scaleFactor);
          RectTransform rectTransform = this.rectTransform;
          rectTransform.position = rectTransform.position + this.DistanceTravelled;
        }
      }
      else if (this.Moving)
      {
        RectTransform rectTransform = this.rectTransform;
        rectTransform.position = rectTransform.position + new Vector3(0.0f, this.Speed * Time.deltaTime * this.canvas.scaleFactor);
      }
      yield return (object) null;
    }
  }
}
