using ApiUniversity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiUniversity.Controllers;

[ApiController]
[Route("api/enrollment")]
public class EnrollmentController : ControllerBase
{
    private readonly UniversityContext _context;

    public EnrollmentController(UniversityContext context)
    {
        _context = context;
    }

    // GET: api/enrollment/5
    [HttpGet("{id}")]
    public async Task<ActionResult<DetailedEnrollmentDTO>> GetEnrollment(int id)
    {
        // Find enrollment and related list
        // SingleAsync() throws an exception if no enrollment is found (which is possible, depending on id)
        // SingleOrDefaultAsync() is a safer choice here
        var enrollment = await _context.Enrollments.SingleOrDefaultAsync(t => t.Id == id);

        if (enrollment == null)
        {
            return NotFound();
        }

        return new DetailedEnrollmentDTO(enrollment);
    }

    // POST: api/enrollment
    [HttpPost]
    public async Task<ActionResult<Enrollment>> PostEnrollment(DetailedEnrollmentDTO DetailedEnrollmentDTO)
    {
        Enrollment enrollment = new(DetailedEnrollmentDTO);

        _context.Enrollments.Add(enrollment);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetEnrollment), new { id = enrollment.Id }, new DetailedEnrollmentDTO(enrollment));
    }
}