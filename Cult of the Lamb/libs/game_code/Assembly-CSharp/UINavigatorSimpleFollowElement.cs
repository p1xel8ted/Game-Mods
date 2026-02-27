// Decompiled with JetBrains decompiler
// Type: UINavigatorSimpleFollowElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UINavigatorSimpleFollowElement : MonoBehaviour
{
  [SerializeField]
  public RectTransform _rectTransform;
  [SerializeField]
  public UI_NavigatorSimple _navigatorSimple;
  public Vector3 _velocity = Vector3.zero;
  public Selectable _previousSelectable;
  public bool _forceInstantNext;

  public void Start()
  {
    DOTween.Init((bool?) new bool?(), (bool?) new bool?(), (LogBehaviour?) new LogBehaviour?());
  }

  public void OnEnable()
  {
    this._navigatorSimple.OnDefaultSetComplete += new System.Action(this.OnDefaultSelectableSet);
    this._navigatorSimple.OnChangeSelection += new UI_NavigatorSimple.ChangeSelection(this.OnSelectableChanged);
  }

  public void OnDisable()
  {
    this._navigatorSimple.OnDefaultSetComplete -= new System.Action(this.OnDefaultSelectableSet);
    this._navigatorSimple.OnChangeSelection -= new UI_NavigatorSimple.ChangeSelection(this.OnSelectableChanged);
  }

  public void OnDefaultSelectableSet() => this.DoMoveButton(this._navigatorSimple.selectable, true);

  public void OnSelectableChanged(Selectable newSelectable, Selectable previous)
  {
    this.DoMoveButton(newSelectable);
  }

  public void DoMoveButton(Selectable to, bool instant = false)
  {
    this.DoShakeScale();
    this.StopAllCoroutines();
    if ((bool) (UnityEngine.Object) to.GetComponent<UIIgnoreFollowElement>())
    {
      this._rectTransform.localPosition = Vector3.one * float.MaxValue;
      this._forceInstantNext = true;
    }
    else
      this.StartCoroutine(this.MoveButtonRoutine(to, instant));
  }

  public IEnumerator MoveButtonRoutine(Selectable moveTo, bool instant = false)
  {
    yield return (object) null;
    Vector3 targetLocalPosition = this._rectTransform.parent.InverseTransformPoint(moveTo.transform.position);
    Vector3 currentLocalPosition = this._rectTransform.localPosition;
    if (!instant && !this._forceInstantNext)
    {
      float progress = 0.0f;
      while ((double) (progress += Time.unscaledDeltaTime * 5f) <= 1.0)
      {
        this._rectTransform.localPosition = Vector3.SmoothDamp(targetLocalPosition, currentLocalPosition, ref this._velocity, progress);
        yield return (object) null;
      }
    }
    this._forceInstantNext = false;
    this._rectTransform.localPosition = targetLocalPosition;
    this._rectTransform.localScale = Vector3.one;
  }

  public void DoShakeScale()
  {
    DOTween.Kill((object) this._rectTransform);
    this._rectTransform.localScale = Vector3.one;
    this._rectTransform.DOShakeScale(0.1f, new Vector3(-0.1f, 0.1f, 1f), 5, fadeOut: false).SetUpdate<Tweener>(true);
  }
}
