using System;
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

        public const string UpdateBalance = @"update [AccountStock]
    set Amount=@add_sum+(select Amount from AccountStock where StockType=1 and SuperAccountId=(select SuperAccountId from AccountStock where StockType=21 and Code=@key))
    where StockType=1 and SuperAccountId=(select SuperAccountId from AccountStock where StockType=21 and Code=@key)
    select Amount from AccountStock where StockType = 1 and SuperAccountId=(select SuperAccountId from AccountStock where StockType=21 and Code=@key)";

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
