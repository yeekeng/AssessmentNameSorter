namespace NameSorter.Models
{
    public class Person
    {
        public string LastName { get; set; }
        public string GivenName { get; set; }

        public override string ToString()
        {
            return GivenName + " " + LastName;
        }
    }
}
