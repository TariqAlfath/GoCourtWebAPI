using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Data.SqlClient;


namespace GoCourtWebAPI.LogicLayer.Extension
{
    public class AuthenticationMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor contextAccessor;

        public AuthenticationMiddleWare(RequestDelegate request, IConfiguration configuration, IHttpContextAccessor contextAccessor)
        {
            this._next = request;
            this.configuration = configuration;
            this.contextAccessor = contextAccessor;
        }

        public async Task Invoke(HttpContext context)
        {
            // Your logic to check for the token goes here
            try
            {
                if (context.Request.Headers.TryGetValue("Authorization", out var token))
                {
                    var stream = token.ToString().Split(' ').Last();
                    var handler = new JwtSecurityTokenHandler();
                    var jsonToken = handler.ReadToken(stream);
                    var tokenS = jsonToken as JwtSecurityToken;

                    var user = tokenS.Claims.First(claim => claim.Type == "Username").Value;
                    var roles = tokenS.Claims.First(claim => claim.Type == "role").Value;

                    var conString = configuration["ConnectionStrings:DefaultConnection"];

                    using (SqlConnection con = new SqlConnection(conString))
                    {
                        var sqlQuery = "SELECT COUNT(*) FROM tbl_user WHERE Username = @Username AND status = 1 AND role = @Roles";

                        con.Open();

                        using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                        {
                            cmd.Parameters.AddWithValue("@Username", user);
                            cmd.Parameters.AddWithValue("@Roles", roles);

                            // Use ExecuteScalarAsync on SqlCommand
                            var result = await cmd.ExecuteScalarAsync();

                            // Convert the result to an integer
                            var rowCount = Convert.ToInt32(result);

                            if (rowCount == 0)
                            {
                                // User not found, return 401 Unauthorized
                                context.Response.StatusCode = 401;
                                await context.Response.WriteAsync("Unauthorized");
                                return;
                            }

                            // Your logic with userData
                        }
                    }
                }



            }
            catch (Exception ex) { 
            }

            // Call the next middleware in the pipeline
            await _next(context);
        }
    }
    public static class AuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthenticationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthenticationMiddleWare>();
        }
    }
}
