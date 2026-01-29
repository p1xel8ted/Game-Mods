// Decompiled with JetBrains decompiler
// Type: UIDoctrineCategory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
public class UIDoctrineCategory : BaseMonoBehaviour
{
  public TextMeshProUGUI Title;
  public Transform Container;

  public void Play(SermonCategory Category)
  {
    this.Title.text = DoctrineUpgradeSystem.GetSermonCategoryLocalizedName(Category);
  }
}
