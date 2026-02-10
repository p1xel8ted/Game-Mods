// Decompiled with JetBrains decompiler
// Type: ActivateMiniMap
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMBiomeGeneration;
using MMTools;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

#nullable disable
public class ActivateMiniMap : BaseMonoBehaviour
{
  public static ActivateMiniMap Instance;
  public MiniMap miniMap;
  public RectTransform miniMapRT;
  public StateMachine playerState;
  public Vector2 SmallSize = new Vector2(250f, 150f);
  public Vector2 TargetSize = new Vector2(1500f, 800f);
  public Vector2 DefaultLocation;
  public GameObject TeleportPrompt;
  public TextMeshProUGUI Text;
  public static bool IsPlaying;
  public HUD_InventoryIcon[] InventoryIcons;
  public static bool DisableTeleporting;
  public RectTransform Marker;
  public float MoveCursorSpeedX = 75f;
  public float MoveCursorSpeedY = 50f;
  public float SnapToClosestAcceleration = 15f;
  public float SnapToClosestSpeed = 25f;
  public float PanSpeed = 15f;
  public MiniMapIcon Closest;
  public float Delay;

  public void OnEnable()
  {
    ActivateMiniMap.Instance = this;
    this.miniMapRT = this.miniMap.GetComponent<RectTransform>();
    this.DefaultLocation = (Vector2) this.miniMapRT.localPosition;
    this.TeleportPrompt.SetActive(false);
    if (DataManager.Instance.PlayerFleece != 13)
      return;
    this.gameObject.SetActive(false);
  }

  public void OnDisable()
  {
    if (!((UnityEngine.Object) ActivateMiniMap.Instance == (UnityEngine.Object) this))
      return;
    ActivateMiniMap.Instance = (ActivateMiniMap) null;
  }

  public static void Show(StateMachine state)
  {
    if (!((UnityEngine.Object) ActivateMiniMap.Instance != (UnityEngine.Object) null))
      return;
    ActivateMiniMap.Instance.Init(state);
  }

  public void Init(StateMachine state)
  {
    this.playerState = state;
    this.playerState.CURRENT_STATE = StateMachine.State.Map;
    this.StartCoroutine((IEnumerator) this.MoveMap());
    ActivateMiniMap.IsPlaying = true;
  }

  public void ItemUpdated()
  {
    foreach (HUD_InventoryIcon inventoryIcon in this.InventoryIcons)
      inventoryIcon.InitFromType();
  }

  public IEnumerator MoveMap()
  {
    ActivateMiniMap activateMiniMap = this;
    float Timer = 0.0f;
    Vector3 Velocity = Vector3.zero;
    while ((double) (Timer += Time.deltaTime) < 0.30000001192092896)
    {
      activateMiniMap.miniMapRT.localPosition = Vector3.SmoothDamp(activateMiniMap.miniMapRT.localPosition, Vector3.zero, ref Velocity, 0.1f);
      yield return (object) null;
    }
    activateMiniMap.miniMapRT.localPosition = Vector3.zero;
    activateMiniMap.StartCoroutine((IEnumerator) activateMiniMap.ScaleMap());
  }

  public IEnumerator ScaleMap()
  {
    ActivateMiniMap activateMiniMap = this;
    float Timer = 0.0f;
    Vector3 Velocity = Vector3.zero;
    while ((double) (Timer += Time.deltaTime) < 0.30000001192092896)
    {
      activateMiniMap.miniMapRT.sizeDelta = (Vector2) Vector3.SmoothDamp((Vector3) activateMiniMap.miniMapRT.sizeDelta, (Vector3) activateMiniMap.TargetSize, ref Velocity, 0.1f);
      yield return (object) null;
    }
    activateMiniMap.StartCoroutine((IEnumerator) activateMiniMap.Close());
  }

