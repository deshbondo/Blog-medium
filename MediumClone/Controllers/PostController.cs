using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediumClone.Models;
using Microsoft.AspNetCore.Identity;

namespace MediumClone.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public PostController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Post
        public async Task<IActionResult> Index(string category, string sortOrder)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["CurrentCategory"] = category;

            var posts = from p in _context.Posts.Include(p => p.User)
                        select p;

            if (!string.IsNullOrEmpty(category) && category != "All")
            {
                posts = posts.Where(p => p.Category.ToLower() == category.ToLower());
            }

            switch (sortOrder)
            {
                case "mostViewed":
                    posts = posts.OrderByDescending(p => p.ViewCount);
                    break;
                case "recent":
                    posts = posts.OrderByDescending(p => p.CreatedAt);
                    break;
                default:
                    posts = posts.OrderBy(p => p.Title);
                    break;
            }

            return View(await posts.ToListAsync());
        }

        // GET: Post/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            // Increment the view count
            post.ViewCount++;
            _context.Update(post);
            await _context.SaveChangesAsync();

            return View(post);
        }

        // GET: Post/Create
        public IActionResult Create()
        {
            return View(new Post()); // Pass a new Post object to the view
        }

        // POST: Post/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Content,Category")] Post post)
        {
            if (ModelState.IsValid)
            {
                post.CreatedAt = DateTime.Now;
                post.ViewCount = 0;
                post.UserId = _userManager.GetUserId(User); // Retrieve the logged-in user's ID
                post.Username = User.Identity.Name; // Set the username to the logged-in user's username
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // Redirect to the Index page where users can manage their posts
            }
            return View(post);
        }

        // GET: Post/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);
            if (post == null || post.UserId != _userManager.GetUserId(User))
            {
                return Forbid(); // Restrict access if the user is not the creator
            }

            return View(post);
        }

        // POST: Post/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,Category")] Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (post.UserId != _userManager.GetUserId(User))
            {
                return Forbid(); // Restrict access if the user is not the creator
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        // GET: Post/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);
            if (post == null || post.UserId != _userManager.GetUserId(User))
            {
                return Forbid(); // Restrict access if the user is not the creator
            }

            return View(post);
        }

        // POST: Post/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post.UserId != _userManager.GetUserId(User))
            {
                return Forbid(); // Restrict access if the user is not the creator
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Trending()
        {
            var posts = await _context.Posts
                .OrderByDescending(p => p.ViewCount)
                .ToListAsync();
            return View("Index", posts);
        }

        public async Task<IActionResult> Latest()
        {
            var posts = await _context.Posts
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
            return View("Index", posts);
        }
    }
}