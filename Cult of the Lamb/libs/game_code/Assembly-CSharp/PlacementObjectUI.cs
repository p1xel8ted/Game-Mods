// Decompiled with JetBrains decompiler
// Type: PlacementObjectUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
public class PlacementObjectUI : BaseMonoBehaviour
{
  [SerializeField]
  public Vector3 positionOffset;
  [SerializeField]
  public TMP_Text costText;
  public PlacementObject placementObject;
  public Camera mainCamera;
  public CanvasGroup canvasGroup;
  public bool hiding;

  public void Awake()
  {
    this.canvasGroup = this.GetComponent<CanvasGroup>();
    this.canvasGroup.alpha = 0.0f;
    this.mainCamera = Camera.main;
  }

  public void Play(PlacementObject placement, Structure structure)
  {
    this.placementObject = placement;
    this.gameObject.SetActive(true);
    this.UpdateText(structure.Type);
    this.UpdatePosition();
    this.canvasGroup.alpha = 1f;
  }

  public void Hide() => this.gameObject.SetActive(false);

  public void Update() => this.UpdatePosition();

  public void UpdatePosition()
  {
    if (!(bool) (Object) this.placementObject)
      return;
    this.transform.position = this.mainCamera.WorldToScreenPoint(this.placementObject.transform.position) + this.positionOffset;
  }

  public void UpdateText(StructureBrain.TYPES type)
  {
    this.costText.text = this.GetCostText(type);
    this.costText.isRightToLeftText = LocalizeIntegration.IsArabic();
  }

  public string GetCostText(StructureBrain.TYPES type)
  {
    return CostFormatter.FormatCosts(StructuresData.GetCost(type));
  }
}
