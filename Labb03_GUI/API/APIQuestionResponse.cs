using Labb03_GUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb03_GUI.API
{
    class APIQuestionResponse
    {
        public int Response_Code { get; set; }
        public List<APIQuestion>? Results { get; set; }
    }
}
