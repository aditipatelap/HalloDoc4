using BusinessLogic.Interfaces;
using DataAccess.CustomModels;
using DataAccess.Data;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BusinessLogic.Repositories
{

    public class AssignmentRepository : IAssignmentInterface
    {
        private readonly ApplicationDbContext _db;
        public AssignmentRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public MainModel GetProjectData(string searchval,int currentpage)
        {
            if(currentpage==0)
            {
                currentpage = 1;
            }
            var res= (from Projects in _db.Projects
                      where (searchval == null || Projects.ProjectName.ToLower().Contains(searchval))
                      select new ProjectManagement
                      {
               ProjectId= Projects.Id,
               ProjectName= Projects.ProjectName,
               Assignee= Projects.Assignee,
               Description= Projects.Description,   
               DueDate= Projects.DueDate,
               Domain= Projects.Domain,
               City= Projects.City
           }).ToList();
            int totalrecords = res.Count();
            int pagesize = 2;
            int totalPages = (int)Math.Ceiling((double)totalrecords / pagesize);
            var paginateddashboard = res.Skip((currentpage - 1) * pagesize).Take(pagesize).ToList();
            MainModel adminDashboard = new MainModel()
            {
                ProjectManagement = paginateddashboard,
                CurrentPage = currentpage,
                TotalPages = totalPages,
            };
          
           
            return adminDashboard;
        }
        public MainModel AddFormDataGEt()
        {
          var result = _db.Domains.ToList();
            MainModel mainModel= new MainModel();
            mainModel.domains = result;
            return mainModel;
        }
        public void AddFormDataPost(ProjectManagement projectManagement)
        {
            Project project = new Project();
            Domain domain = new Domain();
            var res = _db.Domains.Where(x => x.Name == projectManagement.Domain).FirstOrDefault();
            if (res == null)
            {
                domain.Name = projectManagement.Domain;
                _db.Add(domain);
                _db.SaveChanges();
            }
            var result = _db.Domains.Where(x => x.Name == projectManagement.Domain).FirstOrDefault();
            project.ProjectName = projectManagement.ProjectName;
            project.Assignee = projectManagement.Assignee;
            project.Description = projectManagement.Description;
            project.DueDate = projectManagement.DueDate;
            project.City=projectManagement.City;
            project.Domain = projectManagement.Domain;
            project.DomainId = result.Id;
          _db.Add(project);
            _db.SaveChanges();

        }
        public MainModel EditFormDataGet(int id)
        {

            var res = _db.Projects.Where(x=>x.Id==id).Select(x => new ProjectManagement
            {
                ProjectId = x.Id,
                ProjectName = x.ProjectName,
                Assignee = x.Assignee,
                Description = x.Description,
                DueDate = x.DueDate,
                Domain = x.Domain,
                City = x.City
            }).FirstOrDefault();
            var result = _db.Domains.ToList();
            MainModel mainModel = new MainModel();
            mainModel.AddData = res;
            mainModel.domains=result;
            return mainModel;

        }
        public void EditFormDataPost(ProjectManagement projectManagement)
        {
            Domain domain = new Domain();
            var res = _db.Domains.Where(x => x.Name == projectManagement.Domain).FirstOrDefault();
            if (res == null)
            {
                domain.Name = projectManagement.Domain;
                _db.Add(domain);
                _db.SaveChanges();
            }
            var result= _db.Domains.Where(x => x.Name == projectManagement.Domain).FirstOrDefault();
            var req = _db.Projects.Where(x => x.Id == projectManagement.ProjectId).FirstOrDefault();

            req.ProjectName = projectManagement.ProjectName;
            req.Assignee = projectManagement.Assignee;
            req.Description = projectManagement.Description;
            req.DueDate = projectManagement.DueDate;
            req.Domain = projectManagement.Domain;
            req.DomainId = result.Id;
            _db.SaveChanges();

        }
        public void DeleteFormDataPost(int Projectid)
        {
            var data3 = _db.Projects.Where(x => x.Id == Projectid).FirstOrDefault();
            if (data3 != null)
            {
                _db.Remove(data3);
                _db.SaveChanges();
            }

        }

    }
}
