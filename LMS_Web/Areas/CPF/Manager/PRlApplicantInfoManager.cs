using LMS_Web.Areas.CPF.Interface;
using LMS_Web.Areas.CPF.Models;
using LMS_Web.Data;
using LMS_Web.Manager;
using LMS_Web.Repository;
using System.Collections.Generic;

namespace LMS_Web.Areas.CPF.Manager
{
    public class PRlApplicantInfoManager:BaseManager<PRlApplicantInfo>, IPRlApplicantInfoManager
    {
        public PRlApplicantInfoManager(ApplicationDbContext db) : base(new BaseRepository<PRlApplicantInfo>(db))
        {

        }
        public ICollection<PRlApplicantInfo> GetList()
        {
            return Get(c => true,c => c.AppUser);
        }
        public PRlApplicantInfo GetById(int id)
        {
            return GetFirstOrDefault(x => x.Id == id, c=>c.AppUser);
        }

        public PRlApplicantInfo GetListByUser(string appUserId)
        {
            return GetFirstOrDefault(e =>e.AppUserId == appUserId);
        }
    }
}
