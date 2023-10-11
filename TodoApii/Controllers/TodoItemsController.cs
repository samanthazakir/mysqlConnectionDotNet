using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using TodoApi.Models;
using TodoApii.Models;

namespace TodoApii.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly TimetableDbContext _timetableDbContext;

        private readonly IConfiguration _configuration;


        public TodoItemsController(TodoContext context, IConfiguration configuration, 
            TimetableDbContext timetableDbCOntext)
        {
            _context = context;
            _timetableDbContext = timetableDbCOntext;
            _configuration = configuration;
        }

        // GET: api/TodoItems/GetTodoItems
        [HttpGet]
        public ActionResult<IEnumerable<TodoItem>> GetTodoItems()
        {
          if (_context.TodoItems == null)
          {
              return NotFound();
          }
            //string connectionString = "Server=127.0.0.1;Database=timetable;User=root;Password=1234;";
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT * FROM timetable_user";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Access columns by name or index
                                int id = reader.GetInt32("id");
                                string name = reader.GetString("name");
                                int age = reader.GetInt32("age");

                                Console.WriteLine($"ID: {id}, Name: {name}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"MySQL Error: {ex.Message}");
                }
            }
            string param1 = HttpContext.Request.Query["param1"];
            string param2 = HttpContext.Request.Query["param2"];

            return _context.TodoItems.ToList();
        }
        // GET: api/TodoItems/5/8
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Timetable_User>>> GetTimetableUser(long id)
        {
            var users = _timetableDbContext.TimetableUsers.ToList();

            return users;

            //if (_context.TodoItems == null)
            //{
            //    return NotFound();
            //}
            //var todoItem = await _context.TodoItems.FindAsync(id);

            //if (todoItem == null)
            //{
            //    return NotFound();
            //}

            //return todoItem;
        }
        // GET: api/TodoItems/5/8
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
          if (_context.TodoItems == null)
          {
              return NotFound();
          }
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
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

        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
        {
          if (_context.TodoItems == null)
          {
              return Problem("Entity set 'TodoContext.TodoItems'  is null.");
          }
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            if (_context.TodoItems == null)
            {
                return NotFound();
            }
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoItemExists(long id)
        {
            return (_context.TodoItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
