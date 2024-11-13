using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiUniversity.Models;

namespace ApiUniversity.Controllers;

[ApiController]
[Route("api/student")]
public class StudentController : ControllerBase
{
    private readonly UniversityContext _context;

    public StudentController(UniversityContext context)
    {
        _context = context;
    }

    // GET: api/student
    [HttpGet]
    public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudents()
    {
        var students = await _context.Students.ToListAsync();
        var studentDTOs = students.Select(student => new StudentDTO
        {
            Id = student.Id,
            FirstName = student.FirstName,
            LastName = student.LastName,
            Email = student.Email,
            EnrollmentDate = student.EnrollmentDate
        }).ToList();

        return studentDTOs;
    }



    // GET: api/student/2
    [HttpGet("{id}")]
    public async Task<ActionResult<StudentDTO>> GetStudent(int id)
    {
        // Find student and related list
        // SingleAsync() throws an exception if no student is found (which is possible, depending on id)
        // SingleOrDefaultAsync() is a safer choice here
        var student = await _context.Students.SingleOrDefaultAsync(t => t.Id == id);

        if (student == null)
            return NotFound();

        return new StudentDTO(student);
    }

    // POST: api/student
    [HttpPost]
    public async Task<ActionResult<StudentDTO>> PostStudent(StudentDTO studentDTO)
    {
        // Conversion du DTO en entité Student
        var student = new Student
        {
            Id = studentDTO.Id, // Si tu veux permettre à l'utilisateur de spécifier un Id, sinon laisse le vide.
            FirstName = studentDTO.FirstName,
            LastName = studentDTO.LastName,
            // Ajoute d'autres propriétés selon ton modèle
        };

        // Ajout du nouvel étudiant dans la base de données
        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        // Retourne le DTO correspondant
        var createdStudentDTO = new StudentDTO
        {
            Id = student.Id,
            FirstName = student.FirstName,
            LastName = student.LastName,
            // Ajoute d'autres propriétés si nécessaire
        };

        return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, createdStudentDTO);
    }

    // PUT: api/student/2
    [HttpPut("{id}")]
    public async Task<IActionResult> PutStudent(int id, StudentDTO studentDTO)
    {
        if (id != studentDTO.Id)
            return BadRequest();

        // Conversion du StudentDTO en Student
        var student = new Student
        {
            Id = studentDTO.Id,
            FirstName = studentDTO.FirstName,
            LastName = studentDTO.LastName,
            // Ajoute d'autres propriétés selon ton modèle
        };

        // Mise à jour de l'étudiant dans la base de données
        _context.Entry(student).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Students.Any(m => m.Id == id))
                return NotFound();
            else
                throw;
        }

        return NoContent();
    }


    // DELETE: api/student/2
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudentItem(int id)
    {
        var student = await _context.Students.FindAsync(id);

        if (student == null)
            return NotFound();

        // Suppression de l'étudiant
        _context.Students.Remove(student);
        await _context.SaveChangesAsync();

        // Retourner le DTO de l'étudiant supprimé (si nécessaire)
        var studentDTO = new StudentDTO
        {
            Id = student.Id,
            FirstName = student.FirstName,
            LastName = student.LastName,
            // Ajoute d'autres propriétés si nécessaire
        };

        return Ok(studentDTO); // Optionnel, retourne un DTO après la suppression
    }

}