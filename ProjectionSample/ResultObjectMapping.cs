using System.Collections;
using System.Collections.Generic;
using Remotion.Data.Linq.Clauses;

namespace ProjectionSample
{
  // This class holds all the result objects for each "row" returned by the query. The objects can be accessed by the IQuerySource that yielded them.
  // Each "row" gets its own ResultObjectMapping.
  public class ResultObjectMapping : IEnumerable<KeyValuePair<IQuerySource, object>>
  {
    private readonly Dictionary<IQuerySource, object> _resultObjectsBySource = new Dictionary<IQuerySource, object> ();

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
      return _resultObjectsBySource.GetEnumerator ();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}