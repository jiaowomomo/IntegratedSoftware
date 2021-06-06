using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using System.ComponentModel;
using System.Linq;

namespace CommonLibrary.ExtensionUtils
{
    public static class IQueryableUtils
    {
        public static BindingList<T> ToBindingList<T>(this List<T> source)
        {
            return new BindingList<T>(source.ToList());
        }

        public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate, bool condition)
        {
            return condition ? source.Where(predicate) : source;
        }

        public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, Expression<Func<T, int, bool>> predicate, bool condition)
        {
            return condition ? source.Where(predicate) : source;
        }

        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, Func<T, bool> predicate, bool condition)
        {
            return condition ? source.Where(predicate) : source;
        }

        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, Func<T, int, bool> predicate, bool condition)
        {
            return condition ? source.Where(predicate) : source;
        }

        public static bool ContainsStartWith(this IEnumerable<string> source, string item)
        {
            return source.Where(obj => obj.StartsWith(item)).Count() > 0;
        }

        public static double[] Average<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int[]>> selectors)
        {
            var exps = (selectors.Body as NewArrayExpression).Expressions;
            var lst = new List<double>();
            foreach (var item in exps)
            {
                var exp = Expression.Lambda(item, selectors.Parameters);
                dynamic selector = source.Average(exp as Expression<Func<TSource, int>>);
                lst.Add(selector);
            }
            return lst.ToArray();
        }

        public static float[] Average<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, float[]>> selectors)
        {
            var exps = (selectors.Body as NewArrayExpression).Expressions;
            var lst = new List<float>();
            foreach (var item in exps)
            {
                var exp = Expression.Lambda(item, selectors.Parameters);
                dynamic selector = source.Average(exp as Expression<Func<TSource, decimal>>);
                lst.Add(selector);
            }
            return lst.ToArray();
        }

        public static float?[] Average<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, float?[]>> selectors)
        {
            var exps = (selectors.Body as NewArrayExpression).Expressions;
            var lst = new List<float?>();
            foreach (var item in exps)
            {
                var exp = Expression.Lambda(item, selectors.Parameters);
                dynamic selector = source.Average(exp as Expression<Func<TSource, decimal>>);
                lst.Add(selector);
            }
            return lst.ToArray();
        }

        public static double[] Average<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, long[]>> selectors)
        {
            var exps = (selectors.Body as NewArrayExpression).Expressions;
            var lst = new List<double>();
            foreach (var item in exps)
            {
                var exp = Expression.Lambda(item, selectors.Parameters);
                dynamic selector = source.Average(exp as Expression<Func<TSource, decimal>>);
                lst.Add(selector);
            }
            return lst.ToArray();
        }

        public static double?[] Average<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, long?[]>> selectors)
        {
            var exps = (selectors.Body as NewArrayExpression).Expressions;
            var lst = new List<double?>();
            foreach (var item in exps)
            {
                var exp = Expression.Lambda(item, selectors.Parameters);
                dynamic selector = source.Average(exp as Expression<Func<TSource, decimal>>);
                lst.Add(selector);
            }
            return lst.ToArray();
        }

        public static double[] Average<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, double[]>> selectors)
        {
            var exps = (selectors.Body as NewArrayExpression).Expressions;
            var lst = new List<double>();
            foreach (var item in exps)
            {
                var exp = Expression.Lambda(item, selectors.Parameters);
                dynamic selector = source.Average(exp as Expression<Func<TSource, decimal>>);
                lst.Add(selector);
            }
            return lst.ToArray();
        }

        public static double?[] Average<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, double?[]>> selectors)
        {
            var exps = (selectors.Body as NewArrayExpression).Expressions;
            var lst = new List<double?>();
            foreach (var item in exps)
            {
                var exp = Expression.Lambda(item, selectors.Parameters);
                dynamic selector = source.Average(exp as Expression<Func<TSource, decimal>>);
                lst.Add(selector);
            }
            return lst.ToArray();
        }

        public static decimal[] Average<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, decimal[]>> selectors)
        {
            var exps = (selectors.Body as NewArrayExpression).Expressions;
            var lst = new List<decimal>();
            foreach (var item in exps)
            {
                var exp = Expression.Lambda(item, selectors.Parameters);
                dynamic selector = source.Average(exp as Expression<Func<TSource, decimal>>);
                lst.Add(selector);
            }
            return lst.ToArray();
        }

        public static decimal?[] Average<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, decimal?[]>> selectors)
        {
            var exps = (selectors.Body as NewArrayExpression).Expressions;
            var lst = new List<decimal?>();
            foreach (var item in exps)
            {
                var exp = Expression.Lambda(item, selectors.Parameters);
                dynamic selector = source.Average(exp as Expression<Func<TSource, decimal>>);
                lst.Add(selector);
            }
            return lst.ToArray();
        }

        public static double?[] Average<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int?[]>> selectors)
        {
            var exps = (selectors.Body as NewArrayExpression).Expressions;
            var lst = new List<double?>();
            foreach (var item in exps)
            {
                var exp = Expression.Lambda(item, selectors.Parameters);
                dynamic selector = source.Average(exp as Expression<Func<TSource, decimal>>);
                lst.Add(selector);
            }
            return lst.ToArray();
        }

        /// <summary>
        /// 单波峰形态序列的上升速率
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="valueSelector"></param>
        /// <param name="dateTimeSelector"></param>
        /// <returns></returns>
        public static decimal SinglePeakRaiseRate<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, decimal>> valueSelector, Expression<Func<TSource, DateTime>> dateTimeSelector)
        {
            var para = valueSelector.Parameters;
            var value = valueSelector.Body;
            var valueLambda = Expression.Lambda(value, para) as Expression<Func<TSource, decimal>>;
            var dateTime = valueSelector.CloneBody(dateTimeSelector);
            var dateTimeLambda = Expression.Lambda(dateTime, para) as Expression<Func<TSource, DateTime>>;

            source = source.OrderBy(dateTimeLambda);
            var maxRecordValue = source.Max(valueLambda);
            var exp = Expression.Equal(value, Expression.Constant(maxRecordValue));
            var selector = Expression.Lambda(exp, para);
            var maxRecordDateTime = source.Where(selector as Expression<Func<TSource, bool>>).Select(dateTimeLambda).FirstOrDefault();

            var minExp = Expression.Lambda(Expression.LessThan(dateTime, Expression.Constant(maxRecordDateTime)), para);
            var startRange = source.Where(minExp as Expression<Func<TSource, bool>>);
            if (startRange.Count() == 0)
            {
                return 0;
            }
            var startRecordValue = startRange.Min(valueLambda);
            var startRecordExp = Expression.And(minExp.Body, Expression.Equal(value, Expression.Constant(startRecordValue)));
            var lam = Expression.Lambda(startRecordExp, para);
            var startDateTime = source.Where(lam as Expression<Func<TSource, bool>>).OrderByDescending(dateTimeLambda).Select(dateTimeLambda).FirstOrDefault();
            var span = maxRecordDateTime - startDateTime;
            return Math.Abs((maxRecordValue - startRecordValue)) / span.Minutes;
        }

        /// <summary>
        /// 单波峰形态序列的下降速率
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="valueSelector"></param>
        /// <param name="dateTimeSelector"></param>
        /// <returns></returns>
        public static decimal SinglePeakReduceRate<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, decimal>> valueSelector, Expression<Func<TSource, DateTime>> dateTimeSelector)
        {
            var para = valueSelector.Parameters;
            var value = valueSelector.Body;
            var valueLambda = Expression.Lambda(value, para) as Expression<Func<TSource, decimal>>;
            var dateTime = valueSelector.CloneBody(dateTimeSelector);
            var dateTimeLambda = Expression.Lambda(dateTime, para) as Expression<Func<TSource, DateTime>>;

            source = source.OrderBy(dateTimeLambda);
            var maxRecordValue = source.Max(valueLambda);
            var exp = Expression.Equal(value, Expression.Constant(maxRecordValue));
            var selector = Expression.Lambda(exp, para);
            var maxRecordDateTime = source.Where(selector as Expression<Func<TSource, bool>>).Select(dateTimeLambda).FirstOrDefault();

            var minExp = Expression.Lambda(Expression.GreaterThan(dateTime, Expression.Constant(maxRecordDateTime)), para);
            var endRange = source.Where(minExp as Expression<Func<TSource, bool>>);
            if (endRange.Count() == 0)
            {
                return 0;
            }
            var endRecordValue = source.Where(minExp as Expression<Func<TSource, bool>>).Min(valueLambda);
            var endRecordExp = Expression.And(minExp.Body, Expression.Equal(value, Expression.Constant(endRecordValue)));
            var lam = Expression.Lambda(endRecordExp, para);
            var endDateTime = source.Where(lam as Expression<Func<TSource, bool>>).OrderByDescending(dateTimeLambda).Select(dateTimeLambda).FirstOrDefault();
            var span = maxRecordDateTime - endDateTime;
            return Math.Abs((maxRecordValue - endRecordValue)) / span.Minutes;
        }

        public static Expression CloneBody<TSource, T1, T2>(this Expression<Func<TSource, T1>> source, Expression<Func<TSource, T2>> from)
        {
            var map = source.Parameters
                .Select((f, i) => new { f, s = from.Parameters[i] })
                .ToDictionary(p => p.s, p => p.f);

            return PredicateBuilder.ParameterRebinder.ReplaceParameters(map, from.Body);
        }

        /// <summary>
        /// 单波谷形态序列的上升速率
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="valueSelector"></param>
        /// <param name="dateTimeSelector"></param>
        /// <returns></returns>
        public static decimal SingleTroughRaiseRate<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, decimal>> valueSelector, Expression<Func<TSource, DateTime>> dateTimeSelector)
        {
            var para = valueSelector.Parameters;
            var value = valueSelector.Body;
            var valueLambda = Expression.Lambda(value, para) as Expression<Func<TSource, decimal>>;
            var dateTime = valueSelector.CloneBody(dateTimeSelector);
            var dateTimeLambda = Expression.Lambda(dateTime, para) as Expression<Func<TSource, DateTime>>;

            //1.按时间排序
            source = source.OrderBy(dateTimeLambda);
            //2.取得波谷最小值
            var minRecordValue = source.Min(valueLambda);
            //3.取得波谷最小值时间
            var exp = Expression.Equal(value, Expression.Constant(minRecordValue));
            var selector = Expression.Lambda(exp, para) as Expression<Func<TSource, bool>>;
            var minRecordDateTime = source.Where(selector).Select(dateTimeLambda).FirstOrDefault();
            //4.获取波谷左侧最大值作为起始点
            var maxExp = Expression.Lambda(Expression.LessThan(dateTime, Expression.Constant(minRecordDateTime)), para);
            var startRange = source.Where(maxExp as Expression<Func<TSource, bool>>);
            if (startRange.Count() == 0) {
                return 0;
            }
            var startRecordValue = startRange.Max(valueLambda);

            var startRecordExp = Expression.And(maxExp.Body, Expression.Equal(value, Expression.Constant(startRecordValue)));

            var lam = Expression.Lambda(startRecordExp, para);

            var startDateTime = source.Where(lam as Expression<Func<TSource, bool>>).OrderByDescending(dateTimeLambda).Select(dateTimeLambda).FirstOrDefault();
            var span = minRecordDateTime - startDateTime;
            return Math.Abs((minRecordValue - startRecordValue)) / span.Minutes;
        }

        /// <summary>
        /// 单波谷形态序列的下降速率
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="valueSelector"></param>
        /// <param name="dateTimeSelector"></param>
        /// <returns></returns>
        public static decimal SingleTroughReduceRate<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, decimal>> valueSelector, Expression<Func<TSource, DateTime>> dateTimeSelector)
        {
            var para = valueSelector.Parameters;
            var value = valueSelector.Body;
            var valueLambda = Expression.Lambda(value, para) as Expression<Func<TSource, decimal>>;
            var dateTime = valueSelector.CloneBody(dateTimeSelector);
            var dateTimeLambda = Expression.Lambda(dateTime, para) as Expression<Func<TSource, DateTime>>;

            var maxRecordValue = source.Max(valueLambda);
            var exp = Expression.Equal(value, Expression.Constant(maxRecordValue));
            var selector = Expression.Lambda(exp, para) as Expression<Func<TSource, bool>>;
            var maxRecordDateTime = source.Where(selector).Select(dateTimeLambda).FirstOrDefault();

            var minExp = Expression.Lambda(Expression.GreaterThan(dateTime, Expression.Constant(maxRecordDateTime)), para);
            var endRange = source.Where(minExp as Expression<Func<TSource, bool>>);
            if (endRange.Count() == 0)
            {
                return 0;
            }
            var endRecordValue = endRange.Max(valueLambda);
            var endRecordExp = Expression.And(minExp.Body, Expression.Equal(value, Expression.Constant(endRecordValue)));
            var lam = Expression.Lambda(endRecordExp, para);
            var endDateTime = source.Where(lam as Expression<Func<TSource, bool>>).OrderByDescending(dateTimeLambda).Select(dateTimeLambda).FirstOrDefault();
            var span = maxRecordDateTime - endDateTime;
            return Math.Abs((maxRecordValue - endRecordValue)) / span.Minutes;
        }

        /// <summary>
        /// 计算dynamic（必须为值类型）未排序序列(抽取的样本)的样本方差（无偏总体方差、无偏估计、无偏方差）。
        /// 对大小为N的数据集，使用N-1进行标准化。
        /// </summary>
        /// <param name="source">简单的未排序的值序列。</param>
        /// <param name="selector">要应用于每个元素的投影函数。</param>
        /// <returns>值序列的样本方差，如果数据连小于2，或者数据为NaN，则返回NaN</returns>
        public static decimal SampleVariance<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, decimal>> selector)
        {
            var samples = source.Select(selector).ToArray();
            if (!selector.Body.Type.IsValueType)
            {
                throw new ArgumentException("传入的类型必须为值类型！");
            }
            if (samples.Length <= 1)
            {
                throw new ArgumentException("传入的数组长度必须大于1！");
            }

            double variance = 0;
            double t = samples[0].ToDouble();
            for (int i = 1; i < samples.Length; i++)
            {
                t += samples[i].ToDouble();
                double diff = ((i + 1) * samples[i].ToDouble()) - t;
                variance += (diff * diff) / ((i + 1.0) * i);
            }

            return (variance / (samples.Length - 1)).ToDecimal();
        }

        /// <summary>
        /// 计算dynamic（必须为值类型）未排序序列的总体方差（有偏总体方差、有偏估计、有偏方差）。
        /// 对大小为N的数据集，使用N进行标准化，因此是有偏差的。
        /// </summary>
        /// <param name="source">简单的未排序的值序列。</param>
        /// <param name="selector">要应用于每个元素的投影函数。</param>
        /// <returns>值序列的总体方差，如果数据为NaN，则返回NaN</returns>
        public static decimal Variance<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, decimal>> selector)
        {
            var population = source.Select(selector).ToArray();
            if (!selector.Body.Type.IsValueType)
            {
                throw new ArgumentException("传入的类型必须为值类型！");
            }
            if (population.Length == 0)
            {
                throw new ArgumentException("传入的数组长度必须大于1！");
            }

            double variance = 0;
            double t = population[0].ToDouble();
            for (int i = 1; i < population.Length; i++)
            {
                t += population[i].ToDouble();
                double diff = ((i + 1) * population[i].ToDouble()) - t;
                variance += (diff * diff) / ((i + 1.0) * i);
            }

            return (variance / population.Length).ToDecimal();
        }

        /// <summary>
        /// 计算dynamic（必须为值类型）未排序序列(抽取的样本)的样本总体标准方差（无偏总体标准方差、无偏标准方差）：
        /// 对大小为N的数据集，使用N-1进行标准化.
        /// </summary>
        /// <param name="source">简单的未排序的值序列。</param>
        /// <param name="selector">要应用于每个元素的投影函数。</param>
        /// <returns>值序列的样本总体标准方差，如果数据连小于2，或者数据为NaN，则返回NaN</returns>
        public static decimal SampleStandardDeviation<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, decimal>> selector)
        {
            return Math.Sqrt(SampleVariance(source, selector).ToDouble()).ToDecimal();
        }

        /// <summary>
        /// 计算dynamic（必须为值类型）未排序序列的总体标准差
        /// 对大小为N的数据集，使用N进行标准化.
        /// </summary>
        /// <param name="source">简单的未排序的值序列。</param>
        /// <param name="selector">要应用于每个元素的投影函数。</param>
        /// <returns>值序列的总体标准差，如果数据为NaN，则返回NaN.</returns>
        public static decimal StandardDeviation<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, decimal>> selector)
        {
            return Math.Sqrt(Variance(source, selector).ToDouble()).ToDecimal();
        }
    }

    /// <summary>
    /// Enables the efficient, dynamic composition of query predicates.
    /// </summary>
    public static class PredicateBuilder
    {
        /// <summary>
        /// Creates a predicate that evaluates to true.
        /// </summary>
        public static Expression<Func<T, bool>> True<T>() { return param => true; }

        /// <summary>
        /// Creates a predicate that evaluates to false.
        /// </summary>
        public static Expression<Func<T, bool>> False<T>() { return param => false; }

        /// <summary>
        /// Creates a predicate expression from the specified lambda expression.
        /// </summary>
        public static Expression<Func<T, bool>> Create<T>(Expression<Func<T, bool>> predicate) { return predicate; }

        /// <summary>
        /// Combines the first predicate with the second using the logical "and".
        /// </summary>
        public static Expression<Func<T, bool>> AndDynamic<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.AndAlso);
        }

        /// <summary>
        /// Combines the first predicate with the second using the logical "or".
        /// </summary>
        public static Expression<Func<T, bool>> OrDynamic<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.OrElse);
        }

        /// <summary>
        /// Negates the predicate.
        /// </summary>
        public static Expression<Func<T, bool>> NotDynamic<T>(this Expression<Func<T, bool>> expression)
        {
            var negated = Expression.Not(expression.Body);
            return Expression.Lambda<Func<T, bool>>(negated, expression.Parameters);
        }

        /// <summary>
        /// Combines the first expression with the second using the specified merge function.
        /// </summary>
        static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            // zip parameters (map from parameters of second to parameters of first)
            var map = first.Parameters
                .Select((f, i) => new { f, s = second.Parameters[i] })
                .ToDictionary(p => p.s, p => p.f);

            // replace parameters in the second lambda expression with the parameters in the first
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            // create a merged lambda expression with parameters from the first expression
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        /// <summary>
        /// ParameterRebinder
        /// </summary>
        public class ParameterRebinder : ExpressionVisitor
        {
            /// <summary>
            /// The ParameterExpression map
            /// </summary>
            readonly Dictionary<ParameterExpression, ParameterExpression> map;

            /// <summary>
            /// Initializes a new instance of the <see cref="ParameterRebinder"/> class.
            /// </summary>
            /// <param name="map">The map.</param>
            ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
            {
                this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
            }

            /// <summary>
            /// Replaces the parameters.
            /// </summary>
            /// <param name="map">The map.</param>
            /// <param name="exp">The exp.</param>
            /// <returns>Expression</returns>
            public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
            {
                return new ParameterRebinder(map).Visit(exp);
            }

            /// <summary>
            /// Visits the parameter.
            /// </summary>
            /// <param name="p">The p.</param>
            /// <returns>Expression</returns>
            protected override Expression VisitParameter(ParameterExpression p)
            {
                ParameterExpression replacement;

                if (map.TryGetValue(p, out replacement))
                {
                    p = replacement;
                }

                return base.VisitParameter(p);
            }
        }
    }
}
