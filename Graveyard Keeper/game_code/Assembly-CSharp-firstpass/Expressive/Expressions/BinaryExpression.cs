// Decompiled with JetBrains decompiler
// Type: Expressive.Expressions.BinaryExpression
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using Expressive.Exceptions;
using Expressive.Helpers;
using LinqTools;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Expressive.Expressions;

public class BinaryExpression : IExpression
{
  public BinaryExpressionType _expressionType;
  public IExpression _leftHandSide;
  public IExpression _rightHandSide;

  public BinaryExpression(BinaryExpressionType type, IExpression lhs, IExpression rhs)
  {
    this._expressionType = type;
    this._leftHandSide = lhs;
    this._rightHandSide = rhs;
  }

  public object Evaluate(IDictionary<string, object> variables)
  {
    if (this._leftHandSide == null)
      throw new MissingParticipantException("The left hand side of the operation is missing.");
    if (this._rightHandSide == null)
      throw new MissingParticipantException("The right hand side of the operation is missing.");
    object obj = this._leftHandSide.Evaluate(variables);
    switch (this._expressionType)
    {
      case BinaryExpressionType.And:
        return this.Evaluate(obj, this._rightHandSide, variables, (Func<object, object, object>) ((l, r) => !Convert.ToBoolean(l) ? (object) false : (Convert.ToBoolean(r) ? (object) true : (object) false)));
      case BinaryExpressionType.Or:
        return this.Evaluate(obj, this._rightHandSide, variables, (Func<object, object, object>) ((l, r) => Convert.ToBoolean(l) ? (object) true : (Convert.ToBoolean(r) ? (object) true : (object) false)));
      case BinaryExpressionType.NotEqual:
        if (obj == null)
          return this._rightHandSide.Evaluate(variables) != null ? (object) true : (object) false;
        object b1 = this._rightHandSide.Evaluate(variables);
        return b1 == null ? (object) true : (object) (Comparison.CompareUsingMostPreciseType(obj, b1) != 0);
      case BinaryExpressionType.LessThanOrEqual:
        if (obj == null)
          return (object) null;
        object b2 = this._rightHandSide.Evaluate(variables);
        return b2 == null ? (object) null : (object) (Comparison.CompareUsingMostPreciseType(obj, b2) <= 0);
      case BinaryExpressionType.GreaterThanOrEqual:
        if (obj == null)
          return (object) null;
        object b3 = this._rightHandSide.Evaluate(variables);
        return b3 == null ? (object) null : (object) (Comparison.CompareUsingMostPreciseType(obj, b3) >= 0);
      case BinaryExpressionType.LessThan:
        if (obj == null)
          return (object) null;
        object b4 = this._rightHandSide.Evaluate(variables);
        return b4 == null ? (object) null : (object) (Comparison.CompareUsingMostPreciseType(obj, b4) < 0);
      case BinaryExpressionType.GreaterThan:
        if (obj == null)
          return (object) null;
        object b5 = this._rightHandSide.Evaluate(variables);
        return b5 == null ? (object) null : (object) (Comparison.CompareUsingMostPreciseType(obj, b5) > 0);
      case BinaryExpressionType.Equal:
        if (obj == null)
          return this._rightHandSide.Evaluate(variables) == null ? (object) true : (object) false;
        object b6 = this._rightHandSide.Evaluate(variables);
        return b6 == null ? (object) false : (object) (Comparison.CompareUsingMostPreciseType(obj, b6) == 0);
      case BinaryExpressionType.Subtract:
        return this.Evaluate(obj, this._rightHandSide, variables, (Func<object, object, object>) ((l, r) => Numbers.Subtract(l, r)));
      case BinaryExpressionType.Add:
        return obj is string ? (object) ((string) obj + this._rightHandSide.Evaluate(variables)?.ToString()) : this.Evaluate(obj, this._rightHandSide, variables, (Func<object, object, object>) ((l, r) => Numbers.Add(l, r)));
      case BinaryExpressionType.Modulus:
        return this.Evaluate(obj, this._rightHandSide, variables, (Func<object, object, object>) ((l, r) => Numbers.Modulus(l, r)));
      case BinaryExpressionType.Divide:
        this._rightHandSide.Evaluate(variables);
        return this.Evaluate(obj, this._rightHandSide, variables, (Func<object, object, object>) ((l, r) => l != null && r != null && !BinaryExpression.IsReal(l) && !BinaryExpression.IsReal(r) ? Numbers.Divide((object) Convert.ToDouble(l), r) : Numbers.Divide(l, r)));
      case BinaryExpressionType.Multiply:
        return this.Evaluate(obj, this._rightHandSide, variables, (Func<object, object, object>) ((l, r) => Numbers.Multiply(l, r)));
      case BinaryExpressionType.BitwiseOr:
        return this.Evaluate(obj, this._rightHandSide, variables, (Func<object, object, object>) ((l, r) => (object) ((int) Convert.ToUInt16(l) | (int) Convert.ToUInt16(r))));
      case BinaryExpressionType.BitwiseAnd:
        return this.Evaluate(obj, this._rightHandSide, variables, (Func<object, object, object>) ((l, r) => (object) ((int) Convert.ToUInt16(l) & (int) Convert.ToUInt16(r))));
      case BinaryExpressionType.BitwiseXOr:
        return this.Evaluate(obj, this._rightHandSide, variables, (Func<object, object, object>) ((l, r) => (object) ((int) Convert.ToUInt16(l) ^ (int) Convert.ToUInt16(r))));
      case BinaryExpressionType.LeftShift:
        return this.Evaluate(obj, this._rightHandSide, variables, (Func<object, object, object>) ((l, r) => (object) ((int) Convert.ToUInt16(l) << (int) Convert.ToUInt16(r))));
      case BinaryExpressionType.RightShift:
        return this.Evaluate(obj, this._rightHandSide, variables, (Func<object, object, object>) ((l, r) => (object) ((int) Convert.ToUInt16(l) >> (int) Convert.ToUInt16(r))));
      case BinaryExpressionType.NullCoalescing:
        return this.Evaluate(obj, this._rightHandSide, variables, (Func<object, object, object>) ((l, r) => l ?? r));
      default:
        return (object) null;
    }
  }

