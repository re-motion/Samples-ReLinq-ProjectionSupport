// Copyright (c) 2010 rubicon informationstechnologie gmbh
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Reflection;
using Remotion.Data.Linq.Clauses.Expressions;
using Remotion.Data.Linq.Parsing;
using System.Linq.Expressions;

namespace ProjectionSample
{
  // Builds an in-memory projector for a given select expression. Uses ResultObjectMapping to resolve references to query sources. Does not support 
  // sub-queries.
  public class ProjectorBuildingExpressionTreeVisitor : ExpressionTreeVisitor
  {
    // This is the generic ResultObjectMapping.GetObject<T>() method we'll use to obtain a queried object for an IQuerySource.
    private static readonly MethodInfo s_getObjectGenericMethodDefinition = typeof (ResultObjectMapping).GetMethod ("GetObject");

    // Call this method to get the projector. T is the type of the result (after the projection).
    public static Func<ResultObjectMapping, T> BuildProjector<T> (Expression selectExpression)
    {
      // This is the parameter of the delegat we're building. It's the ResultObjectMapping, which holds all the input data needed for the projection.
      var resultItemParameter = Expression.Parameter (typeof (ResultObjectMapping), "resultItem");

      // The visitor gives us the projector's body. It simply replaces all QuerySourceReferenceExpressions with calls to ResultObjectMapping.GetObject<T>().
      var visitor = new ProjectorBuildingExpressionTreeVisitor (resultItemParameter);
      var body = visitor.VisitExpression (selectExpression);

      // Construct a LambdaExpression from parameter and body and compile it into a delegate.
      var projector = Expression.Lambda<Func<ResultObjectMapping, T>> (body, resultItemParameter);
      return projector.Compile();
    }

    private readonly ParameterExpression _resultItemParameter;

    private ProjectorBuildingExpressionTreeVisitor (ParameterExpression resultItemParameter)
    {
      _resultItemParameter = resultItemParameter;
    }

    protected override Expression VisitQuerySourceReferenceExpression (QuerySourceReferenceExpression expression)
    {
      // Substitute generic parameter "T" of ResultObjectMapping.GetObject<T>() with type of query source item, then return a call to that method
      // with the query source referenced by the expression.
      var getObjectMethod = s_getObjectGenericMethodDefinition.MakeGenericMethod (expression.Type);
      return Expression.Call (_resultItemParameter, getObjectMethod, Expression.Constant (expression.ReferencedQuerySource));
    }

    protected override Expression VisitSubQueryExpression (SubQueryExpression expression)
    {
      throw new NotSupportedException ("This provider does not support subqueries in the select projection.");
    }
  }
}