using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AYAK.Common.NetCore
{
    public class QueryBuilder<T>
    {

        public QueryBuilder()
        {
            Type = typeof(T);
            Table t = (Table)Table.GetCustomAttribute(Type, typeof(Table));
            if (t != null)
            {
                Table = t;
            }
            else
            {
                throw new Exception("Table belirtilmemiş bir nesnede Query Builder kullanılamaz.");
            }

            WhereClause = new StringBuilder();
            ColumnsClause = new StringBuilder();
            OrderByClause = new StringBuilder();
            GroupByClause = new StringBuilder();
            JoinClause = new StringBuilder();
            Parameters = new Dictionary<string, object>();
            AnonymMembers = new List<MemberInfo>();
            HavingClause = new StringBuilder();
        }
        public QueryBuilder(Table t)
        {
            Type = typeof(T);

            if (t != null)
            {
                Table = t;
            }
            else
            {
                throw new Exception("Table belirtilmemiş bir nesnede Query Builder kullanılamaz.");
            }

            WhereClause = new StringBuilder();
            ColumnsClause = new StringBuilder();
            OrderByClause = new StringBuilder();
            GroupByClause = new StringBuilder();
            JoinClause = new StringBuilder();
            Parameters = new Dictionary<string, object>();
            AnonymMembers = new List<MemberInfo>();
            HavingClause = new StringBuilder();
        }

        public QueryBuilder<RT> Clone<RT>()
        {
            QueryBuilder<RT> q = new QueryBuilder<RT>(this.Table);
            q.AnonymMembers = this.AnonymMembers;
            q.ColumnsClause = this.ColumnsClause;
            q.GroupByClause = this.GroupByClause;
            q.JoinClause = this.JoinClause;
            q.OrderByClause = this.OrderByClause;
            q.ParameterCount = this.ParameterCount;
            q.Parameters = this.Parameters;
            q.Table = this.Table;
            q.Type = this.Type;
            q.WhereClause = this.WhereClause;
            q.HavingClause = this.HavingClause;
            return q;
        }

        private string query;

        public string Query
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT ");
                if (string.IsNullOrEmpty(ColumnsClause.ToString()))
                {
                    sb.Append(" * ");
                }
                else
                {
                    sb.Append(ColumnsClause);
                }

                sb.Append(" FROM ");

                //Şema Adı
                if (!string.IsNullOrEmpty(Table.SchemaName.Trim()))
                {
                    sb.Append(Table.SchemaName);
                    sb.Append(".");
                }
                //Tablo Adı
                if (!string.IsNullOrEmpty(Table.TableName.Trim()))
                {
                    sb.Append(Table.TableName);
                    sb.Append(" ");
                }



                //WHERE
                if (!string.IsNullOrEmpty(WhereClause.ToString().Trim()))
                {
                    sb.Append(" WHERE ");
                    sb.Append(WhereClause);
                }
                //GROUP BY
                if (!string.IsNullOrEmpty(GroupByClause.ToString().Trim()))
                {
                    sb.Append(" GROUP BY ");
                    sb.Append(GroupByClause);
                }
                if (HavingClause.Length > 0)
                {
                    sb.Append(" HAVING ");
                    sb.Append(HavingClause);
                }
                //ORDER BY
                if (!string.IsNullOrEmpty(OrderByClause.ToString()))
                {
                    sb.Append(" ORDER BY ");
                    sb.Append(OrderByClause);
                }

                return sb.ToString();
            }

        }

        public Table Table { get; set; }
        public Type Type { get; set; }
        public StringBuilder ColumnsClause { get; set; }
        public StringBuilder WhereClause { get; set; }
        public StringBuilder OrderByClause { get; set; }
        public StringBuilder GroupByClause { get; set; }
        public StringBuilder HavingClause { get; set; }
        public StringBuilder JoinClause { get; set; }

        public Dictionary<string, object> Parameters { get; set; }
        public List<MemberInfo> AnonymMembers { get; set; }



        int ParameterCount = 1;

        #region WHERE


        public QueryBuilder<T> Where(Expression<Func<T, bool>> exp)
        {

            if (GroupByClause.Length > 0)
            {
                if (HavingClause.Length > 0)
                {
                    HavingClause.Append(" AND ");
                }
                HavingClause.Append(Where(exp.Body));
            }
            else
            {
                if (WhereClause.Length > 0)
                {
                    WhereClause.Append(" AND ");
                }
                StringBuilder sb = Where(exp.Body);
                WhereClause.Append(sb);
            }







            return this;
        }

        StringBuilder Where(Expression exp)
        {
            StringBuilder sb = new StringBuilder();

            if (exp is BinaryExpression)
            {
                BinaryExpression b = (BinaryExpression)exp;
                sb.Append("(");
                sb.Append(Where(b.Left));

                switch (b.NodeType)
                {
                    case ExpressionType.And:
                    case ExpressionType.AndAlso:
                        sb.Append(" AND ");
                        break;
                    case ExpressionType.Or:
                    case ExpressionType.OrElse:
                        sb.Append(" OR ");
                        break;
                    case ExpressionType.Equal:
                        sb.Append(" = ");
                        break;
                    case ExpressionType.NotEqual:
                        sb.Append(" <> ");
                        break;
                    case ExpressionType.GreaterThan:
                        sb.Append(" > ");
                        break;
                    case ExpressionType.GreaterThanOrEqual:
                        sb.Append(" >= ");
                        break;
                    case ExpressionType.LessThan:
                        sb.Append(" < ");
                        break;
                    case ExpressionType.LessThanOrEqual:
                        sb.Append(" <= ");
                        break;
                    case ExpressionType.Add:
                        sb.Append("+");
                        break;
                    case ExpressionType.Subtract:
                        sb.Append("-");
                        break;
                    case ExpressionType.Multiply:
                        sb.Append("*");
                        break;
                    case ExpressionType.Divide:
                        sb.Append("/");
                        break;
                    case ExpressionType.Modulo:
                        sb.Append("%");
                        break;
                }


                sb.Append(Where(b.Right));
                sb.Append(")");
            }
            else
            {

                if (exp is MemberExpression)
                {
                    MemberExpression m = exp as MemberExpression;
                    sb.Append(GetFieldName(m));

                }
                else
                {
                    string pname = string.Format("@p{0}", ParameterCount);
                    ParameterCount++;
                    object value = GetValue(exp);
                    Parameters.Add(pname, value);
                    sb.Append(pname);
                }

            }

            return sb;

        }


        #endregion

        #region ORDER BY

        public QueryBuilder<T> OrderBy<TKey>(Expression<Func<T, TKey>> exp)
        {
            if (OrderByClause.Length > 0)
            {
                OrderByClause.Append(",");
            }
            if (exp.Body is NewExpression)
            {
                NewExpression n = (NewExpression)exp.Body;
                foreach (MemberExpression item in n.Arguments)
                {
                    string fn = GetFieldName(item);

                    OrderByClause.Append(fn);
                    OrderByClause.Append(",");

                }
                OrderByClause.Remove(OrderByClause.Length - 1, 1);
            }
            else
            {
                string fn = GetFieldName(exp.Body);
                OrderByClause.Append(fn);
            }


            return this;

        }

        #endregion


        #region GROUP BY

        public QueryBuilder<Groupped<RT, T>> GroupBy<RT>(Expression<Func<T, RT>> exp)
        {
            QueryBuilder<Groupped<RT, T>> q = this.Clone<Groupped<RT, T>>();

            if (q.GroupByClause.Length > 0)
            {
                q.GroupByClause.Append(",");
            }
            if (exp.Body is NewExpression)
            {
                NewExpression n = (NewExpression)exp.Body;
                foreach (MemberExpression item in n.Arguments)
                {
                    string fn = GetFieldName(item);

                    q.GroupByClause.Append(fn);
                    q.GroupByClause.Append(",");
                    q.ColumnsClause.Append(fn);
                    q.ColumnsClause.Append(",");

                }
                q.GroupByClause.Remove(GroupByClause.Length - 1, 1);
                q.ColumnsClause.Remove(ColumnsClause.Length - 1, 1);
            }
            else
            {
                string fn = GetFieldName(exp.Body);
                q.GroupByClause.Append(fn);
                q.ColumnsClause.Append(fn);
            }

            return q;
        }


        #endregion

        public QueryBuilder<R> Select<R>(Expression<Func<T, R>> exp)
        {
            if (ColumnsClause.Length > 0)
            {
                ColumnsClause.Clear();
            }
            if (exp.Body is NewExpression)
            {
                NewExpression n = (NewExpression)exp.Body;
                for (int i = 0; i < n.Arguments.Count; i++)
                {
                    MemberInfo col = n.Members[i];
                    if (n.Arguments[i] is MemberExpression)
                    {
                        MemberExpression item = (MemberExpression)n.Arguments[i];

                        string fn = GetFieldName(item);
                        ColumnsClause.Append(fn);


                    }
                    else if (n.Arguments[i] is MethodCallExpression)
                    {
                        MethodCallExpression mc = n.Arguments[i] as MethodCallExpression;

                        LambdaExpression e = (LambdaExpression)mc.Arguments[1];

                        switch (mc.Method.Name)
                        {
                            case "Sum":
                                ColumnsClause.Append("SUM");
                                break;
                            case "Count":
                                ColumnsClause.Append("COUNT");

                                break;
                            case "Average":
                                ColumnsClause.Append("AVG");

                                break;
                            case "Max":
                                ColumnsClause.Append("MAX");

                                break;
                            case "Min":
                                ColumnsClause.Append("MIN");

                                break;
                            case "Any":
                                ColumnsClause.Append("EXISTS");

                                break;

                        }

                        ColumnsClause.Append("(");

                        ColumnsClause.Append(Arithmetic(e.Body));

                        ColumnsClause.Append(")");


                    }
                    ColumnsClause.Append(" AS '");
                    ColumnsClause.Append(col.Name);
                    ColumnsClause.Append("'");
                    ColumnsClause.Append(",");
                    AnonymMembers.Add(col);
                }

                if (n.Type.Name != Type.Name)
                {
                    Type = n.Type;
                }

                ColumnsClause.Remove(ColumnsClause.Length - 1, 1);
            }


            QueryBuilder<R> r = this.Clone<R>();
            return r;
        }


        public QueryBuilder<T> Sum<TKey>(Expression<Func<T, TKey>> exp)
        {
            if (ColumnsClause.Length > 0)
            {
                ColumnsClause.Append(",");


            }
            ColumnsClause.Append("SUM(");
            ColumnsClause.Append(Arithmetic(exp.Body));
            ColumnsClause.Append(")");

            return this;

        }

        StringBuilder Arithmetic(Expression exp)
        {
            StringBuilder sb = new StringBuilder();

            if (exp is BinaryExpression)
            {
                BinaryExpression bex = exp as BinaryExpression;
                sb.Append("(");
                sb.Append(Arithmetic(bex.Left));
                switch (bex.NodeType)
                {
                    case ExpressionType.Add:
                        sb.Append("+");
                        break;
                    case ExpressionType.Subtract:
                        sb.Append("-");
                        break;
                    case ExpressionType.Multiply:
                        sb.Append("*");
                        break;
                    case ExpressionType.Divide:
                        sb.Append("/");
                        break;
                    case ExpressionType.Modulo:
                        sb.Append("%");
                        break;
                }

                sb.Append(Arithmetic(bex.Right));
                sb.Append(")");

            }
            else if (exp is MemberExpression)
            {
                MemberExpression mex = exp as MemberExpression;
                sb.Append(mex.Member.Name);
            }
            else if (exp is ConstantExpression)
            {
                ConstantExpression c = exp as ConstantExpression;
                sb.Append(GetValue(c));

            }
            else if (exp is UnaryExpression)
            {
                sb.Append(GetValue(exp as UnaryExpression));
            }
            else
            {
                throw new Exception("Aritmetik fonksiyonu için tanınamayan değer gönderildi");
            }
            return sb;

        }
        string GetFieldName(Expression exp)
        {
            if (exp.NodeType == ExpressionType.MemberAccess)
            {
                return GetFieldName(exp as MemberExpression);
            }
            else
            {
                return null;
            }
        }
        string GetFieldName(MemberExpression exp)
        {
            return exp.Member.Name;
        }


        object GetValue(Expression exp)
        {
            if (exp is UnaryExpression)
            {
                return GetValue(exp as UnaryExpression);
            }
            if (exp is MemberExpression)
            {
                return GetValue(exp as MemberExpression);
            }
            if (exp is ConstantExpression)
            {
                return GetValue(exp as ConstantExpression);
            }
            throw new Exception("Değer getirilemeyen Expression");
            return null;
        }
        /// <summary>
        /// Node Type Convert olan elemanlarda UnaryExpression gelir. Unary den value getiren komut.
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        object GetValue(UnaryExpression exp)
        {
            if (exp.Operand is MemberExpression)
            {
                MemberExpression m = (MemberExpression)exp.Operand;
                return GetValue(m);
            }
            else if (exp.Operand is MethodCallExpression)
            {
                MethodCallExpression m = (MethodCallExpression)exp.Operand;
                return GetValue(m);
            }
            return null;


        }

        object GetValue(MethodCallExpression exp)
        {
            object[] arg = exp.Arguments.Select(x => GetValue(x)).ToArray();
            object p = exp.Method.Invoke(null, arg);

            return p;
        }
        object GetValue(MemberExpression exp)
        {
            ConstantExpression c = exp.Expression as ConstantExpression;
            object p = GetValue(c);
            return p;
        }

        object GetValue(ConstantExpression exp)
        {
            if (exp.Value is string || exp.Value is char ||
                exp.Value is byte || exp.Value is sbyte ||
                exp.Value is short || exp.Value is ushort ||
                exp.Value is int || exp.Value is uint ||
                exp.Value is long || exp.Value is ulong ||
                exp.Value is decimal || exp.Value is double ||
                exp.Value is float || exp.Value is bool ||
                exp.Value is DateTime || exp.Value is Guid
                )
            {
                return exp.Value;
            }
            else
            {
                object p = exp.Value.GetType().GetFields()[0].GetValue(exp.Value);
                return p;
            }

        }



    }


    public class Groupped<K, T> : IGrouping<K, T>
    {
        public K Key { get; set; }

        public List<T> List { get; set; }

        public IEnumerator<T> GetEnumerator()
        {
            return List.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return List.GetEnumerator();
        }
    }


}
