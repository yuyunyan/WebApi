using System.Collections.Generic;
using Sourceportal.Domain.Models.API.Requests.WorkflowManagement;
using Sourceportal.Domain.Models.DB.WorkflowManagement;

namespace Sourceportal.DB.WorkflowManagement
{
    public interface IWorkflowManagementRepository
    {
        IList<ConditionsDb> StateEngineConditionsGet(int objectTypeId);
        IList<RuleGroupDb> StateEngineRuleGroupsGet(int ruleId);
        IList<StateEngineTriggerDb> StateEngineTriggersGet(int objectTypeId);
        IList<StateEngineRuleDb> StateEngineRulesGet(int objectTypeId);
        int StateEngineRuleDetailSet(RuleDetailSetRequest ruleDetailSetRequest);
        int StateEngineRuleGroupSet(RuleGroupRequest ruleGroup, int? parentGroupId, int ruleId);
        IList<RuleActionDb> StateEngineRuleActionsGet(int ruleId);
        IList<ActionDb> StateEngineActionListGet(int objectTypeId);
        int StateEngineRuleActionsSet(List<RuleActionRequest> ruleActionRequests, int ruleId);
    }
}
