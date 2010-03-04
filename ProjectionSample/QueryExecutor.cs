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
using System.Collections.Generic;
using Remotion.Data.Linq;

namespace ProjectionSample
{
  // The IQueryExecutor implementation for this sample. Only ExecuteCollection is implemented.
  public class QueryExecutor : IQueryExecutor
  {
    public T ExecuteScalar<T> (QueryModel queryModel)
    {
      throw new NotImplementedException();
    }

    public T ExecuteSingle<T> (QueryModel queryModel, bool returnDefaultWhenEmpty)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<T> ExecuteCollection<T> (QueryModel queryModel)
    {
      // We build an in-memory projector for the query's select clause. The projector takes a ResultObjectMapping and transforms it into the 
      // projection result.
      Func<ResultObjectMapping, T> projector = ProjectorBuildingExpressionTreeVisitor.BuildProjector<T> (queryModel.SelectClause.Selector);

      // Execute the query and apply the projector to each result item.
      var resultItems = ExecuteQuery (queryModel);
      foreach (var resultItem in resultItems)
        yield return projector (resultItem);
    }

    // This method returns a number of ResultItems holding the queried objects. The ResultItems will be used as the input of the in-memory projector,
    // so they must hold a queried object for every IQuerySource in the QueryModel (MainFromClause, AdditionalFromClauses, JoinClauses, ...) that is
    // used by the SelectClause.
    private IEnumerable<ResultObjectMapping> ExecuteQuery (QueryModel queryModel)
    {
      // We'll simplify the number of cases we have to handle.

      if (queryModel.BodyClauses.Count > 0 || queryModel.ResultOperators.Count > 0)
        throw new NotSupportedException ("This query provider does not support queries with body clauses or result operators.");

      if (queryModel.MainFromClause.ItemType != typeof (Person))
        throw new NotSupportedException ("This query provider only supports queries on the Person data source.");

      // Each ResultObjectMapping constitutes a "row" in the result, mapping query sources to queried objects. We only have to deal with one query 
      // source, but for more general cases with more than one query source, that mapping is required.

      // Pretend we've received two following result items...
      yield return new ResultObjectMapping {{queryModel.MainFromClause, new Person ("John", "Doe", "New York City", "Unknown person")}};
      yield return
          new ResultObjectMapping {{queryModel.MainFromClause, new Person ("Jane", "Doe", "Washington D.C.", "Unknown, too, but same last name?")}};
    }
  }
}