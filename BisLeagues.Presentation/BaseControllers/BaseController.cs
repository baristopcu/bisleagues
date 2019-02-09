using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Core.Models;
using BisLeagues.Presentation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BisLeagues.Presentation.BaseControllers
{
    public abstract partial class BaseController<T> : Controller where T : BaseController<T>
    {
       // private readonly IPlayerRepository _playerRepository;

        public BaseController(/*IPlayerRepository playerRepository*/)
        {
            //_playerRepository = playerRepository;
        }

        /* @MessageCode 
*  0 : Error
*  1 : Success
*  2 : Warning
*  3 : Info
* */

        public int MessageCode
        {
            get { return TempData["MessageCode"] == null ? -1 : Convert.ToInt16(TempData["MessageCode"].ToString()); }
            set { TempData["MessageCode"] = value; }
        }

        /* @Message
         * Message's itself
         * */

        public String Message
        {
            get { return TempData["Message"] == null ? String.Empty : TempData["Message"].ToString(); }
            set { TempData["Message"] = value; }
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //Player player = _playerRepository.Get(1);
            if (context.Controller is Controller controller)
            {
                //controller.ViewBag.CompanyName = player.Name.ToString();
            }
            base.OnActionExecuting(context);
        }
    }
}
