
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backlogged_api.Data;
using backlogged_api.Models;
using backlogged_api.DTO.User;
using Microsoft.AspNetCore.Identity;
using backlogged_api.Helpers;
using backlogged_api.DTO.Backlog;
using backlogged_api.DTO.Game;
using Newtonsoft.Json;
using backlogged_api.DTO.Review;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace backlogged_api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly BackloggedDBContext _context;
        private readonly IConfiguration _configuration;

        public UserController(BackloggedDBContext context, UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _configuration = configuration;
        }

        /// <summary>
        /// Gets all Users.
        /// </summary>
        /// <returns>All Users</returns>
        /// <response code="200">Returns the Users correctly</response>
        // GET: api/Users
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers([FromQuery] PagingParams pagingParams)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }

            var users = await PageListBuilder.CreatePagedListAsync(
                _context.Users,
                m => m.UserName,
                OrderDirection.Ascending,
                pagingParams.PageNumber,
                pagingParams.PageSize);

            var metadata = new
            {
                users.TotalCount,
                users.PageSize,
                users.CurrentPage,
                users.TotalPages,
                users.HasNext,
                users.HasPrevious
            };

            Response.Headers.Add("X-Pagination", Newtonsoft.Json.JsonConvert.SerializeObject(metadata));

            var userDtos = users.Select(s => new UserDto
            {
                Id = s.Id,
                UserName = s.UserName,
                Email = s.Email,
                PasswordHash = s.PasswordHash,
                ProfileImageUrl = s.ProfileImageUrl,
                NormalizedUserName = s.NormalizedUserName,
                NormalizedEmail = s.NormalizedEmail,
                SecurityStamp = s.SecurityStamp,
                ConcurrencyStamp = s.ConcurrencyStamp,
                BacklogId = s.BacklogId.HasValue ? s.BacklogId.Value : Guid.Empty,
            });
            return Ok(userDtos);
        }
        /// <summary>
        /// Gets a User based on its' id.
        /// </summary>
        /// <returns>User</returns>
        /// <response code="200">Returns the user</response>
        /// <response code="404">User not found</response>
        // GET: api/Users/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<UserDto>> GetUser(Guid id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                return NotFound();
            }

            return Ok(new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                ProfileImageUrl = user.ProfileImageUrl,
                NormalizedUserName = user.NormalizedUserName,
                NormalizedEmail = user.NormalizedEmail,
                SecurityStamp = user.SecurityStamp,
                ConcurrencyStamp = user.ConcurrencyStamp,
            });
        }

        /// <summary>
        /// Logs in a User based on their username and password.Attribute
        /// </summary>
        /// <returns>User</returns>
        /// <response code="201">JWT token created</response>
        /// <response code="404">User not found</response>
        // POST: api/Users/login
        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserDto loginUserDto)
        {

            if (loginUserDto.Email == null)
            {
                return BadRequest("Email is required.");
            }
            var user = await _userManager.FindByEmailAsync(loginUserDto.Email);

            if (user == null)
            {
                return NotFound();
            }
            var res = await _userManager.CheckPasswordAsync(user, loginUserDto.Password);

            if (res == true)
            {
                var token = await CreateTokenAsync(user);
                return Ok(new { token });
            }
            else
            {
                return BadRequest("Password is incorrect.");
            }
        }

        private async Task<List<Claim>> GetClaims(User user)
        {
            var claims = new List<Claim>(){
                new Claim(ClaimTypes.Name, user.UserName!),
            };
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;

        }
        private async Task<string> CreateTokenAsync(User user)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims(user);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return token;
        }
        private SigningCredentials GetSigningCredentials()
        {
            var jwtConfig = _configuration.GetSection("JwtSettings");
            var key = Encoding.UTF8.GetBytes(jwtConfig["Key"]!);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            );
            return token;
        }
        /// <summary>
        /// Updates a User's email based on their id.
        /// </summary>
        /// <returns>User's email</returns>
        /// <response code="200">Returns the user's email</response>
        /// <response code="404">User not found</response>
        // GET: api/Users/5/email
        [HttpPatch("{id}/Email")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PatchEmail(Guid id, UpdateEmailDto emailDto)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                return NotFound();
            }

            if (emailDto.NewEmail == null)
            {
                return BadRequest("New email is required.");
            }

            if (emailDto.CurrentPassword == null)
            {
                return BadRequest("Current password is required.");
            }

            var result = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, emailDto.CurrentPassword);

            if (result != PasswordVerificationResult.Success)
            {
                return BadRequest("Current password is incorrect.");
            }

            user.Email = emailDto.NewEmail;

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                return BadRequest(updateResult.Errors);
            }

            return NoContent();

        }


        /// <summary>
        /// Updates a User's password based on their id.
        /// </summary>
        /// <response code="204">Updates the user's password</response>
        /// <response code="404">User not found</response>
        /// <response code="400">Bad request</response>
        // GET: api/Users/5/email
        [HttpPatch("{id}/Password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PatchPassword(Guid id, UpdatePasswordDto passwordDto)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (passwordDto.NewPassword != passwordDto.ConfirmNewPassword)
            {
                return BadRequest("New passwords must match.");
            }
            var result = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, passwordDto.CurrentPassword);

            if (result != PasswordVerificationResult.Success)
            {
                return BadRequest("Current password is incorrect.");
            }


            var updateResult = await _userManager.ChangePasswordAsync(user, passwordDto.CurrentPassword, passwordDto.NewPassword);

            if (!updateResult.Succeeded)
            {
                return BadRequest(updateResult.Errors);
            }

            return NoContent();

        }




        /// <summary>
        /// Updates a User based on their id.
        /// </summary>
        /// <response code="204">User updated, no response</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">User not found</response>
        // PATCH: api/Users/5
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PatchUser(Guid id, UpdateUserDto userDto)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }

            if (userDto.UserName == user.UserName)
            {
                return BadRequest("Username cannot be the same.");
            }
            user.ProfileImageUrl = userDto.ProfileImageUrl ?? user.ProfileImageUrl;
            user.UserName = userDto.UserName ?? user.UserName;

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _userManager.UpdateAsync(user);
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
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Adds a new User to the store.
        /// </summary>
        /// <returns>User</returns>
        /// <response code="201">Returns the user</response>
        /// <response code="400">Bad request</response>
        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<ActionResult<UserDto>> PostUser(CreateUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_context.Users == null)
            {
                return Problem("Entity set 'BackloggedDBContext.User'  is null.");
            }

            if (_userManager.FindByEmailAsync(userDto.Email).Result != null)
            {
                return BadRequest("Email is already in use.");
            }

            var user = new User
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
            };
            var result = await _userManager.CreateAsync(user, userDto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            // SCUFFED BEYOND ALL MEASURES, BUT IT WORKS (will have to check it sometime though)
            var backlog = new Backlog
            {
                UserId = user.Id
            };


            await _userManager.UpdateAsync(user);
            _context.Backlogs.Add(backlog);
            await _context.SaveChangesAsync();
            user.BacklogId = backlog.Id;
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Created("", user);
        }

        /// <summary>
        /// Gets the reviews for a User.
        /// </summary>
        /// <returns>backlog</returns>
        /// <response code="200">Reviews</response>
        /// <response code="404">Backlog not found</response>
        // Get: api/Backlogs/uuid/Reviews
        [HttpGet("{id}/Reviews")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviews(Guid id, [FromQuery] PagingParams pagingParams)
        {
            if (_context.Backlogs == null)
            {
                return NotFound();
            }

            var reviews = await PageListBuilder.CreatePagedListAsync(_context.Users
                .Include(i => i.Reviews)
                .Where(w => w.Id == id)
                .SelectMany(s => s.Reviews), m => m.Rating, pagingParams.PageNumber, pagingParams.PageSize);

            var reviewDtos = reviews.Select(s => new ReviewDto
            {
                Id = s.Id,
                GameId = s.GameId,
                AuthorId = s.AuthorId,
                Rating = s.Rating,
                Details = s.Details,
                CreatedAt = s.CreatedAt,
            });

            var metadata = new
            {
                reviews.TotalCount,
                reviews.PageSize,
                reviews.CurrentPage,
                reviews.TotalPages,
                reviews.HasNext,
                reviews.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(reviewDtos);
        }

        /// <summary>
        /// Deletes a User from the store.
        /// </summary>
        /// <returns>User</returns>
        /// <response code="204">User deleted, no response</response>
        /// <response code="404">User not found</response>
        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                return NotFound();
            }
            var backlog = await _context.Backlogs.FindAsync(user.BacklogId);

            if (backlog == null)
            {
                return NotFound("Backlog not found.");
            }

            _context.Backlogs.Remove(backlog);
            await _userManager.DeleteAsync(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Gets a User's backlog.
        /// </summary>
        /// <returns>Backlog</returns>
        /// <response code="200">Returns the users correctly</response>
        /// <response code="404">User not found</response>
        // GET: api/Users
        [HttpGet("{id}/backlog")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUserBacklog(Guid id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }

            var backlog = await _context.Backlogs
            .Where(w => w.UserId == id)
            .Select(s => new BacklogDto
            {
                Id = s.Id,
                UserId = s.UserId,
                IsVisible = s.IsVisible,
            }).FirstOrDefaultAsync();



            if (backlog == null)
            {
                return NotFound();
            }


            return Ok(backlog);
        }
        private bool UserExists(Guid id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
