using System;
using System.Collections.Generic;

namespace coding_test_ranking.infrastructure.persistence.models
{
    public class AdVO : Entity
    {
        public string Typology { get; set; }
        public string Description { get; set; }
        public List<int> Pictures { get; set; }
        public int? HouseSize { get; set; }
        public int? GardenSize { get; set; }
        public int? Score { get; set; }
        public DateTime? IrrelevantSince { get; set; }

        public AdVO Clone()
        {
            return (AdVO)MemberwiseClone();
        }
    }
}
