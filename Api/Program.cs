using Api.StartupExtension;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHsts();
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseCors("newPolicy");

app.UseRouting();
app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
