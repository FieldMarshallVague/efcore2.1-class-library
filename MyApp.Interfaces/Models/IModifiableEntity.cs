namespace MyApp.Interfaces.Models
{
    /// <summary>
    /// Interface to make it easier to keep track of Entity-ViewModel changes
    /// </summary>
    public interface IModifiableEntity
    {
        string Name { get; set; }
    }
}
