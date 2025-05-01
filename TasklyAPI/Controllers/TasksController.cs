using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Taskly.Core;
using Taskly.Core.DTOs;
using Taskly.Core.Enums;
using Taskly.Core.Models;


namespace TasklyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TasksController(IUnitOfWork unitOfWork , IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpPost("CreateTask")]
        [Authorize]
        public IActionResult CreateTask(TaskDTO Request)
        {
            var CurrentUser = _httpContextAccessor.HttpContext?.User;
            TaskTodo Task = new()
            {
                Title = Request.TaskTitle,
                Description = Request.TaskDescription,
                DeadLine = Request.TaskDeadLine,
                ownerId = CurrentUser.FindFirst("AppUserId").Value,
                Status = Request.TaskStatus
                
            };
            _unitOfWork.TasksToDo.Create(Task);
            _unitOfWork.complete();
            return Created();
        }

        [HttpGet("getAllTasks")]
        public IActionResult GetAllTasks()
        {
            var CurrentUser = _httpContextAccessor.HttpContext?.User;
            var AllUserTasks = _unitOfWork.TasksToDo.getAll();
                //.Where(T => T.ownerId == CurrentUser.FindFirst("AppUserId").Value);
            return Ok(AllUserTasks);
        }


        [HttpPut("UpdateTask")]
        [Authorize]
        public IActionResult UpdateTask(TaskTodo request)
        {
            var UpdatedTask = _unitOfWork.TasksToDo.Update(request);
            _unitOfWork.complete();
            return Ok(UpdatedTask);
        }

        [HttpDelete("DeleteTaskById {id:int}")]
        [Authorize]
        public IActionResult DeleteTask(int id)
        {
            _unitOfWork.TasksToDo.DeleteById(T => T.Id == id);
            _unitOfWork.complete();
            return Ok();
        }

        [HttpGet("getTaskByStatus")]
        [Authorize]
        public IActionResult GetTaskById(TaskTodoStaus Status)
        {
            var CurrentUser = _httpContextAccessor.HttpContext?.User;

            var userTasks = _unitOfWork.TasksToDo.getAll().Where(T => T.ownerId == CurrentUser.FindFirst("AppUserId").Value);

            var Response = userTasks.Where(T => T.Status == Status);
                            
            
            return Ok(Response);
        }


    }
}
