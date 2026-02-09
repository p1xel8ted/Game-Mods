// Decompiled with JetBrains decompiler
// Type: NPCItemGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class NPCItemGUI : MonoBehaviour
{
  public NPCListQuestText quest_prefab;
  public UI2DSprite char_spr;
  public UIProgressBar relation_pb;
  public UILabel relation_txt;
  public UILabel npc_name;
  public UILabel npc_descr;
  public UILabel top_icon_txt;
  public GameObject go_relation;
  public SimpleUITable table;
  public KnownNPC _linked_npc;

  public void Draw(KnownNPC npc)
  {
    this._linked_npc = npc;
    this.quest_prefab.gameObject.SetActive(false);
    if (npc == null)
    {
      Debug.LogError((object) "Trying to draw a null npc");
    }
    else
    {
      ObjectDefinition dataOrNull = GameBalance.me.GetDataOrNull<ObjectDefinition>(npc.npc_id);
      foreach (Object componentsInChild in this.GetComponentsInChildren<NPCListQuestText>())
        NGUITools.Destroy(componentsInChild);
      foreach (KnownNPC.TaskState task in this._linked_npc.tasks)
      {
        if (task.state != KnownNPC.TaskState.State.Complete)
          this.quest_prefab.Copy<NPCListQuestText>().Draw(task);
      }
      this.npc_name.text = GJL.L(npc.npc_id);
      this.npc_descr.text = GJL.L("desc_" + npc.npc_id);
      this.top_icon_txt.text = dataOrNull == null ? "" : dataOrNull.day_icon;
      int relation = WorldGameObject.GetRelation(npc.npc_id);
      this.relation_txt.text = relation.ToString();
      this.relation_pb.value = (float) relation / 100f;
      this.npc_descr.ProcessText();
      this.npc_name.ProcessText();
      string sprite_name = "char_" + npc.npc_id;
      if (dataOrNull != null && !string.IsNullOrEmpty(dataOrNull.npc_alias))
        sprite_name = "char_" + dataOrNull.npc_alias;
      this.char_spr.sprite2D = EasySpritesCollection.GetSprite(sprite_name);
      this.go_relation.SetActive(npc.npc_id != "player");
      this.table.Reposition();
    }
  }
}
