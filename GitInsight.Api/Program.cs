

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<GitInsightContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString")));
builder.Services.AddScoped<IDBAnalysisRepository, DBAnalysisRepository>();
builder.Services.AddScoped<IDBFrequencyRepository, DBFrequencyRepository>(); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var analysis_s = app.MapGroup("/analysis_s").WithOpenApi();

analysis_s.MapGet("", (IDBAnalysisRepository repository) => repository.Read());
analysis_s.MapGet("/{id}", (int id, IDBAnalysisRepository repository) => repository.Find(id));
analysis_s.MapPost("", (DBAnalysisCreateDTO analysis, IDBAnalysisRepository repository) => repository.Create(analysis));
analysis_s.MapPut("", (DBAnalysisUpdateDTO analysis, IDBAnalysisRepository repository) => repository.Update(analysis));
analysis_s.MapDelete("/{id}", (int id, IDBAnalysisRepository repository) => repository.Delete(id));

var frequencies = app.MapGroup("/frequencies").WithOpenApi();

frequencies.MapGet("", (IDBFrequencyRepository repository) => repository.Read());
frequencies.MapGet("/{id}", (int id, DateTime date, IDBFrequencyRepository repository) => repository.Find(id, date));
frequencies.MapPost("", (DBFrequencyCreateDTO frequency, IDBFrequencyRepository repository) => repository.Create(frequency));
frequencies.MapPut("", (DBFrequencyUpdateDTO frequency, IDBFrequencyRepository repository) => repository.Update(frequency));
frequencies.MapDelete("/{id}", (int Analysisid, DateTime date, IDBFrequencyRepository repository) => repository.Delete(Analysisid, date));

app.Run();
