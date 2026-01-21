dotnet ef dbcontext scaffold "Name=ConnectionStrings:Default" Microsoft.EntityFrameworkCore.SqlServer --output-dir Models --context-dir Data --context SchoolDbContext --table Users --table Colleges --table AcademicYears --table Classes --table Sections --table Students --table Teachers --table Attendance --table Fees --table Exams --table Results --table UserClaims --table RevokedTokens --force
  cd D:\MultiTenantSchoolApp\SmartEduHub\SmartEduHub

