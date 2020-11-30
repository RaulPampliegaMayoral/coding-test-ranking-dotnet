using coding_test_ranking.infrastructure.api;
using System.Collections.Generic;

namespace coding_test_ranking.infrastructure.services
{
    public interface IAdsService
    {
        void calculateScoreForAds();
        List<QualityAd> GetQualityAds();
        List<PublicAd> GetPublicAds();

    }
}
