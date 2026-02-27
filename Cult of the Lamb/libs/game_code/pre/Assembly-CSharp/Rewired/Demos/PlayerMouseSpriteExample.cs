// Decompiled with JetBrains decompiler
// Type: Rewired.Demos.PlayerMouseSpriteExample
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace Rewired.Demos;

[AddComponentMenu("")]
public class PlayerMouseSpriteExample : MonoBehaviour
{
  [Tooltip("The Player that will control the mouse")]
  public int playerId;
  [Tooltip("The Rewired Action used for the mouse horizontal axis.")]
  public string horizontalAction = "MouseX";
  [Tooltip("The Rewired Action used for the mouse vertical axis.")]
  public string verticalAction = "MouseY";
  [Tooltip("The Rewired Action used for the mouse wheel axis.")]
  public string wheelAction = "MouseWheel";
  [Tooltip("The Rewired Action used for the mouse left button.")]
  public string leftButtonAction = "MouseLeftButton";
  [Tooltip("The Rewired Action used for the mouse right button.")]
  public string rightButtonAction = "MouseRightButton";
  [Tooltip("The Rewired Action used for the mouse middle button.")]
  public string middleButtonAction = "MouseMiddleButton";
  [Tooltip("The distance from the camera that the pointer will be drawn.")]
  public float distanceFromCamera = 1f;
  [Tooltip("The scale of the sprite pointer.")]
  public float spriteScale = 0.05f;
  [Tooltip("The pointer prefab.")]
  public GameObject pointerPrefab;
  [Tooltip("The click effect prefab.")]
  public GameObject clickEffectPrefab;
  [Tooltip("Should the hardware pointer be hidden?")]
  public bool hideHardwarePointer = true;
  [NonSerialized]
  private GameObject pointer;
  [NonSerialized]
  private PlayerMouse mouse;

  private void Awake()
  {
    this.pointer = UnityEngine.Object.Instantiate<GameObject>(this.pointerPrefab);
    this.pointer.transform.localScale = new Vector3(this.spriteScale, this.spriteScale, this.spriteScale);
    if (this.hideHardwarePointer)
      Cursor.visible = false;
    this.mouse = PlayerMouse.Factory.Create();
    this.mouse.playerId = this.playerId;
    this.mouse.xAxis.actionName = this.horizontalAction;
    this.mouse.yAxis.actionName = this.verticalAction;
    this.mouse.wheel.yAxis.actionName = this.wheelAction;
    this.mouse.leftButton.actionName = this.leftButtonAction;
    this.mouse.rightButton.actionName = this.rightButtonAction;
    this.mouse.middleButton.actionName = this.middleButtonAction;
    this.mouse.pointerSpeed = 1f;
    this.mouse.wheel.yAxis.repeatRate = 5f;
    this.mouse.screenPosition = new Vector2((float) Screen.width * 0.5f, (float) Screen.height * 0.5f);
    this.mouse.ScreenPositionChangedEvent += (Action<Vector2>) new Action<Vector2>(this.OnScreenPositionChanged);
    this.OnScreenPositionChanged(this.mouse.screenPosition);
  }

  private void Update()
  {
    if (!ReInput.isReady)
      return;
    this.pointer.transform.Rotate(Vector3.forward, this.mouse.wheel.yAxis.value * 20f);
    if (this.mouse.leftButton.justPressed)
      this.CreateClickEffect(new Color(0.0f, 1f, 0.0f, 1f));
    if (this.mouse.rightButton.justPressed)
      this.CreateClickEffect(new Color(1f, 0.0f, 0.0f, 1f));
    if (!this.mouse.middleButton.justPressed)
      return;
    this.CreateClickEffect(new Color(1f, 1f, 0.0f, 1f));
  }

  private void OnDestroy()
  {
    if (!ReInput.isReady)
      return;
    this.mouse.ScreenPositionChangedEvent -= (Action<Vector2>) new Action<Vector2>(this.OnScreenPositionChanged);
  }

  private void CreateClickEffect(Color color)
  {
    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.clickEffectPrefab);
    gameObject.transform.localScale = new Vector3(this.spriteScale, this.spriteScale, this.spriteScale);
    gameObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(this.mouse.screenPosition.x, this.mouse.screenPosition.y, this.distanceFromCamera));
    gameObject.GetComponentInChildren<SpriteRenderer>().color = color;
    UnityEngine.Object.Destroy((UnityEngine.Object) gameObject, 0.5f);
  }

  private void OnScreenPositionChanged(Vector2 position)
  {
    this.pointer.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, this.distanceFromCamera));
  }
}
