namespace FubuMovies.Core 
{
    [Plural("Movies")]
    public class Movie : IEntity
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}