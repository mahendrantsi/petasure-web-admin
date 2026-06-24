using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts;
using System.Threading;

namespace SmartPay.BCAccounts.AccountManagement.ContractDefinition
{


    public partial class AccountManagementDeployment : AccountManagementDeploymentBase
    {
        public AccountManagementDeployment() : base(BYTECODE) { }
        public AccountManagementDeployment(string byteCode) : base(byteCode) { }
    }

    public class AccountManagementDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "608060405234801561001057600080fd5b5061034e806100206000396000f3fe608060405234801561001057600080fd5b50600436106100625760003560e01c80635aef6b9b146100675780635e44342d1461009d57806393de9b09146100d4578063d2bff17a146100f7578063da0a75c814610120578063dbfc6b201461014f575b600080fd5b6100846004803603602081101561007d57600080fd5b5035610172565b6040805192835290151560208301528051918290030190f35b6100c0600480360360408110156100b357600080fd5b508035906020013561018e565b604080519115158252519081900360200190f35b6100c0600480360360408110156100ea57600080fd5b50803590602001356101dc565b6100c06004803603606081101561010d57600080fd5b5080359060208101359060400135610237565b61013d6004803603602081101561013657600080fd5b50356102cb565b60408051918252519081900360200190f35b6100c06004803603604081101561016557600080fd5b50803590602001356102dd565b6000602081905290815260409020805460019091015460ff1682565b600082815260208190526040812060019081015460ff161514156101d2575060008281526020819052604090208181556001908101805460ff1916821790556101d6565b5060005b92915050565b600082815260208190526040812060019081015460ff16151514156101d25760008381526020819052604090205482811061022d5760008481526020819052604090209083900390555060016101d6565b60009150506101d6565b600083815260208190526040812060019081015460ff1615151480156102735750600083815260208190526040902060019081015460ff161515145b156102c0576000848152602081905260409020548281106102b65760008581526020819052604080822092859003909255848152208054830190555060016102c4565b60009150506102c4565b5060005b9392505050565b60009081526020819052604090205490565b600082815260208190526040812060019081015460ff16151514156101d25750600082815260208190526040902080548201905560016101d656fea264697066735822122079d14836246fd6e5768e61295587e147828cdd7ee2c97b3aa594906bcc0269cf64736f6c63430007040033";
        public AccountManagementDeploymentBase() : base(BYTECODE) { }
        public AccountManagementDeploymentBase(string byteCode) : base(byteCode) { }

    }

    public partial class AddAccountFunction : AddAccountFunctionBase { }

    [Function("addAccount", "bool")]
    public class AddAccountFunctionBase : FunctionMessage
    {
        [Parameter("int256", "_accountId", 1)]
        public virtual BigInteger AccountId { get; set; }
        [Parameter("uint256", "_balance", 2)]
        public virtual BigInteger Balance { get; set; }
    }

    public partial class DepositFunction : DepositFunctionBase { }

    [Function("deposit", "bool")]
    public class DepositFunctionBase : FunctionMessage
    {
        [Parameter("int256", "_accountId", 1)]
        public virtual BigInteger AccountId { get; set; }
        [Parameter("uint256", "_balance", 2)]
        public virtual BigInteger Balance { get; set; }
    }

    public partial class GetBalanceFunction : GetBalanceFunctionBase { }

    [Function("getBalance", "uint256")]
    public class GetBalanceFunctionBase : FunctionMessage
    {
        [Parameter("int256", "_accountId", 1)]
        public virtual BigInteger AccountId { get; set; }
    }

    public partial class TransferFunction : TransferFunctionBase { }

    [Function("transfer", "bool")]
    public class TransferFunctionBase : FunctionMessage
    {
        [Parameter("int256", "_fromAccountId", 1)]
        public virtual BigInteger FromAccountId { get; set; }
        [Parameter("int256", "_toAccountId", 2)]
        public virtual BigInteger ToAccountId { get; set; }
        [Parameter("uint256", "_balance", 3)]
        public virtual BigInteger Balance { get; set; }
    }

    public partial class WalletFunction : WalletFunctionBase { }

    [Function("wallet", typeof(WalletOutputDTO))]
    public class WalletFunctionBase : FunctionMessage
    {
        [Parameter("int256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class WithdrawFunction : WithdrawFunctionBase { }

    [Function("withdraw", "bool")]
    public class WithdrawFunctionBase : FunctionMessage
    {
        [Parameter("int256", "_accountId", 1)]
        public virtual BigInteger AccountId { get; set; }
        [Parameter("uint256", "_balance", 2)]
        public virtual BigInteger Balance { get; set; }
    }





    public partial class GetBalanceOutputDTO : GetBalanceOutputDTOBase { }

    [FunctionOutput]
    public class GetBalanceOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }



    public partial class WalletOutputDTO : WalletOutputDTOBase { }

    [FunctionOutput]
    public class WalletOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "balance", 1)]
        public virtual BigInteger Balance { get; set; }
        [Parameter("bool", "exists", 2)]
        public virtual bool Exists { get; set; }
    }


}
