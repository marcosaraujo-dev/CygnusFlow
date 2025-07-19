using CygnusFlow.App.Forms;
using CygnusFlow.App.Services;
using CygnusFlow.Application.Services;
using CygnusFlow.Application.UseCases.Projetos;
using CygnusFlow.Domain.Interfaces.Repositories;
using CygnusFlow.Infrastructure.Data.Context;
using CygnusFlow.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WinFormsApp = System.Windows.Forms.Application;


namespace CygnusFlow.App
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            WinFormsApp.EnableVisualStyles();
            WinFormsApp.SetCompatibleTextRenderingDefault(false);

            // Configurar DI Container
            var services = new ServiceCollection();
            ConfigureServices(services);

            var serviceProvider = services.BuildServiceProvider();

            // Executar migrations automaticamente
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<CygnusFlowContext>();
                context.Database.EnsureCreated();
            }

            var frmMain = serviceProvider.GetRequiredService<frmMain>();
            WinFormsApp.Run(frmMain);
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            // Configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
            .Build();

            services.AddSingleton<IConfiguration>(configuration);

            // Database
            services.AddDbContext<CygnusFlowContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Repositories
            services.AddScoped<IProjetoRepository, ProjetoRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IAtividadeRepository, AtividadeRepository>();

            // Use Cases
            services.AddScoped<CriarProjetoUseCase>();
            services.AddScoped<ListarProjetosUseCase>();
            services.AddScoped<EditarProjetoUseCase>();

            // Services
            services.AddScoped<DashboardService>();
            services.AddScoped<AuthService>();
            services.AddScoped<RelatorioService>();
            services.AddScoped<ExportService>();

            // Form Services
            services.AddSingleton<FormService>();
            services.AddSingleton<NotificationService>();

            // Forms
            //services.AddTransient<LoginForm>();
            services.AddTransient<frmMain>();
            //services.AddTransient<DashboardForm>();
            //services.AddTransient<ProjetoListForm>();
            //services.AddTransient<ProjetoEditForm>();
            //services.AddTransient<AtividadeListForm>();
            //services.AddTransient<AtividadeEditForm>();
            //services.AddTransient<RelatorioProjetosForm>();
            //services.AddTransient<RelatorioAtividadesForm>();
        }
    }
}