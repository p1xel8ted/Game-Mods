// Decompiled with JetBrains decompiler
// Type: Buttons
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;

#nullable disable
[Serializable]
public class Buttons
{
  public GameObject Button;
  public bool isSetting;
  public int Index;
  public bool selected;
  public buttons buttonTypes;
  public UnityEngine.UI.Button nextButton;
  public UnityEngine.UI.Button prevButton;
  public UnityEngine.UI.Slider slider;
  public UnityEngine.UI.Button SwitchButton;
  public TMP_InputField inputField;
  public bool canUse = true;
}
