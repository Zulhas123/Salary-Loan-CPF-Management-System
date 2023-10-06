using LMS_Web.Areas.CPF.Models;
using LMS_Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LMS_Web.Areas.Loan.Models;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Areas.Settings.Models;

namespace LMS_Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>(entity => entity.Property(m => m.Id).HasMaxLength(127));
            builder.Entity<IdentityRole>(entity => entity.Property(m => m.ConcurrencyStamp).HasColumnType("varchar(256)"));
            builder.Entity<AppUser>()
                .HasOne<Designation>(s => s.Designation)
                .WithMany(g => g.AppUsers)
                .HasForeignKey(s => s.DesignationId).OnDelete(DeleteBehavior.NoAction);

            builder.Entity<AppUser>()
                .HasOne<Department>(s => s.Department)
                .WithMany(g => g.AppUser)
                .HasForeignKey(s => s.DepartmentId).OnDelete(DeleteBehavior.NoAction);

            builder.Entity<AppUser>()
                .HasOne<Wing>(s => s.Wing)
                .WithMany(g => g.AppUser)
                .HasForeignKey(s => s.WingId).OnDelete(DeleteBehavior.NoAction);

            builder.Entity<AppUser>()
                .HasOne<Section>(s => s.Section)
                .WithMany(g => g.AppUser)
                .HasForeignKey(s => s.SectionId).OnDelete(DeleteBehavior.NoAction);

            builder.Entity<GradeWisePayScale>()
                .HasOne<Grade>(s => s.Grade)
                .WithMany(g => g.GradeWisePayScales)
                .HasForeignKey(s => s.GradeId).OnDelete(DeleteBehavior.NoAction);

            builder.Entity<GradeWisePayScale>()
                .HasOne<PayScale>(s => s.PayScale)
                .WithMany(g => g.GradeWisePayScales)
                .HasForeignKey(s => s.PayScaleId).OnDelete(DeleteBehavior.NoAction);

            builder.Entity<UserDeduction>()
                .HasOne<Deduction>(s => s.Deduction)
                .WithMany(g => g.UserDeductions)
                .HasForeignKey(s => s.DeductionId).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<AppUser>()
                .HasOne<Station>(s => s.Station)
                .WithMany(g => g.AppUsers)
                .HasForeignKey(s => s.StationId).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<AppUser>()
                .HasOne<Grade>(s => s.Grade)
                .WithMany(g => g.AppUsers)
                .HasForeignKey(s => s.GradeId).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<AppUser>()
                .HasOne<ResidentStatus>(s => s.ResidentStatus)
                .WithMany(g => g.AppUsers)
                .HasForeignKey(s => s.ResidentialStatusId).OnDelete(DeleteBehavior.NoAction); 

            ModelBuilder modelBuilder = builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.Property(m => m.LoginProvider).HasMaxLength(127);
                entity.Property(m => m.ProviderKey).HasMaxLength(127);
            });

            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.Property(m => m.UserId).HasMaxLength(127);
                entity.Property(m => m.RoleId).HasMaxLength(127);
            });

            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.Property(m => m.UserId).HasMaxLength(127);
                entity.Property(m => m.LoginProvider).HasMaxLength(127);
                entity.Property(m => m.Name).HasMaxLength(127);
            });




            //builder.Entity<AppUser>().HasData(
            //    new AppUser()
            //    {
            //        Id = "352ef6af-8b2e-4d0d-bb01-88dfa619f9f2",
            //        UserName = "01715637290",
            //        Email = "hmuzzal@mail.com",
            //        EmailConfirmed = true,
            //        PasswordHash =
            //            "AQAAAAEAACcQAAAAEIW60+N8AuqIonyaDD/ODWNY/GCLpkM2khiNDoTwsWZEtyg+iIjuAgGIej2cqvNiKA==",
            //        SecurityStamp = "IEC2QG3OUXJYUJGNKQBKIWXFGXKHVEXF",
            //        ConcurrencyStamp = "c00fcf43-bb53-4671-91cd-2fb94c70f1cb",
            //        PhoneNumber = "01715637290",
            //        CreatedDateTime = DateTime.Now,
            //        BirthDate = DateTime.Now.AddYears(-30),
            //        DepartmentId = 1,
            //        DesignationId = 1,
            //        JoiningDate = DateTime.Now.AddYears(-10),
            //        Gender = "Male",
            //        FirstName = "Hasan",
            //        LastName = "Mahmud",
            //        FullName = "Hasan Mahmud",
            //        IsActive = true,
            //        Image = "1.jpg"
            //    },
            //    new AppUser()
            //    {
            //        Id = "352ef6af-8b2e-4l0d-bb01-88dfa619f6o2",
            //        UserName = "01715637291",
            //        Email = "hmuzzal@mail.com",
            //        EmailConfirmed = true,
            //        PasswordHash =
            //            "AQAAAAEAACcQAAAAEIW60+N8AuqIonyaDD/ODWNY/GCLpkM2khiNDoTwsWZEtyg+iIjuAgGIej2cqvNiKA==",
            //        SecurityStamp = "IEC2QG3OUXJYUJGNKQBKIWXFGXKHVEXF",
            //        ConcurrencyStamp = "c00fcf43-bb53-4671-91cd-2fb94c70f1cb",
            //        PhoneNumber = "01715637291",
            //        CreatedDateTime = DateTime.Now,
            //        BirthDate = DateTime.Now.AddYears(-30),
            //        DepartmentId = 1,
            //        DesignationId = 1,
            //        JoiningDate = DateTime.Now.AddYears(-10),
            //        Gender = "Male",
            //        FirstName = "Safkat",
            //        LastName = "Mahmud",
            //        FullName = "Safkat Mahmud",
            //        IsActive = true,
            //        Image = "2.jpg"
            //    },
            //    new AppUser()
            //    {
            //        Id = "352ef6af-8t3e-4l0d-bb01-88dfa619qwo2",
            //        UserName = "01715637292",
            //        Email = "hmuzzal@mail.com",
            //        EmailConfirmed = true,
            //        PasswordHash =
            //            "AQAAAAEAACcQAAAAEIW60+N8AuqIonyaDD/ODWNY/GCLpkM2khiNDoTwsWZEtyg+iIjuAgGIej2cqvNiKA==",
            //        SecurityStamp = "IEC2QG3OUXJYUJGNKQBKIWXFGXKHVEXF",
            //        ConcurrencyStamp = "c00fcf43-bb53-4671-91cd-2fb94c70f1cb",
            //        PhoneNumber = "01715637292",
            //        CreatedDateTime = DateTime.Now,
            //        BirthDate = DateTime.Now.AddYears(-30),
            //        DepartmentId = 1,
            //        DesignationId = 1,
            //        JoiningDate = DateTime.Now.AddYears(-10),
            //        Gender = "Male",
            //        FirstName = "Asaduzzaman",
            //        LastName = "Khan",
            //        FullName = "Asaduzzaman Khan",
            //        IsActive = true,
            //        Image = "3.jpg"
            //    }
            //);

            //builder.Entity<IdentityRole>().HasData(
            //    new IdentityRole() { Id = "1", ConcurrencyStamp = "1", Name = "Super Admin", NormalizedName = "Super Admin" },
            //    new IdentityRole() { Id = "2", ConcurrencyStamp = "2", Name = "Admin", NormalizedName = "Admin" },
            //    new IdentityRole() { Id = "3", ConcurrencyStamp = "3", Name = "User", NormalizedName = "User" }
            //);

            //builder.Entity<IdentityUserRole<string>>().HasData(
            //              new IdentityUserRole<string>() { UserId = "352ef6af-8b2e-4d0d-bb01-88dfa619f9f2", RoleId = "1" },
            //              new IdentityUserRole<string>() { UserId = "352ef6af-8b2e-4l0d-bb01-88dfa619f6o2", RoleId = "2" },
            //              new IdentityUserRole<string>() { UserId = "352ef6af-8b2e-4l0d-bb01-88dfa619qwo2", RoleId = "3" }
            //          );


        //    builder.Entity<Holiday>().HasData(
        //        new Holiday() { Id = 1, FromDate = new DateTime(2021, 2, 21), ToDate = new DateTime(2021, 2, 21), Name = "একুশে ফেব্রুয়ারি", Remarks = "" },
        //        new Holiday() { Id = 2, FromDate = new DateTime(2021, 3, 26), ToDate = new DateTime(2021, 3, 26), Name = "২৬শে মার্চ", Remarks = "" }
        //    );

        //    builder.Entity<EarnLeaveType>().HasData(
        //        new EarnLeaveType() { Id = 1, Type = "গড়" },
        //        new EarnLeaveType() { Id = 2, Type = "অর্ধ গড়" }
        //    );

        //    builder.Entity<Department>().HasData(
        //        new Department() { Id = 1, Name = "IT" },
        //        new Department() { Id = 2, Name = "Account & Finance" }
        //         );

        //    builder.Entity<Designation>().HasData(
        //        new Designation() { Id = 1, Name = "System Analyst" },
        //        new Designation() { Id = 2, Name = "Programmer" },
        //        new Designation() { Id = 3, Name = "Assistant Director" }
        //         );
        //    builder.Entity<LeaveType>().HasData(
        //        new LeaveType() { Id = 1, Name = "নৈমিত্তিক" },
        //        new LeaveType() { Id = 2, Name = "অসুস্থতাজনিত" },
        //        new LeaveType() { Id = 3, Name = "অর্জিত" },
        //        new LeaveType() { Id = 4, Name = "অসাধারণ" },
        //        new LeaveType() { Id = 5, Name = "অধ্যয়ন" },
        //        new LeaveType() { Id = 6, Name = "সংগনিরোধ" },
        //        new LeaveType() { Id = 7, Name = "প্রসূতি" },
        //        new LeaveType() { Id = 8, Name = "প্রাপ্যতাবিহীন" },
        //        new LeaveType() { Id = 9, Name = "অবসর উত্তর" },
        //        new LeaveType() { Id = 10, Name = "ঐচ্ছিক" },
        //        new LeaveType() { Id = 11, Name = "শ্রান্তি ও বিনোদন" },
        //        new LeaveType() { Id = 12, Name = "অক্ষমতাজনিত বিশেষ" },
        //        new LeaveType() { Id = 13, Name = "বাধ্যতামূলক" },
        //        new LeaveType() { Id = 14, Name = "বিনা বেতনে" }
        //    );
        }



        public DbSet<InterestSlab> InterestSlabs { get; set; }
        public DbSet<LeaveType> LeaveType { get; set; }
        public DbSet<LeaveApplication> LeaveApplications { get; set; }
        public DbSet<Holiday> Holiday { get; set; }
        public DbSet<UserLeaveQuota> UserLeaveQuotas { get; set; }
        public DbSet<ApprovedHistory> ApprovedHistory { get; set; }
        public DbSet<MaternityLeave> MaternityLeave { get; set; }
        public DbSet<NotDueLeave> NotDueLeave { get; set; }
        public DbSet<RestAndRecreationLeave> RestAndRecreationLeave { get; set; }
        public DbSet<SpecialDisabilityLeave> SpecialDisabilityLeave { get; set; }
        public DbSet<StudyLeave> StudyLeave { get; set; }
        public DbSet<EarnLeave> EarnLeave { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Designation> Designation { get; set; }
        public DbSet<EarnLeaveType> EarnLeaveTypes { get; set; }
        public DbSet<Wing> Wing { get; set; }
        public DbSet<Section> Section { get; set; }
        public DbSet<LeaveReason> LeaveReason { get; set; }
        public DbSet<UserSignInHistory> UserSignInHistory { get; set; }
        public DbSet<LeaveHistory> LeaveHistories { get; set; }
        public DbSet<BacklogEntry> BacklogEntries { get; set; }
        public DbSet<ApprovalDesignation> ApprovalDesignation { get; set; }
        public DbSet<LoanHead> LoanHead { get; set; }
        public DbSet<UserWiseLoan> UserWiseLoans { get; set; }
        public DbSet<Deduction> Deductions { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<GradeStepBasic> GradeSteps { get; set; }
        public DbSet<GradeWisePayScale> GradWisePayScales { get; set; }
        public DbSet<PayScale> PayScales { get; set; }
        public DbSet<ResidentStatus> ResidentStatus { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<UserDeduction> UserDeductions { get; set; }
        public DbSet<MainMenu> MainMenus { get; set; }
        public DbSet<SubMenu> SubMenus { get; set; }
        public DbSet<RoleSubMenu> RoleSubMenus { get; set; }
        public DbSet<DueDuringSuspension> DueDuringSuspensions { get; set; }
        public DbSet<FixedDeduction> FixedDeductions { get; set; }
        public DbSet<LienUser> LienUsers { get; set; }
        public DbSet<SuspensionHistory> SuspensionHistories { get; set; }
        public DbSet<UserHouseRent> UserHouseRents { get; set; }
        public DbSet<ChildrenInfo> ChildrenInfos { get; set; }
        public DbSet<UserStationPermission> UserStationPermissions { get; set; }
        public DbSet<UserTax> UserTaxes { get; set; }
        public DbSet<HouseRentRules> HouseRentRules { get; set; }
        public DbSet<GradeWiseFixedDeduction> GradeWiseFixedDeductions { get; set; }
        public DbSet<UserSpecificAllowance> UserSpecificAllowances { get; set; }
        public DbSet<TransferHistory> TransferHistories { get; set; }
        public DbSet<CpfPercent> CpfPercents { get; set; }
        public DbSet<LoanInstallmentInfo> LoanInstallmentInfo { get; set; }
        public DbSet<TaxInstallmentInfo> TaxInstallmentInfo { get; set; }
        public DbSet<FiscalYear> FiscalYears { get; set; }
        public DbSet<CpfInfo> CpfInfo { get; set; }
        public DbSet<InvestmentInfo> InvestmentInfo { get; set; }
        public DbSet<UserFundInfo> UserFundInfo { get; set; }
        public DbSet<PRlApplicantInfo> PRlApplicantInfo { get; set; }
        public DbSet<FiscalYearWiseInvestmentInfo> FiscalYearWiseInvestmentInfo { get; set; }
        public DbSet<Bonus> Bonus { get; set; }
        public DbSet<LoanPaymentHistory> LoanPaymentHistories { get; set; }
        public DbSet<ProcessSalary> ProcessSalary { get; set; }
        public DbSet<SalaryIncrement> SalaryIncrement { get; set; }

        public DbSet<SalaryHistory> SalaryHistory { get; set; }
    }
}
