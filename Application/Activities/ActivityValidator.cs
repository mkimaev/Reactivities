using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using FluentValidation;

namespace Application.Activities
{
    public class ActivityValidator: AbstractValidator<Activity>
    {
        public ActivityValidator()
        {
            RuleFor(item => item.Title).NotEmpty();
            RuleFor(item => item.Description).NotEmpty();
            RuleFor(item => item.Date).NotEmpty();
            RuleFor(item => item.Category).NotEmpty();
            RuleFor(item => item.City).NotEmpty();
            RuleFor(item => item.Venue).NotEmpty();
        }
    }
}