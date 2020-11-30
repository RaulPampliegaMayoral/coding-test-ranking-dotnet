using coding_test_ranking.infrastructure.persistence.models;
using System.Collections.Generic;

namespace coding_test_ranking.infrastructure.services.Calculator
{
    public class WordAppearanceRule : IRule
    {
        private IList<string> wordsToCheck;

        public WordAppearanceRule()
        {
            wordsToCheck = new List<string>
            {
                "luminoso",
                "nuevo",
                "céntrico",
                "reformado",
                "ático"
            };
        }
        public int Calculate(AdVO ad)
        {
            var score = 0;
            if (string.IsNullOrEmpty(ad.Description))
                return score;

            var descriptionToLower = ad.Description.ToLower();
            foreach(var word in wordsToCheck)
            {
                score += descriptionToLower.Contains(word) ? 5 : 0;
            }

            return score;
        }
    }
}
