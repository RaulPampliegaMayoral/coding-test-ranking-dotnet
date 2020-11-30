using coding_test_ranking.infrastructure.persistence.models;

namespace coding_test_ranking.infrastructure.services
{
    public interface IPicturesService
    {
        PictureVO GetOrSavePicture(string url, string quality);
    }
}
