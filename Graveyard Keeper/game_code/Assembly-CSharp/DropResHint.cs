// Decompiled with JetBrains decompiler
// Type: DropResHint
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Text;
using UnityEngine;

#nullable disable
public class DropResHint : MonoBehaviour
{
  [HideInInspector]
  public UILabel label;
  public DropResGameObject _drop;
  public bool _enabled;
  public int _update_text_counter = 5;
  public bool _show_durability;
  public StringBuilder _sb = new StringBuilder();
  public static List<DropResHint> _list = new List<DropResHint>();

  public void Init()
  {
    this.label = this.GetComponentInChildren<UILabel>();
    this.gameObject.SetActive(false);
  }

  public static DropResHint Show(DropResGameObject drop_res, bool show_durability)
  {
    DropResHint dropResHint = GUIElements.me.drop_res_hint.Copy<DropResHint>();
    dropResHint._drop = drop_res;
    dropResHint.gameObject.SetActive(true);
    dropResHint._enabled = true;
    dropResHint._show_durability = show_durability;
    dropResHint.UpdateText();
    DropResHint._list.Add(dropResHint);
    return dropResHint;
  }

  public void LateUpdate()
  {
    if (!this._enabled)
      return;
    this.transform.SetGUIPosToWorldPos(this._drop.object_transform.position + new Vector3(0.0f, 30f, 0.0f), MainGame.me.world_cam, MainGame.me.gui_cam);
    if (--this._update_text_counter > 0)
      return;
    this._update_text_counter = 5;
    this.UpdateText();
  }

  public void UpdateText()
  {
    if (!this._enabled)
      return;
    this._sb.Length = 0;
    if (this._show_durability)
    {
      this._sb.Append("(hp)");
      this._sb.Append(Item.FloatNumberToPercentString(this._drop.res.durability));
    }
    this.label.text = this._sb.ToString();
    if (string.IsNullOrEmpty(this._drop.res.sub_name))
      return;
    this.label.text = $"{this._drop.res.sub_name}\n{this.label.text}";
  }

  public void DestroyMe()
  {
    Object.Destroy((Object) this.gameObject);
    DropResHint._list.Remove(this);
    this._enabled = false;
  }

  public static void DestroyAll()
  {
    while (DropResHint._list.Count > 0)
      DropResHint._list[0].DestroyMe();
  }
}
