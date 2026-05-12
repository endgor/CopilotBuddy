using System.Collections.Generic;

namespace Styx.Logic.Pathing
{
    public interface ITerrainHeightProvider
    {
        List<float> FindHeights(float x, float y);
    }
}
