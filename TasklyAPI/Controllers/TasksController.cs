using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IBaseRepository<TaskTodo> TasksRepository;
        public TasksController(IBaseRepository<TaskTodo> _TasksRepository)
        {
            TasksRepository = _TasksRepository;
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
            TasksRepository.Create(Task);
            return Created();
        }

        [HttpGet("getAllTasks")]
        public IActionResult GetAllTasks()
        {
            return Ok(TasksRepository.getAll());
        }

        [HttpDelete("DeleteTaskById {id:int}")]
        public IActionResult DeleteTask(int id)
        {
            return Ok(TasksRepository.DeleteById(T=>T.Id == id));
        }

        [HttpGet("getTaskById{id:int}")]
        public IActionResult GetTaskById(int id)
        {
            return Ok(TasksRepository.GetById(T => T.Id == id));
        }
       
        
    }
}
