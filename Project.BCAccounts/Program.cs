using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using SmartPay.BCAccounts.AccountManagement.ContractDefinition;
using SmartPay.BCAccounts.AccountManagement;
using System;
using System.Threading.Tasks;

namespace SmartPay.BCAccounts
{
    public class Program
    {
        static void Main(string[] args)
        {
            //Demo().Wait();

            //AccountUtility.Deploy().Wait();
            //AccountUtility.Balance();
        }

        static async Task Demo()
        {
            try
            {
                // Setup
                // Here we're using local chain eg Geth https://github.com/Nethereum/TestChains#geth
                var url = "https://rinkeby.infura.io/v3/4af023be889f4c85b5f25ef2094b42a4";
                //var privateKey = "0xb5b1870957d373ef0eeffecc6e4812c0fd08f554b37b233526acc331bf1544f7";
                var privateKey = "6ada1e3d940cd5582ddba26ecdbfa472c4e588cd9f32a391b777721a87a8db4f";
                
                var account = new Account(privateKey);
                var web3 = new Web3(account, url);

                ////Code for deploy////
                Console.WriteLine("Deploying...");
                var deployment = new AccountManagementDeployment();
                var receipt = await AccountManagementService.DeployContractAndWaitForReceiptAsync(web3, deployment);
                var service = new AccountManagementService(web3, receipt.ContractAddress);
                Console.WriteLine($"Contract Deployment Tx Status: {receipt.Status.Value}");
                Console.WriteLine($"Contract Address: {service.ContractHandler.ContractAddress}");
                Console.WriteLine("");
                ////Code for deploy////

                ////Code for AddAccount////
                AddAccountFunction objAddAccount = new AddAccountFunction()
                {
                    AccountId = 1,
                    Balance = Web3.Convert.ToWei(5)
                };
                var receiptForAddAccount = await service.AddAccountRequestAndWaitForReceiptAsync(objAddAccount);
                Console.WriteLine($"Finished storing an int: Tx Hash: {receiptForAddAccount.TransactionHash}");
                Console.WriteLine($"Finished storing an int: Tx Status: {receiptForAddAccount.Status.Value}");
                Console.WriteLine("");
                ////Code for AddAccount////


                ////Code for Deposit////
                Console.WriteLine("Calling the function get()...");
                DepositFunction objDeposit = new DepositFunction()
                {
                    AccountId = 1,
                    Balance = Web3.Convert.ToWei(2)
                };
                var receiptForDepositRequest = await service.DepositRequestAndWaitForReceiptAsync(objDeposit);
                Console.WriteLine($"Finished storing an int: Tx Hash: {receiptForDepositRequest.TransactionHash}");
                Console.WriteLine($"Finished storing an int: Tx Status: {receiptForDepositRequest.Status.Value}");
                Console.WriteLine("");
                ////Code for Deposit////

                ////Code for GetBalance////
                GetBalanceFunction objGetBalance = new GetBalanceFunction()
                {
                    AccountId = 1,
                };
                var getBalance = await service.GetBalanceQueryAsync(objGetBalance);
                decimal balance = Web3.Convert.FromWei(getBalance);
                Console.WriteLine($"Balance is: {balance}");
                Console.WriteLine("");
                ////Code for GetBalance////

                ////Code for Withdraw////
                WithdrawFunction objWithdraw = new WithdrawFunction()
                {
                    AccountId = 1,
                    Balance = 2
                };
                var receiptForWithdrawRequest = await service.WithdrawRequestAndWaitForReceiptAsync(objWithdraw);
                Console.WriteLine($"Finished storing an int: Tx Hash: {receiptForWithdrawRequest.TransactionHash}");
                Console.WriteLine($"Finished storing an int: Tx Status: {receiptForWithdrawRequest.Status.Value}");
                Console.WriteLine("");
                ////Code for Withdraw////

                ////Code for Withdraw////
                TransferFunction objTransfer = new TransferFunction()
                {
                    FromAccountId = 1,
                    ToAccountId = 2,
                    Balance = 2
                };
                var receiptForTransferRequest = await service.TransferRequestAndWaitForReceiptAsync(objTransfer);
                Console.WriteLine($"Finished storing an int: Tx Hash: {receiptForTransferRequest.TransactionHash}");
                Console.WriteLine($"Finished storing an int: Tx Status: {receiptForTransferRequest.Status.Value}");
                Console.WriteLine("");
                ////Code for Withdraw////

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.WriteLine("Finished");
            Console.ReadLine();
        }
    }
}