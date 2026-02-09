// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Services.MessageRouter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

#nullable disable
namespace ParadoxNotion.Services;

public class MessageRouter : 
  MonoBehaviour,
  IPointerEnterHandler,
  IEventSystemHandler,
  IPointerExitHandler,
  IPointerDownHandler,
  IPointerUpHandler,
  IPointerClickHandler,
  IDragHandler,
  IScrollHandler,
  IUpdateSelectedHandler,
  ISelectHandler,
  IDeselectHandler,
  IMoveHandler,
  ISubmitHandler
{
  public Dictionary<string, List<object>> listeners = new Dictionary<string, List<object>>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
  public object _animator;

  public Animator animator
  {
    get
    {
      if (this._animator == null)
      {
        this._animator = (object) this.GetComponent<Animator>();
        if (this._animator == null)
          this._animator = new object();
      }
      return this._animator as Animator;
    }
  }

  public void OnPointerEnter(PointerEventData eventData)
  {
    this.Dispatch<PointerEventData>(nameof (OnPointerEnter), eventData);
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    this.Dispatch<PointerEventData>(nameof (OnPointerExit), eventData);
  }

  public void OnPointerDown(PointerEventData eventData)
  {
    this.Dispatch<PointerEventData>(nameof (OnPointerDown), eventData);
  }

  public void OnPointerUp(PointerEventData eventData)
  {
    this.Dispatch<PointerEventData>(nameof (OnPointerUp), eventData);
  }

  public void OnPointerClick(PointerEventData eventData)
  {
    this.Dispatch<PointerEventData>(nameof (OnPointerClick), eventData);
  }

  public void OnDrag(PointerEventData eventData)
  {
    this.Dispatch<PointerEventData>(nameof (OnDrag), eventData);
  }

  public void OnDrop(BaseEventData eventData)
  {
    this.Dispatch<BaseEventData>(nameof (OnDrop), eventData);
  }

  public void OnScroll(PointerEventData eventData)
  {
    this.Dispatch<PointerEventData>(nameof (OnScroll), eventData);
  }

  public void OnUpdateSelected(BaseEventData eventData)
  {
    this.Dispatch<BaseEventData>(nameof (OnUpdateSelected), eventData);
  }

  public void OnSelect(BaseEventData eventData)
  {
    this.Dispatch<BaseEventData>(nameof (OnSelect), eventData);
  }

  public void OnDeselect(BaseEventData eventData)
  {
    this.Dispatch<BaseEventData>(nameof (OnDeselect), eventData);
  }

  public void OnMove(AxisEventData eventData)
  {
    this.Dispatch<AxisEventData>(nameof (OnMove), eventData);
  }

  public void OnSubmit(BaseEventData eventData)
  {
    this.Dispatch<BaseEventData>(nameof (OnSubmit), eventData);
  }

  public void OnAnimatorIK(int layerIndex) => this.Dispatch<int>(nameof (OnAnimatorIK), layerIndex);

  public void OnAnimatorMove()
  {
    if (this.Dispatch(nameof (OnAnimatorMove)))
      return;
    this.animator.ApplyBuiltinRootMotion();
  }

  public void OnBecameInvisible() => this.Dispatch(nameof (OnBecameInvisible));

  public void OnBecameVisible() => this.Dispatch(nameof (OnBecameVisible));

  public void OnCollisionEnter(Collision collisionInfo)
  {
    this.Dispatch<Collision>(nameof (OnCollisionEnter), collisionInfo);
  }

  public void OnCollisionExit(Collision collisionInfo)
  {
    this.Dispatch<Collision>(nameof (OnCollisionExit), collisionInfo);
  }

  public void OnCollisionStay(Collision collisionInfo)
  {
    this.Dispatch<Collision>(nameof (OnCollisionStay), collisionInfo);
  }

  public void OnCollisionEnter2D(Collision2D collisionInfo)
  {
    this.Dispatch<Collision2D>(nameof (OnCollisionEnter2D), collisionInfo);
  }

  public void OnCollisionExit2D(Collision2D collisionInfo)
  {
    this.Dispatch<Collision2D>(nameof (OnCollisionExit2D), collisionInfo);
  }

  public void OnCollisionStay2D(Collision2D collisionInfo)
  {
    this.Dispatch<Collision2D>(nameof (OnCollisionStay2D), collisionInfo);
  }

  public void OnTriggerEnter(Collider other)
  {
    this.Dispatch<Collider>(nameof (OnTriggerEnter), other);
  }

  public void OnTriggerExit(Collider other)
  {
    this.Dispatch<Collider>(nameof (OnTriggerExit), other);
  }

  public void OnTriggerStay(Collider other)
  {
    this.Dispatch<Collider>(nameof (OnTriggerStay), other);
  }

  public void OnTriggerEnter2D(Collider2D other)
  {
    this.Dispatch<Collider2D>(nameof (OnTriggerEnter2D), other);
  }

  public void OnTriggerExit2D(Collider2D other)
  {
    this.Dispatch<Collider2D>(nameof (OnTriggerExit2D), other);
  }

  public void OnTriggerStay2D(Collider2D other)
  {
    this.Dispatch<Collider2D>(nameof (OnTriggerStay2D), other);
  }

  public void OnMouseDown() => this.Dispatch(nameof (OnMouseDown));

  public void OnMouseDrag() => this.Dispatch(nameof (OnMouseDrag));

  public void OnMouseEnter() => this.Dispatch(nameof (OnMouseEnter));

  public void OnMouseExit() => this.Dispatch(nameof (OnMouseExit));

  public void OnMouseOver() => this.Dispatch(nameof (OnMouseOver));

  public void OnMouseUp() => this.Dispatch(nameof (OnMouseUp));

  public void OnControllerColliderHit(ControllerColliderHit hit)
  {
    this.Dispatch<ControllerColliderHit>(nameof (OnControllerColliderHit), hit);
  }

  public void OnParticleCollision(GameObject other)
  {
    this.Dispatch<GameObject>(nameof (OnParticleCollision), other);
  }

  public void OnEnable() => this.Dispatch(nameof (OnEnable));

  public void OnDisable() => this.Dispatch(nameof (OnDisable));

  public void OnDestroy() => this.Dispatch(nameof (OnDestroy));

  public void OnTransformChildrenChanged() => this.Dispatch(nameof (OnTransformChildrenChanged));

  public void OnTransformParentChanged() => this.Dispatch(nameof (OnTransformParentChanged));

  public void OnCustomEvent(EventData eventData)
  {
    this.Dispatch<EventData>(nameof (OnCustomEvent), eventData);
  }

  public void Register(object target, params string[] messages)
  {
    if (target == null)
      return;
    for (int index = 0; index < messages.Length; ++index)
    {
      if (MethodInfo.op_Equality(target.GetType().RTGetMethod(messages[index]), (MethodInfo) null))
      {
        Debug.LogError((object) $"Type '{target.GetType().FriendlyName()}' does not implement a method named '{messages[index]}', for the registered event to use.");
      }
      else
      {
        List<object> objectList = (List<object>) null;
        if (!this.listeners.TryGetValue(messages[index], out objectList))
        {
          objectList = new List<object>();
          this.listeners[messages[index]] = objectList;
        }
        if (!objectList.Contains(target))
          objectList.Add(target);
      }
    }
  }

  public void RegisterCallback(string message, Action callback)
  {
    this.Internal_RegisterCallback(message, (Delegate) callback);
  }

  public void RegisterCallback<T>(string message, Action<T> callback)
  {
    this.Internal_RegisterCallback(message, (Delegate) callback);
  }

  public void Internal_RegisterCallback(string message, Delegate callback)
  {
    List<object> objectList = (List<object>) null;
    if (!this.listeners.TryGetValue(message, out objectList))
    {
      objectList = new List<object>();
      this.listeners[message] = objectList;
    }
    if (objectList.Contains((object) callback))
      return;
    objectList.Add((object) callback);
  }

  public void UnRegister(object target)
  {
    if (target == null)
      return;
    foreach (string key in this.listeners.Keys)
    {
      foreach (object obj in this.listeners[key].ToArray())
      {
        if (obj == target)
          this.listeners[key].Remove(target);
        else if ((object) (obj as Delegate) != null && (obj as Delegate).Target == target)
          this.listeners[key].Remove(obj);
      }
    }
  }

  public void UnRegister(object target, params string[] messages)
  {
    if (target == null)
      return;
    for (int index = 0; index < messages.Length; ++index)
    {
      string message = messages[index];
      if (this.listeners.ContainsKey(message))
      {
        foreach (object obj in this.listeners[message].ToArray())
        {
          if (obj == target)
            this.listeners[message].Remove(target);
          else if ((object) (obj as Delegate) != null && (obj as Delegate).Target == target)
            this.listeners[message].Remove(obj);
        }
      }
    }
  }

  public bool Dispatch(string message) => this.Dispatch<object>(message, (object) null);

  public bool Dispatch<T>(string message, T arg)
  {
    List<object> objectList;
    if (!this.listeners.TryGetValue(message, out objectList))
      return false;
    for (int index = 0; index < objectList.Count; ++index)
    {
      object del = objectList[index];
      if (del != null)
      {
        MethodInfo methodInfo = (object) (del as Delegate) == null ? del.GetType().RTGetMethod(message) : (del as Delegate).RTGetDelegateMethodInfo();
        if (MethodInfo.op_Equality(methodInfo, (MethodInfo) null))
        {
          Debug.LogError((object) $"Can't resolve method {del.GetType().Name}.{message}.");
        }
        else
        {
          ParameterInfo[] parameters1 = methodInfo.GetParameters();
          if (parameters1.Length > 1)
          {
            Debug.LogError((object) $"Parameters on method {del.GetType().Name}.{message}, are more than 1.");
          }
          else
          {
            object[] parameters2 = (object[]) null;
            if (parameters1.Length == 1)
              parameters2 = new object[1]
              {
                !typeof (MessageRouter.MessageData).RTIsAssignableFrom(parameters1[0].ParameterType) ? (object) arg : (object) new MessageRouter.MessageData<T>(arg, this.gameObject)
              };
            if ((object) (del as Delegate) != null)
              (del as Delegate).DynamicInvoke(parameters2);
            else if (System.Type.op_Equality(methodInfo.ReturnType, typeof (IEnumerator)))
              MonoManager.current.StartCoroutine((IEnumerator) methodInfo.Invoke(del, parameters2));
            else
              methodInfo.Invoke(del, parameters2);
          }
        }
      }
    }
    return true;
  }

  public class MessageData
  {
    [CompilerGenerated]
    public GameObject \u003Creceiver\u003Ek__BackingField;

    public GameObject receiver
    {
      get => this.\u003Creceiver\u003Ek__BackingField;
      set => this.\u003Creceiver\u003Ek__BackingField = value;
    }

    public MessageData()
    {
    }

    public MessageData(GameObject receiver) => this.receiver = receiver;
  }

  public class MessageData<T> : MessageRouter.MessageData
  {
    [CompilerGenerated]
    public T \u003Cvalue\u003Ek__BackingField;

    public T value
    {
      get => this.\u003Cvalue\u003Ek__BackingField;
      set => this.\u003Cvalue\u003Ek__BackingField = value;
    }

    public MessageData(T value, GameObject receiver)
      : base(receiver)
    {
      this.value = value;
    }
  }
}
