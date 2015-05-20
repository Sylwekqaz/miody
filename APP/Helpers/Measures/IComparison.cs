using APP.Model;

namespace APP.Helpers.Measures
{
    public interface IComparison
    {
        Result GetResult(Contour a, Contour b);
    }
}