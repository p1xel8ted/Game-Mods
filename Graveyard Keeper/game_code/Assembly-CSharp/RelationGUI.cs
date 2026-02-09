// Decompiled with JetBrains decompiler
// Type: RelationGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class RelationGUI : MonoBehaviour
{
  public UI2DSprite face;
  public UILabel label_name;
  public UILabel label_description;
  public UILabel label_relation;
  public Color color_neutral;
  public Color color_negative;
  public Color color_positive;
  public UIWidget back;
  public Transform anchor_zone_name;
  public Transform anchor_zone_info;
  public Transform anchor_no_zone;
  public WorldGameObject _npc;
  public int _current_relation;
  public int _hide_frame;
  public GameObject _zone_info;
  public GameObject _hud_obj;
  public UIRect _hud_rect;
  public UIRect _self_rect;
  public UIProgressBar progress_bar;
  public Transform bubble_pos_tf;
  public ObjectDefinition _npc_obj_def;
  public GameObject relation_bar_go;
  public string _npc_id = "";
  public HUDTasksGUI npc_tasks;
  public bool additional;
  public Transform additional_point;

  public string npc_id => this._npc_id;

  public void Init()
  {
    this.transform.parent.gameObject.SetActive(true);
    this.Hide();
    this._self_rect = this.GetComponent<UIRect>();
    this._zone_info = GUIElements.me.hud.zone_descr_object;
    this._hud_obj = GUIElements.me.hud.gameObject;
    this._hud_rect = this._hud_obj.GetComponent<UIRect>();
    this.npc_tasks.Init();
  }

  public void Open(WorldGameObject npc, bool animated = true)
  {
    this._npc_obj_def = string.IsNullOrEmpty(npc.obj_def.npc_alias) ? npc.obj_def : GameBalance.me.GetData<ObjectDefinition>(npc.obj_def.npc_alias);
    this._npc_id = this._npc_obj_def.id;
    bool flag = npc.IsWorker();
    if (!this._npc_obj_def.IsNPC() && !flag)
      return;
    if (this._npc_obj_def.npc_in_list && !flag)
      MainGame.me.save.OnMetNPC(this._npc_id);
    UnityEngine.Sprite headSprite = npc.GetHeadSprite();
    this.face.sprite2D = headSprite;
    this.face.transform.parent.gameObject.SetActive((Object) headSprite != (Object) null);
    this.relation_bar_go.SetActive(!flag);
    this.label_description.gameObject.SetActive(flag);
    if (flag)
    {
      this.label_description.text = npc.worker.GetWorkerEfficiencyText();
      int positive;
      npc.data.GetBodySkulls(out int _, out positive, out int _, true);
      this.label_name.text = "";
      for (int index = 0; index < positive; ++index)
        this.label_name.text += "(skull)";
    }
    else
      this.label_name.text = GJL.L(this._npc_id);
    GJL.L("desc_" + this._npc_id);
    this.back.height = 64 /*0x40*/;
    this._current_relation = int.MaxValue;
    this._npc = npc;
    this.gameObject.Activate();
    this.Update();
    this.npc_tasks.Draw(this._npc_id);
    GJL.EnsureChildLabelsHasCorrectFont(this.gameObject);
    GameObject gameObject = this.progress_bar.transform.parent.gameObject;
    if ((Object) gameObject != (Object) null)
      gameObject.SetActive(!flag);
    this.label_relation.SetActive(!flag);
  }

  public void Update()
  {
    if (this.additional)
    {
      if ((Object) this._zone_info == (Object) null || !this.gameObject.activeSelf)
        return;
      this.transform.position = (!GUIElements.me.relation.gameObject.activeSelf ? (!this._hud_obj.activeSelf || this._hud_rect.alpha.EqualsTo(0.0f) ? this.anchor_no_zone : (this._zone_info.activeSelf ? this.anchor_zone_info : this.anchor_zone_name)) : this.additional_point).position;
    }
    else
    {
      if ((Object) this._zone_info == (Object) null || !this.gameObject.activeSelf)
        return;
      this.transform.position = (!this._self_rect.alpha.EqualsTo(1f) || this._hud_obj.activeSelf && !this._hud_rect.alpha.EqualsTo(0.0f) ? (this._zone_info.activeSelf ? this.anchor_zone_info : this.anchor_zone_name) : this.anchor_no_zone).position;
      this.RedrawRelation();
    }
  }

  public void Hide()
  {
    this.gameObject.Deactivate();
    this._hide_frame = Time.frameCount;
    this._npc = (WorldGameObject) null;
    this._npc_obj_def = (ObjectDefinition) null;
    this.npc_tasks.Draw(string.Empty);
  }

  public void OnShownRelationBubble(WorldGameObject npc)
  {
    ObjectDefinition objectDefinition = string.IsNullOrEmpty(npc.obj_def.npc_alias) ? npc.obj_def : GameBalance.me.GetData<ObjectDefinition>(npc.obj_def.npc_alias);
    if (objectDefinition == this._npc_obj_def)
      this._current_relation = WorldGameObject.GetRelation(objectDefinition.id);
    this.RedrawRelation();
  }

  public void RedrawRelation()
  {
    int relation = WorldGameObject.GetRelation(this._npc_id);
    if (relation == this._current_relation)
      return;
    this.label_relation.text = relation.ToString();
    this.progress_bar.value = (float) relation / 100f;
    if (this._current_relation != int.MaxValue)
    {
      if ((double) this._npc.GetParam("it_is_a_copy") == 1.0)
        return;
      this._npc.ShowRelationChangeBubble(relation - this._current_relation);
    }
    this._current_relation = relation;
  }

  public void ChangeHUDAlpha(bool show, bool animated)
  {
    Debug.Log((object) ("ChangeHUDAlpha show=" + show.ToString()));
    this.gameObject.TryFinishAlphaTween();
    if (!animated)
      this._self_rect.alpha = show ? 1f : 0.0f;
    else
      this._self_rect.ChangeAlpha(this._self_rect.alpha, show ? 1f : 0.0f, 0.2f);
  }

  public ObjectDefinition GetCurrentInteractiveNPC()
  {
    return !((Object) this._npc == (Object) null) ? this._npc_obj_def : (ObjectDefinition) null;
  }
}
