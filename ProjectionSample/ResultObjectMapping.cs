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
using System.Collections;
using System.Collections.Generic;
using Remotion.Data.Linq.Clauses;

namespace ProjectionSample
{
  // This class holds all the result objects for each "row" returned by the query. The objects can be accessed by the IQuerySource that yielded them.
  // Each "row" gets its own ResultObjectMapping.
  public class ResultObjectMapping : IEnumerable<KeyValuePair<IQuerySource, object>>
  {
    private readonly Dictionary<IQuerySource, object> _resultObjectsBySource = new Dictionary<IQuerySource, object>();

    public void Add (IQuerySource querySource, object resultObject)
    {
      _resultObjectsBySource.Add (querySource, resultObject);
    }

    public T GetObject<T> (IQuerySource source)
    {
      return (T) _resultObjectsBySource[source];
    }

    public IEnumerator<KeyValuePair<IQuerySource, object>> GetEnumerator()
    {
      return _resultObjectsBySource.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}