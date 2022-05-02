﻿using Microsoft.Extensions.Options;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Models;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;

namespace TrialByFire.Tresearch.Services.Implementations
{
    public class RateService: IRateService
    {
        private ISqlDAO _sqlDAO { get; set; }

        private ILogService _logService { get; set; }

        private IMessageBank _messageBank { get; set; }

        private BuildSettingsOptions _options { get; }
        public RateService(ISqlDAO sqlDAO, ILogService logService, IMessageBank messageBank, IOptions<BuildSettingsOptions> options)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _messageBank = messageBank;
            _options = options.Value;
        }
        /// <summary>
        ///  
        /// </summary>
        /// <param name="userHash">User hash of user rating</param>
        /// <param name="nodeID">Node id to rate</param>
        /// <param name="rating">Rate (1-5)</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns></returns>
        public async Task<IResponse<NodeRating>> RateNodeAsync(string userHash, long nodeID, int rating, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                NodeRating nodeRating = new NodeRating(userHash, nodeID, rating);

                IResponse<NodeRating> result = await _sqlDAO.RateNodeAsync(nodeRating, cancellationToken);

                return result;

            }
            catch (OperationCanceledException)
            {
                //rollback not necessary
                return new RateResponse<NodeRating>(await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested), new NodeRating(), 408, false);
            }
            catch (Exception ex)
            {
                return new RateResponse<NodeRating>(await _messageBank.GetMessage(IMessageBank.Responses.unhandledException), new NodeRating(), 500, false);
            }
        }


        public async Task<IResponse<double>> GetNodeRatingAsync(long nodeID, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                IResponse<double> result = await _sqlDAO.GetNodeRatingAsync(nodeID, cancellationToken);

                return result;

            }
            catch (OperationCanceledException)
            {
                //rollback not necessary
                return new RateResponse<double>(await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested), 0, 408, false);
            }
            catch (Exception ex)
            {
                return new RateResponse<double>(await _messageBank.GetMessage(IMessageBank.Responses.unhandledException) + ex.Message, 0, 500, false);
            }
        }
    }
}
