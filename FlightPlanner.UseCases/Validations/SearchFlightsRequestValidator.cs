using FlightPlanner.Core.Models;
using FluentValidation;

namespace FlightPlanner.UseCases.Validations
{
    public class SearchFlightsRequestValidator : AbstractValidator<SearchFlightsRequest>
    {
        public SearchFlightsRequestValidator()
        {
            RuleFor(search => search.From).NotEmpty();
            RuleFor(search => search.To).NotEmpty();
            RuleFor(search => search).Must(search => search.From != search.To);
        }
    }
}
