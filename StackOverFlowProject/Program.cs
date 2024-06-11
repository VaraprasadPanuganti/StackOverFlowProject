using DomainModel;
using Microsoft.EntityFrameworkCore;
using Repositories;
using ServiceLayer;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = false;
    options.Cookie.IsEssential = false;
});
// Add HttpContextAccessor
builder.Services.AddHttpContextAccessor();


//Dependency injection
builder.Services.AddScoped<IQuestionsService, QuestionsService>();
builder.Services.AddScoped<IQuestionRepository, QuestionsRepository>();

builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();

builder.Services.AddScoped<ICategoriesService, CategoriesService>();
builder.Services.AddScoped<ICategoriesRepository, CategoriesRepository>();

builder.Services.AddScoped<IAnswersService, AnswersService>();
builder.Services.AddScoped<IAnswersRepository, AnswersRepository>();

builder.Services.AddDbContext<StackOverflowDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.UseSession();

app.Run();
