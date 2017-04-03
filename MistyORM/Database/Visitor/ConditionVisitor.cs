using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using MySql.Data.MySqlClient;

namespace MistyORM.Database.Visitor
{
    public enum VisitType
    {
        None = 0,
        Unary = 1,
        Binary = 2,
        Member = 3,
        Constant = 4,
        Call = 5
    }

    public class ConditionVisitor
    {
        internal MySqlParameter[] Parameters => ParameterHolder.ToArray();
        private readonly List<MySqlParameter> ParameterHolder;
        private int ParameterCounter;

        internal string Conditions => ConditionBuilder.ToString();
        private readonly StringBuilder ConditionBuilder;

        internal bool Visited { get; private set; }

        private static readonly Dictionary<ExpressionType, string> ExpressionTypeMap = new Dictionary<ExpressionType, string>() 
        {
            { ExpressionType.AndAlso, " AND " },
            { ExpressionType.OrElse, " OR " },
            { ExpressionType.Equal, " = " }
        };

        internal ConditionVisitor()
        {
            ParameterHolder = new List<MySqlParameter>();
            ParameterCounter = 1;

            ConditionBuilder = new StringBuilder();

            Visited = false;
        }

        internal void Visit(Expression ExpressionBody)
        {
            if (ExpressionBody is LambdaExpression)
                ExpressionBody = (ExpressionBody as LambdaExpression).Body;

            switch (GetVisitDispatcher(ExpressionBody))
            {
                case VisitType.Unary:
                {
                    UnaryExpression Expression = ExpressionBody as UnaryExpression;

                    ConditionBuilder.Append("NOT (");

                    Visit(Expression.Operand);

                    ConditionBuilder.Append(")");
                    break;
                }
                case VisitType.Binary:
                {
                    BinaryExpression Expression = ExpressionBody as BinaryExpression;

                    ConditionBuilder.Append("(");

                    Visit(Expression.Left);

                    ConditionBuilder.Append(ExpressionTypeMap[ExpressionBody.NodeType]);

                    Visit(Expression.Right);

                    ConditionBuilder.Append(")");
                    break;
                }
                case VisitType.Member:
                {
                    MemberExpression Expression = ExpressionBody as MemberExpression;

                    if (Expression.Expression != null && Expression.Expression.NodeType == ExpressionType.Parameter)
                        ConditionBuilder.Append($"`{Expression.Member.Name}`");
                    else
                        AppendParameter(GetMemberExpressionValue(Expression));
                    break;
                }
                case VisitType.Constant:
                {
                    ConstantExpression Expression = ExpressionBody as ConstantExpression;

                    AppendParameter((Expression.Value ?? string.Empty).ToString());
                    break;
                }
            }

            Visited = true;
        }

        private string GetMemberExpressionValue(MemberExpression Expression)
        {
            if (Expression.Expression == null)
                return GetValue(Expression.Member, null);
            else
            {
                MemberExpression NestedExpression = Expression.Expression as MemberExpression;
                while (NestedExpression is MemberExpression)
                    NestedExpression = NestedExpression.Expression as MemberExpression;

                ConstantExpression ConstantExpression = Expression.Expression as ConstantExpression;
                if (ConstantExpression != null)
                    return GetValue(Expression.Member, ConstantExpression.Value.GetType().GetRuntimeField(NestedExpression.Member.Name).GetValue(ConstantExpression.Value));

                return GetValue(Expression.Member, null);
            }
        }

        private string GetValue(MemberInfo Member, object Reference)
        {
            FieldInfo FieldInfo = Member as FieldInfo;
            PropertyInfo PropertyInfo = Member as PropertyInfo;

            return ToDbFormat(FieldInfo?.GetValue(Reference) ?? PropertyInfo.GetValue(Reference), FieldInfo?.FieldType ?? PropertyInfo.PropertyType); 
        }

        private string ToDbFormat(object Input, Type Type = null)
        {
            if (Input is bool)
                return Convert.ToByte(Input).ToString();
            else if (Type.GetTypeInfo().IsEnum)
                return Convert.ChangeType(Input, (Type ?? Input.GetType()).GetTypeInfo().GetEnumUnderlyingType()).ToString();

            return Convert.ChangeType(Input, Type ?? Input.GetType()).ToString();
        }

        private void AppendParameter(string Value)
        {
            ConditionBuilder.Append($"@{ParameterCounter}");
            ParameterHolder.Add(new MySqlParameter
            {
                ParameterName = "@" + ParameterCounter,
                Value = Value
            });

            ++ParameterCounter;
        }

        private VisitType GetVisitDispatcher(Expression Expression)
        {
            switch (Expression.NodeType)
            {
                case ExpressionType.Not:
                    return VisitType.Unary;
                case ExpressionType.AndAlso:
                case ExpressionType.OrElse:
                case ExpressionType.Equal:
                    return VisitType.Binary;
                case ExpressionType.MemberAccess:
                    return VisitType.Member;
                case ExpressionType.Constant:
                    return VisitType.Constant;
                default:
                    throw new NotSupportedException($"{Expression.NodeType} is not supported.");
            }
        }
    }
}
