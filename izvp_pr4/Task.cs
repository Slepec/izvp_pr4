using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace izvp_pr4
{
    public class Task
    {
        private string description;
        private DateTime date;
        private bool status;

        public Task(string description, DateTime date, bool status)
        {
            this.Description = description;
            this.Date = date;
            this.Status = status;
        }

        public string Description { get => description; set => description = value; }
        public DateTime Date { get => date; set => date = value; }
        public bool Status { get => status; set => status = value; }
    }
}
