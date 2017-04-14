using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using MySql.Data.MySqlClient;

namespace MistyORM.Database.Visitors
{
    internal sealed class ConditionVisitor : ExpressionVisitor
    {
        public IEnumerable<MySqlParameter> GetParameters() => ParameterHolder;
        private readonly List<MySqlParameter> ParameterHolder;

        private int ParameterIdentifier;

        public string GetConditions() => ConditionBuilder.ToString();
        private readonly StringBuilder ConditionBuilder;

        public ConditionVisitor()
        {
            ParameterHolder = new List<MySqlParameter>();
            ParameterIdentifier = 0;

            ConditionBuilder = new StringBuilder();
        }

        protected override Expression VisitUnary(UnaryExpression Expression)
        {
            ConditionBuilder.Append("NOT (");

            Visit(Expression.Operand);

            ConditionBuilder.Append(")");

            return Expression;
        }

        protected override Expression VisitBinary(BinaryExpression Expression)
        {
            ConditionBuilder.Append("(");

            Visit(Expression.Left);

            ConditionBuilder.Append(ExpressionTypeMap[Expression.NodeType]);

            Visit(Expression.Right);

            ConditionBuilder.Append(")");

            return Expression;
        }

        protected override Expression VisitMember(MemberExpression Expression)
        {
            if (Expression.Expression != null && Expression.Expression.NodeType == ExpressionType.Parameter)
                ConditionBuilder.Append($"`{Expression.Member.Name}`");
            else
                AppendParameter(GetMemberExpressionValue(Expression));

            return Expression;
        }

        protected override Expression VisitConstant(ConstantExpression Expression)
        {
            AppendParameter((Expression.Value ?? string.Empty).ToString());

            return Expression;
        }

        protected override Expression VisitMethodCall(MethodCallExpression Expression)
        {
            throw new NotImplementedException("Visity type 'Call' is not implemented.");
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

            string GetValue(MemberInfo Member, object Reference)
            {
                FieldInfo FieldInfo = Member as FieldInfo;
                PropertyInfo PropertyInfo = Member as PropertyInfo;

                return ToDbFormat(FieldInfo?.GetValue(Reference) ?? PropertyInfo.GetValue(Reference), FieldInfo?.FieldType ?? PropertyInfo.PropertyType);
            }
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
            ConditionBuilder.Append($"@{++ParameterIdentifier}");
            ParameterHolder.Add(new MySqlParameter
            {
                ParameterName = "@" + ParameterIdentifier,
                Value = Value
            });
        }

        private static readonly IDictionary<ExpressionType, string> ExpressionTypeMap = new Dictionary<ExpressionType, string>()
        {
            // todo: implement everything else
            { ExpressionType.AndAlso, " AND " },
            { ExpressionType.OrElse, " OR " },
            { ExpressionType.Equal, " = " },
        };
    }
}
