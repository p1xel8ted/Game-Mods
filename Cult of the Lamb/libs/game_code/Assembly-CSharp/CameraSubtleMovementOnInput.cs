// Decompiled with JetBrains decompiler
// Type: CameraSubtleMovementOnInput
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Rewired;
using UnityEngine;

#nullable disable
public class CameraSubtleMovementOnInput : MonoBehaviour
{
  public float moveSpeed = 5f;
  public float springiness = 0.1f;
  public float maxMovement = 10f;
  public Vector3 startPosition;
  public Vector3 targetPosition;
  public bool blockMovement;

  public void OnEnable() => this.startPosition = this.transform.position;

  public int GetPadID()
  {
    Controller activeController = InputManager.General.GetLastActiveController();
    int padId = -1;
    if (activeController != null)
      padId = activeController.id;
    return padId;
  }

  public void Reset(float duration = 2f)
  {
    this.transform.DOKill();
    this.transform.DOMove(this.startPosition, duration).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart);
  }

  public void Update()
  {
    Vector3 input = this.GetInput();
    this.targetPosition = this.startPosition + Vector3.ClampMagnitude(input * this.moveSpeed, this.maxMovement);
    this.transform.position = Vector3.Lerp(this.transform.position, this.targetPosition, this.springiness);
    if ((double) input.magnitude != 0.0)
      return;
    this.transform.position = Vector3.Lerp(this.transform.position, this.startPosition, this.springiness * 2f);
  }

  public Vector3 GetInput()
  {
    if (this.blockMovement)
      return Vector3.zero;
    if (InputManager.General.InputIsController())
    {
      float horizontalSecondaryAxis = InputManager.Gameplay.GetHorizontalSecondaryAxis();
      float verticalSecondaryAxis = InputManager.Gameplay.GetVerticalSecondaryAxis();
      if ((double) Mathf.Abs(horizontalSecondaryAxis) > 0.10000000149011612 || (double) Mathf.Abs(verticalSecondaryAxis) > 0.10000000149011612)
        return new Vector3(horizontalSecondaryAxis, verticalSecondaryAxis, 0.0f);
    }
    Vector3 vector3 = Input.mousePosition - new Vector3((float) (Screen.width / 2), (float) (Screen.height / 2), 0.0f);
    vector3.x /= (float) (Screen.width / 2);
    vector3.y /= (float) (Screen.height / 2);
    return new Vector3(vector3.x, vector3.y, 0.0f);
  }
}
