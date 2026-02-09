// Decompiled with JetBrains decompiler
// Type: BodyStorageGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

#nullable disable
public class BodyStorageGUI : BaseGUI
{
  public BodyPanelGUI[] body_panels;
  public UniversalObjectInfoGUI _universal_info;
  public Item[] _bodies = new Item[2];
  public WorldGameObject _wgo;

  public override void Init()
  {
    this._universal_info = this.GetComponentInChildren<UniversalObjectInfoGUI>(true);
    base.Init();
    this.body_panels[0].skull_bar.on_enable_skulls_frame += new System.Action(this.OnSkullsOver);
    this.body_panels[1].skull_bar.on_enable_skulls_frame += new System.Action(this.OnSkullsOver);
    this.body_panels[0].skull_bar.on_disable_skulls_frame += new System.Action(this.OnSkullsOut);
    this.body_panels[1].skull_bar.on_disable_skulls_frame += new System.Action(this.OnSkullsOut);
  }

  public void Open(WorldGameObject craft_obj)
  {
    this.Open();
    this._wgo = craft_obj;
    int insertItemsLimit = this._wgo.obj_def.can_insert_items_limit;
    this.body_panels[1].gameObject.SetActive(insertItemsLimit > 1);
    this.GetComponentInChildren<UITableOrGrid>(true).Reposition();
    this._bodies[0] = craft_obj.GetBodyFromInventory();
    this._bodies[1] = craft_obj.GetBodyFromInventory(false);
    if (this._bodies[1] == this._bodies[0])
      this._bodies[1] = (Item) null;
    for (int index = 0; index < insertItemsLimit; ++index)
    {
      bool has_body = (UnityEngine.Object) this.body_panels[index] != (UnityEngine.Object) null;
      int n = index;
      this.body_panels[index].Draw(this._bodies[index]);
      this.body_panels[index].button_item.SetCallbacks((GJCommons.VoidDelegate) (() =>
      {
        this.button_tips.Print(GameKeyTip.Select(has_body), GameKeyTip.Close());
        Sounds.OnGUIHover();
      }), (GJCommons.VoidDelegate) (() => { }), (GJCommons.VoidDelegate) (() => this.DropBody(n)));
    }
    this._universal_info.Draw(craft_obj.GetUniversalObjectInfo());
    if (!BaseGUI.for_gamepad)
      return;
    this.button_tips.Print(GameKeyTip.Close());
    this.gamepad_controller.ReinitItems(false);
    this.gamepad_controller.FocusOnFirstActive();
  }

  public void DropBody_1() => this.DropBody(0);

  public void DropBody_2() => this.DropBody(1);

  public void DropBody(int n)
  {
    if (this._bodies[n] == null)
      return;
    this._wgo.GiveItemToPlayersHands(this._bodies[n]);
    this.Hide(false);
  }

  public override bool OnPressedBack()
  {
    this.OnClosePressed();
    return true;
  }

  public void OnSkullsOver()
  {
    if (!BaseGUI.for_gamepad)
      return;
    this.button_tips.Print(GameKeyTip.Close(), GameKeyTip.RightStick(text: "move_tip"));
  }

  public void OnSkullsOut()
  {
    int num = BaseGUI.for_gamepad ? 1 : 0;
  }
}
