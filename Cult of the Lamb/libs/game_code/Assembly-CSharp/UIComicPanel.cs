// Decompiled with JetBrains decompiler
// Type: UIComicPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class UIComicPanel : MonoBehaviour
{
  [SerializeField]
  public List<UIComicPanel.ComicSegment> segments;
  [HideInInspector]
  public bool RequiresInput = true;
  [HideInInspector]
  public bool RequiresInputPost = true;
  [HideInInspector]
  public bool RequiresFade = true;
  [HideInInspector]
  public bool CombineTextures = true;
  [HideInInspector]
  public bool InvertPromptsColouring;
  [HideInInspector]
  public string Atmo;
  [HideInInspector]
  public string Music = "event:/music/comic/comic";
  [HideInInspector]
  public string MusicParamater;
  [HideInInspector]
  public int MusicParamaterValue;
  [HideInInspector]
  public string AltMusicParamater;
  [HideInInspector]
  public float AltMusicParamaterValue;
  public CanvasGroup canvasGroup;

  public List<UIComicPanel.ComicSegment> Segments => this.segments;

  public CanvasGroup CanvasGroup
  {
    get
    {
      if ((UnityEngine.Object) this.canvasGroup == (UnityEngine.Object) null)
      {
        this.canvasGroup = this.gameObject.AddComponent<CanvasGroup>();
        this.CanvasGroup.blocksRaycasts = false;
        foreach (UIComicPanel.ComicSegment segment in this.Segments)
        {
          if (segment.Segment.IsChoice)
            this.CanvasGroup.blocksRaycasts = true;
        }
      }
      return this.canvasGroup;
    }
  }

  public void Configure(bool animate)
  {
    if (!this.RequiresInput)
      this.RequiresFade = false;
    foreach (UIComicPanel.ComicSegment segment in this.segments)
    {
      DOTweenAnimation component = segment.Segment.GetComponent<DOTweenAnimation>();
      switch (segment.Segment.Transition)
      {
        case UIComicSegment.SegmentTransition.Fade:
          if ((UnityEngine.Object) segment.Segment.Dissolve != (UnityEngine.Object) null)
          {
            segment.Segment.Dissolve.material.SetFloat("_Dissolve", animate ? 1f : 0.0f);
            segment.Segment.Dissolve.gameObject.SetActive(false);
            segment.Segment.Image.color = new Color(segment.Segment.Image.color.r, segment.Segment.Image.color.g, segment.Segment.Image.color.b, animate ? 0.0f : 1f);
            break;
          }
          segment.Segment.Image.material.SetFloat("_Dissolve", animate ? 0.0f : 1f);
          break;
        case UIComicSegment.SegmentTransition.Punch:
          segment.Segment.GetComponent<CanvasGroup>().alpha = (double) component.delay > 0.0 ? 0.0f : 1f;
          break;
        case UIComicSegment.SegmentTransition.Scale:
          segment.Segment.transform.localScale = Vector3.zero;
          break;
        case UIComicSegment.SegmentTransition.Zoom:
          segment.Segment.transform.DOKill();
          segment.Segment.Image.rectTransform.sizeDelta = segment.Segment.Size * segment.Segment.Scale;
          segment.Segment.transform.DOScale(segment.Segment.ZoomScale, component.duration).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(segment.Segment.Delay).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(component.easeType);
          break;
      }
      segment.Segment.gameObject.AddComponent<UIComicTriggerSfx>().SetupSfx("event:/comic sfx/page_turn", 0.5f, segment.Segment);
    }
  }

  public void SetSegmentsMaterial(Material material)
  {
    foreach (UIComicPanel.ComicSegment segment in this.segments)
      segment.Segment.Image.material = material;
  }

  public void UpdateSegmentsMaterial(bool animate = true)
  {
    foreach (UIComicPanel.ComicSegment segment in this.segments)
      segment.Segment.UpdateMaterial(animate);
  }

  [Serializable]
  public class ComicSegment
  {
    public UIComicSegment Segment;
  }
}
