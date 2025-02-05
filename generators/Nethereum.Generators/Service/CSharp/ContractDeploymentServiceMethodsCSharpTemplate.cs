using Nethereum.Generators.Core;
using Nethereum.Generators.CQS;
using System;

namespace Nethereum.Generators.Service
{
    public class ContractDeploymentServiceMethodsCSharpTemplate
    {
        private ContractDeploymentCQSMessageModel _contractDeploymentCQSMessageModel;
        private ServiceModel _serviceModel;
        private static readonly string SpaceFollowingFunction = (Environment.NewLine + Environment.NewLine);

        public ContractDeploymentServiceMethodsCSharpTemplate(ServiceModel model)
        {
            _contractDeploymentCQSMessageModel = model.ContractDeploymentCQSMessageModel;
            _serviceModel = model;
        }

        public string GenerateMethods()
        {
            var messageType = _contractDeploymentCQSMessageModel.GetTypeName();
            var messageVariableName =
                _contractDeploymentCQSMessageModel.GetVariableName();

            var sendRequestReceipt = 
            $@"{SpaceUtils.TwoTabs}public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.IWeb3 web3, {messageType} {messageVariableName}, CancellationTokenSource cancellationTokenSource = null)
{SpaceUtils.TwoTabs}{{
{SpaceUtils.ThreeTabs}return web3.Eth.GetContractDeploymentHandler<{messageType}>().SendRequestAndWaitForReceiptAsync({messageVariableName}, cancellationTokenSource);
{SpaceUtils.TwoTabs}}}";

            var sendRequest =
                $@"{SpaceUtils.TwoTabs}public static Task<string> DeployContractAsync(Nethereum.Web3.IWeb3 web3, {messageType} {messageVariableName})
{SpaceUtils.TwoTabs}{{
{SpaceUtils.ThreeTabs}return web3.Eth.GetContractDeploymentHandler<{messageType}>().SendRequestAsync({messageVariableName});
{SpaceUtils.TwoTabs}}}";

            var sendRequestContract =
                $@"{SpaceUtils.TwoTabs}public static async Task<{_serviceModel.GetTypeName()}> DeployContractAndGetServiceAsync(Nethereum.Web3.IWeb3 web3, {messageType} {messageVariableName}, CancellationTokenSource cancellationTokenSource = null)
{SpaceUtils.TwoTabs}{{
{SpaceUtils.ThreeTabs}var receipt = await DeployContractAndWaitForReceiptAsync(web3, {messageVariableName}, cancellationTokenSource);
{SpaceUtils.ThreeTabs}return new {_serviceModel.GetTypeName()}(web3, receipt.ContractAddress);
{SpaceUtils.TwoTabs}}}";

            return string.Join(SpaceFollowingFunction, sendRequestReceipt, sendRequest, sendRequestContract);
        }
    }
}