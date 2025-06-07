using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeToolsWebApp.Application.Features.Tools.Queries.GetToolDetails
{
    public record GetToolDetailsQuery(Guid Id) : IRequest<GetToolDetailsDto>;
}
