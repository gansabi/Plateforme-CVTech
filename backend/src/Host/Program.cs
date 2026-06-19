using GestionIdentite.Loader;
using CatalogueEmploi.Loader;
using AppelOffreFreelance.Loader;
using ActualiteEtAbonnement.Loader;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// --- Swagger ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "Plateforme-CVTech API", Version = "v1" });
});

// --- Controllers ---
builder.Services.AddControllers()
    .AddApplicationPart(typeof(GestionIdentite.Client.Controllers.AuthController).Assembly)
    .AddApplicationPart(typeof(CatalogueEmploi.Client.Controllers.AnnoncesController).Assembly)
    .AddApplicationPart(typeof(AppelOffreFreelance.Client.Controllers.AppelsOffreController).Assembly)
    .AddApplicationPart(typeof(ActualiteEtAbonnement.Client.Controllers.RssController).Assembly);

// --- CORS ---
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

// --- Modules (repositories, services, MediatR, validators) ---
builder.Services.AjouterModuleGestionIdentite(builder.Configuration);
builder.Services.AjouterModuleCatalogueEmploi(builder.Configuration);
builder.Services.AjouterModuleAppelOffreFreelance(builder.Configuration);
builder.Services.AjouterModuleActualiteEtAbonnement(builder.Configuration);

// --- DbContexts — PostgreSQL ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<GestionIdentite.Infrastructure.Persistance.GestionIdentiteDbContext>(
    options => options.UseNpgsql(connectionString));
builder.Services.AddDbContext<CatalogueEmploi.Infrastructure.Persistance.CatalogueEmploiDbContext>(
    options => options.UseNpgsql(connectionString));
builder.Services.AddDbContext<AppelOffreFreelance.Infrastructure.Persistance.AppelOffreFreelanceDbContext>(
    options => options.UseNpgsql(connectionString));
builder.Services.AddDbContext<ActualiteEtAbonnement.Infrastructure.Persistance.ActualiteEtAbonnementDbContext>(
    options => options.UseNpgsql(connectionString));

var app = builder.Build();

// --- Création automatique des tables + Seed ---
using (var scope = app.Services.CreateScope())
{
    var identiteDb = scope.ServiceProvider.GetRequiredService<GestionIdentite.Infrastructure.Persistance.GestionIdentiteDbContext>();
    var catalogueDb = scope.ServiceProvider.GetRequiredService<CatalogueEmploi.Infrastructure.Persistance.CatalogueEmploiDbContext>();
    var freelanceDb = scope.ServiceProvider.GetRequiredService<AppelOffreFreelance.Infrastructure.Persistance.AppelOffreFreelanceDbContext>();
    var actualiteDb = scope.ServiceProvider.GetRequiredService<ActualiteEtAbonnement.Infrastructure.Persistance.ActualiteEtAbonnementDbContext>();

    // EnsureCreated sur le premier crée la base, mais pas les tables des autres.
    // On utilise GetPendingMigrations/CreateTables pour forcer la création de toutes les tables.
    identiteDb.Database.EnsureCreated();

    // Pour les DbContexts suivants, on crée les tables manuellement si elles n'existent pas
    try { catalogueDb.Database.ExecuteSqlRaw("SELECT 1 FROM \"AnnoncesEmploi\" LIMIT 1"); }
    catch { CreateTablesFor(catalogueDb); }

    try { freelanceDb.Database.ExecuteSqlRaw("SELECT 1 FROM \"AppelsOffre\" LIMIT 1"); }
    catch { CreateTablesFor(freelanceDb); }

    try { actualiteDb.Database.ExecuteSqlRaw("SELECT 1 FROM \"ArticlesActualite\" LIMIT 1"); }
    catch { CreateTablesFor(actualiteDb); }
}

static void CreateTablesFor(DbContext context)
{
    var creator = context.Database.GetService<Microsoft.EntityFrameworkCore.Storage.IRelationalDatabaseCreator>();
    creator.CreateTables();
}

if (app.Environment.IsDevelopment())
{
    PlateformeCVTech.Api.DonneesDemonstration.Initialiser(app.Services);
}

// --- Middleware ---
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors();
app.MapControllers();

app.Run();
