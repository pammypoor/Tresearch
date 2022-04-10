﻿using Xunit;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Services.Implementations;
using TrialByFire.Tresearch.Models.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace TrialByFire.Tresearch.Tests.IntegrationTests.Registration
{
    public class RegistrationServiceShould : TestBaseClass
    {
        public RegistrationServiceShould() : base(new string[] { })
        {
            TestServices.AddScoped<IRegistrationService, RegistrationService>();
            TestProvider = TestServices.BuildServiceProvider();
        }


        [Theory]
        [InlineData("IntegrationRegistrationService1@gmail.com", "myRegisterPassword", "user", "200: Server: success")]
        [InlineData("IntegrationRegistrationService2@gmail.com", "unFortunateName", "user", "200: Server: success")]
        [InlineData("IntegrationRegistrationService3@gmail.com", "unFortunateName", "user", "409: Server: Account  already exists")]
        public async Task CreateTheAccountAsync(string email, string passphrase, string authorizationLevel, string statusCode)
        {

            //Arrange
            IAccount account = new Account(email, passphrase, "user", true, false);
            IRegistrationService registrationService = TestProvider.GetService<IRegistrationService>();
            string expected = statusCode;
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));

            //Act
            Tuple<int, string> results = await registrationService.CreateAccountAsync(email, passphrase, authorizationLevel, cancellationTokenSource.Token).ConfigureAwait(false);

            //Assert
            Assert.Equal(expected, results.Item2);
        }


        [Theory]
        [InlineData("IntegrationRegistrationService4@gmail.com", "user", "200: Server: success")]
        [InlineData("IntegrationRegistrationService5@gmail.com", "user", "409: Database: The confirmation link already exists.")]
        [InlineData("IntegrationRegistrationService99@gmail.com", "user", "500: Database: The Account was not found.")]
        public async Task CreateTheLinkAsync(string email, string authorizationLevel, string statusCode)
        {
            //Arrange
            IRegistrationService registrationService = TestProvider.GetService<IRegistrationService>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));

            //Act
            Tuple<IConfirmationLink, string> results = await registrationService.CreateConfirmationAsync(email, authorizationLevel, cancellationTokenSource.Token).ConfigureAwait(false);

            //Assert
            Assert.Equal(statusCode, results.Item2);        // GUID contains 36 characters
        }

        [Theory]
        [InlineData("IntegrationRegistrationService6@gmail.com", "user", "200: Server: success")]
        [InlineData("IntegrationRegistrationService7@gmail.com", "user", "200: Server: success")]
        [InlineData("IntegrationRegistrationService99@gmail.com", "user", "500: Database: The Account was not found.")]
        public async Task ConfirmTheUserAsync(string email, string authenticationLevel, string statusCode)
        {
            //Arrange
            IRegistrationService registrationService = TestProvider.GetService<IRegistrationService>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));
            string expected = statusCode;

            //Act
            string result = await registrationService.ConfirmAccountAsync(email, authenticationLevel, cancellationTokenSource.Token).ConfigureAwait(false);

            //Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("7eeb0847-f9f7-4ff4-b7e1-de4a4160c965", "200: Server: success")]
        [InlineData("7eeb0847-f9f7-4ff4-b7e1-ab4a4160c965", "404: Database: The confirmation link was not found.")]
        public async Task GetConfirmationLink(string guid, string statusCode)
        {
            //Arrange
            IRegistrationService registrationService = TestProvider.GetService<IRegistrationService>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));
            string expected = statusCode;

            //Act
            Tuple<IConfirmationLink, string> link = await registrationService.GetConfirmationLinkAsync(guid, cancellationTokenSource.Token);
            string result = link.Item2;

            //Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("pammypoor@gmail.com", "1D479F5F473B624F8DAE5A64BA677DAD94F0ED9C4B091D9B812B363B37BF070F3656867B3D3D4E318B04404DC2001F53E5DBA2069EF40C46C0DF77EF8FEF95A6")]
        public async Task HashValue(string value, string expected)
        {
            //Arrange
            IRegistrationService registrationService = TestProvider.GetService<IRegistrationService>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));

            //Act
            string result = await registrationService.HashValueAsync(value, cancellationTokenSource.Token);

            //Assert
            Assert.Equal(expected, result);

        }
    }
}
