// Decompiled with JetBrains decompiler
// Type: Lamb.UI.BuildMenu.BuildMenuCategory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using src.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI.BuildMenu;

public abstract class BuildMenuCategory : UISubmenuBase
{
  public Action<StructureBrain.TYPES> OnBuildingChosen;
  [Header("Build Menu Category")]
  [SerializeField]
  public MMScrollRect _scrollRect;
  [SerializeField]
  public BuildMenuItem _buildMenuItemTemplate;
  public List<BuildMenuItem> _buildItems = new List<BuildMenuItem>();

  public MMScrollRect ScrollRect => this._scrollRect;

  public List<BuildMenuItem> BuildItems => this._buildItems;

  public override void OnShowStarted()
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

  public abstract void Populate();

  public void Populate(List<StructureBrain.TYPES> types, RectTransform contentContainer)
  {
    List<StructureBrain.TYPES> typesList1 = new List<StructureBrain.TYPES>();
    List<StructureBrain.TYPES> typesList2 = new List<StructureBrain.TYPES>();
    foreach (StructureBrain.TYPES type in types)
    {
      if (StructuresData.GetUnlocked(type))
        typesList1.Add(type);
      else if (!StructuresData.HiddenUntilUnlocked(type))
        typesList2.Add(type);
    }
    foreach (StructureBrain.TYPES structureType in typesList1)
    {
      BuildMenuItem buildMenuItem = this._buildMenuItemTemplate.Instantiate<BuildMenuItem>((Transform) contentContainer);
      buildMenuItem.Configure(structureType);
      buildMenuItem.OnStructureSelected += new Action<StructureBrain.TYPES>(this.ChosenBuilding);
      this._buildItems.Add(buildMenuItem);
    }
    foreach (StructureBrain.TYPES structureType in typesList2)
    {
      BuildMenuItem buildMenuItem = this._buildMenuItemTemplate.Instantiate<BuildMenuItem>((Transform) contentContainer);
      buildMenuItem.Configure(structureType);
      this._buildItems.Add(buildMenuItem);
    }
  }

  public override IEnumerator DoShowAnimation()
  {
    BuildMenuCategory buildMenuCategory = this;
    if (buildMenuCategory._buildItems.Count == 0)
      yield return (object) buildMenuCategory.\u003C\u003En__0();
    buildMenuCategory._canvasGroup.alpha = 1f;
    yield return (object) null;
  }

  public void ChosenBuilding(StructureBrain.TYPES structure)
  {
    Action<StructureBrain.TYPES> onBuildingChosen = this.OnBuildingChosen;
    if (onBuildingChosen == null)
      return;
    onBuildingChosen(structure);
  }

  [CompilerGenerated]
  [DebuggerHidden]
  public IEnumerator \u003C\u003En__0() => base.DoShowAnimation();
}
