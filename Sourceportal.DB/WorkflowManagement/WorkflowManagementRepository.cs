using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Newtonsoft.Json;
using Sourceportal.Domain.Models.API.Requests.WorkflowManagement;
using Sourceportal.Domain.Models.DB.WorkflowManagement;
using Sourceportal.Utilities;

namespace Sourceportal.DB.WorkflowManagement
{
    public class WorkflowManagementRepository : IWorkflowManagementRepository
    {
        private static readonly string ConnectionString = ConfigurationManager
            .ConnectionStrings["SourcePortalConnection"].ConnectionString;

        public IList<ConditionsDb> StateEngineConditionsGet(int objectTypeId)
        {
            List<ConditionsDb> ConditionDb;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@ObjectTypeID", objectTypeId);
                ConditionDb = con.Query<ConditionsDb>("uspStateEngineConditionsGet", param,
                    commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return ConditionDb;
        }

        public IList<RuleGroupDb> StateEngineRuleGroupsGet(int ruleId)
        {
            List<RuleGroupDb> RuleGroupDb;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@RuleID", ruleId);
                RuleGroupDb = con.Query<RuleGroupDb>("uspStateEngineRuleGroupsGet", param,
                    commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return RuleGroupDb;
        }

        public IList<StateEngineTriggerDb> StateEngineTriggersGet(int objectTypeId)
        {
            List<StateEngineTriggerDb> stateEngineTriggerDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@ObjectTypeID", objectTypeId);
                stateEngineTriggerDbs = con.Query<StateEngineTriggerDb>("uspStateEngineTriggersGet", param,
                    commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return stateEngineTriggerDbs;
        }

        public IList<StateEngineRuleDb> StateEngineRulesGet(int objectTypeId)
        {
            List<StateEngineRuleDb> stateEngineRuleDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@ObjectTypeID", objectTypeId);
                stateEngineRuleDbs = con.Query<StateEngineRuleDb>("uspStateEngineRulesGet", param,
                    commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return stateEngineRuleDbs;
        }

        public int StateEngineRuleGroupSet(RuleGroupRequest ruleGroup, int? parentGroupId, int ruleId)
        {
            int ruleGroupId;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@RuleGroupID", ruleGroup.RuleGroupID);
                param.Add("@RuleID", ruleId);
                param.Add("@ParentGroupID", parentGroupId ?? ruleGroup.ParentGroupID);
                param.Add("@IsAll", ruleGroup.IsAll);
                param.Add("@UserID", UserHelper.GetUserId());
                param.Add("@RuleConditionsJSON", JsonConvert.SerializeObject(ruleGroup.Conditions));
                ruleGroupId = con.Query<int>("uspStateEngineRuleGroupSet", param,
                    commandType: CommandType.StoredProcedure).First();
                con.Close();
            }
            return ruleGroupId;
        }

        public int StateEngineRuleDetailSet(RuleDetailSetRequest ruleDetailSetRequest)
        {
            int ruleId;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@RuleID", ruleDetailSetRequest.RuleID);
                param.Add("@TriggerID", ruleDetailSetRequest.TriggerID);
                param.Add("@RuleOrder", ruleDetailSetRequest.RuleOrder);
                param.Add("@RuleName", ruleDetailSetRequest.RuleName);
                param.Add("@ObjectTypeID", ruleDetailSetRequest.ObjectTypeID);
                param.Add("@IsDeleted", ruleDetailSetRequest.IsDeleted);
                param.Add("@UserID", UserHelper.GetUserId());
                ruleId = con.Query<int>("uspStateEngineRuleDetailSet", param,
                    commandType: CommandType.StoredProcedure).First();
                con.Close();
            }
            return ruleId;
        }

        public IList<RuleActionDb> StateEngineRuleActionsGet(int ruleId)
        {
            List<RuleActionDb> ruleActionDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@RuleID", ruleId);
                ruleActionDbs = con.Query<RuleActionDb>("uspStateEngineRuleActionsGet", param,
                    commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return ruleActionDbs;
        }

        public IList<ActionDb> StateEngineActionListGet(int objectTypeId)
        {
            List<ActionDb> actionDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@ObjectTypeID", objectTypeId);
                actionDbs = con.Query<ActionDb>("uspStateEngineActionListGet", param,
                    commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return actionDbs;
        }

        public int StateEngineRuleActionsSet(List<RuleActionRequest> ruleActionRequests, int ruleId)
        {
            int rowcount;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@RuleID", ruleId);
                param.Add("@UserID", UserHelper.GetUserId());
                param.Add("@RuleActionsJSON", JsonConvert.SerializeObject(ruleActionRequests));
                rowcount = con.Query<int>("mapStateEngineRuleActionsSet", param,
                    commandType: CommandType.StoredProcedure).First();
                con.Close();
            }
            return rowcount;
        }
    }
}
