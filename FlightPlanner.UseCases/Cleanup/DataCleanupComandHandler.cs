using FlightPlanner.Core.Models;
using FlightPlanner.Core.Services;
using FlightPlanner.UseCases.Models;
using MediatR;
using System.Net;

namespace FlightPlanner.UseCases.Cleanup
{
    public class DataCleanupComandHandler : IRequestHandler<DataCleanupComand, ServiceResult>
    {
        private readonly IDbService _dbService;
        public DataCleanupComandHandler (IDbService dbService)
        {
            _dbService = dbService;
        }
        public Task<ServiceResult> Handle(DataCleanupComand request, CancellationToken cancellationToken)
        {
            _dbService.DeleteAll<Flight>();
            _dbService.DeleteAll<Airport>();

            return Task.FromResult(new ServiceResult());
        }
    }
}
