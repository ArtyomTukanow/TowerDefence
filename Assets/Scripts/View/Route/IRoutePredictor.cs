namespace View.Route
{
    public interface IRoutePredictor
    {
        float Speed { get; }
        float GetMoveTimeToPoint(BaseRouteView route, float prc);
    }
}