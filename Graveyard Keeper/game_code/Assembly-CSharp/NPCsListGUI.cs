// Decompiled with JetBrains decompiler
// Type: NPCsListGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class NPCsListGUI : BaseGameGUI
{
  public NPCItemGUI prefab;
  public UIScrollView scroll;
  public float gamepad_scroll_speed = 5f;

  public override void Init()
  {
    base.Init();
    this.prefab.gameObject.SetActive(false);
  }

  public override void Open()
  {
    base.Open();
    foreach (Component componentsInChild in this.GetComponentsInChildren<NPCItemGUI>())
      NGUITools.Destroy((Object) componentsInChild.gameObject);
    this.scroll.ResetPosition();
    MainGame.me.save.known_npcs.Sort();
    foreach (KnownNPC npc in MainGame.me.save.known_npcs.npcs)
    {
      if (npc.npc_id != "player")
      {
        ObjectDefinition dataOrNull = GameBalance.me.GetDataOrNull<ObjectDefinition>(npc.npc_id);
        if (dataOrNull == null || !dataOrNull.IsRelationVisible() || npc.npc_id.StartsWith("worker_zombie"))
          continue;
      }
      this.prefab.Copy<NPCItemGUI>().Draw(npc);
    }
    this.GetComponentInChildren<UIGrid>().Reposition();
    this.GetComponentInChildren<UIGrid>().repositionNow = true;
    this.scroll.RestrictWithinBounds(true);
    this.scroll.ResetPosition();
  }

  public override bool OnPressedBack()
  {
    GUIElements.me.game_gui.Hide(true);
    return true;
  }

  public override bool OnPressedDown()
  {
    this.scroll.Scroll(this.gamepad_scroll_speed);
    return true;
  }

  public override bool OnPressedUp()
  {
    this.scroll.Scroll(-this.gamepad_scroll_speed);
    return true;
  }
}
