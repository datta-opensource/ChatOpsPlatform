using Microsoft.EntityFrameworkCore;
using SwiftLux.WhatsApp.Api.Configurations;
using SwiftLux.WhatsApp.Api.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IConversationService, ConversationService>();
// Add services
builder.Services.Configure<WhatsAppOptions>(
 builder.Configuration.GetSection(WhatsAppOptions.SectionName));
builder.Services.AddHttpClient<IWhatsAppService, WhatsAppService>(); 
builder.Services.AddScoped<WebhookEventProcessor>();
builder.Services.AddScoped<ICommandHandler, SimpleCommandHandler>();
builder.Services.AddScoped<IConversationService, ConversationService>();
builder.Services.Configure<OpenAIOptions>(
    builder.Configuration.GetSection(OpenAIOptions.SectionName));
builder.Services.AddHttpClient<IAgentService, OpenAIAgentService>();

//builder.Services.AddSingleton<IConversationService, InMemoryConversationService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// HTTP pipeline
    app.UseSwagger();
    app.UseSwaggerUI();


app.MapGet("/", () => Results.Ok("SwiftLux WhatsApp Bot is running"));
app.MapGet("/health", () => Results.Ok(new { status = "Healthy" }));
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();