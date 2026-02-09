// Decompiled with JetBrains decompiler
// Type: TechBranchGUIItem
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class TechBranchGUIItem : MonoBehaviour
{
  public int _branch_id;
  public UILabel txt_name;
  public UI2DSprite icon;
  public UI2DSprite icon_2;
  public GameObject go_selected;
  public GameObject go_gamepad_over_full;
  public GameObject go_gamepad_over_folded;
  public GameObject go_not_selected;
  public int width_not_selected;
  public int width_selected;
  public bool _is_selected;
  public TechTreeGUI _tech_tree;

  public void Draw(int branch_id)
  {
    this._branch_id = branch_id;
    this.icon_2.sprite2D = this.icon.sprite2D = EasySpritesCollection.GetSprite("i_tbranch_" + branch_id.ToString());
    this.txt_name.text = GJL.L("tbranch_" + branch_id.ToString());
    this.GetComponent<Tooltip>().SetText(this.txt_name.text);
    this.Redraw();
    this._tech_tree = GUIElements.me.tech_tree;
    this.OnGamepadOut();
  }

  public void Redraw()
  {
    this._is_selected = MainGame.me.gui_elements.tech_tree.current_branch == this._branch_id;
    this.GetComponent<Tooltip>().available = !this._is_selected;
    this.go_selected.SetActive(this._is_selected);
    this.go_not_selected.SetActive(!this._is_selected);
    this.GetComponent<UIWidget>().width = this._is_selected ? this.width_selected : this.width_not_selected;
  }

  public void OnGamepadSelectedTechBranch()
  {
    this.SelectCurrentTechBranch();
    this.OnGamepadOver();
  }

  public void SelectCurrentTechBranch()
  {
    if (BaseGUI.IsLastClickRightButton())
      return;
    this._tech_tree.SelectTechBranch(this._branch_id);
    if (Sounds.WasAnySoundPlayedThisFrame())
      return;
    Sounds.OnGUIClick();
  }

  public void OnGamepadOver()
  {
    this._tech_tree.gamepad_controller.restore_last_in_group = false;
    if (this._tech_tree.current_branch == this._branch_id)
    {
      this.go_gamepad_over_full.SetActive(true);
      this.go_gamepad_over_folded.SetActive(false);
      this._tech_tree.button_tips.Print(GameKeyTip.Close());
    }
    else
    {
      this.go_gamepad_over_full.SetActive(false);
      this.go_gamepad_over_folded.SetActive(true);
      this._tech_tree.button_tips.Print(GameKeyTip.Select("open"), GameKeyTip.Close());
    }
    if (Sounds.WasAnySoundPlayedThisFrame())
      return;
    Sounds.OnGUIHover();
  }

  public void OnGamepadOut()
  {
    this.go_gamepad_over_full.SetActive(false);
    this.go_gamepad_over_folded.SetActive(false);
  }

  public void OnMouseOvered()
  {
    if (BaseGUI.for_gamepad || Sounds.WasAnySoundPlayedThisFrame())
      return;
    Sounds.OnGUIHover();
  }

  public void OnMouseOuted()
  {
    int num = BaseGUI.for_gamepad ? 1 : 0;
  }
}
