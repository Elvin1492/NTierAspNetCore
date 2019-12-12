using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using StartApp.API.Infrastructure;
using StartApp.API.ViewModels;
using StartApp.Core.Domain.Identity;
using StartApp.Service.Identity;

namespace StartApp.API.Controllers.Identity
{

    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private readonly ITokenService _tokenService;
        private readonly IAppUserService _appUserService;
        private readonly IAppRoleService _appRoleService;
        private readonly IConfiguration _configuration;

        private readonly SignInManager<AppUser> _signInManager;
        #region Constructor

        public TokenController(IConfiguration configuration, ITokenService tokenService, IAppUserService appUserService, SignInManager<AppUser> signInManager, IAppRoleService appRoleService)
        {
            _configuration = configuration;
            _tokenService = tokenService;
            _appUserService = appUserService;
            _signInManager = signInManager;
            _appRoleService = appRoleService;

            JsonSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            };
        }
        #endregion

        [HttpPost("Auth")]
        public async Task<IActionResult>
            Jwt([FromBody] TokenRequestViewModel model)
        {
            // return a generic HTTP Status 500 (Server Error)
            // if the client payload is invalid.
            if (model == null) return new StatusCodeResult(500);
            return model.grant_type switch
            {
                "password" => await GetToken(model),
                "refresh_token" => await RefreshToken(model),
                "sing_out" => await SignOut(),
                _ => new UnauthorizedResult()
            };
        }

       
        private async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync();
            return Ok();
        }

        private async Task<IActionResult>
            GetToken(TokenRequestViewModel model)
        {
            try
            {
                // check if there's an user with the given username
                var user = await
                    _appUserService.FindByNameAsync(model.username);
                // fallback to support e-mail address instead of username
                if (user == null && model.username.Contains("@"))
                    user = await
                        _appUserService.FindByEmailAsync(model.username);
                if (user == null
                    || !await _appUserService.CheckPasswordAsync(user,
                        model.password))
                {
                    // user does not exists or password mismatch
                    return new UnauthorizedResult();
                }
                // username & password matches: create the refresh token
                var refreshToken = CreateRefreshToken(model.provider_id, user.Id, model.username);
                // add the new refresh token to the DB
                _tokenService.Add(refreshToken);
                // create & return the access token
                var token = CreateAccessToken(user, refreshToken.Value);
                return Json(token);
            }
            catch (Exception ex)
            {
                return new UnauthorizedResult();
            }
        }

        private async Task<IActionResult> RefreshToken(TokenRequestViewModel model)
        {
            try
            {
                var rt = _tokenService.FindByKeys(model.provider_id, model.refresh_token);

                if (rt == null)
                {
                    return new UnauthorizedResult();
                }

                var user = await _appUserService.FindByIdAsync(rt.UserId);

                if (user == null)
                {
                    return new UnauthorizedResult();
                }

                var rtNew = CreateRefreshToken(rt.LoginProvider, rt.UserId, user.UserName);

                _tokenService.Remove(rt);
                _tokenService.Add(rtNew);

                var response = CreateAccessToken(user, rtNew.Value);

                return Json(response);
            }
            catch (Exception ex)
            {
                return new UnauthorizedResult();
            }
        }

        private AppUserToken CreateRefreshToken(string clientId, string userId, string name)
        {
            return new AppUserToken()
            {
                LoginProvider = clientId,
                UserId = userId,
                Name = name,
                Type = 0,
                Value = Guid.NewGuid().ToString("N"),
                AddedDate = DateTime.UtcNow
            };
        }

        private TokenResponseViewModel CreateAccessToken(AppUser user, string
            refreshToken)
        {
            var now = DateTime.UtcNow;

            // add the registered claims for JWT (RFC7519).
            // For more info, see https://tools.ietf.org/html/rfc7519#section-4.1
            var claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUnixTimeSeconds().ToString()),
				// TODO: add additional claims here
				new Claim(ClaimTypes.Role, "Akuna"),
                new Claim(ClaimTypes.WindowsUserClaim, "Matata")
            };

            var roles = _appUserService.GetRolesAsync(user).Result;

            if (roles != null && roles.Count > 0)
            {
                claims.AddRange(roles.Select(x => new Claim(ClaimTypes.Role, x)));
            }


            var tokenExpirationMins = _configuration.GetValue<int>("Auth:Jwt:TokenExpirationInMinutes");
            var issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Auth:Jwt:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Auth:Jwt:Issuer"],
                audience: _configuration["Auth:Jwt:Audience"],
                claims: claims.ToArray(),
                notBefore: now,
                expires: now.Add(TimeSpan.FromMinutes(tokenExpirationMins)),
                signingCredentials: new SigningCredentials(
                    issuerSigningKey, SecurityAlgorithms.HmacSha256)
            );

            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);

            return new TokenResponseViewModel()
            {
                token = encodedToken,
                expiration = tokenExpirationMins,
                refresh_token = refreshToken
            };
        }

        [HttpPost("Facebook")]
        public async Task<IActionResult> Facebook([FromBody]ExternalLoginRequestViewModel model)
        {
            try
            {
                const string fbApiUrl = "https://graph.facebook.com/v2.10/";
                var fbApiQueryString = $"me?scope=email&access_token={model.access_token}&fields=id,name,email";
                string result;

                // fetch the user info from Facebook Graph v2.10
                using (var c = new HttpClient())
                {
                    c.BaseAddress = new Uri(fbApiUrl);
                    var response = await c
                        .GetAsync(fbApiQueryString);
                    if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadAsStringAsync();
                    }
                    else throw new Exception("Authentication error");
                }

                // load the resulting Json into a dictionary
                var epInfo = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
                var info = new UserLoginInfo("facebook", epInfo["id"], "Facebook");

                // Check if this user already registered himself with this external provider before
                var user = await _appUserService.FindByLoginAsync(
                    info.LoginProvider, info.ProviderKey);
                if (user == null)
                {
                    // If we reach this point, it means that this user never tried to logged in
                    // using this external provider. However, it could have used other providers 
                    // and /or have a local account. 
                    // We can find out if that's the case by looking for his e-mail address.

                    // Lookup if there's an username with this e-mail address in the Db
                    user = await _appUserService.FindByEmailAsync(epInfo["email"]);
                    if (user == null)
                    {
                        // No user has been found: register a new user using the info 
                        //  retrieved from the provider
                        var now = DateTime.Now;
                        var username = String.Format("FB{0}{1}",
                                epInfo["id"],
                                Guid.NewGuid().ToString("N")
                            );
                        user = new AppUser()
                        {
                            SecurityStamp = Guid.NewGuid().ToString(),
                            // ensure the user will have an unique username
                            UserName = username,
                            Email = epInfo["email"],
                            DisplayName = epInfo["name"],
                            CreatedDate = now,
                            LastModifiedDate = now,
                            EmailConfirmed = true,
                            LockoutEnabled = false
                        };

                        // Add the user to the Db with a random password
                        await _appUserService.CreateAsync(user,
                            DataHelper.GenerateRandomPassword());

                        // Assign the user to the 'RegisteredUser' role.
                        await _appUserService.AddToRoleAsync(user, "RegisteredUser");
                    }
                    // Register this external provider to the user
                    var ir = await _appUserService.AddLoginAsync(user, info);
                    if (!ir.Succeeded)
                    {
                        throw new Exception("Authentication error");
                    }
                }

                // create the refresh token
                var rt = CreateRefreshToken(model.client_id, user.Id, user.UserName);

                // add the new refresh token to the DB
                _tokenService.Add(rt);

                // create & return the access token
                var t = CreateAccessToken(user, rt.Value);
                return Json(t);
            }
            catch (Exception ex)
            {
                // return a HTTP Status 400 (Bad Request) to the client
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpGet("ExternalLogin/{provider}")]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            switch (provider.ToLower())
            {
                case "facebook":
                    // case "google":
                    // case "twitter":
                    // todo: add all supported providers here

                    // Redirect the request to the external provider.
                    var redirectUrl = Url.Action(
                        nameof(ExternalLoginCallback),
                        "Token",
                        new { returnUrl });
                    var properties =
                        _signInManager.ConfigureExternalAuthenticationProperties(
                            provider,
                            redirectUrl);
                    return Challenge(properties, provider);
                default:
                    // provider not supported
                    return BadRequest(new
                    {
                        Error = $"Provider '{provider}' is not supported."
                    });
            }
        }

        [HttpGet("ExternalLoginCallback")]
        public async Task<IActionResult> ExternalLoginCallback(
            string returnUrl = null, string remoteError = null)
        {
            if (!string.IsNullOrEmpty(remoteError))
            {
                // TODO: handle external provider errors
                throw new Exception($"External Provider error: {remoteError}");
            }

            // Extract the login info obtained from the External Provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                // if there's none, emit an error
                throw new Exception("ERROR: No login info available.");
            }

            // Check if this user already registered himself with this external provider before
            var user = await _appUserService.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            if (user == null)
            {
                // If we reach this point, it means that this user never tried to logged in
                // using this external provider. However, it could have used other providers 
                // and /or have a local account. 
                // We can find out if that's the case by looking for his e-mail address.

                // Retrieve the 'emailaddress' claim
                const string emailKey = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";
                var email = info.Principal.FindFirst(emailKey).Value;

                // Lookup if there's an username with this e-mail address in the Db
                user = await _appUserService.FindByEmailAsync(email);
                if (user == null)
                {
                    // No user has been found: register a new user 
                    // using the info retrieved from the provider
                    var now = DateTime.Now;

                    // Create a unique username using the 'nameidentifier' claim
                    const string idKey = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
                    var username =
                        $"{info.LoginProvider}{info.Principal.FindFirst(idKey).Value}{Guid.NewGuid():N}";

                    user = new AppUser()
                    {
                        SecurityStamp = Guid.NewGuid().ToString(),
                        UserName = username,
                        Email = email,
                        CreatedDate = now,
                        LastModifiedDate = now,
                        EmailConfirmed = true,
                        LockoutEnabled = false
                    };

                    // Add the user to the Db with a random password
                    await _appUserService.CreateAsync(
                        user,
                        DataHelper.GenerateRandomPassword());

                    // Assign the user to the 'RegisteredUser' role.
                    await _appUserService.AddToRoleAsync(user, "RegisteredUser");

                    // Remove Lockout and E-Mail confirmation
                    //user.EmailConfirmed = true;
                    //user.LockoutEnabled = false;;
                }
                // Register this external provider to the user
                var ir = await _appUserService.AddLoginAsync(user, info);
                if (!ir.Succeeded)
                {
                    throw new Exception("Authentication error");
                }

            }

            // create the refresh token
            var rt = CreateRefreshToken("TestMakerFree", user.Id, user.UserName);

            // add the new refresh token to the DB
            _tokenService.Add(rt);

            // create & return the access token
            var t = CreateAccessToken(user, rt.Value);

            // output a <SCRIPT> tag to call a JS function 
            // registered into the parent window global scope
            return Content(
                "<script type=\"text/javascript\">" +
                "window.opener.externalProviderLogin(" +
                    JsonConvert.SerializeObject(t, JsonSettings) +
                ");" +
                "window.close();" +
                "</script>",
                "text/html"
                );
        }

        protected JsonSerializerSettings JsonSettings
        {
            get;
        }
    }
}