using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Microsoft.AspNet.Identity.Owin;
using YetAnotherTodo.Domain;
using YetAnotherTodo.EF.Repository;
using YetAnotherTodo.WebApi.Models;

namespace YetAnotherTodo.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/TodoItem")]
    [EnableCors("http://localhost:64791", "*", "*", SupportsCredentials = true)]
    public class TodoItemController : ApiController
    {
        private TodoItemRepository _todoItemRepository;
        private ApplicationUserManager _userManager;

        public TodoItemController()
        {
        }

        public TodoItemController(TodoItemRepository todoItemRepository, ApplicationUserManager userManager)
        {
            TodoItemRepository = todoItemRepository;
            UserManager = userManager;
        }

        private TodoItemRepository TodoItemRepository
        {
            get
            {
                return _todoItemRepository ?? Request.GetOwinContext().Get<TodoItemRepository>();
            }
            set
            {
                _todoItemRepository = value;
            }
        }

        private ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set
            {
                _userManager = value;
            }
        }

        //GET /api/TodoItem
        /// <summary>
        /// Retrieves all todo items for the current user
        /// </summary>
        /// <returns></returns>
        public async Task<IHttpActionResult> Get()
        {
            /*
             * Note: when using User.Identity.GetLoginId(), make sure that the ID is the same!
             * When logging on using the cookie, the ID will be the value of the NameIdentifier claim (email address in this case).
             * When logging on using the bearer token, the ID will be the actual user ID.
             */
            var data = await TodoItemRepository.FindByUserNameAsync(User.Identity.Name);

            //Transforms each Domain.TodoItem into a Models.Output.TodoItem
            return Ok(data.Select(Models.Output.TodoItem.Project));
        }

        //GET /api/TodoItem/{id}
        /// <summary>
        /// Retrieves the TodoItem that matches the given <paramref name="id"/>.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IHttpActionResult> Get(string id)
        {
            var data = await TodoItemRepository.FindByIdAndUserNameAsync(id, User.Identity.Name);
            if (data == null)
            {
                return NotFound();
            }

            return Ok(Models.Output.TodoItem.Project(data));
        }

        //POST /api/TodoItem
        /// <summary>
        /// Creates a new Todo Item for this user (using POST because we're generating an ID)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IHttpActionResult> Post(TodoItemModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var todoItem = new TodoItem
            {
                User = await UserManager.FindByNameAsync(User.Identity.Name),
                Description = model.Description,
                Done = false
            };

            await TodoItemRepository.CreateAsync(todoItem);
            return CreatedAtRoute("DefaultApi", new { todoItem.Id }, Models.Output.TodoItem.Project(todoItem));
        }

        //POST /api/TodoItem/Done
        /// <summary>
        /// Marks a Todo Item as done
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}/Done")]
        public async Task<IHttpActionResult> MarkAsDone(string id)
        {
            var todoItem = await TodoItemRepository.FindByIdAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.Done = true;
            await TodoItemRepository.UpdateAsync(todoItem);

            return Ok();
        }
    }
}
