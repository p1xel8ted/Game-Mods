// Decompiled with JetBrains decompiler
// Type: MMTools.SpeechBubble
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace MMTools;

public class SpeechBubble : MonoBehaviour
{
  private const float kScreenMarginVertical = 90f;
  private const float kScreenMarginHorizontal = 45f;
  private TextMeshProUGUI TextComponent;
  private RectTransform BubbleRT;
  private RectTransform RT;
  private float width;
  private float height;
  private Vector2 Scale;
  private Vector3 ScaleSpeed;
  [Range(0.1f, 1f)]
  public float Spring = 0.3f;
  [Range(0.1f, 1f)]
  public float Dampening = 0.65f;
  public float MaxSpeed = 50f;
  public Canvas canvas;
  private RectTransform CanvasRect;
  public Image Bubble;
  public Vector2 Margin = new Vector2(200f, 100f);
  private Vector3 _offset = Vector3.zero;
  public float ScreenOffset = 400f;
  private Vector3 TargetPosition;
  private Vector3 _rendererBoundsOffset;
  private Transform _targetTransform;
  private MeshRenderer _targetMeshRenderer;

  public void OnEnable()
  {
  }

  public void Reset(TextMeshProUGUI TextComponent)
  {
    this.TextComponent = TextComponent;
    this.TextComponent.text = "";
    this.Scale = (Vector2) (this.ScaleSpeed = (Vector3) Vector2.zero);
    this.RT = this.GetComponent<RectTransform>();
    this.BubbleRT = this.Bubble.GetComponent<RectTransform>();
    this.BubbleRT.sizeDelta = this.Scale;
    this.RT.sizeDelta = this.Scale;
    this.CanvasRect = this.canvas.GetComponent<RectTransform>();
    this.transform.localScale = Vector3.one;
  }

  private void Update()
  {
    this.width = this.TextComponent.preferredWidth + this.Margin.x;
    if ((double) this.width > (double) this.TextComponent.rectTransform.sizeDelta.x)
      this.width = this.TextComponent.rectTransform.sizeDelta.x + this.Margin.x;
    this.height = this.TextComponent.preferredHeight + this.Margin.y;
    this.ScaleSpeed.x = Mathf.Min(this.ScaleSpeed.x, this.MaxSpeed);
    this.ScaleSpeed.y = Mathf.Min(this.ScaleSpeed.y, this.MaxSpeed);
    this.ScaleSpeed.x = Mathf.Max(this.ScaleSpeed.x, -this.MaxSpeed);
    this.ScaleSpeed.y = Mathf.Max(this.ScaleSpeed.y, -this.MaxSpeed);
    this.BubbleRT.sizeDelta = this.Scale;
    this.RT.sizeDelta = this.Scale;
  }

  private void LateUpdate()
  {
    if ((Object) this._targetTransform == (Object) null)
      return;
    Vector3 position = this._targetTransform.position;
    if ((Object) this._targetMeshRenderer != (Object) null)
    {
      position += this._rendererBoundsOffset;
      Debug.DrawLine(this._targetTransform.position, position, Color.green);
    }
    Vector3 newPos = (Vector3) ((Vector2) Camera.main.WorldToViewportPoint(position + this._offset) * this.CanvasRect.rect.size);
    newPos.y += this.BubbleRT.rect.height * 0.5f;
    if ((Object) this._targetMeshRenderer == (Object) null)
      newPos.y += this.ScreenOffset;
    this.RT.anchoredPosition = (Vector2) this.KeepFullyOnScreen(newPos);
  }

  private void FixedUpdate()
  {
    this.ScaleSpeed.x += (this.width - this.Scale.x) * this.Spring;
    this.Scale.x += (this.ScaleSpeed.x *= this.Dampening);
    this.ScaleSpeed.y += (this.height - this.Scale.y) * this.Spring;
    this.Scale.y += (this.ScaleSpeed.y *= this.Dampening);
  }

  private Vector3 KeepFullyOnScreen(Vector3 newPos)
  {
    Rect rect1 = this.CanvasRect.rect;
    Rect rect2 = this.BubbleRT.rect;
    newPos.x = Mathf.Clamp(newPos.x, (float) ((double) rect2.width * 0.5 + 45.0), (float) ((double) rect1.width - (double) rect2.width * 0.5 - 45.0));
    newPos.y = Mathf.Clamp(newPos.y, (float) ((double) rect2.height * 0.5 + 90.0), (float) ((double) rect1.height - (double) rect2.height * 0.5 - 90.0));
    return newPos;
  }

  public void SetPosition(Vector3 Position)
  {
    this.TargetPosition = Camera.main.WorldToViewportPoint(Position);
    this.TargetPosition = (Vector3) ((Vector2) this.TargetPosition * this.CanvasRect.rect.size);
  }

  public void SetTarget(Transform newTarget, Vector3 offset)
  {
    this._offset = offset;
    if ((Object) this._targetTransform != (Object) newTarget)
    {
      this._targetTransform = newTarget;
      SkeletonAnimation component1;
      if (!newTarget.TryGetComponent<SkeletonAnimation>(out component1))
        component1 = newTarget.GetComponentInChildren<SkeletonAnimation>();
      if ((Object) component1 != (Object) null)
      {
        MeshRenderer component2;
        this._targetMeshRenderer = !component1.TryGetComponent<MeshRenderer>(out component2) ? (MeshRenderer) null : component2;
      }
    }
    if ((Object) this._targetMeshRenderer != (Object) null)
    {
      Bounds bounds = this._targetMeshRenderer.bounds;
      float num = Vector3.Distance(new Vector3(0.0f, bounds.min.y, bounds.min.z), new Vector3(0.0f, bounds.max.y, bounds.max.z));
      this._rendererBoundsOffset = Quaternion.Euler(this._targetMeshRenderer.transform.eulerAngles) * new Vector3(0.0f, num + 0.5f, 0.0f);
    }
    else
      this._rendererBoundsOffset = Vector3.zero;
  }

  public void ClearTarget()
  {
    this._targetTransform = (Transform) null;
    this._targetMeshRenderer = (MeshRenderer) null;
  }

  public float GetAngle(Vector3 fromPosition, Vector3 toPosition)
  {
    return Mathf.Atan2(toPosition.y - fromPosition.y, toPosition.x - fromPosition.x) * 57.29578f;
  }

  public void HidePrompt()
  {
  }
}
