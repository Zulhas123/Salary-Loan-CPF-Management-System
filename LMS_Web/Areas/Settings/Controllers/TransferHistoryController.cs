using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS_Web.Areas.Settings.Manager;
using LMS_Web.Areas.Settings.Models;
using LMS_Web.Data;
using LMS_Web.Models;
using Microsoft.AspNetCore.Identity;

namespace LMS_Web.Areas.Settings.Controllers
{
    public class TransferHistoryController : Controller
    {
        private UserManager<AppUser> userManager;
        private readonly TransferHistoryManager transferHistoryManager;

        public TransferHistoryController(ApplicationDbContext db, UserManager<AppUser> UserManager)
        {
            userManager = UserManager;
            transferHistoryManager = new TransferHistoryManager(db);
        }
        
    }
}
