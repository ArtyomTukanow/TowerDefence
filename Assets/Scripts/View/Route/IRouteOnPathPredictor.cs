namespace View.Route
{
    public interface IRouteOnPathPredictor : IRoutePredictor
    {
        float CurrentPosition { get; }
    }
}