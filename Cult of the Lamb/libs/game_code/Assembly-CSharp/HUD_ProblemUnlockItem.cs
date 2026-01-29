// Decompiled with JetBrains decompiler
// Type: HUD_ProblemUnlockItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using TMPro;
using UnityEngine.UI;

#nullable disable
public class HUD_ProblemUnlockItem : BaseMonoBehaviour
{
  public Image Icon;
  public Image IconBackground;
  public TextMeshProUGUI TypeText;
  public TextMeshProUGUI TitleText;
  public TextMeshProUGUI DescriptionText;
  public TextMeshProUGUI ProsText;
  public TextMeshProUGUI ConsText;

  public void Init(UnlockManager.UnlockNotificationData notification)
  {
    if (notification.SermonRitualType != SermonsAndRituals.SermonRitualType.NONE)
    {
      this.Icon.sprite = SermonsAndRituals.Sprite(notification.SermonRitualType);
      this.IconBackground.gameObject.SetActive(false);
      this.TypeText.text = "Ritual";
      this.TitleText.text = SermonsAndRituals.LocalisedName(notification.SermonRitualType);
      this.DescriptionText.text = SermonsAndRituals.LocalisedDescription(notification.SermonRitualType);
      this.ProsText.text = SermonsAndRituals.LocalisedPros(notification.SermonRitualType);
      this.ConsText.text = SermonsAndRituals.LocalisedCons(notification.SermonRitualType);
    }
    else
    {
      if (notification.StructureType == StructureBrain.TYPES.NONE)
        return;
      this.IconBackground.gameObject.SetActive(true);
      this.TypeText.text = "Building";
      this.TitleText.text = StructuresData.LocalizedName(notification.StructureType);
      this.DescriptionText.text = StructuresData.LocalizedDescription(notification.StructureType);
      this.ProsText.text = StructuresData.LocalizedPros(notification.StructureType);
      this.ConsText.text = StructuresData.LocalizedCons(notification.StructureType);
    }
  }
}
