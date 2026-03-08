using Infrastructure.DbModels;
using MainProject.NotificationHub;
using Microsoft.EntityFrameworkCore;
using Repository.Employees;
using Repository.LeaveRequests;
using Repository.Report;
using Repository.Users;
using Services.Employees;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddSignalR();

builder.Services.AddDbContext<EmpLeaveManagementSystemContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("sqlconnection")));

builder.Services.AddScoped<IUser,Services.Users.DALClass>();
builder.Services.AddScoped<IEmployee, Services.Employees.DALClass>();
builder.Services.AddScoped<ILeaveRequest, Services.LeaveRequests.DALClass>();
builder.Services.AddScoped<IReport, Services.Reports.DALClass>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();
app.MapHub<NotificationHub>("/notificationHub");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");

app.Run();
