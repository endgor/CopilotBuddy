using Tripper.Navigation;

namespace Styx.Logic.Pathing
{
    /// <summary>
    /// WoD-style move path wrapper (PathFindResult + current index).
    /// </summary>
    public class MeshMovePath
    {
        public MeshMovePath(PathFindResult path)
        {
            Path = path;
            Index = 0;
        }

        public PathFindResult Path { get; set; }

        public int Index { get; set; }

        public bool IsExitingGarrison { get; set; }
    }
}
