// Decompiled with JetBrains decompiler
// Type: WeaponSelectMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class WeaponSelectMenu : BaseMonoBehaviour
{
  public float Angle;
  public GameObject PointerRotator;
  public GameObject Pointer;
  public float PointerAngle;
  public GameObject CurrentGameObject;
  public Text text;
  public List<GameObject> Nodes;
  public int CURRENT_SELECTION;

  public void Show() => this.gameObject.SetActive(true);

  public void OnEnable()
  {
    GameManager.SetTimeScale(0.1f);
    this.CURRENT_SELECTION = Inventory.CURRENT_WEAPON;
    this.CurrentGameObject = this.Nodes[this.CURRENT_SELECTION];
    this.PointerAngle = Utils.GetAngle(this.CurrentGameObject.transform.localPosition, Vector3.zero) + 90f;
    this.PointerRotator.transform.eulerAngles = new Vector3(0.0f, 0.0f, this.PointerAngle);
  }

  public void OnDisable() => GameManager.SetTimeScale(1f);

  public void Update()
  {
    if ((double) Mathf.Abs(InputManager.UI.GetHorizontalAxis()) > 0.20000000298023224 || (double) Mathf.Abs(InputManager.UI.GetVerticalAxis()) > 0.20000000298023224)
      this.Angle = Utils.GetAngle(new Vector3(InputManager.UI.GetHorizontalAxis(), InputManager.UI.GetVerticalAxis()), Vector3.zero) + 90f;
    else if ((UnityEngine.Object) this.CurrentGameObject != (UnityEngine.Object) null)
      this.Angle = Utils.GetAngle(this.CurrentGameObject.transform.localPosition, Vector3.zero) + 90f;
    this.PointerAngle += (float) ((double) Mathf.Atan2(Mathf.Sin((float) (((double) this.Angle - (double) this.PointerAngle) * (Math.PI / 180.0))), Mathf.Cos((float) (((double) this.Angle - (double) this.PointerAngle) * (Math.PI / 180.0)))) * 57.295780181884766 / 3.0);
    this.PointerRotator.transform.eulerAngles = new Vector3(0.0f, 0.0f, this.PointerAngle);
    float num1 = float.MaxValue;
    for (int index = 0; index < this.Nodes.Count; ++index)
    {
      GameObject node = this.Nodes[index];
      node.transform.localScale = new Vector3(1f, 1f);
      float num2 = Vector2.Distance((Vector2) this.Pointer.transform.position, (Vector2) node.transform.position);
      if ((UnityEngine.Object) this.CurrentGameObject == (UnityEngine.Object) null)
      {
        this.CurrentGameObject = node;
        num1 = num2;
        this.CURRENT_SELECTION = index;
      }
      if ((double) num2 < (double) num1)
      {
        this.CurrentGameObject = node;
        num1 = num2;
        this.CURRENT_SELECTION = index;
      }
    }
    this.CurrentGameObject.transform.localScale = new Vector3(1.1f, 1.1f);
    Inventory.CURRENT_WEAPON = this.CURRENT_SELECTION;
    this.text.text = $"{Inventory.weapons[this.CURRENT_SELECTION].name}\n{Inventory.weapons[this.CURRENT_SELECTION].GetQuantity()}";
  }
}
