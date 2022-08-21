using Microsoft.EntityFrameworkCore;
using Services.CheckRole;
using Services.AttendanceCheck;
using ScheduledTasks;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
MySqlServerVersion version = new(new Version(8, 0, 30));
builder.Services.AddDbContext<UserDbContext>(options =>
{
    options.UseMySql(connectionString, version);
});
builder.Services.AddDbContext<AttendanceDbContext>(options =>
{
    options.UseMySql(connectionString, version);
});
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services
    .AddIdentity<UserModel, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<UserDbContext>();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

    options.LoginPath = "/Login";
    options.AccessDeniedPath = "/AccessDenied";
});
builder.Services.AddTransient<IAttendanceCheck, AttendanceCheck>();
builder.Services.AddTransient<ICheckRole, CheckRole>();
builder.Services.AddRazorPages();

builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();
    var jobKey = new JobKey("CheckForUncheckedOut");
    q.AddJob<CheckForUncheckedOut>(opts => opts.WithIdentity(jobKey));
    q.AddTrigger(opts =>
    {
        opts.ForJob(jobKey);
        opts.WithIdentity("CheckForUncheckedOut-trigger");
        opts.WithCronSchedule("0 0 16 ? * MON-FRI *");
    });
});
builder.Services.AddQuartzServer(options => options.WaitForJobsToComplete = true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();
