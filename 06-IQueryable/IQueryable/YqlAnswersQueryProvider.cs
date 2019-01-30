using System;
using System.Linq;
using System.Linq.Expressions;

namespace IQueryableTask
{
    public class YqlAnswersQueryProvider : IQueryProvider
    {
        public IQueryable CreateQuery(Expression expression)
        {
            // TODO: Implement CreateQuery
            try
            {
                return (IQueryable)Activator.CreateInstance(typeof(YqlAnswerSearch), new object[] { expression });
            }
            catch (System.Reflection.TargetInvocationException tie)
            {
                throw tie.InnerException;
            }
        }

        public IQueryable<Question> CreateQuery<Question>(Expression expression)
        {
            // TODO: Implement CreateQuery
            return (IQueryable<Question>)new YqlAnswerSearch(expression);
        }

        public object Execute(Expression expression)
        {
            // TODO: Implement Execute
            throw new NotImplementedException();

            // HINT: Use GetYqlQuery to build query
            // HINT: Use YqlAnswersService to fetch data
        }

        public TResult Execute<TResult>(Expression expression)
        {
            // TODO: Implement Execute
            throw new NotImplementedException();
        }

        /// <summary>
        /// Generates YQL Query
        /// </summary>
        /// <param name="expression">Expression tree</param>
        /// <returns></returns>
        public string GetYqlQuery(Expression expression)
        {
            // TODO: Implement GetYqlQuery
            return new QueryTranslate().Translate(expression);


            // HINT: Create a class derived from ExpressionVisitor
        }
    }

    internal class QueryTranslate : ExpressionVisitor
    {
        private const string V = "\"";
        System.Text.StringBuilder sb;
        internal QueryTranslate()
        {

        }

        internal string Translate(Expression expression)
        {
            this.sb = new System.Text.StringBuilder();
            sb.Append("select * from answers.serach");

            this.Visit(expression);
            var ravResult = this.sb.ToString();

            if (ravResult.Contains(" category_name=") && !ravResult.Contains("query="))
            {
                throw new InvalidOperationException("thre is no part of query");
            }

            sb.Clear();

            int position = 0;
            int posKey;
            int posWhitespace = 0;
            string bufValue;

            while (position < ravResult.Length)
            {
                posKey = ravResult.IndexOf(" type=", position);

                if (posKey == -1)
                {
                    sb.Append(ravResult, position, ravResult.Length - position);
                    break;
                }
                sb.Append(ravResult, position, posKey + 6);

                position += posKey + 6;

                posWhitespace = ravResult.IndexOf("", position);
                if (posWhitespace == -1)
                {
                    posWhitespace = ravResult.Length - position;
                }
                bufValue = "\"" + Enum.GetName(typeof(QuestionType),
                    Int32.Parse(ravResult.Substring(position, posWhitespace))).ToLower() + "\"";
                sb.Append(bufValue);
                position += posWhitespace;
            }

            return sb.ToString();

        }

        private static Expression StripQuotes(Expression e)
        {
            if (e.NodeType == ExpressionType.Quote)
            {
                e = ((UnaryExpression)e).Operand;
            }

            return e;
        }

        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            if (m.Method.DeclaringType == typeof(Queryable) && m.Method.Name == "Where")
            {
                sb.Append(" where");
                this.Visit(m.Arguments[0]);

                LambdaExpression lambda = (LambdaExpression)StripQuotes(m.Arguments[0]);

                this.Visit(lambda.Body);

                return m;
            }
            else if (m.Method.Name == "Contains")
            {
                this.Visit(m.Object);
                sb.Append(m.Arguments[0]);
                return m;
            }
            throw new InvalidOperationException();

        }

        protected override Expression VisitUnary(UnaryExpression u)
        {
            switch (u.NodeType)
            {
                case ExpressionType.Not:
                    this.Visit(u.Operand);
                    break;
                case ExpressionType.Convert:
                    this.Visit(u.Operand);
                    break;
                default:
                    throw new NotSupportedException();
            }
            return u;
        }

        protected override Expression VisitBinary(BinaryExpression b)
        {
            this.Visit(b.Left);
            switch (b.NodeType)
            {
                case ExpressionType.Equal:
                    sb.Append("=");
                    break;
                case ExpressionType.NotEqual:
                    sb.Append("<>");
                    break;
                case ExpressionType.LessThan:
                    sb.Append("<");
                    break;
                case ExpressionType.LessThanOrEqual:
                    sb.Append("<=");
                    break;
                case ExpressionType.GreaterThan:
                    sb.Append(">");
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    sb.Append(">=");
                    break;
                case ExpressionType.And:
                    sb.Append(" and");
                    break;
                case ExpressionType.AndAlso:
                    sb.Append(" and");
                    break;
                case ExpressionType.Or:
                    sb.Append(" or");
                    break;

            }
            this.Visit(b.Right);
            return b;
        }

        protected override Expression VisitConstant(ConstantExpression c)
        {
            IQueryable q = c.Value as IQueryable;

            if (q != null)
            {
                sb.Append("select * from");
            }
            else if (c.Value == null)
            {
                sb.Append(" NULL");
            }
            else
            {
                switch (Type.GetTypeCode(c.Value.GetType()))
                {
                    case TypeCode.Boolean:

                        sb.Append(((bool)c.Value) ? 1 : 0);

                        break;

                    case TypeCode.String:

                        sb.Append("\"");
                        sb.Append(c.Value);
                        sb.Append("\"");

                        break;

                    case TypeCode.Object:

                        throw new NotSupportedException();

                    default:

                        sb.Append(c.Value);

                        break;
                }
            }
            return c;
        }

        protected override Expression VisitMember(MemberExpression m )
        {
            if (m.Expression != null && m.Expression.NodeType == ExpressionType.Parameter)
            {
                if (m.Member.Name == "Subject")
                {
                    sb.Append(" query=");
                }
                if (m.Member.Name == "Type")
                {
                    sb.Append(" type");
                }
                if (m.Member.Name == "Category")
                {
                    sb.Append(" category_name");
                }
                return m;
            }
            throw new NotSupportedException();
        } 


    }
}
