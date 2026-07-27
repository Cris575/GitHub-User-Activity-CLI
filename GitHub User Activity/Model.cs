using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace GitHub_User_Activity
{
    public class Model
    {
        public string? name { get; set; }
        public string? type { get; set; }
        public string? action { get; set; }

        public int total { get; set; }
    }
}
