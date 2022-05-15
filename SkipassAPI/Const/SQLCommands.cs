﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkipassAPI.Const
{
    public static class SQLCommands
    {
        public const string GetBalance = @"SELECT 
                                 SuperAccount.SuperAccountId as SuperAccountId
                               
                                ,f1.code as CardCode
                               
                               
                                ,f2.Amount
                               


FROM  (select Amount,code,Name,CategoryId,SuperAccountId,StockType,AccountStockId from AccountStock where StockType = 21) f1 
        full outer join 
       (select Amount,code,Name,CategoryId,SuperAccountId,StockType,AccountStockId from AccountStock where StockType = 1) f2
        on f1.SuperAccountId = f2.SuperAccountId


	INNER JOIN SuperAccount ON f1.SuperAccountId = SuperAccount.SuperAccountId
	INNER JOIN Category ON f1.CategoryId = Category.CategoryId
    LEFT JOIN ClientCategory ON SuperAccount.ClientCategoryId = ClientCategory.ClientCategoryId
	INNER JOIN (
			SELECT
				SuperAccountTo
			FROM
				MasterTransaction 
			GROUP BY
				MasterTransaction.SuperAccountTo
		) MasterTransaction ON f1.SuperAccountId = MasterTransaction.SuperAccountTo
	WHERE
	f1.StockType = 21 and SuperAccount.Type = 0 and f1.Code=@key";

        public const string GetKeyPass = @"SELECT Code FROM [AccountStock] WHERE code like @key";
        public const string GetKeyPassAndName = @"SELECT a.Code, i.LastName, i.FirstName, i.SecondName FROM Fwp.dbo.AccountStock as a inner join Fwp.dbo.PersonalInfo as i on i.SuperAccountId=a.SuperAccountId WHERE a.Code like @key";

        public const string UpdateBalance = @"update [AccountStock]
    set Amount=@add_sum+(select Amount from AccountStock where StockType=1 and SuperAccountId=(select SuperAccountId from AccountStock where StockType=21 and Code=@key))
    where StockType=1 and SuperAccountId=(select SuperAccountId from AccountStock where StockType=21 and Code=@key)
    select Amount from AccountStock where StockType = 1 and SuperAccountId=(select SuperAccountId from AccountStock where StockType=21 and Code=@key)";
        public const string AddAccountStock = @"insert into Fwp.dbo.AccountStock (AccountStockId,SuperAccountId,StockType,IsActive,CategoryId,Amount,Start,[End],IsTimePatternApplied,PassesDone) 
    values (((select MAX(AccountStockId) from Fwp.dbo.AccountStock)+1), (select SuperAccountId from Fwp.dbo.AccountStock where StockType=21 and Code=@key), 6, 1, @catId, @amount, @date_start, @date_end, 1, 0);
    insert into Fwp.dbo.MasterTransaction (TransTime,SuperAccountFrom,UserId,ServicePointId,ServerTime,IsOffline,CheckDetailId,Machine,SecSubjectId)
    values (GETDATE(),3,'CASHDESK2@admin',3,GETDATE(),0,157862,'CASHDESK2@Bars.CashDesk',12);
    insert into Fwp.dbo.TransactionDetail (MasterTransactionId,StockInfoIdFrom,StockInfoIdTo,Amount,SuperAccountIdFrom, SuperAccountIdTo)
    values ((select MAX(MasterTransactionId) from Fwp.dbo.MasterTransaction),(select MAX(AccountStockId) from Fwp.dbo.AccountStock),(select MAX(AccountStockId) from Fwp.dbo.AccountStock),@amount,3,(select SuperAccountId from Fwp.dbo.AccountStock where StockType=21 and Code=@key));
    UPDATE [Fwp].[dbo].[SuperAccount] SET [LastTransactionTime] = GETDATE() WHERE [SuperAccountId] = 3;
    UPDATE [Fwp].[dbo].[SuperAccount] SET [LastTransactionTime] = GETDATE() WHERE [SuperAccountId] = (select SuperAccountId from Fwp.dbo.AccountStock where StockType=21 and Code=@key);
    INSERT INTO [Fwp].[dbo].[GlobalId] ([StockType]) VALUES (0);
    INSERT INTO [Fwp].[dbo].[GlobalId] ([StockType]) VALUES (0);";

        public const string GetAllServices = @"SELECT [CategoryId]
      ,[StockType]
      ,[Name]
  FROM [Fwp].[dbo].[Category]";

        public const string UpdateEmail = @"update pri set Email=@email, Phone=@phone
    from PersonalInfo pri
    inner join AccountStock acs on pri.SuperAccountId=acs.SuperAccountId
    where code=@key
    select Email, Phone
    from PersonalInfo pri
    inner join AccountStock acs on pri.SuperAccountId=acs.SuperAccountId
    where code=@key";

        public const string CheckEmail = @"select Email, Phone
    from PersonalInfo pri
    inner join AccountStock acs on pri.SuperAccountId=acs.SuperAccountId
    where code=@key";

        public const string SaveHistory = @"insert into internet_payments
values(@key,@add_sum,@successed,@email,@phone,@payment_id,@payment_system,@payment_source, @comment)";
    }
}
