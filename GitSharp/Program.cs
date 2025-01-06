﻿using Cocona;
using GitSharp.Commands;

var builder = CoconaApp.CreateBuilder();

var app = builder.Build();

app.AddCommands<CommitCommands>();

app.Run();