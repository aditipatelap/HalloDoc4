using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.CustomModels
{
    public class ProjectManagement
    {
      public int ProjectId { get; set; }
        [Required(ErrorMessage = "the fileld is required")]
        public string? ProjectName { get; set; }
        [Required(ErrorMessage = "the fileld is required")]
        public string? Assignee { get; set; }
        [Required(ErrorMessage = "the fileld is required")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "the fileld is required")]
        public DateTime? DueDate { get; set; }
        [Required(ErrorMessage = "the fileld is required")]
        public string? Domain { get; set; }
        [Required(ErrorMessage = "the fileld is required")]
        public string? City { get; set; }

    }

    public class MainModel
    {
        public List<ProjectManagement> ProjectManagement { get; set; }
        public int TotalPages { get;set; }
        public int CurrentPage { get; set; } = 1;
        public ProjectManagement AddData { get; set; }  
        public IEnumerable<Domain> domains { get; set; }
    }
}
