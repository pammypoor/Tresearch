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

namespace TrialByFire.Tresearch.Services.Implementations
{
    public class IntegrationTestDependences
    {
        public ISqlDAO sqlDAO { get; }
        public ILogService logService { get; }
        public IMessageBank messageBank { get; }
        public IAuthenticationService authenticationService { get; }
        public IAuthorizationService authorizationService { get; }

        public IValidationService validationService { get; }


        public IntegrationTestDependences()
        {
            sqlDAO = new SqlDAO();
            logService = new SqlLogService(sqlDAO);
            messageBank = new MessageBank();
            authenticationService = new AuthenticationService(sqlDAO, logService, messageBank);
            authorizationService = new AuthorizationService(sqlDAO, logService);
            validationService = new ValidationService(messageBank);
        }
    }
}
