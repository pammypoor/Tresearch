﻿using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;

namespace TrialByFire.Tresearch.Managers.Implementations
{
    public class RecoveryManager: IRecoveryManager
    {
        private ISqlDAO _sqlDAO { get; set; }
        private ILogService _logService { get; set; }
        private IMessageBank _messageBank { get; set; }
        private IRecoveryService _recoveryService { get; set; }

        private IMailService _mailService { get; set; }
        public RecoveryManager(ISqlDAO sqlDAO, ILogService logService, IMessageBank messageBank, IRecoveryService recoveryService, IMailService mailService)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _messageBank = messageBank;
            _recoveryService = recoveryService;
            _mailService = mailService;
        }

        public async Task<string> SendRecoveryEmail(string email, string baseurl, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken))
        {
           
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                
                //Check the status of account
                Tuple<IAccount, string> account = await _recoveryService.GetAccountAsync(email, authorizationLevel, cancellationToken);
                
                //Check if cancelled
                if (cancellationToken.IsCancellationRequested)                                                  
                    throw new OperationCanceledException();     //No rollback necessary                                          
                
                //Check if account exists
                if (account.Item2 != _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                    return account.Item2;
         
                //Check if valid account
                if (account.Item1.AccountStatus == true)          
                    return _messageBank.GetMessage(IMessageBank.Responses.alreadyEnabled).Result;

                // Create the recovery link
                Tuple<IRecoveryLink, string> recoveryLink = await _recoveryService.CreateRecoveryLinkAsync(account.Item1, cancellationToken);


                if (cancellationToken.IsCancellationRequested)
                {
                    string rollBackresult = await _recoveryService.RemoveRecoveryLinkAsync(recoveryLink.Item1);
                    if (rollBackresult != _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                        return _messageBank.GetMessage(IMessageBank.Responses.rollbackFailed).Result;
                    else
                        throw new OperationCanceledException();    //rollback taken care of
                }

                //Check if recovery link created in database
                if (recoveryLink.Item2 != _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                    return recoveryLink.Item2;

                //Send Recovery Link --> no possible rollback at this point but can still cancel
                string mailResults = await _mailService.SendRecoveryAsync(email, baseurl+recoveryLink.Item1.GUIDLink, cancellationToken);
                return mailResults;
            }
            catch (OperationCanceledException)
            {
                // Nothing to rollback
                return _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested).Result;
            }
            catch (Exception ex)
            {
                return "500: Server: " + ex;
            }
        }

        public async Task<string> EnableAccountAsync(string guid, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                //Get recoverylink from database
                Tuple<IRecoveryLink, string> recoveryLink = await _recoveryService.GetRecoveryLinkAsync(guid, cancellationToken);

                if (cancellationToken.IsCancellationRequested)
                    return _messageBank.ErrorMessages["cancellationRequested"]; //No rollback necessary

                if (recoveryLink.Item2 != _messageBank.SuccessMessages["generic"])
                    return recoveryLink.Item2;

                //Enable Account
                string enableResult = await _recoveryService.EnableAccountAsync(recoveryLink.Item1.Username, recoveryLink.Item1.AuthorizationLevel, cancellationToken);

                if (cancellationToken.IsCancellationRequested)
                {
                    string rollBack = await _recoveryService.DisableAccountAsync(recoveryLink.Item1.Username, recoveryLink.Item1.AuthorizationLevel);
                    if(rollBack != _messageBank.SuccessMessages["generic"])
                        return _messageBank.ErrorMessages["rollbackFailed"]; 
                    else
                        return _messageBank.ErrorMessages["cancellationRequested"];
                }

                //Make sure to remove recovery link from database and increment database

                return enableResult;

            }
            catch (OperationCanceledException)
            {
                // Nothing to rollback
                return _messageBank.ErrorMessages["cancellationRequested"];
            }
            catch (Exception ex)
            {
                return "500: Server: " + ex;
            }
        }
    }
}
