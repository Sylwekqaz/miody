using APP.Model;

namespace APP.Helpers.Measures
{
    internal interface IComparison
    {
        Result GetResult(Contour a, Contour b);
    }
}