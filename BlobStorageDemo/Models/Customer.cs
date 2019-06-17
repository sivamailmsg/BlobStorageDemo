using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlobStorageDemo.Models
{
    public class Customer
    {
        public int Id{ get; set; }

        public string Name{ get; set; }

        public IFormFile Image{ get; set; }
    }
}
