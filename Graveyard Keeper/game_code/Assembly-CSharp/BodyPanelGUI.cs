// Decompiled with JetBrains decompiler
// Type: BodyPanelGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BodyPanelGUI : MonoBehaviour
{
  public UILabel txt_body;
  public UILabel txt_description;
  public UILabel white_skulls;
  public UILabel red_skulls;
  public GameObject skull_labels_back;
  public UI2DSprite spr_body;
  public BodyPanelSkullBarGUI skull_bar;
  public GameObject btn_remove_body;
  public GameObject no_body_go;
  public GamepadNavigationItem button_item;

  public void Draw(Item body)
  {
    this.spr_body.enabled = true;
    this.skull_bar.gameObject.SetActive(body != null);
    this.btn_remove_body.GetComponent<Collider2D>().enabled = body != null && !GlobalCraftControlGUI.is_global_control_active;
    if ((Object) this.txt_description != (Object) null)
      this.txt_description.text = "";
    foreach (UIButton componentsInChild in this.btn_remove_body.GetComponentsInChildren<UIButton>(true))
    {
      if (GlobalCraftControlGUI.is_global_control_active)
        componentsInChild.SetState(UIButtonColor.State.Disabled, true);
      else
        componentsInChild.SetState(body != null ? UIButtonColor.State.Normal : UIButtonColor.State.Disabled, true);
    }
    if (body == null)
    {
      if ((Object) this.skull_labels_back != (Object) null)
        this.skull_labels_back.gameObject.SetActive(false);
      if ((Object) this.txt_body != (Object) null)
      {
        this.txt_body.text = GJL.L("txt_no_body");
        this.txt_body.gameObject.SetActive(true);
      }
      if ((Object) this.spr_body != (Object) null)
        this.spr_body.sprite2D = EasySpritesCollection.GetSprite("i_body");
      this.skull_bar.NoBodyRedraw();
    }
    else
    {
      if (body.is_worker)
      {
        this.spr_body.sprite2D = EasySpritesCollection.GetSprite(body.worker.GetOnGroundItem().GetIcon());
        if ((Object) this.txt_body != (Object) null)
        {
          this.txt_body.text = body.worker.GetWorkerEfficiencyText();
          this.txt_body.gameObject.SetActive(!string.IsNullOrEmpty(this.txt_body.text));
        }
      }
      else
      {
        this.spr_body.sprite2D = EasySpritesCollection.GetSprite("i_body");
        this.txt_body.text = GJL.L(nameof (body));
        this.txt_body.gameObject.SetActive(true);
      }
      if (!string.IsNullOrEmpty(body.sub_name))
        this.txt_body.text = body.sub_name;
      body.GetBodySkulls(out this.skull_bar.negative, out this.skull_bar.positive, out int _);
      if ((Object) this.skull_labels_back != (Object) null)
      {
        this.skull_labels_back.gameObject.SetActive(true);
        this.red_skulls.text = this.skull_bar.negative.ToString();
        this.white_skulls.text = this.skull_bar.positive.ToString();
      }
      this.skull_bar.durability = body.durability;
      this.skull_bar.Redraw();
    }
  }

  public void DrawWorker(WorldGameObject worker, string no_worker_string = "txt_no_linked_worker")
  {
    if ((Object) worker != (Object) null && !worker.IsWorker())
      worker = (WorldGameObject) null;
    this.Draw((Object) worker == (Object) null ? (Item) null : worker.worker.GetOnGroundItem());
    if ((Object) worker == (Object) null)
      this.txt_body.text = "\n\n" + GJL.L(no_worker_string);
    if (!((Object) this.no_body_go != (Object) null))
      return;
    this.no_body_go.SetActive((Object) worker == (Object) null);
  }
}
