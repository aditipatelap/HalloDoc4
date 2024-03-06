//using DataAccess.ViewModel;
//using DataAccess.Models;
//using Microsoft.AspNetCore.Http;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc.Filters;
//using DocumentFormat.OpenXml.Spreadsheet;

//namespace BusinessLogic.Utils
//{
//    public class SessionUtils
//    {
//        public static UserInfo GetLoggedInUser(ISession session)
//        {
//            UserLoginData userInfo = null;

//            if (!string.IsNullOrEmpty(session.GetString("userId")))
//            {
//                userInfo = new UserLoginData();
//                userInfo.Id = session.GetString("userId");
//                userInfo.Email = session.GetString("Email");
//                userInfo.Role = session.GetString("Role");
//            }

//            return userInfo;
//        }

//        public static void SetLoggedInUser(ISession session, Aspnetuser user)
//        {
//            if (user != null)
//            {
//                session.setstring("userId", user.Id);
//                session.SetString("Email", user.Email);
//                session.SetString("Role", user.Aspnetuserroles.FirstOrDefault().Roleid);
//            }
//        }
//    }
//}
