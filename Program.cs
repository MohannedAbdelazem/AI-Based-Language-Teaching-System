using AI_based_Language_Teaching;
using AI_based_Language_Teaching.Models;
using AI_based_Language_Teaching.Repositories;
using AI_based_Language_Teaching.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Google.Apis.Auth.OAuth2;
var builder = WebApplication.CreateBuilder(args);
// FastAPI URL
builder.Services.AddSingleton<UrlProvider>();

builder.Services.AddSingleton<ExerciseService>();


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
builder.Services.AddScoped<ILessonService, LessonService>();
builder.Services.AddScoped<ICurriculumService, CurriculumService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Dummy email sender
builder.Services.AddSingleton<IEmailSender, DummyEmailSender>();

// API-specific services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
// fetech the FastAPI URL
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var urlProvider = services.GetRequiredService<UrlProvider>();

   string pathToCredentials = Path.Combine(AppContext.BaseDirectory, "gpthing-f922e-firebase-adminsdk-fbsvc-d4c4f4c4ec.json");

    var firestoreBuilder = new FirestoreClientBuilder
    {
        Credential = GoogleCredential.FromFile(pathToCredentials)
    };

    FirestoreDb db = FirestoreDb.Create("gpthing-f922e", firestoreBuilder.Build());
    Console.WriteLine("✅ Connected to Firestore");

    DocumentReference docRef = db.Collection("api_public_urls").Document("fastapi_url");
    DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

    if (snapshot.Exists)
    {
        var data = snapshot.ToDictionary();
        urlProvider.FastApiUrl = data["url"].ToString();
        Console.WriteLine($"🌐 FastAPI URL loaded: {urlProvider.FastApiUrl}");
    }
    else
    {
        Console.WriteLine("⚠️ Firestore document not found.");
    }
}

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

app.Run();