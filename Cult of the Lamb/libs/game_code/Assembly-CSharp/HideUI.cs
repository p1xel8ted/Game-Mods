// Decompiled with JetBrains decompiler
// Type: HideUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMTools;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class HideUI : BaseMonoBehaviour
{
  public static HashSet<GameObject> allHiddenObjects = new HashSet<GameObject>();
  public static HideUI Instance;
  public bool checkParent = true;
  public float hideTime;

  public void Start()
  {
    HideUI.Instance = this;
    UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) this.gameObject);
    MMTransition.OnTransitionCompelte += new System.Action(this.ResetHideTime);
    this.ResetHideTime();
  }

  public void ResetHideTime() => this.hideTime = Time.time + 0.2f;

  public void ResetHideTimeLong() => this.hideTime = Time.time + 1.5f;

  public void Update()
  {
    if (!CheatConsole.HidingUI || (double) this.hideTime <= (double) Time.time)
      return;
    Debug.Log((object) "Hiding all");
    this.HideTagged();
  }

  public void HideTagged()
  {
    this.CleanNulls();
    UnityEngine.Object.FindObjectsOfType<Canvas>(true);
    List<Transform> transformList = new List<Transform>();
    foreach (Canvas canvas in UnityEngine.Object.FindObjectsOfType<Canvas>())
    {
      if (!(canvas.gameObject.name == "CanvasUnify"))
      {
        CanvasGroup canvasGroup = canvas.GetComponent<CanvasGroup>();
        if ((UnityEngine.Object) canvasGroup == (UnityEngine.Object) null)
          canvasGroup = canvas.gameObject.AddComponent<CanvasGroup>();
        bool flag = canvas.gameObject.name != "Canvas - Temple Overlays" && canvas.gameObject.name != "Transition(Clone)";
        if (canvas.gameObject.CompareTag("UI - Ignore HideUI Cheat"))
          flag = false;
        if (this.checkParent && (bool) (UnityEngine.Object) canvas.transform && (bool) (UnityEngine.Object) canvas.transform.parent && canvas.transform.parent.gameObject.CompareTag("UI - Ignore HideUI Cheat"))
          flag = false;
        if (flag)
          canvasGroup.alpha = 0.0f;
      }
    }
    foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("HideableUI"))
    {
      if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null && gameObject.activeSelf)
      {
        gameObject.SetActive(false);
        HideUI.allHiddenObjects.Add(gameObject);
      }
    }
    foreach (HideIfUIDisabled hideIfUiDisabled in UnityEngine.Object.FindObjectsOfType<HideIfUIDisabled>(true))
    {
      if ((UnityEngine.Object) hideIfUiDisabled != (UnityEngine.Object) null && hideIfUiDisabled.enabled && hideIfUiDisabled.gameObject.activeInHierarchy)
        hideIfUiDisabled.HideUI();
    }
  }

  public void ShowUI()
  {
    foreach (Component component1 in UnityEngine.Object.FindObjectsOfType<Canvas>())
    {
      CanvasGroup component2 = component1.GetComponent<CanvasGroup>();
      if ((bool) (UnityEngine.Object) component2)
        component2.alpha = 1f;
    }
    this.CleanNulls();
    foreach (GameObject allHiddenObject in HideUI.allHiddenObjects)
    {
      if ((UnityEngine.Object) allHiddenObject != (UnityEngine.Object) null)
        allHiddenObject.SetActive(true);
    }
    HideUI.allHiddenObjects.Clear();
  }

  public void CleanNulls()
  {
    HideUI.allHiddenObjects.RemoveWhere((Predicate<GameObject>) (g => (UnityEngine.Object) g == (UnityEngine.Object) null));
  }
}
