﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackEnd.Entities;

namespace BackEnd.ResAndReq.Res.Routine
{
    /// <summary>
    /// ✅ CORRECTED: Fixed Error list type
    /// </summary>
    public class ResGetActiveRoutines : ResBase
    {
        public List<ActiveRoutine> ActiveRoutines { get; set; }

        public ResGetActiveRoutines()
        {
            ActiveRoutines = new List<ActiveRoutine>();
        }
    }
}