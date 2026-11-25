using AlgorithmHittingSet.Model;

namespace AlgorithmHittingSet.TriangleEnumeration.SelectElement
{
    public abstract class SelectElementStrategy
    {
        public abstract (int Max, int MaxInd) FirstElement(int pPosition, List<ElementInfo> elements, List<SetInfo> SetList, List<List<int>> rest);
    }
}
