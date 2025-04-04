using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Services;

namespace TodoApp.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoListController : ControllerBase
{
    private readonly TodoListService _service;

    public TodoListController(TodoListService service)
    {
        _service = service;
    }

    [HttpPost("add")]
    public IActionResult AddItem([FromQuery] string title, [FromQuery] string description, [FromQuery] string category)
    {
        try
        {
            int id = _service.CreateTodoItem(title, description, category);
            return Ok(new { Id = id, Message = "TodoItem agregado exitosamente" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpPost("progression")]
    public IActionResult RegisterProgression([FromQuery] int id, [FromQuery] DateTime dateTime, [FromQuery] decimal percent)
    {
        try
        {
            _service.AddProgression(id, dateTime, percent);
            return Ok(new { Message = "Progresión registrada exitosamente" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpPut("update")]
    public IActionResult UpdateItem([FromQuery] int id, [FromQuery] string newDescription)
    {
        try
        {
            _service.UpdateTodoItem(id, newDescription);
            return Ok(new { Message = "TodoItem actualizado exitosamente" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpDelete("remove")]
    public IActionResult RemoveItem([FromQuery] int id)
    {
        try
        {
            _service.RemoveTodoItem(id);
            return Ok(new { Message = "TodoItem removido exitosamente" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpGet("print")]
    public IActionResult PrintItems()
    {
        try
        {
            // En lugar de imprimir en consola, se captura la salida en un string para devolverla.
            // Aquí se redirige la salida estándar temporalmente.
            using var sw = new System.IO.StringWriter();
            var originalOut = Console.Out;
            Console.SetOut(sw);

            _service.PrintTodoItems();

            Console.SetOut(originalOut);
            string result = sw.ToString();

            return Ok(new { Result = result });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
}