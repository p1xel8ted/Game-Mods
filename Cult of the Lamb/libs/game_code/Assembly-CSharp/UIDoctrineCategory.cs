// Decompiled with JetBrains decompiler
// Type: UIDoctrineCategory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
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
