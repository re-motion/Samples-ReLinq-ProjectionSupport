namespace ProjectionSample
{
  // A sample domain class. This is only used in the projection, not as a query source.
  class Customer
  {
    public string Name { get; set; }
    public string Location { get; set; }
    public string Notes { get; set; }
  }
}