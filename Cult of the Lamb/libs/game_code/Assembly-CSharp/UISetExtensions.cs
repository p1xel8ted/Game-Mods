// Decompiled with JetBrains decompiler
// Type: UISetExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Reflection;
using UnityEngine.UI;

#nullable disable
public static class UISetExtensions
{
  public static MethodInfo toggleSetMethod = UISetExtensions.FindSetMethod(typeof (Toggle));
  public static MethodInfo sliderSetMethod = UISetExtensions.FindSetMethod(typeof (UnityEngine.UI.Slider));
  public static MethodInfo scrollbarSetMethod = UISetExtensions.FindSetMethod(typeof (Scrollbar));
  public static FieldInfo dropdownValueField = typeof (Dropdown).GetField("m_Value", BindingFlags.Instance | BindingFlags.NonPublic);

  public static void Set(this Toggle instance, bool value, bool sendCallback = false)
  {
    UISetExtensions.toggleSetMethod.Invoke((object) instance, new object[2]
    {
      (object) value,
      (object) sendCallback
    });
  }

  public static void Set(this UnityEngine.UI.Slider instance, float value, bool sendCallback = false)
  {
    UISetExtensions.sliderSetMethod.Invoke((object) instance, new object[2]
    {
      (object) value,
      (object) sendCallback
    });
  }

  public static void Set(this Scrollbar instance, float value, bool sendCallback = false)
  {
    UISetExtensions.scrollbarSetMethod.Invoke((object) instance, new object[2]
    {
      (object) value,
      (object) sendCallback
    });
  }

  public static void Set(this Dropdown instance, int value)
  {
    UISetExtensions.dropdownValueField.SetValue((object) instance, (object) value);
    instance.RefreshShownValue();
  }

  public static MethodInfo FindSetMethod(System.Type objectType)
  {
    MethodInfo[] methods = objectType.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic);
    for (int index = 0; index < methods.Length; ++index)
    {
      if (methods[index].Name == "Set" && methods[index].GetParameters().Length == 2)
        return methods[index];
    }
    return (MethodInfo) null;
  }
}
