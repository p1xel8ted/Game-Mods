// Decompiled with JetBrains decompiler
// Type: CustomVerticalLayout
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class CustomVerticalLayout : MonoBehaviour
{
  [SerializeField]
  public Transform[] trackedObjects;
  [SerializeField]
  public float updateInterval = 0.5f;
  [SerializeField]
  public Vector2 positionChangeDelta = Vector2.zero;
  [SerializeField]
  public Vector2 initialPosition;
  public Vector2 changedPosition = Vector2.zero;
  public float updateTimer;
  public bool shouldUpdate = true;

  public void Awake()
  {
    this.changedPosition = this.initialPosition + this.positionChangeDelta;
    this.shouldUpdate = true;
  }

  public void Update()
  {
    if (this.trackedObjects == null || this.trackedObjects.Length == 0)
      return;
    this.updateTimer += Time.deltaTime;
    if ((double) this.updateTimer < (double) this.updateInterval)
      return;
    this.updateTimer = 0.0f;
    for (int index = 0; index < this.trackedObjects.Length; ++index)
    {
      if ((Object) this.trackedObjects[index] != (Object) null && this.trackedObjects[index].gameObject.activeInHierarchy)
      {
        if (!this.shouldUpdate)
          return;
        ((RectTransform) this.transform).anchoredPosition = this.changedPosition;
        this.shouldUpdate = false;
        return;
      }
    }
    this.shouldUpdate = true;
    ((RectTransform) this.transform).anchoredPosition = this.initialPosition;
  }

  public void SetInitialPosition()
  {
    this.initialPosition = ((RectTransform) this.transform).anchoredPosition;
    this.changedPosition = this.initialPosition + this.positionChangeDelta;
  }
}
