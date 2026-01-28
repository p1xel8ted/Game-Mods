// Decompiled with JetBrains decompiler
// Type: HUD_Inventory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class HUD_Inventory : BaseMonoBehaviour
{
  public GameObject icon;
  public float Timer;
  public List<Inventory_Icon> icons;
  public static Inventory_Icon CURRENT_SELECTION;
  public RectTransform cursor;
  public RectTransform pointer;
  public float PointerAngle;
  public float PointerDistance;
  public float Delay;

  public void Show() => this.gameObject.SetActive(true);

  public void OnEnable()
  {
    HUD_Inventory.CURRENT_SELECTION = (Inventory_Icon) null;
    this.PointerAngle = this.PointerDistance = this.Timer = 0.0f;
    this.pointer.localPosition = Vector3.zero;
    this.icons = new List<Inventory_Icon>();
    for (int index = 0; index < Inventory.items.Count; ++index)
    {
      Inventory_Icon component = UnityEngine.Object.Instantiate<GameObject>(this.icon, this.transform.parent).GetComponent<Inventory_Icon>();
      if (Inventory.items.Count == 1)
        component.Angle = 90f;
      else if (Inventory.items.Count == 2)
        component.Angle = (float) (180 * index + 90);
      else if (Inventory.items.Count == 3)
      {
        switch (index)
        {
          case 0:
            component.Angle = 90f;
            break;
          case 1:
            component.Angle = 0.0f;
            break;
          case 2:
            component.Angle = 180f;
            break;
        }
      }
      else
        component.Angle = Inventory.items.Count != 4 ? (float) (45 * (Inventory.items.Count - 1 - index) + 270) : (float) (90 * (Inventory.items.Count - 1 - index) + 180);
      component.TargetDistance = 250f;
      component.Delay = (float) index * 0.05f;
      component.TargetLocation = new Vector3(component.TargetDistance * Mathf.Cos(component.Angle * ((float) Math.PI / 180f)), component.TargetDistance * Mathf.Sin(component.Angle * ((float) Math.PI / 180f)));
      component.SetImage(Inventory.items[index].type, Inventory.items[index].quantity);
      this.icons.Add(component);
    }
    this.Delay = (float) Inventory.items.Count * 0.05f;
    this.cursor.gameObject.GetComponent<Image>().enabled = false;
  }

  public void OnDisable()
  {
    foreach (Component icon in this.icons)
      UnityEngine.Object.Destroy((UnityEngine.Object) icon.gameObject);
    this.icons.Clear();
    this.icons = (List<Inventory_Icon>) null;
  }

  public void Update()
  {
    if ((double) (this.Timer += Time.deltaTime) > (double) this.Delay)
    {
      this.cursor.localPosition = Vector3.Lerp(this.cursor.localPosition, new Vector3(250f * InputManager.UI.GetHorizontalAxis(), 250f * InputManager.UI.GetVerticalAxis()), 15f * Time.deltaTime);
      if ((double) Mathf.Abs(InputManager.UI.GetHorizontalAxis()) > 0.30000001192092896 || (double) Mathf.Abs(InputManager.UI.GetVerticalAxis()) > 0.30000001192092896)
      {
        foreach (Inventory_Icon icon in this.icons)
        {
          if ((UnityEngine.Object) HUD_Inventory.CURRENT_SELECTION == (UnityEngine.Object) null)
            HUD_Inventory.CURRENT_SELECTION = icon;
          else if ((UnityEngine.Object) HUD_Inventory.CURRENT_SELECTION != (UnityEngine.Object) icon && (double) Vector2.Distance((Vector2) icon.TargetLocation, (Vector2) this.cursor.localPosition) < (double) Vector2.Distance((Vector2) HUD_Inventory.CURRENT_SELECTION.TargetLocation, (Vector2) this.cursor.localPosition))
            HUD_Inventory.CURRENT_SELECTION = icon;
        }
        if ((double) this.PointerDistance <= 0.0)
          this.PointerAngle = Utils.GetAngle(this.pointer.localPosition, HUD_Inventory.CURRENT_SELECTION.TargetLocation);
      }
      if ((UnityEngine.Object) HUD_Inventory.CURRENT_SELECTION != (UnityEngine.Object) null)
      {
        this.PointerAngle = Mathf.LerpAngle(this.PointerAngle, Utils.GetAngle(this.pointer.localPosition, HUD_Inventory.CURRENT_SELECTION.TargetLocation), 15f * Time.deltaTime);
        this.PointerDistance = Mathf.Lerp(this.PointerDistance, 100f, 15f * Time.deltaTime);
        this.pointer.localPosition = new Vector3(this.PointerDistance * Mathf.Cos(this.PointerAngle * ((float) Math.PI / 180f)), this.PointerDistance * Mathf.Sin(this.PointerAngle * ((float) Math.PI / 180f)));
      }
    }
    if (!InputManager.Gameplay.GetAttackButtonUp())
      return;
    this.Close();
  }

  public void Close() => this.gameObject.SetActive(false);
}
