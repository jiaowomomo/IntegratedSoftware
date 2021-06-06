using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.ExtensionUtils
{
    /// <summary>
    /// 利用表达式树实现高效深复制
    /// </summary>
    public static class DeepCopyByExpressionTrees
    {
        private static readonly object IsStructTypeToDeepCopyDictionaryLocker = new object();
        private static Dictionary<Type, bool> IsStructTypeToDeepCopyDictionary = new Dictionary<Type, bool>();

        private static readonly object CompiledCopyFunctionsDictionaryLocker = new object();
        private static Dictionary<Type, Func<object, Dictionary<object, object>, object>> CompiledCopyFunctionsDictionary =
            new Dictionary<Type, Func<object, Dictionary<object, object>, object>>();

        private static readonly Type ObjectType = typeof(Object);
        private static readonly Type ObjectDictionaryType = typeof(Dictionary<object, object>);

        /// <summary>
        /// 创建对象的深度拷贝
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="original">要复制的对象</param>
        /// <param name="copiedReferencesDict">已复制对象的字典（原始对象，拷贝）</param>
        /// <returns></returns>
        public static T DeepCopy<T>(this T original, Dictionary<object, object> copiedReferencesDict = null)
        {
            return (T)DeepCopyByExpressionTreeObj(original, false, copiedReferencesDict ?? new Dictionary<object, object>(new ReferenceEqualityComparer()));
        }

        private static object DeepCopyByExpressionTreeObj(object original, bool forceDeepCopy, Dictionary<object, object> copiedReferencesDict)
        {
            if (original == null)
            {
                return null;
            }
            var type = original.GetType();
            if (IsDelegate(type))
            {
                return null;
            }
            if (!forceDeepCopy && !IsTypeToDeepCopy(type))
            {
                return original;
            }
            object alreadyCopiedObject;
            if (copiedReferencesDict.TryGetValue(original, out alreadyCopiedObject))
            {
                return alreadyCopiedObject;
            }
            if (type == ObjectType)
            {
                return new object();
            }
            var compiledCopyFunction = GetOrCreateCompiledLambdaCopyFunction(type);
            object copy = compiledCopyFunction(original, copiedReferencesDict);
            return copy;
        }

        private static Func<object, Dictionary<object, object>, object> GetOrCreateCompiledLambdaCopyFunction(Type type)
        {
            // 以下机构确保多个线程使用字典，即使字典被锁定且被其他线程更新
            // 不修改旧的字典，每次都用新的实例替换
            Func<object, Dictionary<object, object>, object> compiledCopyFunction;
            if (!CompiledCopyFunctionsDictionary.TryGetValue(type, out compiledCopyFunction))
            {
                lock (CompiledCopyFunctionsDictionaryLocker)
                {
                    if (!CompiledCopyFunctionsDictionary.TryGetValue(type, out compiledCopyFunction))
                    {
                        var uncompiledCopyFunction = CreateCompiledLambdaCopyFunctionForType(type);

                        compiledCopyFunction = uncompiledCopyFunction.Compile();

                        var dictionaryCopy = CompiledCopyFunctionsDictionary.ToDictionary(pair => pair.Key, pair => pair.Value);

                        dictionaryCopy.Add(type, compiledCopyFunction);

                        CompiledCopyFunctionsDictionary = dictionaryCopy;
                    }
                }
            }
            return compiledCopyFunction;
        }

        private static Expression<Func<object, Dictionary<object, object>, object>> CreateCompiledLambdaCopyFunctionForType(Type type)
        {
            ParameterExpression inputParameter;
            ParameterExpression inputDictionary;
            ParameterExpression outputVariable;
            ParameterExpression boxingVariable;
            LabelTarget endLabel;
            List<ParameterExpression> variables;
            List<Expression> expressions;
            // 初始化表达式和变量
            InitializeExpressions(type, out inputParameter, out inputDictionary, out outputVariable, out boxingVariable, out endLabel, out variables, out expressions);
            // 如果原始对象为空，则直接返回空值
            IfNullThenReturnNullExpression(inputParameter, endLabel, expressions);
            // 浅拷贝原始对象
            MemberwiseCloneInputToOutputExpression(type, inputParameter, outputVariable, expressions);
            // 将复制的对象存储到字典            
            if (IsClassOtherThanString(type))
            {
                StoreReferencesIntoDictionaryExpression(inputParameter, inputDictionary, outputVariable, expressions);
            }
            // 拷贝所有非值或非系统类型字段
            FieldsCopyExpressions(type, inputParameter, inputDictionary, outputVariable, boxingVariable, expressions);
            // 拷贝数组元素
            if (IsArray(type) && IsTypeToDeepCopy(type.GetElementType()))
            {
                CreateArrayCopyLoopExpression(type, inputParameter, inputDictionary, outputVariable, variables, expressions);
            }
            // 将所有表达式合并到lambda函数中
            var lambda = CombineAllIntoLambdaFunctionExpression(inputParameter, inputDictionary, outputVariable, endLabel, variables, expressions);
            return lambda;
        }

        private static void InitializeExpressions(Type type, out ParameterExpression inputParameter, out ParameterExpression inputDictionary, out ParameterExpression outputVariable, out ParameterExpression boxingVariable, out LabelTarget endLabel, out List<ParameterExpression> variables, out List<Expression> expressions)
        {
            inputParameter = Expression.Parameter(ObjectType);
            inputDictionary = Expression.Parameter(ObjectDictionaryType);
            outputVariable = Expression.Variable(type);
            boxingVariable = Expression.Variable(ObjectType);
            endLabel = Expression.Label();
            variables = new List<ParameterExpression>();
            expressions = new List<Expression>();
            variables.Add(outputVariable);
            variables.Add(boxingVariable);
        }

        private static void IfNullThenReturnNullExpression(ParameterExpression inputParameter, LabelTarget endLabel, List<Expression> expressions)
        {
            ///// Intended code:
            /////
            ///// if (input == null)
            ///// {
            /////     return null;
            ///// }
            var ifNullThenReturnNullExpression =
                Expression.IfThen(
                    Expression.Equal(
                        inputParameter,
                        Expression.Constant(null, ObjectType)),
                    Expression.Return(endLabel));
            expressions.Add(ifNullThenReturnNullExpression);
        }

        private static void MemberwiseCloneInputToOutputExpression(Type type, ParameterExpression inputParameter, ParameterExpression outputVariable, List<Expression> expressions)
        {
            ///// Intended code:
            /////
            ///// var output = (<type>)input.MemberwiseClone();           
            var memberwiseCloneMethod = ObjectType.GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance);
            var memberwiseCloneInputExpression =
                Expression.Assign(
                    outputVariable,
                    Expression.Convert(
                        Expression.Call(
                            inputParameter,
                            memberwiseCloneMethod),
                        type));
            expressions.Add(memberwiseCloneInputExpression);
        }

        private static void StoreReferencesIntoDictionaryExpression(ParameterExpression inputParameter, ParameterExpression inputDictionary, ParameterExpression outputVariable, List<Expression> expressions)
        {
            ///// Intended code:
            /////
            ///// inputDictionary[(Object)input] = (Object)output;
            var storeReferencesExpression =
                Expression.Assign(
                    Expression.Property(
                        inputDictionary,
                        ObjectDictionaryType.GetProperty("Item"),
                        inputParameter),
                    Expression.Convert(outputVariable, ObjectType));
            expressions.Add(storeReferencesExpression);
        }

        private static Expression<Func<object, Dictionary<object, object>, object>> CombineAllIntoLambdaFunctionExpression(ParameterExpression inputParameter, ParameterExpression inputDictionary, ParameterExpression outputVariable, LabelTarget endLabel, List<ParameterExpression> variables, List<Expression> expressions)
        {
            expressions.Add(Expression.Label(endLabel));
            expressions.Add(Expression.Convert(outputVariable, ObjectType));
            var finalBody = Expression.Block(variables, expressions);
            var lambda = Expression.Lambda<Func<object, Dictionary<object, object>, object>>(finalBody, inputParameter, inputDictionary);
            return lambda;
        }

        private static void CreateArrayCopyLoopExpression(Type type, ParameterExpression inputParameter, ParameterExpression inputDictionary, ParameterExpression outputVariable, List<ParameterExpression> variables, List<Expression> expressions)
        {
            ///// Intended code:
            /////
            ///// int i1, i2, ..., in; 
            ///// 
            ///// int length1 = inputarray.GetLength(0); 
            ///// i1 = 0; 
            ///// while (true)
            ///// {
            /////     if (i1 >= length1)
            /////     {
            /////         goto ENDLABELFORLOOP1;
            /////     }
            /////     int length2 = inputarray.GetLength(1); 
            /////     i2 = 0; 
            /////     while (true)
            /////     {
            /////         if (i2 >= length2)
            /////         {
            /////             goto ENDLABELFORLOOP2;
            /////         }
            /////         ...
            /////         ...
            /////         ...
            /////         int lengthn = inputarray.GetLength(n); 
            /////         in = 0; 
            /////         while (true)
            /////         {
            /////             if (in >= lengthn)
            /////             {
            /////                 goto ENDLABELFORLOOPn;
            /////             }
            /////             outputarray[i1, i2, ..., in] 
            /////                 = (<elementType>)DeepCopyByExpressionTreeObj(
            /////                        (Object)inputarray[i1, i2, ..., in])
            /////             in++; 
            /////         }
            /////         ENDLABELFORLOOPn:
            /////         ...
            /////         ...  
            /////         ...
            /////         i2++; 
            /////     }
            /////     ENDLABELFORLOOP2:
            /////     i1++; 
            ///// }
            ///// ENDLABELFORLOOP1:
            var rank = type.GetArrayRank();
            var indices = GenerateIndices(rank);
            variables.AddRange(indices);
            var elementType = type.GetElementType();
            var assignExpression = ArrayFieldToArrayFieldAssignExpression(inputParameter, inputDictionary, outputVariable, elementType, type, indices);
            Expression forExpression = assignExpression;
            for (int dimension = 0; dimension < rank; dimension++)
            {
                var indexVariable = indices[dimension];

                forExpression = LoopIntoLoopExpression(inputParameter, indexVariable, forExpression, dimension);
            }
            expressions.Add(forExpression);
        }

        private static List<ParameterExpression> GenerateIndices(int arrayRank)
        {
            ///// Intended code:
            /////
            ///// int i1, i2, ..., in; 
            var indices = new List<ParameterExpression>();
            for (int i = 0; i < arrayRank; i++)
            {
                var indexVariable = Expression.Variable(typeof(int));

                indices.Add(indexVariable);
            }
            return indices;
        }

        private static BinaryExpression ArrayFieldToArrayFieldAssignExpression(ParameterExpression inputParameter, ParameterExpression inputDictionary, ParameterExpression outputVariable, Type elementType, Type arrayType, List<ParameterExpression> indices)
        {
            ///// Intended code:
            /////
            ///// outputarray[i1, i2, ..., in] 
            /////     = (<elementType>)DeepCopyByExpressionTreeObj(
            /////            (Object)inputarray[i1, i2, ..., in]);
            var indexTo = Expression.ArrayAccess(outputVariable, indices);
            var indexFrom = Expression.ArrayIndex(Expression.Convert(inputParameter, arrayType), indices);
            var forceDeepCopy = elementType != ObjectType;
            var rightSide =
                Expression.Convert(
                    Expression.Call(
                        DeepCopyByExpressionTreeObjMethod,
                        Expression.Convert(indexFrom, ObjectType),
                        Expression.Constant(forceDeepCopy, typeof(bool)),
                        inputDictionary),
                    elementType);

            var assignExpression = Expression.Assign(indexTo, rightSide);
            return assignExpression;
        }

        private static BlockExpression LoopIntoLoopExpression(ParameterExpression inputParameter, ParameterExpression indexVariable, Expression loopToEncapsulate, int dimension)
        {
            ///// Intended code:
            /////
            ///// int length = inputarray.GetLength(dimension); 
            ///// i = 0; 
            ///// while (true)
            ///// {
            /////     if (i >= length)
            /////     {
            /////         goto ENDLABELFORLOOP;
            /////     }
            /////     loopToEncapsulate;
            /////     i++; 
            ///// }
            ///// ENDLABELFORLOOP:
            var lengthVariable = Expression.Variable(typeof(Int32));
            var endLabelForThisLoop = Expression.Label();
            var newLoop =
                Expression.Loop(
                    Expression.Block(
                        new ParameterExpression[0],
                        Expression.IfThen(
                            Expression.GreaterThanOrEqual(indexVariable, lengthVariable),
                            Expression.Break(endLabelForThisLoop)),
                        loopToEncapsulate,
                        Expression.PostIncrementAssign(indexVariable)),
                    endLabelForThisLoop);

            var lengthAssignment = GetLengthForDimensionExpression(lengthVariable, inputParameter, dimension);
            var indexAssignment = Expression.Assign(indexVariable, Expression.Constant(0));
            return Expression.Block(new[] { lengthVariable }, lengthAssignment, indexAssignment, newLoop);
        }

        private static BinaryExpression GetLengthForDimensionExpression(ParameterExpression lengthVariable, ParameterExpression inputParameter, int i)
        {
            ///// Intended code:
            /////
            ///// length = ((Array)input).GetLength(i); 
            var getLengthMethod = typeof(Array).GetMethod("GetLength", BindingFlags.Public | BindingFlags.Instance);
            var dimensionConstant = Expression.Constant(i);

            return Expression.Assign(lengthVariable, Expression.Call(Expression.Convert(inputParameter, typeof(Array)),
                    getLengthMethod, new[] { dimensionConstant }));
        }

        private static void FieldsCopyExpressions(Type type, ParameterExpression inputParameter, ParameterExpression inputDictionary, ParameterExpression outputVariable, ParameterExpression boxingVariable, List<Expression> expressions)
        {
            var fields = GetAllRelevantFields(type);
            var readonlyFields = fields.Where(f => f.IsInitOnly).ToList();
            var writableFields = fields.Where(f => !f.IsInitOnly).ToList();
            // 只读字段拷贝（采用装箱操作）
            bool shouldUseBoxing = readonlyFields.Any();
            if (shouldUseBoxing)
            {
                var boxingExpression = Expression.Assign(boxingVariable, Expression.Convert(outputVariable, ObjectType));
                expressions.Add(boxingExpression);
            }
            foreach (var field in readonlyFields)
            {
                if (IsDelegate(field.FieldType))
                {
                    ReadonlyFieldToNullExpression(field, boxingVariable, expressions);
                }
                else
                {
                    ReadonlyFieldCopyExpression(type, field, inputParameter, inputDictionary, boxingVariable, expressions);
                }
            }
            if (shouldUseBoxing)
            {
                var unboxingExpression = Expression.Assign(outputVariable, Expression.Convert(boxingVariable, type));
                expressions.Add(unboxingExpression);
            }
            // 非只读字段拷贝
            foreach (var field in writableFields)
            {
                if (IsDelegate(field.FieldType))
                {
                    WritableFieldToNullExpression(field, outputVariable, expressions);
                }
                else
                {
                    WritableFieldCopyExpression(type, field, inputParameter, inputDictionary, outputVariable, expressions);
                }
            }
        }

        private static FieldInfo[] GetAllRelevantFields(Type type, bool forceAllFields = false)
        {
            var fieldsList = new List<FieldInfo>();
            var typeCache = type;
            while (typeCache != null)
            {
                fieldsList.AddRange(typeCache.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy)
                        .Where(field => forceAllFields || IsTypeToDeepCopy(field.FieldType)));
                typeCache = typeCache.BaseType;
            }
            return fieldsList.ToArray();
        }

        private static FieldInfo[] GetAllFields(Type type)
        {
            return GetAllRelevantFields(type, forceAllFields: true);
        }

        private static readonly Type FieldInfoType = typeof(FieldInfo);
        private static readonly MethodInfo SetValueMethod = FieldInfoType.GetMethod("SetValue", new[] { ObjectType, ObjectType });

        private static void ReadonlyFieldToNullExpression(FieldInfo field, ParameterExpression boxingVariable, List<Expression> expressions)
        {
            ///// Intended code:
            /////
            ///// fieldInfo.SetValue(boxing, <fieldtype>null);
            var fieldToNullExpression =
                    Expression.Call(
                        Expression.Constant(field),
                        SetValueMethod,
                        boxingVariable,
                        Expression.Constant(null, field.FieldType));

            expressions.Add(fieldToNullExpression);
        }

        private static readonly Type ThisType = typeof(DeepCopyByExpressionTrees);
        private static readonly MethodInfo DeepCopyByExpressionTreeObjMethod = ThisType.GetMethod("DeepCopyByExpressionTreeObj", BindingFlags.NonPublic | BindingFlags.Static);

        private static void ReadonlyFieldCopyExpression(Type type, FieldInfo field, ParameterExpression inputParameter, ParameterExpression inputDictionary, ParameterExpression boxingVariable, List<Expression> expressions)
        {
            ///// Intended code:
            /////
            ///// fieldInfo.SetValue(boxing, DeepCopyByExpressionTreeObj((Object)((<type>)input).<field>))
            var fieldFrom = Expression.Field(Expression.Convert(inputParameter, type), field);
            var forceDeepCopy = field.FieldType != ObjectType;
            var fieldDeepCopyExpression =
                Expression.Call(Expression.Constant(field, FieldInfoType), SetValueMethod, boxingVariable,
                    Expression.Call(DeepCopyByExpressionTreeObjMethod, Expression.Convert(fieldFrom, ObjectType), Expression.Constant(forceDeepCopy, typeof(bool)),
                        inputDictionary));
            expressions.Add(fieldDeepCopyExpression);
        }

        private static void WritableFieldToNullExpression(FieldInfo field, ParameterExpression outputVariable, List<Expression> expressions)
        {
            ///// Intended code:
            /////
            ///// output.<field> = (<type>)null;           
            var fieldTo = Expression.Field(outputVariable, field);
            var fieldToNullExpression = Expression.Assign(fieldTo, Expression.Constant(null, field.FieldType));
            expressions.Add(fieldToNullExpression);
        }

        private static void WritableFieldCopyExpression(Type type, FieldInfo field, ParameterExpression inputParameter, ParameterExpression inputDictionary, ParameterExpression outputVariable, List<Expression> expressions)
        {
            ///// Intended code:
            /////
            ///// output.<field> = (<fieldType>)DeepCopyByExpressionTreeObj((Object)((<type>)input).<field>);
            var fieldFrom = Expression.Field(Expression.Convert(inputParameter, type), field);
            var fieldType = field.FieldType;
            var fieldTo = Expression.Field(outputVariable, field);
            var forceDeepCopy = field.FieldType != ObjectType;
            var fieldDeepCopyExpression = Expression.Assign(fieldTo,
                Expression.Convert(Expression.Call(DeepCopyByExpressionTreeObjMethod, Expression.Convert(fieldFrom, ObjectType), Expression.Constant(forceDeepCopy, typeof(bool)), inputDictionary), fieldType));
            expressions.Add(fieldDeepCopyExpression);
        }

        private static bool IsArray(Type type)
        {
            return type.IsArray;
        }

        private static bool IsDelegate(Type type)
        {
            return typeof(Delegate).IsAssignableFrom(type);
        }

        private static bool IsTypeToDeepCopy(Type type)
        {
            return IsClassOtherThanString(type)
                   || IsStructWhichNeedsDeepCopy(type);
        }

        private static bool IsClassOtherThanString(Type type)
        {
            return !type.IsValueType && type != typeof(String);
        }

        private static bool IsStructWhichNeedsDeepCopy(Type type)
        {
            // 多线程访问
            bool isStructTypeToDeepCopy;
            if (!IsStructTypeToDeepCopyDictionary.TryGetValue(type, out isStructTypeToDeepCopy))
            {
                lock (IsStructTypeToDeepCopyDictionaryLocker)
                {
                    if (!IsStructTypeToDeepCopyDictionary.TryGetValue(type, out isStructTypeToDeepCopy))
                    {
                        isStructTypeToDeepCopy = IsStructWhichNeedsDeepCopy_NoDictionaryUsed(type);

                        var newDictionary = IsStructTypeToDeepCopyDictionary.ToDictionary(pair => pair.Key, pair => pair.Value);

                        newDictionary[type] = isStructTypeToDeepCopy;

                        IsStructTypeToDeepCopyDictionary = newDictionary;
                    }
                }
            }
            return isStructTypeToDeepCopy;
        }

        private static bool IsStructWhichNeedsDeepCopy_NoDictionaryUsed(Type type)
        {
            return IsStructOtherThanBasicValueTypes(type) && HasInItsHierarchyFieldsWithClasses(type);
        }

        private static bool IsStructOtherThanBasicValueTypes(Type type)
        {
            return type.IsValueType && !type.IsPrimitive && !type.IsEnum && type != typeof(decimal);
        }

        private static bool HasInItsHierarchyFieldsWithClasses(Type type, HashSet<Type> alreadyCheckedTypes = null)
        {
            alreadyCheckedTypes = alreadyCheckedTypes ?? new HashSet<Type>();
            alreadyCheckedTypes.Add(type);
            var allFields = GetAllFields(type);
            var allFieldTypes = allFields.Select(f => f.FieldType).Distinct().ToList();
            var hasFieldsWithClasses = allFieldTypes.Any(IsClassOtherThanString);
            if (hasFieldsWithClasses)
            {
                return true;
            }
            var notBasicStructsTypes = allFieldTypes.Where(IsStructOtherThanBasicValueTypes).ToList();
            var typesToCheck = notBasicStructsTypes.Where(t => !alreadyCheckedTypes.Contains(t)).ToList();
            foreach (var typeToCheck in typesToCheck)
            {
                if (HasInItsHierarchyFieldsWithClasses(typeToCheck, alreadyCheckedTypes))
                {
                    return true;
                }
            }
            return false;
        }

        public class ReferenceEqualityComparer : EqualityComparer<object>
        {
            public override bool Equals(object x, object y)
            {
                return ReferenceEquals(x, y);
            }

            public override int GetHashCode(object obj)
            {
                if (obj == null) return 0;
                return obj.GetHashCode();
            }
        }
    }
}
