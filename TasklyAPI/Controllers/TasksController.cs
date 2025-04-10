using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Taskly.Core;
using Taskly.Core.DTOs;
using Taskly.Core.Models;
using Taskly.Core.Repositories;
using Taskly.EF.Repositories;

namespace TasklyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        public TasksController(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }
        [HttpPost]
        public IActionResult CreateTask(TaskDTO TaskToCreate)
        {
            TaskTodo Task = new()
            {
                Title = TaskToCreate.TaskTitle,
                Description = TaskToCreate.TaskDescription,
                DeadLine = TaskToCreate.TaskDeadLine,
            };
            unitOfWork.TasksToDo.Create(Task);
            unitOfWork.complete();
            return Created();
        }

        [HttpGet("getAllTasks")]
        [Authorize]
        public IActionResult GetAllTasks()
        {
            return Ok(unitOfWork.TasksToDo.getAll());
        }

        [HttpPut]
        public IActionResult UpdateTask(TaskTodo request)
        {
            var UpdatedTask = unitOfWork.TasksToDo.Update(request);
            unitOfWork.complete();
            return Ok(UpdatedTask);
        }

        [HttpDelete("DeleteTaskById {id:int}")]
        public IActionResult DeleteTask(int id)
        {
            unitOfWork.TasksToDo.DeleteById(T => T.Id == id);
            unitOfWork.complete();
            return Ok();
        }

        [HttpGet("getTaskById{id:int}")]
        public IActionResult GetTaskById(int id)
        {
            return Ok(unitOfWork.TasksToDo.GetById(T => T.Id == id));
        }
       
        
    }
}
