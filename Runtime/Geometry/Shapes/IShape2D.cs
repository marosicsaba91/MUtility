namespace MUtility
{
public interface IShape2D : IPolygon
{
    float Circumference { get; }
    float Area { get; } 
}
}