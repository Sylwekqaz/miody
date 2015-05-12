using APP.Model;

namespace APP.Helpers.Measures
{
    interface IComparison
    {
        Result GetResult(Contour a,Contour b);
    }
}
