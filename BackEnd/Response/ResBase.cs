using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackEnd.Entidades;

namespace BackEnd.Response
{
    public class ResBase
    {
        public bool resultado { get; set; }
        public List<Error> error { get; set; }
    }
}
