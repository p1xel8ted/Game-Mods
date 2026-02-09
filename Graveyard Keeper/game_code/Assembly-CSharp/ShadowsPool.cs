// Decompiled with JetBrains decompiler
// Type: ShadowsPool
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ShadowsPool : MonoBehaviour
{
  public static ShadowsPool _me;
  public static bool _inited;
  public Stack<ObjectDynamicShadowChild> _objs = new Stack<ObjectDynamicShadowChild>();
  public const int POOL_SIZE = 100;
  public int _total_created;

  public static void Init()
  {
    if (ShadowsPool._inited)
      return;
    ShadowsPool._inited = true;
    GameObject target = new GameObject("* Shadows Pool");
    ShadowsPool._me = target.AddComponent<ShadowsPool>();
    Object.DontDestroyOnLoad((Object) target);
  }

  public void Update()
  {
    if (!MainGame.game_started || !ShadowsPool._inited || this._objs.Count >= 100 || (Object) ShadowsPool._me != (Object) this)
      return;
    this.CreateObject();
    this.CreateObject();
  }

  public ObjectDynamicShadowChild CreateObject()
  {
    GameObject gameObject = new GameObject("shadow");
    gameObject.transform.SetParent(this.transform, false);
    gameObject.SetActive(false);
    gameObject.AddComponent<SpriteRenderer>();
    ObjectDynamicShadowChild dynamicShadowChild = gameObject.AddComponent<ObjectDynamicShadowChild>();
    this._objs.Push(dynamicShadowChild);
    ++this._total_created;
    return dynamicShadowChild;
  }

  public static ObjectDynamicShadowChild GetShadow()
  {
    if (!ShadowsPool._inited)
      ShadowsPool.Init();
    if (ShadowsPool._me._objs.Count == 0)
      ShadowsPool._me.CreateObject();
    ObjectDynamicShadowChild shadow = ShadowsPool._me._objs.Pop();
    shadow.gameObject.SetActive(true);
    return shadow;
  }

  public static void CreateObjects(int n)
  {
    for (int index = 0; index < n; ++index)
      ShadowsPool._me.CreateObject();
  }

  public void CustomInspector()
  {
    GUILayout.Label("Total shadows: " + this._total_created.ToString());
    GUILayout.Label("Available shadows: " + this._objs.Count.ToString());
  }
}
