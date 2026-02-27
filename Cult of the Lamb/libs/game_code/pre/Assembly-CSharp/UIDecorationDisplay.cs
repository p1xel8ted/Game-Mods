// Decompiled with JetBrains decompiler
// Type: UIDecorationDisplay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using TMPro;
using UnityEngine;

#nullable disable
public class UIDecorationDisplay : BaseMonoBehaviour
{
  [SerializeField]
  private TMP_Text title;
  [SerializeField]
  private RectTransform container;
  [SerializeField]
  private TMP_Text descriptionText;
  public CanvasGroup canvasGroup;
  [Space]
  [SerializeField]
  private Vector3 offset;
  private RectTransform rectTransform;
  private GameObject lockPosition;
  [SerializeField]
  private Camera camera;
  private StructureBrain.TYPES structureType;

  public void Play(StructureBrain.TYPES structureType, GameObject lockPos)
  {
    this.structureType = structureType;
    this.LocalizeText();
    this.camera = Camera.main;
    this.lockPosition = lockPos;
    this.rectTransform = this.transform as RectTransform;
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

  private void LateUpdate()
  {
    if ((Object) this.lockPosition == (Object) null)
      return;
    this.rectTransform.position = (Vector3) (Vector2) (this.camera.WorldToScreenPoint(this.lockPosition.transform.position) + this.offset);
  }

  private void OnEnable()
  {
    LocalizationManager.OnLocalizeEvent += new LocalizationManager.OnLocalizeCallback(this.LocalizeText);
  }

  private void OnDisable()
  {
    LocalizationManager.OnLocalizeEvent -= new LocalizationManager.OnLocalizeCallback(this.LocalizeText);
  }

  private void LocalizeText()
  {
    this.title.text = StructuresData.LocalizedName(this.structureType);
    this.descriptionText.text = StructuresData.LocalizedDescription(this.structureType);
  }
}
