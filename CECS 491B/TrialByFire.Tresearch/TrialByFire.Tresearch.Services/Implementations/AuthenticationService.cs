﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Services.Contracts;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using TrialByFire.Tresearch.Models.Implementations;
using System.Security.Principal;
using TrialByFire.Tresearch.Exceptions;

namespace TrialByFire.Tresearch.Services.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private ISqlDAO _sqlDAO { get; }
        private ILogService _logService { get; }
        private IMessageBank _messageBank { get; }
        private string _payLoad { get; }

        public AuthenticationService(ISqlDAO sqlDAO, ILogService logService,
            IMessageBank messageBank)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _messageBank = messageBank;
            _payLoad = "";
        }

        public async Task<List<string>> AuthenticateAsync(IOTPClaim otpClaim)
        {
            List<string> results = await _sqlDAO.AuthenticateAsync(otpClaim);
            if (results[0].Equals(_messageBank.SuccessMessages["generic"]))
            {
                return CreateJwtToken(results[1]);
            }
            return results;
        }

        // use microsoft built in jWT
        // use default key, randomizer, replace every 3 months
        // look into AES type 

        private List<string> CreateJwtToken(string _payload)
        {
            List<string> results = new List<string>();

            // break payload into parts
            Dictionary<string, string> claimValuePairs = new Dictionary<string, string>();
            string[] claimValue = _payload.Split(",");
            foreach (string cV in claimValue)
            {
                string[] pair = cV.Split(":");
                claimValuePairs.Add(pair[0], pair[1]);
            }

            // create identity to place into JWT
            try
            {
                IRoleIdentity roleIdentity = new RoleIdentity(true, claimValuePairs["username"], claimValuePairs["authorizationLevel"]);
                //create jwt and set values
                var tokenHandler = new JwtSecurityTokenHandler();
                var keyValue = "akxhBSian218c9pJA98912n4010409AMKLUHqjn2njwaj";
                var key = Encoding.UTF8.GetBytes(keyValue);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] { new Claim("username", claimValuePairs["username"]), 
                        new Claim("authorizationLevel", claimValuePairs["authorizationLevel"]) }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    IssuedAt = DateTime.UtcNow,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                results.Add(_messageBank.SuccessMessages["generic"]);
                results.Add(tokenHandler.WriteToken(token));
            }
            catch (RoleIdentityCreationFailedException ricf)
            {
                results.Add(ricf.Message);
                return results;
            }
            catch (ArgumentNullException ane)
            {
                results.Add("Server: " + ane.Message);
                return results;
            }
            return results;
        }
    }
}