using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using QLKTX.Class;
using QLKTX.Class.Authorization;
using QLKTX.Class.Helper;
using QLKTX.Class.Middleware;
using QLKTX.Class.Middleware.LogRequest;
using QLKTX.Class.Middleware.LogResponse;
using QLKTX.Class.Middleware.UserAccountSession;
using QLKTX.Class.Repository;
using QLKTX.Class.Service;
using Serilog;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;

Log.Logger = new LoggerConfiguration()
                             .Enrich.FromLogContext()
                             .WriteTo.Console()
                             .CreateLogger();
var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
.ReadFrom.Configuration(hostingContext.Configuration));

var env = builder.Environment;
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
builder.Configuration.AddEnvironmentVariables();
var services = builder.Services;

services.AddHttpClient();
services.Configure<ConnectionStrings>(builder.Configuration.GetSection("ConnectionStrings"));
services.AddOptions();
var apiCorsPolicy = "SpecificOrigins";
//begin Cors
services.AddCors(options =>
{
    options.AddPolicy(name: apiCorsPolicy,
    builder =>
    {
        builder.AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});
//end Cors
// Add services to the container.
services.AddRazorPages();
services.AddControllersWithViews(); //.AddNewtonsoftJson()
services.AddResponseCaching();
int TokenExpires = int.Parse(builder.Configuration.GetValue<string>("TokenExpires").ToString());
services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        //options.ExpireTimeSpan = TimeSpan.FromMinutes(TokenExpires);
        //options.SlidingExpiration = true;
        options.LoginPath = "/Home/Login";
        options.AccessDeniedPath = "/Home/Forbidden/";
    });

// configure strongly typed settings object
services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
// Adding Authentication
string issuer = builder.Configuration.GetValue<string>("AppSettings:Issuer");
string audience = builder.Configuration.GetValue<string>("AppSettings:Audience");
string signingKey = builder.Configuration.GetValue<string>("AppSettings:Secret");
byte[] signingKeyBytes = System.Text.Encoding.UTF8.GetBytes(signingKey);
services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidIssuer = issuer,
        ValidateAudience = true,
        ValidAudience = audience,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = System.TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
    };
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers.Add("Token-Expired", "True");
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden; //403
            }
            return Task.CompletedTask;
        }
    };
});

services.AddDistributedMemoryCache();// Đăng ký dịch vụ lưu cache trong bộ nhớ (Session sẽ sử dụng nó)
services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(TokenExpires);
    //options.Cookie.HttpOnly = true;
    //options.Cookie.IsEssential = true;
});
services.AddMemoryCache();

//Register middleware handle error
services.AddScoped<LogRequestMiddleware>();
services.AddScoped<LogResponseMiddleware>();
services.AddScoped<UserAccountSessionMiddleware>();
services.AddScoped<JwtMiddleware>();

// configure DI for application services
services.AddScoped<IJwtUtils, JwtUtils>();
services.AddScoped<IUserAccountService, UserAccountService>();
services.AddScoped<IUploadFileService, UploadFileService>();

