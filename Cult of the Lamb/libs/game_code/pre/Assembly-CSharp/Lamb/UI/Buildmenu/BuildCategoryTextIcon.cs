// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Buildmenu.BuildCategoryTextIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Lamb.UI.BuildMenu;
using TMPro;
using UnityEngine;

#nullable disable
namespace Lamb.UI.Buildmenu;

[ExecuteInEditMode]
public class BuildCategoryTextIcon : MonoBehaviour
{
  [SerializeField]
  private UIBuildMenuController.Category _category;
  [SerializeField]
  private TextMeshProUGUI _label;

  public UIBuildMenuController.Category Category => this._category;
}
