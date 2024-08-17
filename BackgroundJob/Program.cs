using BackgroundJob;
using BackgroundJob.Data;
using BackgroundJob.Services;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddHostedService<UpdateAndSendEmailToUserBackgroundJob>();

var host = builder.Build();
host.Run();
