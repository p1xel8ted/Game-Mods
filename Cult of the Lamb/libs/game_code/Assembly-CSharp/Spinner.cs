// Decompiled with JetBrains decompiler
// Type: Spinner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Spinner : MonoBehaviour
{
  public RectTransform Progress;
  public float RotationSpeed = 100f;

  public void Update() => this.Progress.Rotate(0.0f, 0.0f, this.RotationSpeed * Time.deltaTime);
}
