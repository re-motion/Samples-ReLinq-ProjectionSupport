using System;
using System.Linq;

namespace ProjectionSample
{
  class Program
  {
    static void Main ()
    {
      var people = new Queryable<Person> ();
      var customers = from p in people
                      select new Customer
                      {
                        Name = p.FirstName + " " + p.LastName,
                        Location = p.City,
                        Notes = p.Memo
                      };

      foreach (var customer in customers)
      {
        Console.WriteLine (customer.Name + ", " + customer.Location + ", " + customer.Notes);
      }
    }
  }
}