  public static bool IsReal(object value)
  {
    TypeCode typeCode = ReflectionTools.GetTypeCode(value);
    switch (typeCode)
    {
      case TypeCode.Double:
      case TypeCode.Decimal:
        return true;
      default:
        return typeCode == TypeCode.Single;
    }
  }

  public object Evaluate(
    object lhsResult,
    IExpression rhs,
    IDictionary<string, object> variables,
    Func<object, object, object> resultSelector)
  {
    IList<object> objectList1 = (IList<object>) new List<object>();
    IList<object> objectList2 = (IList<object>) new List<object>();
    object obj1 = rhs.Evaluate(variables);
    if (!(lhsResult is IEnumerable) && !(obj1 is IEnumerable))
      return resultSelector(lhsResult, obj1);
    if (lhsResult is IEnumerable)
    {
      foreach (object obj2 in (IEnumerable) lhsResult)
        objectList1.Add(obj2);
    }
    if (obj1 is IEnumerable)
    {
      foreach (object obj3 in (IEnumerable) obj1)
        objectList2.Add(obj3);
    }
    object[] objArray = (object[]) null;
    if (objectList1.Count == objectList2.Count)
    {
      IList<object> source = (IList<object>) new List<object>();
      for (int index = 0; index < objectList1.Count; ++index)
        source.Add(resultSelector(objectList1[index], objectList2[index]));
      objArray = source.ToArray<object>();
    }
    else if (objectList1.Count == 0)
    {
      IList<object> source = (IList<object>) new List<object>();
      for (int index = 0; index < objectList2.Count; ++index)
        source.Add(resultSelector(lhsResult, objectList2[index]));
      objArray = source.ToArray<object>();
    }
    else if (objectList2.Count == 0)
    {
      IList<object> source = (IList<object>) new List<object>();
      for (int index = 0; index < objectList1.Count; ++index)
        source.Add(resultSelector(objectList1[index], obj1));
      objArray = source.ToArray<object>();
    }
    return (object) objArray;
  }
}
