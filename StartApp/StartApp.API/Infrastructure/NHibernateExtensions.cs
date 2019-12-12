using FluentNHibernate.Cfg;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Cfg;
using NHibernate.Dialect;
using StartApp.DataAccess.Mappings;

namespace StartApp.API.Infrastructure
{
    public static class NHibernateExtensions
    {
        public static IServiceCollection AddNHibernate(this IServiceCollection services, string connectionString)
        {
            var configuration = new Configuration();

            configuration.DataBaseIntegration(c =>
            {
                c.Dialect<MsSql2012Dialect>();
                c.ConnectionString = connectionString;
                c.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
                c.SchemaAction = SchemaAutoAction.Validate;
                c.LogFormattedSql = true;
                c.LogSqlInConsole = true;
                c.SchemaAction = SchemaAutoAction.Update;
            });

            //configuration.AddIdentityMappingsForSqlServer();

            var sessionFactory = Fluently.Configure(configuration).Mappings(x =>
            {
                x.FluentMappings.AddFromAssemblyOf<ProductMap>();
            }).BuildSessionFactory();

            services.AddSingleton(sessionFactory);
            services.AddScoped(factory => sessionFactory.OpenSession());

            return services;
        }
    }
}
