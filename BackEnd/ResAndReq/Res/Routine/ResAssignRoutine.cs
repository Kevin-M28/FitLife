using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackEnd.Entities;

namespace BackEnd.ResAndReq.Res.Routine
{
    public class ResAssignRoutine : ResBase
    {
        public UserAssignedRoutine AssignedRoutine { get; set; }
        public string Message { get; set; }

        public ResAssignRoutine()
        {
            AssignedRoutine = new UserAssignedRoutine();
        }
    }

}
