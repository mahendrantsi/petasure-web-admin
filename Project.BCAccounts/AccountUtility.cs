using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using SmartPay.BCAccounts.AccountManagement.ContractDefinition;
using SmartPay.BCAccounts.AccountManagement;
using System;
using System.Threading.Tasks;

namespace SmartPay.BCAccounts
{
    public static class AccountUtility
    {
        public static Web3 web3 { get; set; }
        public static Account account { get; set; }
        public static AccountManagementDeployment deployment { get; set; }
        private static string ContractAddress { get; set; }

        static AccountUtility()
        {
            var url = "https://rinkeby.infura.io/v3/4af023be889f4c85b5f25ef2094b42a4";
            var privateKey = "6ada1e3d940cd5582ddba26ecdbfa472c4e588cd9f32a391b777721a87a8db4f";

            account = new Account(privateKey);
            web3 = new Web3(account, url);
            deployment = new AccountManagementDeployment();
            ContractAddress = "0x210a358abe2a4fd0fbd120e89c915d546b19cbde";
        }

        public static async Task<String> Deploy()
        {
            try
            {
                var receipt = await AccountManagementService.DeployContractAndWaitForReceiptAsync(web3, deployment);
                if (receipt.HasErrors() == true)
                {
                    return "Failed";
                }
                else
                {
                    ContractAddress = receipt.ContractAddress;
                    return ContractAddress;
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public static async Task<bool> AddAccount(int AccountId)
        {
            try
            {
                AddAccountFunction objAddAccount = new AddAccountFunction()
                {
                    AccountId = AccountId
                };
                var service = new AccountManagementService(web3, ContractAddress);
                var receiptForAddAccount = await service.AddAccountRequestAndWaitForReceiptAsync(objAddAccount);
                if (receiptForAddAccount.Status.Value == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static async Task<bool> Deposit(long AccountId, Decimal Amount)
        {
            try
            {
                DepositFunction objDeposit = new DepositFunction()
                {
                    AccountId = AccountId,
                    Balance = Web3.Convert.ToWei(Amount)
                };
                var service = new AccountManagementService(web3, ContractAddress);
                var receiptForDepositRequest = await service.DepositRequestAndWaitForReceiptAsync(objDeposit);
                if (receiptForDepositRequest.Status.Value == 1)
                {
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static async Task<decimal> Balance(long AccountId)
        {
            try
            {
                GetBalanceFunction objGetBalance = new GetBalanceFunction()
                {
                    AccountId = AccountId,
                };
                var service = new AccountManagementService(web3, ContractAddress);
                var Balance = await service.GetBalanceQueryAsync(objGetBalance);
                return Web3.Convert.FromWei(Balance);

            }
            catch (Exception ex)
            {
                return Web3.Convert.FromWei(0);
            }
        }

        public static async Task<bool> WithDraw(long AccountId, Decimal Amount)
        {
            try
            {
                WithdrawFunction objWithdraw = new WithdrawFunction()
                {
                    AccountId = AccountId,
                    Balance = Web3.Convert.ToWei(Amount)
                };
                var service = new AccountManagementService(web3, ContractAddress);
                var receiptForWithdrawRequest = await service.WithdrawRequestAndWaitForReceiptAsync(objWithdraw);
                if (receiptForWithdrawRequest.Status.Value == 1)
                {
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static async Task<bool> Transfer(long fromAccount, long toAccount, Decimal Amount)
        {
            try
            {
                TransferFunction objTransfer = new TransferFunction()
                {
                    FromAccountId = fromAccount,
                    ToAccountId = toAccount,
                    Balance = Web3.Convert.ToWei(Amount)
                };
                var service = new AccountManagementService(web3, ContractAddress);
                var receiptForTransferRequest = await service.TransferRequestAndWaitForReceiptAsync(objTransfer);
                if (receiptForTransferRequest.Status.Value == 1)
                {
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
