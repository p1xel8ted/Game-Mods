// Decompiled with JetBrains decompiler
// Type: TutorialArrowGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class TutorialArrowGUI : MonoBehaviour
{
  public WorldGameObject _attached_wgo;
  public bool _visible;
  public Transform inside_obj;
  public float time_k = 7f;
  public float w;
  public float h;

  public void Init() => this.gameObject.SetActive(false);

  public void AttachToWGO(WorldGameObject wgo)
  {
    this._attached_wgo = wgo;
    this._visible = (Object) wgo != (Object) null;
    this.gameObject.SetActive(this._visible);
  }

  public void Update()
  {
    if (!this._visible)
      return;
    Vector3 position = this._attached_wgo.pos3;
    if ((Object) this._attached_wgo.bubble_pos_tf != (Object) null)
      position = this._attached_wgo.bubble_pos_tf.transform.position;
    this.transform.SetGUIPosToWorldPos(position, MainGame.me.world_cam, MainGame.me.gui_cam);
    Vector2 localPosition = (Vector2) this.transform.localPosition;
    Vector2 vector2 = localPosition;
    this.w = (float) Screen.width / 6.6f;
    this.h = (float) Screen.height / 6.4f;
    bool flag = false;
    if ((double) localPosition.x > (double) this.w)
    {
      localPosition.x = this.w;
      flag = true;
    }
    else if ((double) localPosition.x < -(double) this.w)
    {
      localPosition.x = -this.w;
      flag = true;
    }
    if ((double) localPosition.y > (double) this.h)
    {
      localPosition.y = this.h;
      flag = true;
    }
    else if ((double) localPosition.y < -(double) this.h)
    {
      localPosition.y = -this.h;
      flag = true;
    }
    if (flag)
    {
      this.transform.localPosition = (Vector3) localPosition;
      this.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, (float) ((double) Mathf.Atan2(vector2.y, vector2.x) * 57.295780181884766 + 90.0));
    }
    else
      this.transform.localRotation = Quaternion.identity;
    this.inside_obj.localPosition = new Vector3(0.0f, (float) ((double) Mathf.Sin(Time.fixedTime * this.time_k) * 4.0 + 4.0));
  }
}
