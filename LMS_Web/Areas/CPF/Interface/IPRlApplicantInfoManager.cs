using LMS_Web.Areas.CPF.Models;
using System.Collections.Generic;

namespace LMS_Web.Areas.CPF.Interface
{
    interface IPRlApplicantInfoManager
    {
        PRlApplicantInfo GetListByUser(string appUserId);
        ICollection<PRlApplicantInfo> GetList();
        PRlApplicantInfo GetById(int id);
    }
}
