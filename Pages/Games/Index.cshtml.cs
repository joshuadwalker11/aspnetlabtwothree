using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using rp_ef_maria.Models;

namespace rp_ef_maria.Pages.Games
{
    public class IndexModel : PageModel
    {
        private readonly StoreContext _context;

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(StoreContext dbcontext, ILogger<IndexModel> logger)
        {
            _context = dbcontext;
            _logger = logger;
        }

        public IList<Game> Game { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string Query { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public DateTime DateSearch { get; set; } = DateTime.Now;

        [BindProperty(SupportsGet = true)]
        public string SelectRange { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool FilterOn { get; set; } // automatically has default of 'false'

        public async Task OnGetAsync()
        {
            IQueryable<Game> games; // story games query
            if (Query != null)
            {
                // if title query is not empty, search for titles that contain the query
                games = _context.Game.Where(g => g.Title.Contains(Query));
            }
            else
            {
                // otherwise, get all games
                games = _context.Game;
            }

            // add to query (further filter) to get games released in the last 5 years
            // games = games.Where(g => g.ReleaseDate > DateTime.Now.AddYears(-5));

            // _logger.LogInformation("filter state: " + FilterOn);
            // _logger.LogInformation("Select Range: " + SelectRange);

            // if filter is 
            if (FilterOn)
            {
                if (SelectRange == "before")
                {
                    games = games.Where(g => g.ReleaseDate <= DateSearch);
                }
                else
                {
                    games = games.Where(g => g.ReleaseDate > DateSearch);
                }
            }

            // do the query, store in a list (do it asynchronously, so other program segments can run)
            Game = await games.ToListAsync();
            // render the page
            Page();

        }

    }
}
