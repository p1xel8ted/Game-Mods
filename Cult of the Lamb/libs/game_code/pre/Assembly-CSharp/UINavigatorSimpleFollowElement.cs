// Decompiled with JetBrains decompiler
// Type: UINavigatorSimpleFollowElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UINavigatorSimpleFollowElement : MonoBehaviour
{
  [SerializeField]
  private RectTransform _rectTransform;
  [SerializeField]
  private UI_NavigatorSimple _navigatorSimple;
  private Vector3 _velocity = Vector3.zero;
  private Selectable _previousSelectable;
  private bool _forceInstantNext;

  private void Start()
  {
    DOTween.Init((bool?) new bool?(), (bool?) new bool?(), (LogBehaviour?) new LogBehaviour?());
  }

  private void OnEnable()
  {
    this._navigatorSimple.OnDefaultSetComplete += new System.Action(this.OnDefaultSelectableSet);
    this._navigatorSimple.OnChangeSelection += new UI_NavigatorSimple.ChangeSelection(this.OnSelectableChanged);
  }

  private void OnDisable()
  {
    this._navigatorSimple.OnDefaultSetComplete -= new System.Action(this.OnDefaultSelectableSet);
    this._navigatorSimple.OnChangeSelection -= new UI_NavigatorSimple.ChangeSelection(this.OnSelectableChanged);
  }

  private void OnDefaultSelectableSet()
  {
    this.DoMoveButton(this._navigatorSimple.selectable, true);
  }

  private void OnSelectableChanged(Selectable newSelectable, Selectable previous)
  {
    this.DoMoveButton(newSelectable);
  }

  private void DoMoveButton(Selectable to, bool instant = false)
  {
    this.DoShakeScale();
    this.StopAllCoroutines();
    if ((bool) (UnityEngine.Object) to.GetComponent<UIIgnoreFollowElement>())
    {
      this._rectTransform.localPosition = Vector3.one * float.MaxValue;
      this._forceInstantNext = true;
    }
    else
      this.StartCoroutine((IEnumerator) this.MoveButtonRoutine(to, instant));
  }

  private IEnumerator MoveButtonRoutine(Selectable moveTo, bool instant = false)
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

  private void DoShakeScale()
  {
    DOTween.Kill((object) this._rectTransform);
    this._rectTransform.localScale = Vector3.one;
    this._rectTransform.DOShakeScale(0.1f, new Vector3(-0.1f, 0.1f, 1f), 5, fadeOut: false).SetUpdate<Tweener>(true);
  }
}
