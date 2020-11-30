using coding_test_ranking.infrastructure.persistence.models;

namespace coding_test_ranking.infrastructure.services.Calculator
{
    public class DescriptionRule : IRule
    {
        public int Calculate(AdVO ad)
        {
            if (string.IsNullOrEmpty(ad.Description))
                return 0;

            var trimmedDescription = ad.Description.Trim();
            return trimmedDescription.Length == 0 ? 0 : 5;
        }
    }
}
