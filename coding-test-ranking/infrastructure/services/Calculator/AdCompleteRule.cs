using coding_test_ranking.infrastructure.persistence.models;

namespace coding_test_ranking.infrastructure.services.Calculator
{
    public class AdCompleteRule : IRule
    {
        public int Calculate(AdVO ad)
        {
            return CheckTypology(ad) && CheckPictures(ad) ? 40 : 0;
        }

        private bool CheckTypology(AdVO ad)
        {
            return CheckFlatTypology(ad) || 
                   CheckChaletTypology(ad) || 
                   CheckGarageTypology(ad);
        }

        private bool CheckPictures(AdVO ad)
        {
            return ad.Pictures != null && ad.Pictures.Count > 0;
        }

        private bool CheckFlatTypology(AdVO ad)
        {
            return ad.Typology == "FLAT" && ad.HouseSize.HasValue && !string.IsNullOrEmpty(ad.Description);
        }

        private bool CheckChaletTypology(AdVO ad)
        {
            return ad.Typology == "CHALET" && ad.HouseSize.HasValue && ad.GardenSize.HasValue && !string.IsNullOrEmpty(ad.Description);
        }

        private bool CheckGarageTypology(AdVO ad)
        {
            return ad.Typology == "GARAGE";
        }
    }
}
