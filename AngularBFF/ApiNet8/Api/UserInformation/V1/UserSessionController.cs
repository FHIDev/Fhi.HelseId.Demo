﻿using AngularBFF.Net8.Api.Api.UserInformation.V1.Dtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace AngularBFF.Net8.Api.UserInformation.V1
{
    record UserInformation();

    [ApiController]
    [Route("/api/v1/user-session")]
    public class UserSessionController : ControllerBase
    {

        [HttpGet(Name = "GetUserSession")]
        public async Task<IActionResult> GetUserSession()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var idToken = await HttpContext.GetTokenAsync("id_token");
            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");

            return Ok(new UserSessionDto(accessToken, idToken, refreshToken));
        }
    }
}