  public IEnumerator Close()
  {
    ActivateMiniMap activateMiniMap = this;
    Coroutine navigateMap = activateMiniMap.StartCoroutine((IEnumerator) activateMiniMap.NavigateMap());
    activateMiniMap.playerState.GetComponent<PlayerFarming>();
    while (!InputManager.UI.GetCancelButtonUp())
    {
      if ((UnityEngine.Object) activateMiniMap.Closest != (UnityEngine.Object) activateMiniMap.miniMap.CurrentIcon && !ActivateMiniMap.DisableTeleporting)
      {
        if (!activateMiniMap.TeleportPrompt.activeSelf)
          activateMiniMap.TeleportPrompt.SetActive(true);
      }
      else if (activateMiniMap.TeleportPrompt.activeSelf)
        activateMiniMap.TeleportPrompt.SetActive(false);
      if (InputManager.UI.GetAcceptButtonUp() && (UnityEngine.Object) activateMiniMap.Closest != (UnityEngine.Object) activateMiniMap.miniMap.CurrentIcon && !ActivateMiniMap.DisableTeleporting)
      {
        activateMiniMap.StopAllCoroutines();
        activateMiniMap.StartCoroutine((IEnumerator) activateMiniMap.ReturnToStartingPositionAndSize());
        activateMiniMap.StartCoroutine((IEnumerator) activateMiniMap.DoChangeRoom());
        activateMiniMap.Marker.position = Vector3.one * -999f;
      }
      yield return (object) null;
    }
    activateMiniMap.playerState.CURRENT_STATE = StateMachine.State.Idle;
    activateMiniMap.StopCoroutine(navigateMap);
    activateMiniMap.StartCoroutine((IEnumerator) activateMiniMap.ReturnToStartingPositionAndSize());
  }

  public IEnumerator DoChangeRoom()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    ActivateMiniMap activateMiniMap = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 0.3f, "", new System.Action(activateMiniMap.ChangeRoom));
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    activateMiniMap.playerState.CURRENT_STATE = StateMachine.State.Teleporting;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(1f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void ChangeRoom()
  {
    this.playerState.CURRENT_STATE = StateMachine.State.Idle;
    BiomeGenerator.Instance.IsTeleporting = true;
    BiomeGenerator.ChangeRoom(this.Closest.X, this.Closest.Y);
  }

  public IEnumerator NavigateMap()
  {
    while ((UnityEngine.Object) this.miniMap.CurrentIcon == (UnityEngine.Object) null)
      yield return (object) null;
    this.playerState.GetComponent<PlayerFarming>();
    this.Marker.position = this.miniMap.CurrentIcon.rectTransform.position;
    Vector3 zero = Vector3.zero;
    while (true)
    {
      float horizontalAxis = InputManager.Gameplay.GetHorizontalAxis();
      float verticalAxis = InputManager.Gameplay.GetVerticalAxis();
      float num1 = float.MaxValue;
      foreach (MiniMapIcon icon in this.miniMap.Icons)
      {
        float num2 = Vector2.Distance((Vector2) this.Marker.position, (Vector2) icon.rectTransform.position);
        if ((double) num2 < (double) num1 && icon.gameObject.activeSelf && icon.room.Visited && !icon.room.IsCustom && icon.room.Completed)
        {
          num1 = num2;
          this.Closest = icon;
        }
      }
      if ((double) (this.Delay -= Time.deltaTime) < 0.0 && ((double) Mathf.Abs(horizontalAxis) > 0.30000001192092896 || (double) Mathf.Abs(verticalAxis) > 0.30000001192092896))
      {
        float f = Utils.GetAngle(Vector3.zero, new Vector3(horizontalAxis, verticalAxis)) * ((float) Math.PI / 180f);
        this.Marker.localPosition = Vector3.zero + new Vector3(this.MoveCursorSpeedX * Mathf.Cos(f), this.MoveCursorSpeedY * Mathf.Sin(f));
        this.Delay = 0.1f;
      }
      else
        this.Marker.position = Vector3.Lerp(this.Marker.position, this.Closest.rectTransform.position, this.SnapToClosestSpeed * Time.deltaTime);
      if ((UnityEngine.Object) this.Closest != (UnityEngine.Object) null)
        this.miniMap.IconContainerRect.localPosition = Vector3.Lerp(this.miniMap.IconContainerRect.localPosition, -this.Closest.rectTransform.localPosition, this.PanSpeed * Time.deltaTime);
      yield return (object) null;
    }
  }

  public IEnumerator ReturnToStartingPositionAndSize()
  {
    float Timer = 0.0f;
    Vector3 VelocitySize = Vector3.zero;
    Vector3 VelocityScale = Vector3.zero;
    this.miniMap.StartCoroutine((IEnumerator) this.miniMap.MoveMiniMap(0.0f));
    this.TeleportPrompt.SetActive(false);
    while ((double) (Timer += Time.deltaTime) < 0.40000000596046448)
    {
      this.miniMapRT.sizeDelta = (Vector2) Vector3.SmoothDamp((Vector3) this.miniMapRT.sizeDelta, (Vector3) this.SmallSize, ref VelocitySize, 0.1f);
      this.miniMapRT.localPosition = Vector3.SmoothDamp(this.miniMapRT.localPosition, (Vector3) this.DefaultLocation, ref VelocityScale, 0.1f);
      yield return (object) null;
    }
    ActivateMiniMap.IsPlaying = false;
  }
}
