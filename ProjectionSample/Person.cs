namespace ProjectionSample
{
  // A sample domain class. This is used as the main query source.
  class Person
  {
    public Person(string firstName, string lastName, string city, string memo)
    {
      FirstName = firstName;
      LastName = lastName;
      City = city;
      Memo = memo;
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string City { get; set; }
    public string Memo { get; set; }
  }
}