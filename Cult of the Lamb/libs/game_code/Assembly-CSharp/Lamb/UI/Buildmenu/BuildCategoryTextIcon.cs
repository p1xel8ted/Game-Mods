// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Buildmenu.BuildCategoryTextIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
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
