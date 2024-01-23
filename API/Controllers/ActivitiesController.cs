using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers
{
    public class ActivitiesController : BaseApiController
    {
        private readonly DataContext _context;
        public ActivitiesController(DataContext context)
        {
            this._context = context;
        }

        [HttpGet] //api/activities
        public async Task<ActionResult<List<Activity>>> GetAcitivties()
        {
            return await _context.Activities.ToListAsync<Activity>(); 
        }

        [HttpGet("{id}")] //api/acitivities/id
        public async Task<ActionResult<Activity>> GetActivity(Guid id)
        {
            return await _context.Activities.FindAsync(id);
        }
    }
}