using UnityEngine;

namespace Utility
{
    public interface I3DSurface
    {
        float Surface { get; }
    }

    public interface I3DVolume
    {
        float Volume { get; }
    }
    public interface IDrag
    {
        float DragCoefficient { get; }
        float CrossSectionArea(Vector3 direction);
    }

    public interface I3DContaining
    {
        bool IsPointInside(Vector3 point);
    }

    public interface IMesh
    {
        void ToMesh(Mesh resultMesh);
    }

}
