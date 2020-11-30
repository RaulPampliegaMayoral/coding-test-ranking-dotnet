using coding_test_ranking.infrastructure.persistence.models;
using coding_test_ranking.infrastructure.persistence.repositories;
using System.Collections.Generic;
using System.Linq;

namespace coding_test_ranking.infrastructure.services.Calculator
{
    public class RuleCalculator : IRuleCalculator
    {
        public List<IRule> rules;
        public RuleCalculator(IPicturesRepository repository)
        {
            rules = new List<IRule>
            {
                new PicturesRule(repository),
                new DescriptionRule(),
                new DescriptionSizeRule(),
                new WordAppearanceRule(),
                new AdCompleteRule()
            };
        }
        public int CalculateScore(AdVO ad)
        {
            return rules.Sum(c => c.Calculate(ad));
        }
    }
}
