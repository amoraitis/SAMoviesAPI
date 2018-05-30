using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAMoviesAPI.Contexts;
using SAMoviesAPI.Models;

namespace SAMoviesAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/users")]
    public class UsersController : Controller
    {
        private readonly UserContext _context;

        public UsersController(UserContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns the list of the Users.
        /// </summary>
        [HttpGet]
        public IEnumerable<User> GetUsers()
        {
            return _context.Users;
        }

        /// <summary>
        /// Finds a User by username and password.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [HttpGet("login")]
        public async Task<IActionResult> GetUserByUserName([FromQuery] string username, [FromQuery] string password)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Users
                .SingleOrDefaultAsync(m => m.Username == username && m.Password == password);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }


        /// <summary>
        /// Find a User by username.
        /// </summary>
        /// <param name="username"></param>
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [HttpGet("exist/{username}")]
        public async Task<IActionResult> UserExist([FromRoute] string username)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Users.SingleOrDefaultAsync(m => m.Username == username);

            if (user == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        /// <summary>
        /// Find a User by id.
        /// </summary>
        /// <param name="id"></param>
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Users.SingleOrDefaultAsync(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        /// <summary>
        /// Update an existing User.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser([FromRoute] int id, [FromBody] User user)
        {
            if (!ModelState.IsValid || user == null)
            {
                return BadRequest(ModelState);
            }

            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest(Json("Couldn't update item!"));
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Creates a User.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>A newly created User</returns> 
        [ProducesResponseType(typeof(User), 201)]
        [ProducesResponseType(400)]
        [HttpPost]
        public async Task<IActionResult> PostUser([FromBody] User user)
        {
            try
            {
                if (!ModelState.IsValid || user == null)
                {
                    return BadRequest(ModelState);
                }

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest(Json("Couldn't create item!"));
            }
            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        /// <summary>
        /// Deletes a User.
        /// </summary>
        /// <param name="id"></param>
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = await _context.Users.SingleOrDefaultAsync(m => m.Id == id);
                if (user == null)
                {
                    return NotFound();
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest(Json("Couldn't delete item!"));
            }
            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}