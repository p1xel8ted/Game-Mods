// Decompiled with JetBrains decompiler
// Type: CardHUD
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class CardHUD : 
  BaseMonoBehaviour,
  IBeginDragHandler,
  IEventSystemHandler,
  IDragHandler,
  IEndDragHandler,
  IPointerEnterHandler,
  IPointerExitHandler
{
  public GameObject cardUnit;
  public bool dragOnSurfaces = true;
  public GameObject m_DraggingIcon;
  public RectTransform m_DraggingPlane;
  public Canvas canvas;
  public Vector3 originalPosition;
  public Vector3 offSetPosition;
  public RectTransform rt;
  public bool dragging;
  public Sprite cardImage;
  public Sprite placeImage;
  public Image image;
  public bool isOver;
  public Vector3 OverY = Vector3.zero;

  public void Start()
  {
    this.originalPosition = this.transform.position;
    this.rt = this.GetComponent<RectTransform>();
    this.image = this.GetComponent<Image>();
  }

  public void Update()
  {
    this.offSetPosition = Vector3.Lerp(Vector3.zero, this.offSetPosition, 40f * Time.deltaTime);
    if (!this.dragging)
    {
      Vector3 position = this.rt.transform.position;
      this.rt.transform.position = position + (this.originalPosition + this.OverY - position) / 5f;
    }
    else if ((double) this.rt.transform.position.y > 100.0)
    {
      if (!((Object) this.image.sprite != (Object) this.placeImage))
        return;
      this.image.sprite = this.placeImage;
      this.image.SetNativeSize();
    }
    else
    {
      if (!((Object) this.image.sprite != (Object) this.cardImage))
        return;
      this.image.sprite = this.cardImage;
      this.image.SetNativeSize();
    }
  }

  public void OnPointerEnter(PointerEventData eventData)
  {
    Debug.Log((object) "Mouse enter");
    this.isOver = true;
    this.OverY = new Vector3(0.0f, 30f, 0.0f);
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    Debug.Log((object) "Mouse exit");
    this.isOver = false;
    this.OverY = Vector3.zero;
  }

  public void OnBeginDrag(PointerEventData eventData)
  {
    this.dragging = true;
    this.canvas = CardHUD.FindInParents<Canvas>(this.gameObject);
    if ((Object) this.canvas == (Object) null)
      return;
    this.m_DraggingIcon = this.gameObject;
    this.m_DraggingPlane = !this.dragOnSurfaces ? this.canvas.transform as RectTransform : this.transform as RectTransform;
    this.offSetPosition = (Vector3) (eventData.position - new Vector2(this.rt.position.x, this.rt.position.y));
    this.SetDraggedPosition(eventData);
  }

  public void OnDrag(PointerEventData data)
  {
    if (!((Object) this.m_DraggingIcon != (Object) null))
      return;
    this.SetDraggedPosition(data);
  }

  public void SetDraggedPosition(PointerEventData data)
  {
    if (this.dragOnSurfaces && (Object) data.pointerEnter != (Object) null && (Object) (data.pointerEnter.transform as RectTransform) != (Object) null)
      this.m_DraggingPlane = data.pointerEnter.transform as RectTransform;
    Vector3 worldPoint;
    if (!RectTransformUtility.ScreenPointToWorldPointInRectangle(this.m_DraggingPlane, data.position, data.pressEventCamera, out worldPoint))
      return;
    Vector3 position = this.rt.transform.position;
    Vector3 vector3 = worldPoint - this.offSetPosition;
    if ((double) data.position.y < 100.0)
    {
      position.x += (float) (((double) this.originalPosition.x - (double) position.x) / 5.0);
      vector3.x = position.x;
    }
    this.rt.position = vector3;
    this.rt.rotation = this.m_DraggingPlane.rotation;
  }

  public void OnEndDrag(PointerEventData eventData)
  {
    this.dragging = false;
    if ((double) this.rt.position.y < 100.0)
      return;
    Object.Instantiate<GameObject>(this.cardUnit, Camera.main.ScreenToWorldPoint(Input.mousePosition with
    {
      z = -Camera.main.transform.position.z
    }), Quaternion.identity, this.canvas.transform.parent);
    if (!((Object) this.m_DraggingIcon != (Object) null))
      return;
    Object.Destroy((Object) this.m_DraggingIcon);
  }

  public static T FindInParents<T>(GameObject go) where T : Component
  {
    if ((Object) go == (Object) null)
      return default (T);
    T component = go.GetComponent<T>();
    if ((Object) component != (Object) null)
      return component;
    for (Transform parent = go.transform.parent; (Object) parent != (Object) null && (Object) component == (Object) null; parent = parent.parent)
      component = parent.gameObject.GetComponent<T>();
    return component;
  }
}
