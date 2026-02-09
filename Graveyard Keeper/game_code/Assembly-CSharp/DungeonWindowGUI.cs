// Decompiled with JetBrains decompiler
// Type: DungeonWindowGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DungeonWindowGUI : BaseGUI
{
  public const int MAX_DUNGEON_LEVELS = 15;
  public const string DUNGEON_UNLOCKED = "dungeon_unlocked_";
  public static int[] NEED_TO_BE_UNLOCKED = new int[1]{ 11 };
  public DungeonLevelGUIItem prefab;
  public UIScrollView scroll;

  public override void Open()
  {
    base.Open();
    this.RemoveGUIItems();
    for (int t_level = 1; t_level <= 15; ++t_level)
      this.prefab.Copy<DungeonLevelGUIItem>(this.prefab.transform.parent).SetDungeonLevel(t_level);
    this.scroll.GetComponentInChildren<UIGrid>().Reposition();
    this.scroll.GetComponentInChildren<UIGrid>().repositionNow = true;
    this.scroll.RestrictWithinBounds(true);
    this.scroll.ResetPosition();
    if (!BaseGUI.for_gamepad)
      return;
    this.gamepad_controller.ReinitItems(true);
  }

  public override void OnClosePressed()
  {
    base.OnClosePressed();
    this.RemoveGUIItems();
  }

  public void RemoveGUIItems()
  {
    this.prefab.gameObject.SetActive(false);
    DungeonLevelGUIItem[] componentsInChildren = this.scroll.GetComponentsInChildren<DungeonLevelGUIItem>(false);
    if (componentsInChildren.Length != 0)
    {
      for (int index = 0; index < componentsInChildren.Length; ++index)
        NGUITools.Destroy((Object) componentsInChildren[index].gameObject);
    }
    this.scroll.ResetPosition();
  }

  public override bool OnPressedBack()
  {
    this.OnClosePressed();
    return true;
  }
}
