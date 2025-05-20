using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BackEnd.Logic.Routine;
using BackEnd.ResAndReq.Req.Routine;
using BackEnd.ResAndReq.Res.Routine;

namespace Fitlife.Controllers
{
    [RoutePrefix("api/routine")]
    public class RoutineController : ApiController
    {
        public RoutineController() { }

        [HttpPost]
        [Route("create")]
        public ResCreateRoutine createRoutine(ReqCreateRoutine req)
        {
            return new LogRoutine().CreateRoutine(req);

        }
        [HttpPost]
        [Route("active/user/view")]
        public ResGetActiveRoutines getRoutines(ReqGetActiveRoutines req)
        {
            return new LogRoutine().GetActiveRoutines(req);

        }

        [HttpPost]
        [Route("exercise/view")]
        public ResGetRoutineExercises GetRoutineExercise(ReqGetRoutineExercises req)
        {
            return new LogRoutine().GetRoutineExercises(req);
        }

        [HttpPost]
        [Route("exercise/progress/register")]
        public ResRegisterExerciseProgress RegisterExerciseProgress(ReqRegisterExerciseProgress req)
        {
            return new LogRoutine().RegisterExerciseProgress(req);
        }

        [HttpPost]
        [Route("rate")]
        public ResRateRoutine RateRoutine(ReqRateRoutine req)
        {
            return new LogRoutine().RateRoutine(req);
        }

        [HttpPost]
        [Route("assign/user")]
        public ResAssignRoutine AssignRoutineToUser(ReqAssignRoutine req)
        {
            return new LogRoutine().AssignRoutineToUser(req);
        }

        [HttpPost]
        [Route("exercise/add")]
        public ResAddExerciseToRoutine AddExerciseToRoutine(ReqAddExerciseToRoutine req)
        {
            return new LogRoutine().AddExerciseToRoutine(req);
        }

        [HttpPost]
        [Route("report")]
        public ResGetRoutineReport GetRoutineReport(ReqGetRoutineReport req)
        {

            return new LogRoutine().GetRoutineReport(req);

        }
    }

}