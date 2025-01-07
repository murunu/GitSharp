using Cocona;
using GitSharp.Commands;
using GitSharp.Helpers;
using Microsoft.Extensions.DependencyInjection;

var builder = CoconaApp.CreateBuilder();

builder.Services.AddSingleton<TreeBuilder>();
builder.Services.AddSingleton<CommitBuilder>();
builder.Services.AddSingleton<TreeCommands>();

var app = builder.Build();

app.AddCommands<CommitCommands>();
app.AddCommands<InitCommands>();
app.AddCommands<ChangesCommands>();

app.Run();