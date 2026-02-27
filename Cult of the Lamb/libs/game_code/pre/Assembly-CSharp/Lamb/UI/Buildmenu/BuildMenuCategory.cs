// Decompiled with JetBrains decompiler
// Type: Lamb.UI.BuildMenu.BuildMenuCategory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using src.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI.BuildMenu;

public abstract class BuildMenuCategory : UISubmenuBase
{
  public Action<StructureBrain.TYPES> OnBuildingChosen;
  [Header("Build Menu Category")]
  [SerializeField]
  protected MMScrollRect _scrollRect;
  [SerializeField]
  protected BuildMenuItem _buildMenuItemTemplate;
  protected List<BuildMenuItem> _buildItems = new List<BuildMenuItem>();

  public MMScrollRect ScrollRect => this._scrollRect;

  public List<BuildMenuItem> BuildItems => this._buildItems;

  protected override void OnShowStarted()
  {
    this._scrollRect.normalizedPosition = Vector2.one;
    if (this._buildItems.Count == 0)
    {
      this._scrollRect.enabled = false;
      this.Populate();
      this._scrollRect.enabled = true;
    }
    if (this._buildItems.Count <= 0)
      return;
    this.OverrideDefault((Selectable) this._buildItems[0].Button);
    this.ActivateNavigation();
  }

  protected abstract void Populate();

  protected void Populate(List<StructureBrain.TYPES> types, RectTransform contentContainer)
  {
    types.Sort((Comparison<StructureBrain.TYPES>) ((a, b) => StructuresData.GetUnlocked(b).CompareTo(StructuresData.GetUnlocked(a))));
    foreach (StructureBrain.TYPES type in types)
    {
      if (!StructuresData.HiddenUntilUnlocked(type) || StructuresData.GetUnlocked(type))
      {
        BuildMenuItem buildMenuItem = this._buildMenuItemTemplate.Instantiate<BuildMenuItem>((Transform) contentContainer);
        buildMenuItem.Configure(type);
        buildMenuItem.OnStructureSelected += new Action<StructureBrain.TYPES>(this.ChosenBuilding);
        this._buildItems.Add(buildMenuItem);
      }
    }
  }

  protected override IEnumerator DoShowAnimation()
  {
    BuildMenuCategory buildMenuCategory = this;
    if (buildMenuCategory._buildItems.Count == 0)
    {
      // ISSUE: reference to a compiler-generated method
      yield return (object) buildMenuCategory.\u003C\u003En__0();
    }
    buildMenuCategory._canvasGroup.alpha = 1f;
    yield return (object) null;
  }

  private void ChosenBuilding(StructureBrain.TYPES structure)
  {
    Action<StructureBrain.TYPES> onBuildingChosen = this.OnBuildingChosen;
    if (onBuildingChosen == null)
      return;
    onBuildingChosen(structure);
  }
}
