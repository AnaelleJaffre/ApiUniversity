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
        var enrollment = await _context.Enrollments
                                    .Include(e => e.Student)  // inclure l'étudiant associé
                                    .Include(e => e.Course)   // inclure le cours associé
                                    .SingleOrDefaultAsync(e => e.Id == id); // trouver l'inscription avec l'ID spécifique

        if (enrollment == null)
        {
            return NotFound();
        }

        return new DetailedEnrollmentDTO(enrollment);
    }

    // POST: api/enrollment
    [HttpPost]
    public async Task<ActionResult<Enrollment>> PostEnrollment(EnrollmentDTO enrollmentDTO)
    {
        Enrollment enrollment = new Enrollment(enrollmentDTO);

        _context.Enrollments.Add(enrollment);
        await _context.SaveChangesAsync();

        enrollment.Student =  await _context.Students.FirstAsync(e => e.Id == enrollment.StudentId); 
        enrollment.Course =  await _context.Courses.FirstAsync(e => e.Id == enrollment.CourseId); 

        return CreatedAtAction(nameof(GetEnrollment), new { id = enrollment.Id }, new DetailedEnrollmentDTO(enrollment));
    }
}