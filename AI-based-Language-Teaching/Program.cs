using AI_based_Language_Teaching;
using AI_based_Language_Teaching.Models;
using AI_based_Language_Teaching.Repositories;
using AI_based_Language_Teaching.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configure database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity (still useful for securing APIs)
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Register services and repository
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IProgressService, ProgressService>();
builder.Services.AddScoped<IAnswerService, AnswerService>();
builder.Services.AddScoped<ILessonService, LessonService>();
builder.Services.AddScoped<ICurriculumService, CurriculumService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Dummy email sender
builder.Services.AddSingleton<IEmailSender, DummyEmailSender>();

// API-specific services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRazorPages();

// Angular (for youssef this next part is to call the API, we need to enable CORS)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy
            .AllowAnyOrigin() // for dev only; restrict this in production
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

// Error handling
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseCors("AllowFrontend");
app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.MapRazorPages();


app.Run();
