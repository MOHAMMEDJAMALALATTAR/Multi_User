using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.DependencyInjection;
using Multi_User.Data;
using Multi_User.Models;

namespace Multi_User
{
    public  static class ServiceExtensions
    {
        public static void configureIdentity(this IServiceCollection services)
        {
            var builder=services.AddIdentityCore<ApiUser>(q=>q.User.RequireUniqueEmail=true);

            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole),services);
            builder.AddEntityFrameworkStores<DataBaseContext>().AddDefaultTokenProviders();
        }
    }
}
