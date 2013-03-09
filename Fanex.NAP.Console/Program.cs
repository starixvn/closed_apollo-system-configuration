using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.NAP.Core.Repository.Impl;
using Fanex.Data;
using Fanex.Data.Configuration;
using Fanex.NAP.Core.Model;
using System.Collections;
using System.Data;
using Fanex.NAP.Core.Service.Impl;
using Fanex.NAP.Core.Repository.Interface;
using Fanex.NAP.Core.Criteria.Site;
using IBCUtils;
using Fanex.Common;
namespace Fanex.NAP.Console
{
    class Program
    {

        public static void TestList(List<string> list)
        {
            list.Add("A");
        }

        static void Main(string[] args)
        {

            List<string> list111 = new List<string>();
            TestList(list111);
            
            
            
            
            
            
            
            
            
            
            
            
            var date = DateTime.Now.ToString("G");
            //var time1 = new TimeSpan(date.Day,date.Hour, date.Minute, date.Second);



            var encrys = IBCUtils.Encryption.CfsEnCode(date);

            //var temp = Cryptography.CfsEnCrypt("8");
            var temp1 = Cryptography.CfsEnCrypt("@tiger#");










            DbSettingProviderManager.Current.Start();

            UserService clsUserService = new UserService();

            clsUserService.ServiceContext = new ServiceContext
            {
                UserId = 7771656,
                SiteId = 5,             
                Ip = "127.0.0.1"
            };


            var user1 = clsUserService.GetById(7771656);

            var user2 = clsUserService.GetByUsername("AT2588002");

            var list = clsUserService.GetAll();

            var list1 = clsUserService.GetBySite(4,false);

            var list2 = clsUserService.GetByGroupAndSite(1,4);

            var list3 = clsUserService.SearchUser("A",1,4);

            var level = clsUserService.GetUserSiteLevel(7771656, 5);

            clsUserService.ChangeUserSiteStatus(7771656, 4, false);
            //GroupRepository clsGroupRepository = new GroupRepository();
            //SiteRepository siteRepository = new SiteRepository();

            //FunctionRepository functionRepository = new FunctionRepository();

            //Function func = (Function)functionRepository.Get(1);

            FunctionService funcService = new FunctionService();

            ////Function func2 = (Function)funcService.GetByNameAndSite("Login", 5);

            ////Function func3 = (Function)funcService.GetById(1);

            //var funcList = funcService.GetAll();

            //var funcDic = funcService.GetAllAndGroupBySite();

            //var funcPolicyList = funcService.GetByUser(1);

            //var funcPolicyDic = funcService.GetByUserAndGroupBySite(1);

            var funcPolicyList = funcService.GetByUserAndSite(1, 5);

            var funcPolicyList2 = funcService.GetByUserAndSiteIncludeGroup(1, 5);


            //SiteRepository siteRepository = new SiteRepository();Repository siteRepository = new SiteRepository();

            //Site site = (Site)siteRepository.Get(4);

            SiteService siteService = new SiteService();

            //Site site = (Site)siteService.GetById(4);

            //Site site1 = (Site)siteService.GetByName("Site test 1");

            //Site site2 = (Site)siteService.GetByName("abc");

            //var siteListAll = siteService.GetAll();

            //var siteList1 = siteService.GetAll("UAT");

            //var siteList2 = siteService.GetAll("LIVE");

            //var siteList3 = siteService.GetByUser(7771656);

            //var criteria = new InsertSiteCriteria
            //{
            //    //SiteId = 7,
            //    SiteName = "Settlement 2",
            //    Description = "ABC",
            //    Url = "/Settlement",
            //    IsActive = true,
            //    Environment = "UAT"
            //};

            //var criteria = new UpdateSiteCriteria
            //{
            //    SiteId = 10,
            //    SiteName = "10 nghin USD",
            //    Description = "ABC10",
            //    Url = "/10kUSD",
            //    IsActive = true,
            //    Environment = "UAT"
            //};

            //var criteria = new UpdateStatusSiteCriteria
            //{
            //    SiteId = 10,
            //    IsActive = false
            //};
            
            //siteService.ServiceContext = new ServiceContext
            //{
            //    UserId = 7771656,
            //    SiteId = 5,
            //    FunctionId = 10,
            //    Ip = "127.0.0.1"
            //};

            //var result = siteService.Insert(criteria);

            //var result = siteService.Update(criteria);

            //var result = siteService.UpdateStatus(criteria);

            //var result = siteService.Update(newSite);
            ////Group objGroup = (Group)clsGroupRepository.Get(1);

        //    ISite GetById(int id);

        //ISite GetByName(string name);

        //IEnumerable<SiteLevel> GetByUser(int userId, string env = Constant.UAT, bool? isActive = null);

        //IEnumerable<ISite> GetAll(string env = Constant.UAT, bool? isActive = null);

        //ICommandResult UpdateStatus(ISite entity, IRepositoryExecLog log);

        //ICommandResult Insert(ISite entity, IRepositoryExecLog log);

        //ICommandResult Update(ISite entity, IRepositoryExecLog log);



            ////Hashtable table = new Hashtable();
            ////table.Add("groupid", 1);
            ////using (ObjectDb db = new ObjectDb("NAP_Group_GetById"))
            ////{
            ////    var list = db.Query<dynamic>(null, table);
            ////}

            //Group objGroup1 = (Group)clsGroupRepository.Single(new { SettingKey = "NAP_Group_GetById", GroupID = 1 });


            //IGroupRepository igroup = new GroupRepository();


            //Group objGroup2 = (Group)igroup.Single(new { SettingKey = "NAP_Group_GetById", GroupID = 1 });



            //GroupService groupService = new GroupService(igroup);

            //Group objGroup3 = (Group)groupService.GetByUser(7771656);

            //Group objGroup4 = (Group)groupService.GetById(1);

            //Group objGroup5 = (Group)groupService.GetByName("administrator 1");
        }
    }

    //public class 
}
