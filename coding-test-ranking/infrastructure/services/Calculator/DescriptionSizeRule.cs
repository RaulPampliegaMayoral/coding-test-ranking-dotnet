using coding_test_ranking.infrastructure.persistence.models;

namespace coding_test_ranking.infrastructure.services.Calculator
{
    public class DescriptionSizeRule : IRule
    {
        private const string FLAT = "FLAT";
        private const string CHALET = "CHALET";

        public int Calculate(AdVO ad)
        {
            if (string.IsNullOrEmpty(ad.Description))
                return 0;

            var score = CheckFlatTypology(ad);
            score += CheckChaletTypology(ad);

            return score;
        }
        private int CheckFlatTypology(AdVO ad)
        {
            if (ad.Typology != FLAT)
                return 0;

            if (ad.Description.Length >= 20 && ad.Description.Length < 50)
                return 10;

            if (ad.Description.Length >= 50)
                return 30;

            return 0;
        }

        private int CheckChaletTypology(AdVO ad)
        {
            if (ad.Typology != CHALET)
                return 0;
            
            if (ad.Description.Length > 50)
                return 20;
            
            return 0;
        }
    }
}
