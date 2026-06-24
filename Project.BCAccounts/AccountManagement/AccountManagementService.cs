using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Contracts;
using System.Threading;
using SmartPay.BCAccounts.AccountManagement.ContractDefinition;

namespace SmartPay.BCAccounts.AccountManagement
{
    public partial class AccountManagementService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, AccountManagementDeployment accountManagementDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<AccountManagementDeployment>().SendRequestAndWaitForReceiptAsync(accountManagementDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, AccountManagementDeployment accountManagementDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<AccountManagementDeployment>().SendRequestAsync(accountManagementDeployment);
        }

        public static async Task<AccountManagementService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, AccountManagementDeployment accountManagementDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, accountManagementDeployment, cancellationTokenSource);
            return new AccountManagementService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public AccountManagementService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<string> AddAccountRequestAsync(AddAccountFunction addAccountFunction)
        {
             return ContractHandler.SendRequestAsync(addAccountFunction);
        }

        public Task<TransactionReceipt> AddAccountRequestAndWaitForReceiptAsync(AddAccountFunction addAccountFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(addAccountFunction, cancellationToken);
        }

        public Task<string> AddAccountRequestAsync(BigInteger accountId, BigInteger balance)
        {
            var addAccountFunction = new AddAccountFunction();
                addAccountFunction.AccountId = accountId;
                addAccountFunction.Balance = balance;
            
             return ContractHandler.SendRequestAsync(addAccountFunction);
        }

        public Task<TransactionReceipt> AddAccountRequestAndWaitForReceiptAsync(BigInteger accountId, BigInteger balance, CancellationTokenSource cancellationToken = null)
        {
            var addAccountFunction = new AddAccountFunction();
                addAccountFunction.AccountId = accountId;
                addAccountFunction.Balance = balance;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(addAccountFunction, cancellationToken);
        }

        public Task<string> DepositRequestAsync(DepositFunction depositFunction)
        {
             return ContractHandler.SendRequestAsync(depositFunction);
        }

        public Task<TransactionReceipt> DepositRequestAndWaitForReceiptAsync(DepositFunction depositFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(depositFunction, cancellationToken);
        }

        public Task<string> DepositRequestAsync(BigInteger accountId, BigInteger balance)
        {
            var depositFunction = new DepositFunction();
                depositFunction.AccountId = accountId;
                depositFunction.Balance = balance;
            
             return ContractHandler.SendRequestAsync(depositFunction);
        }

        public Task<TransactionReceipt> DepositRequestAndWaitForReceiptAsync(BigInteger accountId, BigInteger balance, CancellationTokenSource cancellationToken = null)
        {
            var depositFunction = new DepositFunction();
                depositFunction.AccountId = accountId;
                depositFunction.Balance = balance;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(depositFunction, cancellationToken);
        }

        public Task<BigInteger> GetBalanceQueryAsync(GetBalanceFunction getBalanceFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetBalanceFunction, BigInteger>(getBalanceFunction, blockParameter);
        }

        
        public Task<BigInteger> GetBalanceQueryAsync(BigInteger accountId, BlockParameter blockParameter = null)
        {
            var getBalanceFunction = new GetBalanceFunction();
                getBalanceFunction.AccountId = accountId;
            
            return ContractHandler.QueryAsync<GetBalanceFunction, BigInteger>(getBalanceFunction, blockParameter);
        }

        public Task<string> TransferRequestAsync(TransferFunction transferFunction)
        {
             return ContractHandler.SendRequestAsync(transferFunction);
        }

        public Task<TransactionReceipt> TransferRequestAndWaitForReceiptAsync(TransferFunction transferFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferFunction, cancellationToken);
        }

        public Task<string> TransferRequestAsync(BigInteger fromAccountId, BigInteger toAccountId, BigInteger balance)
        {
            var transferFunction = new TransferFunction();
                transferFunction.FromAccountId = fromAccountId;
                transferFunction.ToAccountId = toAccountId;
                transferFunction.Balance = balance;
            
             return ContractHandler.SendRequestAsync(transferFunction);
        }

        public Task<TransactionReceipt> TransferRequestAndWaitForReceiptAsync(BigInteger fromAccountId, BigInteger toAccountId, BigInteger balance, CancellationTokenSource cancellationToken = null)
        {
            var transferFunction = new TransferFunction();
                transferFunction.FromAccountId = fromAccountId;
                transferFunction.ToAccountId = toAccountId;
                transferFunction.Balance = balance;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferFunction, cancellationToken);
        }

        public Task<WalletOutputDTO> WalletQueryAsync(WalletFunction walletFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<WalletFunction, WalletOutputDTO>(walletFunction, blockParameter);
        }

        public Task<WalletOutputDTO> WalletQueryAsync(BigInteger returnValue1, BlockParameter blockParameter = null)
        {
            var walletFunction = new WalletFunction();
                walletFunction.ReturnValue1 = returnValue1;
            
            return ContractHandler.QueryDeserializingToObjectAsync<WalletFunction, WalletOutputDTO>(walletFunction, blockParameter);
        }

        public Task<string> WithdrawRequestAsync(WithdrawFunction withdrawFunction)
        {
             return ContractHandler.SendRequestAsync(withdrawFunction);
        }

        public Task<TransactionReceipt> WithdrawRequestAndWaitForReceiptAsync(WithdrawFunction withdrawFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(withdrawFunction, cancellationToken);
        }

        public Task<string> WithdrawRequestAsync(BigInteger accountId, BigInteger balance)
        {
            var withdrawFunction = new WithdrawFunction();
                withdrawFunction.AccountId = accountId;
                withdrawFunction.Balance = balance;
            
             return ContractHandler.SendRequestAsync(withdrawFunction);
        }

        public Task<TransactionReceipt> WithdrawRequestAndWaitForReceiptAsync(BigInteger accountId, BigInteger balance, CancellationTokenSource cancellationToken = null)
        {
            var withdrawFunction = new WithdrawFunction();
                withdrawFunction.AccountId = accountId;
                withdrawFunction.Balance = balance;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(withdrawFunction, cancellationToken);
        }
    }
}
