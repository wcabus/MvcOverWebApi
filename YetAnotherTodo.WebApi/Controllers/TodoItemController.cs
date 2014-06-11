using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Microsoft.AspNet.Identity;
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
            var data = await TodoItemRepository.FindByUserIdAsync(User.Identity.GetUserId());

            return Ok(data);
        }

        //GET /api/TodoItem/{id}
        /// <summary>
        /// Retrieves the TodoItem that matches the given <paramref name="id"/>.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IHttpActionResult> Get(string id)
        {
            var data = await TodoItemRepository.FindByIdAndUserIdAsync(id, User.Identity.GetUserId());
            if (data == null)
            {
                return NotFound();
            }

            return Ok(data);
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
                User = await UserManager.FindByIdAsync(User.Identity.GetUserId()),
                Description = model.Description,
                Done = false
            };

            await TodoItemRepository.CreateAsync(todoItem);
            return CreatedAtRoute("DefaultApi", new {todoItem.Id}, todoItem);
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
