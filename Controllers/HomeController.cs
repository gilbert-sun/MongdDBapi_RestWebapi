using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApplication3.Models;
using WebApplication3.Services;

namespace WebApplication3.Controllers
{    
    [Route("")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly RobotPickModelService _robotPickModelService;
        private readonly RobotLogModelServices _robotLogModelServices;
        private readonly Call_dbServiceController _callDbServiceController;

        public HomeController(RobotPickModelService robotPickModelService)
        {
            _robotPickModelService = robotPickModelService;
            _robotLogModelServices = new RobotLogModelServices();
            _callDbServiceController = new Call_dbServiceController();
        }
        
       
        [HttpGet]
        [HttpGet("robot/arm")]
        public ActionResult<List<MongoPickDBmodel>> Get()
        {
            Console.WriteLine("\n--------: HomeController.cs ::--GET--All !\n");
            return _robotPickModelService.Get();
        }

        [HttpGet("robot/log")]
        public ActionResult<List<MongoLogDBmodel>> GetLog()
        {
            Console.WriteLine("\n--------: LogController.cs ::--GET--All !\n");
            return _robotLogModelServices.Get();
        }
        
        [HttpGet("robot/controller")]
        public ActionResult<List<MongoPickDBmodel>> GetController()
        {
            Console.WriteLine("\n--------: Call_dbServiceController.cs ::--GET--All !\n");
            _callDbServiceController.main();
            return this.Get();
        }
        
        [HttpGet("robot/controller1")]
        public ActionResult<List<MongoPickDBmodel>> GetController1()
        {
            Console.WriteLine("\n--------: Call_dbServiceController.cs ::--GET--All MM !\n");
            return _callDbServiceController.ServiceGetMM1(1624182347535);
            //return this.Get();
        }
        
        [HttpGet("robot/controller2")]
        public ActionResult<List<MongoPickDBmodel>> GetController2()
        {
            Console.WriteLine("\n--------: Call_dbServiceController.cs ::--GET2--All HH !\n");
            return _callDbServiceController.ServiceGetHH1(1624201271769);
        }
        
        [HttpGet("robot/controller2/{id:length(13)}")]
        public ActionResult<List<MongoPickDBmodel>> GetController2(long id)
        {
            Console.WriteLine("\n--------: Call_dbServiceController.cs ::--GET2--All HH2 !\n");
            return _callDbServiceController.ServiceGetHH1(id);
        }
                
        [HttpGet("robot/controller3")]
        public ActionResult<List<MongoPickDBmodel>> GetController3()
        {
            Console.WriteLine("\n--------: Call_dbServiceController.cs ::--GET2--All DD !\n");
            return _callDbServiceController.ServiceGetDD1(1624204439082);
        }
        
        [HttpGet("{id:length(13)}")]
        [HttpGet("robot/arm/{id:length(13)}")]
        public ActionResult<MongoPickDBmodel> Get(long id)
        {
            var deltaRobot = _robotPickModelService.Get(id);
            
            Console.WriteLine("\n--------: HomeController.cs ::--GET--ID=[{0}] !\n ",id);
            
            if (deltaRobot == null)
            {
                return NotFound();
            }

            return deltaRobot;
        }
        
        [HttpGet("robot/log/{id:length(13)}")]
        public ActionResult<MongoLogDBmodel> GetLog(long id)
        {
            var deltaRobot = _robotLogModelServices.Get(id);
            
            Console.WriteLine("\n--------: LogController.cs ::--GET--ID=[{0}] !\n ",id);
            
            if (deltaRobot == null)
            {
                return NotFound();
            }

            return deltaRobot;
        }

        [HttpPost]
        [HttpPost("robot/arm")]
        public ActionResult<MongoPickDBmodel> Create(MongoPickDBmodel robotModel1)
        {
            var deltaRobotModel1 = _robotPickModelService.Create(robotModel1);
            return deltaRobotModel1;
        }

        [HttpPost("robot/log")]
        public ActionResult<MongoLogDBmodel> Create(MongoLogDBmodel robotModel1)
        {
            var deltaRobotModel1 = _robotLogModelServices.Create(robotModel1);
            return deltaRobotModel1;
        }
        
        
        [HttpPut]
        [HttpPut("robot/arm")]
        public ActionResult<MongoPickDBmodel> Update(string id,MongoPickDBmodel robotModel1)
        {
            _robotPickModelService.Update(id,robotModel1) ;
            return robotModel1;
        }
        
        [HttpPut("robot/log")]
        public ActionResult<MongoLogDBmodel> Update(string id,MongoLogDBmodel robotModel1)
        {
            _robotLogModelServices.Update(id,robotModel1) ;
            return robotModel1;
        }
        
        [HttpDelete]
        [HttpDelete("robot/arm/{id:length(13)}")]
        public ActionResult Delete(long id)
        {
            var deltaRobotModel1 = _robotPickModelService.Get(id);

            if (deltaRobotModel1 == null)
            {
                return NotFound();
            }

            _robotPickModelService.Remove(deltaRobotModel1.Datetimetag);

            return NoContent();
        }

        // public HomeController(ILogger<HomeController> logger)
        // {
        //     _logger = logger;
        // }
        //
        // public IActionResult Index()
        // {
        //     return View();
        // }
        //
        // public IActionResult Privacy()
        // {
        //     return View();
        // }
        //
        // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        // public IActionResult Error()
        // {
        //     return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        // }
    }
}
