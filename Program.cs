using Amazon.S3;
using Microsoft.Extensions.Options;
using test_s3.Models;
using test_s3.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<AwsSettings>(builder.Configuration.GetSection("AwsSettings"));
builder.Services.AddSingleton<IAmazonS3>(sp =>
{
    var awsSettings = sp.GetRequiredService<IOptions<AwsSettings>>().Value;
    
    var config = new AmazonS3Config
    {
        ServiceURL = awsSettings.ServiceUrl,
        ForcePathStyle = true
    };

    return new AmazonS3Client(
        awsSettings.AccessKey,
        awsSettings.SecretKey,
        config
    );
});
builder.Services.AddSingleton<S3Service>();

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

app.Run();