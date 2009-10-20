using System.Linq;
using System.Linq.Expressions;
using Remotion.Data.Linq;

namespace ProjectionSample
{
  // The IQueryable implementation for this sample. Instantiates MyQueryExecutor.
  public class Queryable<T> : QueryableBase<T>
  {
    // To be called from the user.
    public Queryable ()
      : base (new QueryExecutor ())
    {
    }

    // For infrastructure purposes.
    public Queryable(IQueryProvider provider, Expression expression) : base(provider, expression)
    {
    }
  }
}