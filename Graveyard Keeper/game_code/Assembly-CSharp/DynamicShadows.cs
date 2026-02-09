// Decompiled with JetBrains decompiler
// Type: DynamicShadows
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using LinqTools;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class DynamicShadows : MonoBehaviour
{
  public static DynamicShadows me;
  [NonSerialized]
  public static bool _me_is_set;
  [NonSerialized]
  public List<DynamicLight> lights = new List<DynamicLight>();
  [HideInInspector]
  public bool editor_fake_shadows_on;

  public void Start()
  {
    DynamicShadows.me = this;
    DynamicShadows._me_is_set = true;
  }

  public void Update()
  {
    if (Application.isPlaying)
      return;
    this.lights = ((IEnumerable<DynamicLight>) UnityEngine.Object.FindObjectsOfType<DynamicLight>()).ToList<DynamicLight>();
    DynamicShadows.me = this;
  }

  public static GameObject GetNearestLight(
    out bool found,
    Vector2 pos,
    int n,
    List<GameObject> lights_list)
  {
    found = false;
    if (!DynamicShadows._me_is_set)
      return (GameObject) null;
    List<GameObject> gameObjectList = lights_list;
    if (n >= gameObjectList.Count)
      return (GameObject) null;
    if (gameObjectList.Count == 1)
    {
      found = true;
      return gameObjectList[0];
    }
    gameObjectList.Sort((Comparison<GameObject>) ((v1, v2) => ((Vector2) v1.transform.position - pos).sqrMagnitude.CompareTo(((Vector2) v2.transform.position - pos).sqrMagnitude)));
    found = true;
    return gameObjectList[n];
  }

  public void UpdateEditorFakeShadowsMode()
  {
    World objectOfType = UnityEngine.Object.FindObjectOfType<World>();
    if ((UnityEngine.Object) objectOfType == (UnityEngine.Object) null)
      return;
    foreach (ObjectDynamicShadow componentsInChild in objectOfType.GetComponentsInChildren<ObjectDynamicShadow>(true))
    {
      componentsInChild.InstantiateAdditionalShadows(this.editor_fake_shadows_on ? 1 : 0);
      if (!this.editor_fake_shadows_on)
        componentsInChild.shadow_n = 0;
    }
  }
}
