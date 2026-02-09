// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Buildmenu.BuildCategoryTextIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI.BuildMenu;
using TMPro;
using UnityEngine;

#nullable disable
namespace Lamb.UI.Buildmenu;

[ExecuteInEditMode]
public class BuildCategoryTextIcon : MonoBehaviour
{
  [SerializeField]
  public UIBuildMenuController.Category _category;
  [SerializeField]
  public TextMeshProUGUI _label;

  public UIBuildMenuController.Category Category => this._category;
}
