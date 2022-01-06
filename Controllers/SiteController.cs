using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiteAccessManager.Adapters;
using SiteAccessManager.Models;
using SiteAccessManager.Services;

namespace SiteAccessManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SiteController : ControllerBase
    {
        private readonly SiteContext _context;
        private readonly AccessService accessService = new AccessService();

        public SiteController(SiteContext context)
        {
            _context = context;
        }

        // GET: api/Site
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Site>>> GetSites()
        {
            return await _context.Sites.ToListAsync();
        }

        // GET: api/Site/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Site>> GetSite(long id)
        {
            var site = await _context.Sites.FindAsync(id);

            if (site == null)
            {
                return NotFound();
            }

            return site;
        }

        // PUT: api/Site/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSite(long id, Site site)
        {
            if (id != site.Id)
            {
                return BadRequest();
            }

            _context.Entry(site).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SiteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Site
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Site>> PostSite(Site site)
        {
            _context.Sites.Add(site);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSite), new { id = site.Id }, site);
        }

        // POST: api/Site/hit/5
        [HttpPost("hit/{id}")]
        public async Task<ActionResult<AccessAdapter>> PostSiteHit(long id)
        {
            var site = await _context.Sites.FindAsync(id);

            if (site == null)
            {
                return NotFound();
            }

            return await this.accessService.Hit(site);
        }

        // GET: api/Site/hits/5
        [HttpGet("hits/{id}")]
        public async Task<ActionResult<AccessAdapter>> GetSiteHit(long id)
        {
            var site = await _context.Sites.FindAsync(id);

            if (site == null)
            {
                return NotFound();
            }

            return await this.accessService.Hits(site);
        }

        // DELETE: api/Site/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSite(long id)
        {
            var site = await _context.Sites.FindAsync(id);
            if (site == null)
            {
                return NotFound();
            }

            _context.Sites.Remove(site);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SiteExists(long id)
        {
            return _context.Sites.Any(e => e.Id == id);
        }
    }
}
