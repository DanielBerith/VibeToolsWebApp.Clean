using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibeToolsWebApp.Application.Features.Reviews.Command.CreateReview;
using VibeToolsWebApp.Application.Features.Reviews.Queries.GetReviews;
using VibeToolsWebApp.Domain.Entities;

namespace VibeToolsWebApp.Application.MappingProfile
{
    public class ReviewsProfile : Profile
    {
        public ReviewsProfile()
        {
            CreateMap<CreateReviewCommand, Review>();
            CreateMap<Review, GetReviewsDto>();
        }
    }
}
