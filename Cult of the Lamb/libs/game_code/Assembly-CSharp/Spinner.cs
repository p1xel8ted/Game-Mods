// Decompiled with JetBrains decompiler
// Type: Spinner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Spinner : MonoBehaviour
{
  public RectTransform Progress;
  public float RotationSpeed = 100f;

  public void Update() => this.Progress.Rotate(0.0f, 0.0f, this.RotationSpeed * Time.deltaTime);
}
