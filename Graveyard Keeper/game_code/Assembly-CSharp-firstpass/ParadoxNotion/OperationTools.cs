// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.OperationTools
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace ParadoxNotion;

public static class OperationTools
{
  public static string GetOperationString(OperationMethod om)
  {
    switch (om)
    {
      case OperationMethod.Set:
        return " = ";
      case OperationMethod.Add:
        return " += ";
      case OperationMethod.Subtract:
        return " -= ";
      case OperationMethod.Multiply:
        return " *= ";
      case OperationMethod.Divide:
        return " /= ";
      default:
        return string.Empty;
    }
  }

  public static float Operate(float a, float b, OperationMethod om, float delta = 1f)
  {
    switch (om)
    {
      case OperationMethod.Set:
        return b;
      case OperationMethod.Add:
        return a + b * delta;
      case OperationMethod.Subtract:
        return a - b * delta;
      case OperationMethod.Multiply:
        return a * (b * delta);
      case OperationMethod.Divide:
        return a / (b * delta);
      default:
        return a;
    }
  }

  public static int Operate(int a, int b, OperationMethod om)
  {
    switch (om)
    {
      case OperationMethod.Set:
        return b;
      case OperationMethod.Add:
        return a + b;
      case OperationMethod.Subtract:
        return a - b;
      case OperationMethod.Multiply:
        return a * b;
      case OperationMethod.Divide:
        return a / b;
      default:
        return a;
    }
  }

  public static Vector3 Operate(Vector3 a, Vector3 b, OperationMethod om, float delta = 1f)
  {
    switch (om)
    {
      case OperationMethod.Set:
        return b;
      case OperationMethod.Add:
        return a + b * delta;
      case OperationMethod.Subtract:
        return a - b * delta;
      case OperationMethod.Multiply:
        return Vector3.Scale(a, b * delta);
      case OperationMethod.Divide:
        b *= delta;
        return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
      default:
        return a;
    }
  }

  public static string GetCompareString(CompareMethod cm)
  {
    switch (cm)
    {
      case CompareMethod.EqualTo:
        return " == ";
      case CompareMethod.GreaterThan:
        return " > ";
      case CompareMethod.LessThan:
        return " < ";
      case CompareMethod.GreaterOrEqualTo:
        return " >= ";
      case CompareMethod.LessOrEqualTo:
        return " <= ";
      default:
        return string.Empty;
    }
  }

  public static bool Compare(float a, float b, CompareMethod cm, float floatingPoint)
  {
    switch (cm)
    {
      case CompareMethod.EqualTo:
        return (double) Mathf.Abs(a - b) <= (double) floatingPoint;
      case CompareMethod.GreaterThan:
        return (double) a > (double) b;
      case CompareMethod.LessThan:
        return (double) a < (double) b;
      case CompareMethod.GreaterOrEqualTo:
        return (double) a >= (double) b;
      case CompareMethod.LessOrEqualTo:
        return (double) a <= (double) b;
      default:
        return true;
    }
  }

  public static bool Compare(int a, int b, CompareMethod cm)
  {
    switch (cm)
    {
      case CompareMethod.EqualTo:
        return a == b;
      case CompareMethod.GreaterThan:
        return a > b;
      case CompareMethod.LessThan:
        return a < b;
      case CompareMethod.GreaterOrEqualTo:
        return a >= b;
      case CompareMethod.LessOrEqualTo:
        return a <= b;
      default:
        return true;
    }
  }
}
