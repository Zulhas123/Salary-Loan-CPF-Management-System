using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LMS_Web.Areas.Salary.Models;

namespace LMS_Web.Models
{
    public class AppUser : IdentityUser
    {
        [PersonalData]
        //[Display(Name = "ফাস্ট নাম")]

        //public string FirstName { get; set; }
        //public string FirstNameBangla { get; set; }

        //[Display(Name = "লাস্ট নাম")]
        //   public string LastName { get; set; }
        // public string LastNameBangla { get; set; }
        [Display(Name = "Name")]
        public string FullName { get; set; }
        [Display(Name = "নাম বাংলা")]
        public string FullNameBangla { get; set; }
        public string Image { get; set; }

        //[Display(Name = "কর্মী কোড")]
        public string EmployeeCode { get; set; }
        public string EmployeeCodeBangla { get; set; }
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        [Display(Name = "Designation")]
        public int DesignationId { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime JoiningDate { get; set; }
        public string Gender { get; set; }
        public string NID { get; set; }
        public string Religion { get; set; }
        public string Type { get; set; }


        [Display(Name = "সংরক্ষণ")]
        public string CreatedBy { get; set; }

        [Display(Name = "সংরক্ষণের সময়")]
        public DateTime CreatedDateTime { get; set; }

        [Display(Name = "হালনাগাদ")]
        public string UpdatedBy { get; set; }

        [Display(Name = "হালনাগাদের সময়")]
        public DateTime? UpdatedDateTime { get; set; }
        public bool IsActive { get; set; }
        [Display(Name = "Wing")]
        [ForeignKey("Wing")]
        public int? WingId { get; set; }
        [Display(Name = "Section")]
        public int? SectionId { get; set; }
        public int PlrAge { get; set; }


        [NotMapped]
        [Display(Name = "ছবি")]
        public IFormFile ImagePath { get; set; }

        //New Added
        public int? StationId { get; set; }
        public int? GradeId { get; set; }
        public int? CurrentGradeId { get; set; }
        public decimal? CurrentBasic { get; set; }
        public int? ResidentialStatusId { get; set; }
        
        public bool IsSuspended { get; set; }
        public bool IsLien { get; set; }
        public DateTime?ResignationDate { get; set; }
        public DateTime?DiedDate { get; set; }
        public DateTime?CurrentStationJoiningDate { get; set; }
        public bool IsDied { get; set; }

        [Display(Name = "Bank Account No")]
        public string BankAccountNo { get; set; }
        public string BankAccountNoBangla { get; set; }

        public bool IsAllowedForChargeAllowance { get; set; }
        public bool IsLiveNear3Km { get; set; }
        [Display(Name = "CPF Start Date")]
        public DateTime? CPFStartDate { get; set; }
        //New Added End

        public virtual Station Station { get; set; }
        public virtual Grade Grade { get; set; }
        public virtual ResidentStatus ResidentStatus { get; set; }
        public virtual Designation Designation { get; set; }
        public virtual Wing Wing { get; set; }
        public virtual Section Section { get; set; }
        public virtual Department Department { get; set; }
        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }
        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
        public virtual ICollection<IdentityUserToken<string>> Tokens { get; set; }
        public virtual ICollection<IdentityUserRole<string>> UserRoles { get; set; }

    }
}
