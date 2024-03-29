﻿using FlightPlanner.Handlers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FlightPlanner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            bool useInMemoryDatabase = builder.Configuration.GetValue<bool>("UseInMemoryDatabase");

            builder.Services.AddDbContextPool<FlightPlannerDbContext>(options =>
            {
                if (useInMemoryDatabase)
                {
                    options.UseInMemoryDatabase("flight-planner");
                }
                else
                {
                    options.UseSqlServer(builder.Configuration.GetConnectionString("flight-planner"));
                }
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run(); 
        }
    }
}

