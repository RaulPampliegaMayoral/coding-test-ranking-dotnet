
using coding_test_ranking.infrastructure.persistence.models;

namespace coding_test_ranking.infrastructure.services.Calculator
{
    public interface IRule
    {
        int Calculate(AdVO ad);
    }
}
