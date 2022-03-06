﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;
using Xunit;

namespace TrialByFire.Tresearch.Tests.IntegrationTests.OTPRequest
{
    public class OTPRequestServiceShould : IntegrationTestDependencies
    {
        public OTPRequestServiceShould() : base()
        {

        }
        [Theory]
        [InlineData("larry@gmail.com", "abcDEF123", "user", "success")]
        [InlineData("larry@gmail.com", "#$%", "user", "Data: Invalid Username or " +
            "Passphrase. Please try again.")]
        [InlineData("larry@gmail.com", "abcdef#$%", "user", "Data: Invalid Username or " +
            "Passphrase. Please try again.")]
        [InlineData("larry@gmail.com", "abcdEF123", "user", "Data: Invalid Username or " +
            "Passphrase. Please try again.")]
        [InlineData("larry@gmail.com", "abcDEF123", "admin", "Database: The account was not found or it " +
            "has been disabled.")]
        [InlineData("billy@yahoo.com", "abcDEF123", "admin", "success")]
        [InlineData("joe@outlook.com", "abcDEF123", "user", "success")]
        [InlineData("bob@yahoo.com", "abcDEF123", "user", "Database: The account was not found or it " +
            "has been disabled.")]
        [InlineData("harry@yahoo.com", "abcDEF123", "user", "Database: Please confirm your " +
            "account before attempting to login.")]
        public void RequestTheOTP(string username, string passphrase, string authorizationLevel, string expected)
        {
            // Arrange
            IOTPRequestService otpRequestService = new OTPRequestService(sqlDAO, logService, messageBank);
            IAccount account = new Account(username, passphrase, authorizationLevel);
            IOTPClaim otpClaim = new OTPClaim(account);

            // Act
            string result = otpRequestService.RequestOTP(account, otpClaim);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
