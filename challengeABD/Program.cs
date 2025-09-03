using Microsoft.OpenApi.Models;
using System.ComponentModel;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using challengeABD; 
using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MotoDB>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddOpenApi(option =>
{
    option.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info = new OpenApiInfo
        {
            Title = "Moto API",
            Version = "v1",
            Description = "API de gerenciamento de motos",
            License = new OpenApiLicense()
            {
                Name = "MIT",
                Url = new Uri("https://opensource.org/licenses/MIT")
            }
        };
        return Task.CompletedTask;
    });
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();


var motoGroup = app.MapGroup("/motos").WithTags("Moto");

motoGroup.MapGet("/",
    async (MotoDB db) =>
        TypedResults.Ok(await db.Motos.ToListAsync()))
    .WithSummary("Retorna todas as motos")
    .WithDescription("Retorna uma lista de todas as motos cadastradas.");

motoGroup.MapGet("/{id:int}",
    async Task<Results<Ok<Moto>, NotFound>> ([Description("Id da moto buscada")] int id, MotoDB db) =>
        await db.Motos.FindAsync(id) is { } moto
            ? TypedResults.Ok(moto)
            : TypedResults.NotFound()
    )
    .WithSummary("Busca uma moto por Id")
    .WithDescription("Busca uma moto específica pelo seu Id. Se não encontrada, retorna NotFound (404).");

motoGroup.MapPost("/", async Task<Results<Created<Moto>, ValidationProblem>> (Moto moto, MotoDB db) =>
{
    var validationResults = new List<ValidationResult>();
    if (!Validator.TryValidateObject(moto, new ValidationContext(moto), validationResults, true))
    {
        return TypedResults.ValidationProblem(validationResults.ToDictionary(r => r.MemberNames.FirstOrDefault() ?? "", r => r.ErrorMessage?.Split(',') ?? []));
    }

    db.Motos.Add(moto);
    await db.SaveChangesAsync();

    return TypedResults.Created($"/motos/{moto.Id}", moto);
})
.Accepts<Moto>("application/json", ["application/xml"])
.WithSummary("Cria uma nova moto")
.WithDescription("Recebe os dados de uma nova moto via JSON no corpo da requisição, salva no banco e retorna a moto criada com status 201. Retorna 400 Bad Request se os dados forem inválidos.");

motoGroup.MapPut("/{id:int}", async Task<Results<NoContent, NotFound, ValidationProblem>> (int id, Moto motoAtualizada, MotoDB db) =>
{
    var validationResults = new List<ValidationResult>();
    if (!Validator.TryValidateObject(motoAtualizada, new ValidationContext(motoAtualizada), validationResults, true))
    {
        return TypedResults.ValidationProblem(validationResults.ToDictionary(r => r.MemberNames.FirstOrDefault() ?? "", r => r.ErrorMessage?.Split(',') ?? []));
    }

    var motoExistente = await db.Motos.FindAsync(id);

    if (motoExistente is null)
        return TypedResults.NotFound();

    motoExistente.Modelo = motoAtualizada.Modelo;
    motoExistente.Status = motoAtualizada.Status;
    motoExistente.Placa = motoAtualizada.Placa;

    await db.SaveChangesAsync();

    return TypedResults.NoContent();
})
.Accepts<Moto>("application/json", ["application/xml"])
.WithSummary("Atualiza uma moto existente")
.WithDescription("Recebe o Id da moto na URL e os novos dados via JSON/XML no corpo. Atualiza a moto no banco e retorna 204 No Content se bem-sucedido, 404 Not Found se a moto não existir, ou 400 Bad Request se os dados forem inválidos.");

motoGroup.MapDelete("/{id:int}", async Task<Results<NoContent, NotFound>> (int id, MotoDB db) =>
{
    var moto = await db.Motos.FindAsync(id);

    if (moto is null)
        return TypedResults.NotFound();

    db.Motos.Remove(moto);
    await db.SaveChangesAsync();

    return TypedResults.NoContent();
})
.WithSummary("Deleta uma moto existente")
.WithDescription("Recebe o Id da moto na URL. Deleta a moto correspondente do banco e retorna 204 No Content se bem-sucedido, ou 404 Not Found se a moto não existir.");

app.Run();

public static class ValidationExtensions
{
    public static IDictionary<string, string[]> ToDictionary(this List<ValidationResult> validationResults)
    {
        return validationResults
            .GroupBy(r => r.MemberNames.FirstOrDefault() ?? "")
            .ToDictionary(
                g => g.Key,
                g => g.Select(r => r.ErrorMessage ?? "").ToArray()
            );
    }
}

