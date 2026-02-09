// Decompiled with JetBrains decompiler
// Type: TechConnector
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using SmartPools;
using UnityEngine;

#nullable disable
public class TechConnector : MonoBehaviour
{
  public TechTreeGUIItem tech_1;
  public TechTreeGUIItem tech_2;
  public GameObject go_horiz;
  public GameObject go_diag_up;
  public GameObject go_diag_down;
  public Vector2 p1;
  public Vector2 p2;
  public UIWidget w1;
  public UIWidget w2;

  public static TechConnector Create(TechTreeGUIItem t1, TechTreeGUIItem t2)
  {
    TechConnector techConnector = SmartPooler.CreateObject<TechConnector>();
    techConnector.name = $"{t1.tech_id} -> {t2.tech_id}";
    techConnector.tech_1 = t1;
    techConnector.tech_2 = t2;
    if ((Object) t1 == (Object) null || (Object) t2 == (Object) null)
      return techConnector;
    techConnector.transform.SetParent(MainGame.me.gui_elements.tech_tree.content.gameObject.transform, true);
    techConnector.transform.localPosition = Vector3.zero;
    techConnector.transform.localScale = Vector3.one;
    techConnector.p1 = (Vector2) (t1.gameObject.transform.localPosition + t1.pos2.localPosition);
    techConnector.p2 = (Vector2) (t2.gameObject.transform.localPosition + t2.pos1.localPosition);
    techConnector.w1.transform.localPosition = (Vector3) techConnector.p1;
    techConnector.w2.transform.localPosition = (Vector3) techConnector.p2;
    bool flag1 = (double) Mathf.Abs(techConnector.p1.y - techConnector.p2.y) < 3.0;
    techConnector.go_horiz.SetActive(flag1);
    if (flag1)
    {
      techConnector.go_diag_up.SetActive(false);
      techConnector.go_diag_down.SetActive(false);
    }
    else
    {
      bool flag2 = (double) techConnector.p1.y < (double) techConnector.p2.y;
      techConnector.go_diag_up.SetActive(flag2);
      techConnector.go_diag_down.SetActive(!flag2);
    }
    return techConnector;
  }

  public void SetState(TechDefinition.TechState state)
  {
    foreach (UI2DSprite componentsInChild in this.GetComponentsInChildren<UI2DSprite>(true))
    {
      string sprite_name = componentsInChild.sprite2D.name.Replace("_act", "");
      if (componentsInChild.depth <= -150)
        componentsInChild.depth += 50;
      if (state == TechDefinition.TechState.Purchased)
        sprite_name += "_act";
      else
        componentsInChild.depth -= 50;
      componentsInChild.sprite2D = EasySpritesCollection.GetSprite(sprite_name);
    }
  }

  public void Hide() => SmartPooler.DestroyObject<TechConnector>(this);
}
