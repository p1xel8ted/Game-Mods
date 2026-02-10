// Decompiled with JetBrains decompiler
// Type: BuildSitePlotProgressUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class BuildSitePlotProgressUI : BaseMonoBehaviour
{
  public static List<BuildSitePlotProgressUI> BuildSiteProgressUI = new List<BuildSitePlotProgressUI>();
  public float Scale;
  public float ScaleSpeed;
  public Canvas canvas;
  public Vector3 Offset;
  public float TargetScale = 0.5f;
  public CanvasGroup canvasGroup;
  public Camera camera;
  public Coroutine cFadeOutRoutine;
  public Image image;

  public void OnEnable()
  {
    this.canvas = this.GetComponentInParent<Canvas>();
    this.camera = Camera.main;
    BuildSitePlotProgressUI.BuildSiteProgressUI.Add(this);
  }

  public void OnDisable() => BuildSitePlotProgressUI.BuildSiteProgressUI.Remove(this);

  public void SetPosition(Vector3 Position)
  {
    this.transform.position = this.camera.WorldToScreenPoint(Position + this.Offset);
  }

  public void Show()
  {
    this.gameObject.SetActive(true);
    this.canvasGroup.alpha = 1f;
    this.transform.SetAsFirstSibling();
  }

  public void Hide()
  {
    if (!this.gameObject.activeSelf)
      return;
    if (this.cFadeOutRoutine != null)
      this.StopCoroutine(this.cFadeOutRoutine);
    this.StartCoroutine((IEnumerator) this.FadeOutRoutine());
  }

  public IEnumerator FadeOutRoutine()
  {
    BuildSitePlotProgressUI sitePlotProgressUi = this;
    float Progress = 0.0f;
    float Duration = 0.5f;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      sitePlotProgressUi.canvasGroup.alpha = (float) (1.0 - (double) Progress / (double) Duration);
      yield return (object) null;
    }
    sitePlotProgressUi.gameObject.SetActive(false);
  }

  public void LateUpdate() => this.transform.localScale = Vector3.one * this.Scale;

  public void FixedUpdate()
  {
    this.ScaleSpeed += (float) (((double) this.TargetScale - (double) this.Scale) * 0.30000001192092896);
    this.Scale += (this.ScaleSpeed *= 0.8f);
  }

  public void UpdateProgress(float Progress) => this.image.fillAmount = Progress;

  public static void HideAll()
  {
    for (int index = BuildSitePlotProgressUI.BuildSiteProgressUI.Count - 1; index >= 0; --index)
    {
      if ((Object) BuildSitePlotProgressUI.BuildSiteProgressUI[index] != (Object) null)
        BuildSitePlotProgressUI.BuildSiteProgressUI[index].gameObject.SetActive(false);
    }
  }
}
