using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace PRN222.Assignment.FPTURoomBooking.Services.Utils;

public static class Helper
{
    /// <summary>
    /// Combine two expressions with AndAlso operator (&&)
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Expression<Func<T, bool>> CombineAndAlsoExpressions<T>(this Expression<Func<T, bool>> first,
        Expression<Func<T, bool>> second)
    {
        return CombineExpressions(first, second, Expression.AndAlso);
    }

    /// <summary>
    /// Combine two expressions with OrElse operator (||)
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Expression<Func<T, bool>> CombineOrExpressions<T>(this Expression<Func<T, bool>> first,
        Expression<Func<T, bool>> second)
    {
        return CombineExpressions(first, second, Expression.OrElse);
    }

    private static Expression<Func<T, bool>> CombineExpressions<T>(this Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2, Func<Expression, Expression, BinaryExpression> combiner)
    {
        var parameter = Expression.Parameter(typeof(T));

        var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
        var left = leftVisitor.Visit(expr1.Body);

        var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
        var right = rightVisitor.Visit(expr2.Body);

        return Expression.Lambda<Func<T, bool>>(combiner(left, right), parameter);
    }

    // Apply initial sorting 
    public static IOrderedQueryable<T> ApplySorting<T>(this IQueryable<T> source, bool isDescending,
        Expression<Func<T, object>> sortProperty)
    {
        return isDescending ? source.OrderByDescending(sortProperty) : source.OrderBy(sortProperty);
    }

    // Apply more sorting
    public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, bool isDescending,
        Expression<Func<T, object>> sortProperty)
    {
        return isDescending ? source.ThenByDescending(sortProperty) : source.ThenBy(sortProperty);
    }

    public static long ToMilliseconds(this DateTime dateTime)
    {
        return new DateTimeOffset(dateTime).ToUnixTimeMilliseconds();
    }

    private static readonly string[] MemberNames = ["Error"];

    public static ValidationResult CreateValidationResult(string error)
    {
        return new ValidationResult(error, MemberNames);
    }

    public static bool IsNullOrGuidEmpty(this Guid? guid)
    {
        return guid == null || guid == Guid.Empty;
    }
}

internal class ReplaceExpressionVisitor(Expression oldValue, Expression newValue) : ExpressionVisitor
{
    public override Expression Visit(Expression node)
    {
        return node == oldValue ? newValue : base.Visit(node);
    }
}