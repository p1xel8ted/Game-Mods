// Decompiled with JetBrains decompiler
// Type: Unity.VideoHelper.OverlayController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace Unity.VideoHelper;

public class OverlayController : MonoBehaviour
{
  [SerializeField]
  public GameObject target;
  public GameObject blocker;

  public GameObject Target
  {
    get => this.target;
    set
    {
      if (!((UnityEngine.Object) value != (UnityEngine.Object) null) || !((UnityEngine.Object) this.target != (UnityEngine.Object) value))
        return;
      this.target = value;
      this.SetupTarget();
    }
  }

  public void Start() => this.SetupTarget();

  public void ToggleHideOrShow()
  {
    if ((UnityEngine.Object) this.blocker == (UnityEngine.Object) null)
      this.Show();
    else
      this.Hide();
  }

  public void Show()
  {
    if ((UnityEngine.Object) this.blocker != (UnityEngine.Object) null)
      return;
    this.target.SetActive(true);
    List<Canvas> results = new List<Canvas>();
    this.GetComponentsInParent<Canvas>(false, results);
    if (results.Count == 0)
      return;
    this.blocker = this.CreateBlocker(results[0]);
  }

  public void Hide()
  {
    if ((UnityEngine.Object) this.blocker == (UnityEngine.Object) null)
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.blocker);
    this.target.SetActive(false);
    this.blocker = (GameObject) null;
  }

  public void SetupTarget()
  {
    Canvas orAddComponent = this.target.GetOrAddComponent<Canvas>();
    orAddComponent.overrideSorting = true;
    orAddComponent.sortingOrder = 1000;
    this.target.AddComponent<GraphicRaycaster>();
    this.target.SetActive(false);
  }

  public GameObject CreateBlocker(Canvas root)
  {
    GameObject blocker = new GameObject("Blocker", new System.Type[5]
    {
      typeof (RectTransform),
      typeof (Canvas),
      typeof (GraphicRaycaster),
      typeof (Image),
      typeof (Button)
    });
    RectTransform component1 = blocker.GetComponent<RectTransform>();
    component1.SetParent(root.transform, false);
    component1.anchorMin = Vector2.zero;
    component1.anchorMax = Vector2.one;
    component1.sizeDelta = Vector2.zero;
    Canvas component2 = this.target.GetComponent<Canvas>();
    Canvas component3 = blocker.GetComponent<Canvas>();
    component3.overrideSorting = true;
    component3.sortingLayerID = component2.sortingLayerID;
    component3.sortingOrder = component2.sortingOrder - 1;
    blocker.GetComponent<Image>().color = Color.clear;
    blocker.GetComponent<Button>().onClick.AddListener(new UnityAction(this.Hide));
    return blocker;
  }
}
