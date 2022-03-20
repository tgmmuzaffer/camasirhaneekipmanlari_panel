using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace panel.Models
{
    public class Contact : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Adress { get; set; }
        public string MapAdress { get; set; }
        public string Email { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string WorkDay1 { get; set; }
        public string WorkHour1 { get; set; }
        public string WorkDay2 { get; set; }
        public string WorkHour2 { get; set; }
        public string WorkHour3 { get; set; }
        public string WorkDay3 { get; set; }
        public string ContactTitle { get; set; }
        public string ContactContent { get; set; }
        public string InstagramAccount { get; set; }
        public string LinkedInAccount { get; set; }
        public string TwitterAccount { get; set; }
    }
}
