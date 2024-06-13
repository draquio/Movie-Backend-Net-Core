using MovieCRUD_NCapas.DTO.Review;

namespace MovieCRUD_NCapas.Services.Interface
{
    public interface IReviewService
    {
        Task<List<ReviewDTO>> GetReviews();
        Task<ReviewDTO> GetById(int id);
        Task<ReviewDTO> Create(ReviewDTO reviewDTO);
        Task<bool> Update(ReviewDTO reviewDTO);
        Task<bool> Delete(int id);
    }
}
