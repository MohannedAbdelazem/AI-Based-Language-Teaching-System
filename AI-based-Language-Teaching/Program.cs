using AI_based_Language_Teaching;
using AI_based_Language_Teaching.Models;
using AI_based_Language_Teaching.Repositories;
using AI_based_Language_Teaching.Service;
// using Microsoft.AspNetCore.Identity; // Uncomment this if you want to use Identity for user management
// using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer; //Added by moha
using Microsoft.IdentityModel.Tokens;   //Added by moha
using System.Text; // Added by moha
var builder = WebApplication.CreateBuilder(args);

// Key for JWT authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
// var key = jwtSettings["Key"];
var key = jwtSettings["Key"]; // Use byte array for security
// Console.WriteLine($"Key: {key}"); // For debugging purposes, remove in production
// var key = "SuperSecretKeyThatNeedsToBeLongForTheAi-Based-Language-Teaching-SystemProject!"; // move this to appsettings.json in production
// Configure JWT authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Set to true in production
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = jwtSettings["Issuer"], // Set to your issuer in production
        ValidAudience = jwtSettings["Audience"], // Set to your audience in production
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        ValidateIssuer = true, // Set to true in production
        ValidateAudience = true, // Set to true in production
        ValidateLifetime = true, // Validate the token's expiration
        ValidateIssuerSigningKey = true, // Set to true in production
    };
});
// Add authorization to builder
builder.Services.AddAuthorization();



// FastAPI URL
builder.Services.AddSingleton<UrlProvider>();



// Configure database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity (still useful for securing APIs)

/*
// Uncomment this section if you want to use Identity for user management
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();
*/

// Register services and repository
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped< ILessonService,LessonService>();
builder.Services.AddScoped<ICurriculumService, CurriculumService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IQuizMaker, QuizMaker>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ILanguageService, LanguageService>();
builder.Services.AddScoped<IProgressService, ProgressService>();
builder.Services.AddScoped<ICefrManagerService,CefrManagerService>();
builder.Services.AddScoped<IExerciseService, ExerciseService>();


// builder.Services.AddScoped<CefrManagerService>();
builder.Services.AddScoped<JwtHelper>();
// Dummy email sender

// API-specific services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// Swagger configuration
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "AI-Based Language Teaching API",
        Version = "v1"
    });

    // Add JWT Authentication to Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme.  
Enter your token in the text input below.  
Example: **Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...**",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2", // Just needed to pass validation
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

// end of swagger config
builder.Services.AddRazorPages(); // This is for Razor Pages
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
// app.UseHttpsRedirection(); // Uncomment this line if you want to enforce HTTPS redirection
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.MapRazorPages(); // This is for Razor Pages
app.MapControllers();
app.Run();