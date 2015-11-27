//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using DHTMLX.Scheduler;
//using DHTMLX.Common;
//using DHTMLX.Scheduler.Data;
//using DHTMLX.Scheduler.Controls;

//using Silownia.Models;
//using Silownia.DAL;
//namespace Silownia.Controllers
//{
//    public class CalendarController : Controller
//    {
//        public ActionResult Index()
//        {
//            //Being initialized in that way, scheduler will use CalendarController.
//            //Data as a the datasource and CalendarController.Save to process changes
//            var scheduler = new DHXScheduler(this);

//            scheduler.LoadData = true;
//            scheduler.EnableDataprocessor = true;

//            //
//            scheduler.BeforeInit.Add(string.Format("initResponsive({0})", scheduler.Name));

//            return View(scheduler);
//        }

//        public ContentResult Data()
//        {
//            var data = new SchedulerAjaxData(new SilowniaContext().Treningi);
//            return (ContentResult)data;
//        }

//        public ContentResult Save(int? id, FormCollection actionValues)
//        {
//            var action = new DataAction(actionValues);
//            var data = new SilowniaContext();
            
//            try
//            {
//                var changedEvent = (ZajeciaGrupowe)DHXEventsHelper.Bind(typeof(ZajeciaGrupowe), actionValues);

     

//                switch (action.Type)
//                {
//                    case DataActionTypes.Insert:
//                        //do insert
//                        // action.TargetId = changedEvent.id;//assign postoperational id
//                    data.ZajeciaGrup.InsertOnSubmit(changedEvent);
//                        break;
//                    case DataActionTypes.Delete:
//                      changedEvent = data.ZajeciaGrup.SingleOrDefault(ev => ev.TreningID == action.SourceId);
//                      data.Treningi.DeleteOnSubmit(changedEvent);
//                        //do delete
//                        break;
//                    default:// "update" 
//                        var treningToUpdate = data.Trenerzy.SingleOrDefault(ev => ev.TreningID == action.SourceId);
//                        DHXEventsHelper.Update(treningToUpdate, changedEvent, new List<string>() {""});
//                     //do update
//                        break;
//                }
//                data.SubmitChanges();
//                //action.TargetId = changedEvent.TreningID();

//            }
//            catch
//            {
//                action.Type = DataActionTypes.Error;
//            }
//            return (ContentResult)new AjaxSaveResponse(action);
//        }
//    }
//}

