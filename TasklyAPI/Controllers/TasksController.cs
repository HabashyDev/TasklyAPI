using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Taskly.Core;
using Taskly.Core.DTOs;
using Taskly.Core.Enums;
using Taskly.Core.Models;


namespace TasklyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TasksController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _usermanger;

        public TasksController(IUnitOfWork unitOfWork,UserManager<AppUser> userManager)
        {
            
            _unitOfWork = unitOfWork;
            _usermanger = userManager;

        }


        [HttpPost("CreateTask")]
        public async Task<IActionResult> CreateTask(TaskDTO Request)
        {
            // Get the current user's ID from claims
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Check if user is authenticated
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authenticated");
            }

            TaskTodo task = new()
            {
                Title = Request.TaskTitle,
                Description = Request.TaskDescription,
                DeadLine = Request.TaskDeadLine,
                Status = Request.TaskStatus,
                AppUserId = userId
            };

            _unitOfWork.TasksToDo.Create(task);
            _unitOfWork.complete(); 

            // Return Created with the created resource
            return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, task);
        }


        [HttpGet("getAllTasks")]
        public async Task<IActionResult> GetAllTasks()
        {
            // Get the current user's ID from claims
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Check if user is authenticated
            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized("User not authenticated");
            }

            // Filter tasks for the current user
            var userTasks = _unitOfWork.TasksToDo.getAll()
                .Where(t => t.AppUserId == currentUserId)
                .ToList(); // Execute the query


            return Ok(userTasks);
        }



        [HttpPut("UpdateTask")]
        public IActionResult UpdateTask(TaskTodo request)
        {
            var UpdatedTask = _unitOfWork.TasksToDo.Update(request);
            _unitOfWork.complete();
            return Ok(UpdatedTask);
        }

        [HttpDelete("DeleteTaskById {id:int}")]
        public IActionResult DeleteTask(int id)
        {
            _unitOfWork.TasksToDo.DeleteById(T => T.Id == id);
            _unitOfWork.complete();
            return Ok();
        }

        [HttpGet("getTaskByStatus")]
        public IActionResult GetTaskById(TaskTodoStaus Status)
        {

            var userTasks = _unitOfWork.TasksToDo.getAll().Where(T => T.AppUserId == User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var Response = userTasks.Where(T => T.Status == Status);
            return Ok(Response);
        }
    }
}
