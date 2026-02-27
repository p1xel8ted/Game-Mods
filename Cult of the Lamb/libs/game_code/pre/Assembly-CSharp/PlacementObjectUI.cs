// Decompiled with JetBrains decompiler
// Type: PlacementObjectUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
public class PlacementObjectUI : BaseMonoBehaviour
{
  [SerializeField]
  private Vector3 positionOffset;
  [SerializeField]
  private TMP_Text costText;
  private PlacementObject placementObject;
  private Camera mainCamera;
  private CanvasGroup canvasGroup;
  private bool hiding;

  private void Awake()
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

  private void Update()
  {
    if (!(bool) (Object) this.placementObject)
      return;
    this.transform.position = this.mainCamera.WorldToScreenPoint(this.placementObject.transform.position) + this.positionOffset;
  }

  public void UpdateText(StructureBrain.TYPES type) => this.costText.text = this.GetCostText(type);

  private string GetCostText(StructureBrain.TYPES type)
  {
    return CostFormatter.FormatCosts(StructuresData.GetCost(type));
  }
}
