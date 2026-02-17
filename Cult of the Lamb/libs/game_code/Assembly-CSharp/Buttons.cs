// Decompiled with JetBrains decompiler
// Type: Buttons
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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
