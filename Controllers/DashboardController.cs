using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task9.Data;
using Task9.Models;

namespace Task9.Controllers;

[Authorize]
public class DashboardController : Controller
{
    private readonly AppDbContext _context;

    public DashboardController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var notes = await _context.Notes
            .Where(n => n.AppUserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();

        ViewBag.Notes = notes;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddNote(string title, string content)
    {
        var uid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(content))
            return RedirectToAction("Index");

        var note = new UserNote();
        note.AppUserId = uid;
        note.Title = title;
        note.Content = content;
        note.CreatedAt = DateTime.Now;

        _context.Notes.Add(note);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }
}
