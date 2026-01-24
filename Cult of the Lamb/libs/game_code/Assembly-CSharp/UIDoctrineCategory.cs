// Decompiled with JetBrains decompiler
// Type: UIDoctrineCategory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
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
