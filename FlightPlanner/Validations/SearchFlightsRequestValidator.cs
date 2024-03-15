using FlightPlanner.Core.Models;
using FluentValidation;

namespace FlightPlanner.Validations
{
    public class SearchFlightsRequestValidator : AbstractValidator<SearchFlightsRequest>
    {
        public SearchFlightsRequestValidator()
        {
            RuleFor(search => search.From).NotEmpty().WithMessage("Departure airport is required.");
            RuleFor(search => search.To).NotEmpty().WithMessage("Destination airport is required.");
            RuleFor(search => search).Must(x => x.From != x.To).WithMessage("Departure and destination airports cannot be the same.");
        }
    }
}