//Add Repository
services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
services.AddTransient<IComboRepository, ComboRepository>();
services.AddTransient<IComboPermissRepository, ComboPermissRepository>();
services.AddTransient<IAutocomplexRepository, AutocomplexRepository>();
services.AddTransient<IAutoTransNoRepository, AutoTransNoRepository>();
services.AddTransient<IUserAccountRepository, UserAccountRepository>();
services.AddTransient<IUserPermissionRepository, UserPermissionRepository>();
services.AddTransient<IUserGroupRepository, UserGroupRepository>();
services.AddTransient<IFunctionListRepository, FunctionListRepository>();
services.AddTransient<IPermissionRepository, PermissionRepository>();
services.AddTransient<ISystemVarRepository, SystemVarRepository>();
services.AddTransient<IApiLogRepository, ApiLogRepository>();
services.AddTransient<IMenuRepository, MenuRepository>();
services.AddTransient<ICompanyStructureRepository, CompanyStructureRepository>();
services.AddTransient<IPartnerGroupRepository, PartnerGroupRepository>();
services.AddTransient<IBuildingRepository, BuildingRepository>();
services.AddTransient<IDepartmentRepository, DepartmentRepository>();
services.AddTransient<IDepartmentLocationRepository, DepartmentLocationRepository>();
services.AddTransient<IContractDeclareRepository, ContractDeclareRepository>();
services.AddTransient<IContractRepository, ContractRepository>();
services.AddTransient<IPartnerRepository, PartnerRepository>();
services.AddTransient<IReasonRepository, ReasonRepository>();
services.AddTransient<ICategoryRepository, CategoryRepository>();
services.AddTransient<IFileUploadRepository, FileUploadRepository>();
services.AddTransient<IVoucherRepository, VoucherRepository>();
services.AddTransient<IDebtRepository, DebtRepository>();
services.AddTransient<IPosDeviceRepository, PosDeviceRepository>();
services.AddTransient<IPosVoucherRepository, PosVoucherRepository>();
services.AddTransient<ITagConfigRepository, TagConfigRepository>();
services.AddTransient<IContractExpiresRepository, ContractExpiresRepository>();
services.AddTransient<IDebtExpiresRepository, DebtExpiresRepository>();
services.AddTransient<IDeptLocationExpiresRepository, DeptLocationExpiresRepository>();
services.AddTransient<ICustomerDebtOverviewRepository, CustomerDebtOverviewRepository>();
services.AddTransient<ISummaryVoucherReportRepository, SummaryVoucherReportRepository>();
services.AddTransient<IVoucherDetailReportRepository, VoucherDetailReportRepository>();
services.AddTransient<IVoucherDReasonReportRepository, VoucherDReasonReportRepository>();
services.AddTransient<IClearsEmpDebtReportRepository, ClearsEmpDebtReportRepository>();
services.AddTransient<IStatisticsOfReceiptsRepository, StatisticsOfReceiptsRepository>();
services.AddTransient<IStatisticsOfPaymentsRepository, StatisticsOfPaymentsRepository>();
services.AddTransient<IContractDetailReportRepository, ContractDetailReportRepository>();
services.AddTransient<IContractStatisticsReportRepository, ContractStatisticsReportRepository>();
services.AddTransient<IContractStatisticsReasonReportRepository, ContractStatisticsReasonReportRepository>();
services.AddTransient<IDepartmentReportRepository, DepartmentReportRepository>();

services.AddControllers()
    .AddJsonOptions(x =>
    {
        // serialize enums as strings in api responses (e.g. Role)
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        //phải có dòng này để nó response đúng định dạng model. không thôi sẽ bị viết thường
        x.JsonSerializerOptions.PropertyNamingPolicy = null;
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        //options.SuppressInferBindingSourcesForParameters = false;
        options.SuppressModelStateInvalidFilter = true; //valid model phải set cái này
    });

services.AddHttpContextAccessor(); //thử dùng cái này để get HttpContext.User xem

builder.WebHost.ConfigureKestrel((context, options) =>
{
    options.Limits.MaxConcurrentConnections = 100;
    options.Limits.MaxConcurrentUpgradedConnections = 100;
    options.Limits.MaxRequestBodySize = 12428800;  //10MB
    options.AddServerHeader = false;

    //test http3
    //options.Listen(IPAddress.Any, 9001, listenOptions =>
    //{
    //    // Use HTTP/3
    //    listenOptions.Protocols = HttpProtocols.Http1AndHttp2AndHttp3;
    //    listenOptions.UseHttps();
    //});
    //end test http3
})
.UseIISIntegration()
.UseIIS(); //có cái này mới build prod

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Home/HandleError/{0}"); //404 notfound
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseCors(apiCorsPolicy);
app.UseResponseCaching();
app.UseAuthorization();
app.UseSession();
app.UseSerilogRequestLogging(); //Serilog 
//loging phải nằm dưới middleware để get được context.Items["UserMember"]
app.UseMiddleware<UserAccountSessionMiddleware>(); //0
// custom jwt auth middleware    
app.UseMiddleware<JwtMiddleware>(); //1
app.UseMiddleware<LogRequestMiddleware>(); //2
app.UseMiddleware<LogResponseMiddleware>(); //3
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Login}/{id?}");
    endpoints.MapRazorPages();
    //endpoints.MapControllers();
});

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
