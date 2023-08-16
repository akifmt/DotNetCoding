using AutoMapper;
using BlazorAppQuartzNETScheduler.Data;
using BlazorAppQuartzNETScheduler.Models;
using BlazorAppQuartzNETScheduler.Services;
using BlazorAppQuartzNETScheduler.ViewModels;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Quartz;
using System.Data.SQLite;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        string quartzConnectionString = builder.Configuration.GetConnectionString("QuartzConnectionString");
        SqliteConnectionStringBuilder quartzSqliteConnectionStringBuilder = new SqliteConnectionStringBuilder(
            quartzConnectionString);
        // to create database dbcontext models
        // dotnet ef dbcontext scaffold "Data Source=data.db;Cache=Shared" Microsoft.EntityFrameworkCore.Sqlite -o Models
        CheckandCreateQuartzDatabase(quartzSqliteConnectionStringBuilder).GetAwaiter().GetResult();

        // Add services to the container.
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase("ConnectionInMemory")
         );

        builder.Services.AddScoped<SeedData>();
        builder.Services.AddScoped<BlogPostService>();
        var mapperConfiguration = new MapperConfiguration(configuration =>
        {
            var profile = new MappingProfile();
            configuration.AddProfile(profile);
        });
        var mapper = mapperConfiguration.CreateMapper();
        builder.Services.AddSingleton(mapper);

        // add Quartz service
        builder.Services.AddDbContext<QuartzDbContext>(options =>
        {
            options.UseSqlite(quartzConnectionString);
        });
        builder.Services.AddQuartz(q =>
        {
            q.UsePersistentStore(per =>
            {
                per.UseSQLite(quartzSqliteConnectionStringBuilder.ConnectionString);
                per.UseJsonSerializer();
            });
            q.UseMicrosoftDependencyInjectionJobFactory();
        });
        builder.Services.AddQuartzHostedService(opt =>
        {
            opt.WaitForJobsToComplete = true;
        });

        builder.Services.CreateDatabase().GetAwaiter().GetResult();

        // check and create jobs
        builder.Services.CheckandCreateJobs().GetAwaiter().GetResult();

        builder.Services.AddDatabaseDeveloperPageExceptionFilter();
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseRouting();

        app.MapBlazorHub();
        app.MapFallbackToPage("/_Host");

        app.Run();
    }

    public static Task CheckandCreateQuartzDatabase(SqliteConnectionStringBuilder connectionString)
    {
        // https://github.com/quartznet/quartznet/blob/main/database/tables/tables_sqlite.sql

        if (File.Exists(connectionString.DataSource)) return Task.CompletedTask;

        SQLiteConnection.CreateFile(connectionString.DataSource);

        SQLiteConnection m_dbConnection = new SQLiteConnection(connectionString.ConnectionString);
        m_dbConnection.Open();
        string sql = "DROP TABLE IF EXISTS QRTZ_FIRED_TRIGGERS;\r\nDROP TABLE IF EXISTS QRTZ_PAUSED_TRIGGER_GRPS;\r\nDROP TABLE IF EXISTS QRTZ_SCHEDULER_STATE;\r\nDROP TABLE IF EXISTS QRTZ_LOCKS;\r\nDROP TABLE IF EXISTS QRTZ_SIMPROP_TRIGGERS;\r\nDROP TABLE IF EXISTS QRTZ_SIMPLE_TRIGGERS;\r\nDROP TABLE IF EXISTS QRTZ_CRON_TRIGGERS;\r\nDROP TABLE IF EXISTS QRTZ_BLOB_TRIGGERS;\r\nDROP TABLE IF EXISTS QRTZ_TRIGGERS;\r\nDROP TABLE IF EXISTS QRTZ_JOB_DETAILS;\r\nDROP TABLE IF EXISTS QRTZ_CALENDARS;\r\n\r\n\r\nCREATE TABLE QRTZ_JOB_DETAILS\r\n  (\r\n    SCHED_NAME NVARCHAR(120) NOT NULL,\r\n\tJOB_NAME NVARCHAR(150) NOT NULL,\r\n    JOB_GROUP NVARCHAR(150) NOT NULL,\r\n    DESCRIPTION NVARCHAR(250) NULL,\r\n    JOB_CLASS_NAME   NVARCHAR(250) NOT NULL,\r\n    IS_DURABLE BIT NOT NULL,\r\n    IS_NONCONCURRENT BIT NOT NULL,\r\n    IS_UPDATE_DATA BIT  NOT NULL,\r\n\tREQUESTS_RECOVERY BIT NOT NULL,\r\n    JOB_DATA BLOB NULL,\r\n    PRIMARY KEY (SCHED_NAME,JOB_NAME,JOB_GROUP)\r\n);\r\n\r\nCREATE TABLE QRTZ_TRIGGERS\r\n  (\r\n    SCHED_NAME NVARCHAR(120) NOT NULL,\r\n\tTRIGGER_NAME NVARCHAR(150) NOT NULL,\r\n    TRIGGER_GROUP NVARCHAR(150) NOT NULL,\r\n    JOB_NAME NVARCHAR(150) NOT NULL,\r\n    JOB_GROUP NVARCHAR(150) NOT NULL,\r\n    DESCRIPTION NVARCHAR(250) NULL,\r\n    NEXT_FIRE_TIME BIGINT NULL,\r\n    PREV_FIRE_TIME BIGINT NULL,\r\n    PRIORITY INTEGER NULL,\r\n    TRIGGER_STATE NVARCHAR(16) NOT NULL,\r\n    TRIGGER_TYPE NVARCHAR(8) NOT NULL,\r\n    START_TIME BIGINT NOT NULL,\r\n    END_TIME BIGINT NULL,\r\n    CALENDAR_NAME NVARCHAR(200) NULL,\r\n    MISFIRE_INSTR INTEGER NULL,\r\n    JOB_DATA BLOB NULL,\r\n    PRIMARY KEY (SCHED_NAME,TRIGGER_NAME,TRIGGER_GROUP),\r\n    FOREIGN KEY (SCHED_NAME,JOB_NAME,JOB_GROUP)\r\n        REFERENCES QRTZ_JOB_DETAILS(SCHED_NAME,JOB_NAME,JOB_GROUP)\r\n);\r\n\r\nCREATE TABLE QRTZ_SIMPLE_TRIGGERS\r\n  (\r\n    SCHED_NAME NVARCHAR(120) NOT NULL,\r\n\tTRIGGER_NAME NVARCHAR(150) NOT NULL,\r\n    TRIGGER_GROUP NVARCHAR(150) NOT NULL,\r\n    REPEAT_COUNT BIGINT NOT NULL,\r\n    REPEAT_INTERVAL BIGINT NOT NULL,\r\n    TIMES_TRIGGERED BIGINT NOT NULL,\r\n    PRIMARY KEY (SCHED_NAME,TRIGGER_NAME,TRIGGER_GROUP),\r\n    FOREIGN KEY (SCHED_NAME,TRIGGER_NAME,TRIGGER_GROUP)\r\n        REFERENCES QRTZ_TRIGGERS(SCHED_NAME,TRIGGER_NAME,TRIGGER_GROUP) ON DELETE CASCADE\r\n);\r\n\r\nCREATE TRIGGER DELETE_SIMPLE_TRIGGER DELETE ON QRTZ_TRIGGERS\r\nBEGIN\r\n\tDELETE FROM QRTZ_SIMPLE_TRIGGERS WHERE SCHED_NAME=OLD.SCHED_NAME AND TRIGGER_NAME=OLD.TRIGGER_NAME AND TRIGGER_GROUP=OLD.TRIGGER_GROUP;\r\nEND\r\n;\r\n\r\nCREATE TABLE QRTZ_SIMPROP_TRIGGERS \r\n  (\r\n    SCHED_NAME NVARCHAR (120) NOT NULL ,\r\n    TRIGGER_NAME NVARCHAR (150) NOT NULL ,\r\n    TRIGGER_GROUP NVARCHAR (150) NOT NULL ,\r\n    STR_PROP_1 NVARCHAR (512) NULL,\r\n    STR_PROP_2 NVARCHAR (512) NULL,\r\n    STR_PROP_3 NVARCHAR (512) NULL,\r\n    INT_PROP_1 INT NULL,\r\n    INT_PROP_2 INT NULL,\r\n    LONG_PROP_1 BIGINT NULL,\r\n    LONG_PROP_2 BIGINT NULL,\r\n    DEC_PROP_1 NUMERIC NULL,\r\n    DEC_PROP_2 NUMERIC NULL,\r\n    BOOL_PROP_1 BIT NULL,\r\n    BOOL_PROP_2 BIT NULL,\r\n    TIME_ZONE_ID NVARCHAR(80) NULL,\r\n\tPRIMARY KEY (SCHED_NAME,TRIGGER_NAME,TRIGGER_GROUP),\r\n\tFOREIGN KEY (SCHED_NAME,TRIGGER_NAME,TRIGGER_GROUP)\r\n        REFERENCES QRTZ_TRIGGERS(SCHED_NAME,TRIGGER_NAME,TRIGGER_GROUP) ON DELETE CASCADE\r\n);\r\n\r\nCREATE TRIGGER DELETE_SIMPROP_TRIGGER DELETE ON QRTZ_TRIGGERS\r\nBEGIN\r\n\tDELETE FROM QRTZ_SIMPROP_TRIGGERS WHERE SCHED_NAME=OLD.SCHED_NAME AND TRIGGER_NAME=OLD.TRIGGER_NAME AND TRIGGER_GROUP=OLD.TRIGGER_GROUP;\r\nEND\r\n;\r\n\r\nCREATE TABLE QRTZ_CRON_TRIGGERS\r\n  (\r\n    SCHED_NAME NVARCHAR(120) NOT NULL,\r\n\tTRIGGER_NAME NVARCHAR(150) NOT NULL,\r\n    TRIGGER_GROUP NVARCHAR(150) NOT NULL,\r\n    CRON_EXPRESSION NVARCHAR(250) NOT NULL,\r\n    TIME_ZONE_ID NVARCHAR(80),\r\n    PRIMARY KEY (SCHED_NAME,TRIGGER_NAME,TRIGGER_GROUP),\r\n    FOREIGN KEY (SCHED_NAME,TRIGGER_NAME,TRIGGER_GROUP)\r\n        REFERENCES QRTZ_TRIGGERS(SCHED_NAME,TRIGGER_NAME,TRIGGER_GROUP) ON DELETE CASCADE\r\n);\r\n\r\nCREATE TRIGGER DELETE_CRON_TRIGGER DELETE ON QRTZ_TRIGGERS\r\nBEGIN\r\n\tDELETE FROM QRTZ_CRON_TRIGGERS WHERE SCHED_NAME=OLD.SCHED_NAME AND TRIGGER_NAME=OLD.TRIGGER_NAME AND TRIGGER_GROUP=OLD.TRIGGER_GROUP;\r\nEND\r\n;\r\n\r\nCREATE TABLE QRTZ_BLOB_TRIGGERS\r\n  (\r\n    SCHED_NAME NVARCHAR(120) NOT NULL,\r\n\tTRIGGER_NAME NVARCHAR(150) NOT NULL,\r\n    TRIGGER_GROUP NVARCHAR(150) NOT NULL,\r\n    BLOB_DATA BLOB NULL,\r\n    PRIMARY KEY (SCHED_NAME,TRIGGER_NAME,TRIGGER_GROUP),\r\n    FOREIGN KEY (SCHED_NAME,TRIGGER_NAME,TRIGGER_GROUP)\r\n        REFERENCES QRTZ_TRIGGERS(SCHED_NAME,TRIGGER_NAME,TRIGGER_GROUP) ON DELETE CASCADE\r\n);\r\n\r\nCREATE TRIGGER DELETE_BLOB_TRIGGER DELETE ON QRTZ_TRIGGERS\r\nBEGIN\r\n\tDELETE FROM QRTZ_BLOB_TRIGGERS WHERE SCHED_NAME=OLD.SCHED_NAME AND TRIGGER_NAME=OLD.TRIGGER_NAME AND TRIGGER_GROUP=OLD.TRIGGER_GROUP;\r\nEND\r\n;\r\n\r\nCREATE TABLE QRTZ_CALENDARS\r\n  (\r\n    SCHED_NAME NVARCHAR(120) NOT NULL,\r\n\tCALENDAR_NAME  NVARCHAR(200) NOT NULL,\r\n    CALENDAR BLOB NOT NULL,\r\n    PRIMARY KEY (SCHED_NAME,CALENDAR_NAME)\r\n);\r\n\r\nCREATE TABLE QRTZ_PAUSED_TRIGGER_GRPS\r\n  (\r\n    SCHED_NAME NVARCHAR(120) NOT NULL,\r\n\tTRIGGER_GROUP NVARCHAR(150) NOT NULL, \r\n    PRIMARY KEY (SCHED_NAME,TRIGGER_GROUP)\r\n);\r\n\r\nCREATE TABLE QRTZ_FIRED_TRIGGERS\r\n  (\r\n    SCHED_NAME NVARCHAR(120) NOT NULL,\r\n\tENTRY_ID NVARCHAR(140) NOT NULL,\r\n    TRIGGER_NAME NVARCHAR(150) NOT NULL,\r\n    TRIGGER_GROUP NVARCHAR(150) NOT NULL,\r\n    INSTANCE_NAME NVARCHAR(200) NOT NULL,\r\n    FIRED_TIME BIGINT NOT NULL,\r\n    SCHED_TIME BIGINT NOT NULL,\r\n\tPRIORITY INTEGER NOT NULL,\r\n    STATE NVARCHAR(16) NOT NULL,\r\n    JOB_NAME NVARCHAR(150) NULL,\r\n    JOB_GROUP NVARCHAR(150) NULL,\r\n    IS_NONCONCURRENT BIT NULL,\r\n    REQUESTS_RECOVERY BIT NULL,\r\n    PRIMARY KEY (SCHED_NAME,ENTRY_ID)\r\n);\r\n\r\nCREATE TABLE QRTZ_SCHEDULER_STATE\r\n  (\r\n    SCHED_NAME NVARCHAR(120) NOT NULL,\r\n\tINSTANCE_NAME NVARCHAR(200) NOT NULL,\r\n    LAST_CHECKIN_TIME BIGINT NOT NULL,\r\n    CHECKIN_INTERVAL BIGINT NOT NULL,\r\n    PRIMARY KEY (SCHED_NAME,INSTANCE_NAME)\r\n);\r\n\r\nCREATE TABLE QRTZ_LOCKS\r\n  (\r\n    SCHED_NAME NVARCHAR(120) NOT NULL,\r\n\tLOCK_NAME  NVARCHAR(40) NOT NULL, \r\n    PRIMARY KEY (SCHED_NAME,LOCK_NAME)\r\n);";
        SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
        command.ExecuteNonQuery();
        m_dbConnection.Close();

        return Task.CompletedTask;
    }
}

public static class ServiceCollectionExtensitions
{
    public static async Task CreateDatabase(this IServiceCollection services)
    {
        using (IServiceScope tmp = services.BuildServiceProvider().CreateScope())
        {
            await using var _context = tmp.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var seedData = tmp.ServiceProvider.GetRequiredService<SeedData>();
            await seedData.CreateInitialData();
        }
    }

    public static async Task CheckandCreateJobs(this IServiceCollection services)
    {
        using (IServiceScope tmp = services.BuildServiceProvider().CreateScope())
        {
            await using var _context = tmp.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var seedData = tmp.ServiceProvider.GetRequiredService<SeedData>();
            await seedData.CheckandCreateJobsData();
        }
    }
}

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BlogPost, BlogPostViewModel>().ReverseMap();
    }
}