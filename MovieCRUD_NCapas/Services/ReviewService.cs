using AutoMapper;
using MovieCRUD_NCapas.DTO.Review;
using MovieCRUD_NCapas.Models;
using MovieCRUD_NCapas.Repository.Interface;
using MovieCRUD_NCapas.Services.Interface;

namespace MovieCRUD_NCapas.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IGenericRepository<Review> _reviewRepository;
        private readonly IMapper _mapper;
        public ReviewService(IGenericRepository<Review> reviewRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }
        public async Task<List<ReviewDTO>> GetReviews(int page, int pageSize)
        {
            try
            {
                List<Review> listReviews = await _reviewRepository.GetAll(page, pageSize);
                if (listReviews == null || !listReviews.Any())
                {
                    return new List<ReviewDTO>();
                }
                List<ReviewDTO> listReviewDTO = _mapper.Map<List<ReviewDTO>>(listReviews);
                return listReviewDTO;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while retrieving the Reviews: {ex.Message}", ex);
            }
        }
        public async Task<ReviewDTO> GetById(int id)
        {
            try
            {
                Review reviewSearched = await _reviewRepository.GetById(id);
                if (reviewSearched == null)
                {
                    throw new KeyNotFoundException($"Review with ID {id} not found");
                }
                ReviewDTO reviewDTO = _mapper.Map<ReviewDTO>(reviewSearched);
                return reviewDTO;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while retrieving the Review: {ex.Message}", ex);
            }
        }
        public async Task<ReviewDTO> Create(ReviewDTO reviewDTO)
        {
            try
            {
                Review review = _mapper.Map<Review>(reviewDTO);
                Review reviewCreated = await _reviewRepository.Create(review);
                if (reviewCreated == null || reviewCreated.Id == 0)
                {
                    throw new InvalidOperationException("Review couldn't be created");
                }
                ReviewDTO reviewCreatedDTO = _mapper.Map<ReviewDTO>(reviewCreated);
                return reviewCreatedDTO;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while creating the Review: {ex.Message}", ex);
            }
        }
        public async Task<bool> Update(ReviewDTO reviewDTO)
        {
            try
            {
                Review reviewSearched = await _reviewRepository.GetById(reviewDTO.Id);
                if (reviewSearched == null)
                {
                    throw new KeyNotFoundException($"Review with ID {reviewDTO.Id} not found");
                }
                _mapper.Map(reviewDTO, reviewSearched);
                bool response = await _reviewRepository.Update(reviewSearched);
                if (!response)
                {
                    throw new InvalidOperationException("Review couldn't be updated");
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while updating the Review: {ex.Message}", ex);
            }
        }
        public async Task<bool> Delete(int id)
        {
            try
            {
                Review reviewSearched = await _reviewRepository.GetById(id);
                if (reviewSearched == null)
                {
                    throw new KeyNotFoundException($"Review with ID {id} not found");
                }
                bool response = await _reviewRepository.Delete(reviewSearched);
                if (!response)
                {
                    throw new InvalidOperationException("Review couldn't be deleted");
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while deleting the Review: {ex.Message}", ex);
            }
        }
    }
}
