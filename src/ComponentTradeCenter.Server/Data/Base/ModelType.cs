using System.ComponentModel.DataAnnotations;

namespace ComponentTradeCenter.Server.Data.Base
{
    public enum ModelType
    {
        [Display(Name = "顾客")]
        Customer,

        [Display(Name = "供应商")]
        Supplier,

        [Display(Name = "交易员")]
        Trader,

        [Display(Name = "管理员")]
        Administrator,

        [Display(Name = "零件")]
        Component,

        [Display(Name = "收购")]
        Need,

        [Display(Name = "出售")]
        Supply,

        [Display(Name = "交易建议")]
        TradeSuggest,

        [Display(Name = "交易协议")]
        Agreement,

        [Display(Name = "交易记录")]
        Trade,
    }
}
