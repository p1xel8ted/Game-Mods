// Decompiled with JetBrains decompiler
// Type: UIDecorationDisplay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

#nullable disable
public class UIDecorationDisplay : BaseMonoBehaviour
{
  [SerializeField]
  public TMP_Text title;
  [SerializeField]
  public RectTransform container;
  [SerializeField]
  public TMP_Text descriptionText;
  public CanvasGroup canvasGroup;
  [Space]
  [SerializeField]
  public Vector3 offset;
  public RectTransform rectTransform;
  public GameObject lockPosition;
  [SerializeField]
  public Camera camera;
  public StructureBrain.TYPES structureType;
  public Canvas canvas;
  public RectTransform parentRect;
  public Camera uiCam;

  public void Play(StructureBrain.TYPES structureType, GameObject lockPos)
  {
    this.structureType = structureType;
    this.LocalizeText();
    this.camera = Camera.main;
    this.lockPosition = lockPos;
    this.rectTransform = this.transform as RectTransform;
    this.canvas = this.GetComponentInParent<Canvas>();
    this.parentRect = this.rectTransform.parent as RectTransform;
    this.uiCam = !((Object) this.canvas != (Object) null) || this.canvas.renderMode == RenderMode.ScreenSpaceOverlay ? (Camera) null : this.canvas.worldCamera;
    Vector3 offset = this.offset;
    this.offset += Vector3.up * 165f;
    DOTween.To((DOGetter<Vector3>) (() => this.offset), (DOSetter<Vector3>) (x => this.offset = x), offset, 0.3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    this.canvasGroup = this.GetComponent<CanvasGroup>();
    this.canvasGroup.alpha = 0.0f;
    DOTween.To((DOGetter<float>) (() => this.canvasGroup.alpha), (DOSetter<float>) (x => this.canvasGroup.alpha = x), 1f, 0.5f);
  }

  public void Shake(float progress, float normAmount)
  {
    this.rectTransform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.2f, normAmount);
    this.container.localPosition = (Vector3) (Random.insideUnitCircle * progress * 2f);
  }

  public void LateUpdate()
  {
    if ((Object) this.lockPosition == (Object) null || (Object) this.parentRect == (Object) null)
      return;
    Vector2 localPoint;
    RectTransformUtility.ScreenPointToLocalPointInRectangle(this.parentRect, (Vector2) Camera.main.WorldToScreenPoint(this.lockPosition.transform.position), this.uiCam, out localPoint);
    Vector2 vector2 = localPoint + new Vector2(this.offset.x, this.offset.y);
    Vector2 size = this.rectTransform.rect.size;
    Vector2 pivot = this.rectTransform.pivot;
    float min1 = this.parentRect.rect.xMin + size.x * pivot.x;
    float max1 = this.parentRect.rect.xMax - size.x * (1f - pivot.x);
    float min2 = this.parentRect.rect.yMin + size.y * pivot.y;
    float max2 = this.parentRect.rect.yMax - size.y * (1f - pivot.y);
    vector2.x = Mathf.Clamp(vector2.x, min1, max1);
    vector2.y = Mathf.Clamp(vector2.y, min2, max2);
    this.rectTransform.anchoredPosition = vector2;
  }

  public void OnEnable()
  {
    LocalizationManager.OnLocalizeEvent += new LocalizationManager.OnLocalizeCallback(this.LocalizeText);
  }

  public void OnDisable()
  {
    LocalizationManager.OnLocalizeEvent -= new LocalizationManager.OnLocalizeCallback(this.LocalizeText);
  }

  public void LocalizeText()
  {
    this.title.text = StructuresData.LocalizedName(this.structureType);
    this.descriptionText.text = StructuresData.LocalizedDescription(this.structureType);
  }

  [CompilerGenerated]
  public Vector3 \u003CPlay\u003Eb__12_0() => this.offset;

  [CompilerGenerated]
  public void \u003CPlay\u003Eb__12_1(Vector3 x) => this.offset = x;

  [CompilerGenerated]
  public float \u003CPlay\u003Eb__12_2() => this.canvasGroup.alpha;

  [CompilerGenerated]
  public void \u003CPlay\u003Eb__12_3(float x) => this.canvasGroup.alpha = x;
}
