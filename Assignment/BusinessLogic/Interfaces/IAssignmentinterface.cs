using DataAccess.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IAssignmentInterface
    {
        public MainModel GetProjectData(string searchval,int currentpage);
        public MainModel AddFormDataGEt();
        public void AddFormDataPost(ProjectManagement projectManagement);
        public MainModel EditFormDataGet(int id);
        public void EditFormDataPost(ProjectManagement projectManagement);
        public void DeleteFormDataPost(int Projectid);
    }
}
