// SPDX-License-Identifier: MIT
pragma solidity 0.7.4;

contract AccountManagement
{
    struct Wallet {
        uint256 balance;
        bool exists;
    }
    
    mapping(int => Wallet) public wallet;
    
    function addAccount(int _accountId, uint256 _balance) public returns(bool)
    {  
        if(!wallet[_accountId].exists == true)
        {
            wallet[_accountId].balance = _balance;
            wallet[_accountId].exists = true;
            return true;
        }
        else
        {
            return false;
        }
    }
    
    function deposit(int _accountId, uint256 _balance) public returns(bool)
    {
        if(wallet[_accountId].exists == true)
        {
            uint256 bal = wallet[_accountId].balance;
            wallet[_accountId].balance = bal + _balance;
            return true;
        }
        else
        {
            return false;
        }
    }
    
    function withdraw(int _accountId, uint256 _balance) public returns(bool)
    {
        if(wallet[_accountId].exists == true)
        {
            uint256 bal = wallet[_accountId].balance;
            if(bal >= _balance)
            {
                wallet[_accountId].balance = bal - _balance;
                return true;
            }
            else
            {
                return false;    
            }
        }
        else
        {
            return false;
        }
    }
    
    function getBalance(int _accountId) view public returns(uint256)
    {
        return wallet[_accountId].balance;
    }
	
	function transfer(int _fromAccountId, int _toAccountId, uint256 _balance)  public returns(bool)
    {
        if(wallet[_fromAccountId].exists == true && wallet[_toAccountId].exists == true)
        {
            uint256 wbal = wallet[_fromAccountId].balance;
            if(wbal >= _balance)
            {
                wallet[_fromAccountId].balance = wbal - _balance;
                uint256 dbal = wallet[_toAccountId].balance;
                wallet[_toAccountId].balance = dbal + _balance;
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}