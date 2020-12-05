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
        public const string GetKeyPass = @"SELECT Code FROM [Ski2Db_2015-2016].[dbo].[AccountStock] WHERE code like @key";
    }
}
