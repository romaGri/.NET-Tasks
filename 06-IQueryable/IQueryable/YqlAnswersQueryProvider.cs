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
                sb.Append(ravResult , position , posKey + 6);

                position += posKey + 6;

                posWhitespace = ravResult.IndexOf("", position);
                if (posWhitespace == -1)
                {
                    posWhitespace = ravResult.Length - position;
                }
                bufValue = "\"" + Enum.GetName(typeof(QuestionType),
                    Int32.Parse(ravResult.Substring(position, posWhitespace))).ToLower() +"\"";
                sb.Append(bufValue);
                position += posWhitespace;
            }

            return sb.ToString();

        }


    }
}
