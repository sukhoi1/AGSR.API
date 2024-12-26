using System.Linq.Expressions;
using AGSR.TestTask.Models;

namespace AGSR.TestTask.DynamicQueryExpressions;

public static class DynamicExpressions
{
    public static Expression<Func<PatientModel, bool>> CreateEQExpression(ParameterExpression param, MemberExpression member, DateTimeOffset value)
    {
        var expression = GetEQExpression(value, member);
        return Expression.Lambda<Func<PatientModel, bool>>(expression, param);
    }

    public static Expression<Func<PatientModel, bool>> CreateNEExpression(ParameterExpression param, MemberExpression member, DateTimeOffset value)
    {
        var expression = Expression.Not(GetEQExpression(value, member));
        return Expression.Lambda<Func<PatientModel, bool>>(expression, param);
    }

    private static Expression GetEQExpression(DateTimeOffset value, MemberExpression member)
    {
        var constantA = Expression.Constant(value.UtcDateTime.Date);
        var expressionA = Expression.GreaterThanOrEqual(member, constantA);

        var constantB = Expression.Constant(value.UtcDateTime.AddDays(1).Date);
        var expressionB = Expression.LessThan(member, constantB);

        return Expression.And(expressionA, expressionB);
    }
}
