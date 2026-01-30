// Decompiled with JetBrains decompiler
// Type: PlacementObjectUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
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
    this.mainCamera = Camera.main;
  }

  public void Play(PlacementObject placement, Structure structure)
  {
    this.placementObject = placement;
    this.gameObject.SetActive(true);
    this.UpdateText(structure.Type);
  }

  public void Hide() => this.gameObject.SetActive(false);

  public void Update()
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
