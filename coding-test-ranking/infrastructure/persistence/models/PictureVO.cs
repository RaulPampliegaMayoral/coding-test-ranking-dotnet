
namespace coding_test_ranking.infrastructure.persistence.models
{
    public class PictureVO : Entity
    {
        public string Url { get; set; }
        public string Quality { get; set; }

        public PictureVO Clone()
        {
            return (PictureVO)MemberwiseClone();
        }

    }
}
