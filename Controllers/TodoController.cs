using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TodoController : ControllerBase
	{
		private readonly TodoContext _todoContext;
		public TodoController(TodoContext todoContext)
		{
			_todoContext = todoContext;
		}

		[HttpGet]
		public async Task<IActionResult> GetAction()
		{
			var res = _todoContext.TodoItem.FromSqlRaw("SELECT id, title, isComplete FROM FUNC_TODOINFO() Order By Title DESC")
				.ToList();
			return Ok(res);
		}

		[HttpPost("{comment}")]
		//[Route()]
		public async Task<IActionResult> GetActionWithComment(TodoItem todoItem, string comment = "Default")
		{
			_todoContext.TodoItem.FromSqlRaw($"EXEC DBO.PROC_ADDTODO @TITLE='{todoItem.Title}', @COMMENT='{comment}'");
			return Ok("OK");
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetActionByID(int id)
		{
			var todoItem = await _todoContext.TodoItem.FindAsync(id);
			if (todoItem == null) { return NotFound(); }
			return Ok(todoItem);
		}


		[HttpGet]
		[Route("cake-by-id")]
		public async Task<IActionResult> GetActionByID_Route(int id)
		{
			var todoItem = await _todoContext.TodoItem.FindAsync(id);
			if (todoItem == null) { return NotFound(); }
			return Ok(todoItem);
		}

		[HttpPost]
		public async Task<IActionResult> PostAction(TodoItem todoItem)
		{
			_todoContext.TodoItem.Add(todoItem);
			await _todoContext.SaveChangesAsync();
			return Created("/", todoItem);
		}

		[HttpPut]
		public async Task<IActionResult> PutAction(TodoItem todoItem)
		{
			_todoContext.Update(todoItem);
			await _todoContext.SaveChangesAsync();
			return Ok(todoItem);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteAction(int id)
		{
			var todo = await _todoContext.TodoItem.FindAsync(id);
			if (todo == null)
			{
				return NotFound();
			}
			_todoContext.Remove(todo);
			await _todoContext.SaveChangesAsync();
			return Created("/", todo);
		}
	}
}
