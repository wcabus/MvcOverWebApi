using System.Collections.Generic;
using Newtonsoft.Json;

namespace YetAnotherTodo.Mvc.Models
{
    public class JsonBadRequest
    {
        public string Error { get; set; }
        
        [JsonProperty("error_description")]
        public string ErrorDescription { get; set; }
        
        public string Message { get; set; }
 
        public Dictionary<string, ICollection<string>> ModelState { get; set; }
    }
}